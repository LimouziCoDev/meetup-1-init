package main

import (
	"encoding/json"
	"fmt"
	"log"
	"net/http"
	"os"
	"os/signal"
	"strconv"
	"syscall"
	"time"

	"github.com/LimouziCoDev/meetup-1-init/devops/elastic"
	elasticapi "gopkg.in/olivere/elastic.v5"
)

func main() {
	// Errors channel
	errc := make(chan error)

	// Create a client for elasticsearch
	elasticURL := os.Getenv("ES_URL")
	if elasticURL == "" {
		elasticURL = "http://127.0.0.1:9200/"
	}
	client, err := elasticapi.NewClient(elasticapi.SetSniff(false), elasticapi.SetURL(elasticURL))
	if err != nil {
		// trick for docker-compose wait until ES is ready
		fmt.Println("ES not ready, wait for 10s")
		time.Sleep(time.Second * 10)
		client, err = elasticapi.NewClient(elasticapi.SetSniff(false), elasticapi.SetURL(elasticURL))
		if err != nil {
			panic(err)
		}
	}

	// Interrupt handler.
	go func() {
		c := make(chan os.Signal, 1)
		signal.Notify(c, syscall.SIGINT, syscall.SIGTERM)
		errc <- fmt.Errorf("%s", <-c)
	}()

	go func() {
		// index endpoint
		http.HandleFunc("/", func(w http.ResponseWriter, r *http.Request) {
			w.WriteHeader(http.StatusOK)
			fmt.Fprintln(w, "Welcome to the first Limouzi meetup!")
		})

		http.HandleFunc("/bank/search/", func(w http.ResponseWriter, r *http.Request) {
			ages, ok := r.URL.Query()["age"]

			if !ok || len(ages) < 1 {
				log.Println("Url Param 'age' is missing")
				w.WriteHeader(http.StatusBadRequest)
				return
			}

			age, _ := strconv.Atoi(ages[0])
			accounts, err := elastic.GetAccountByAge(client, int(age))
			if err != nil {
				w.WriteHeader(http.StatusInternalServerError)
				log.Println(err)
				return
			}

			w.Header().Set("Content-Type", "application/json; charset=utf-8")
			w.WriteHeader(http.StatusOK)
			json.NewEncoder(w).Encode(accounts)
		})

		log.Println("The microservice bookkeeping-data-migration is started on port 8080")
		errc <- http.ListenAndServe(":8080", nil)
	}()

	log.Println("exit", <-errc)
}
