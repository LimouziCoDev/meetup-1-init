package elastic

import (
	"context"
	"fmt"
	"reflect"

	elasticapi "gopkg.in/olivere/elastic.v5"
)

type Account struct {
	Number    int    `json:"account_number"`
	Balance   int    `json:"balance"`
	FirstName string `json:"firstname"`
	LastName  string `json:"lastname"`
	Age       int    `json:"age"`
	Gender    string `json:"gender"`
	Address   string `json:"address"`
	Employer  string `json:"employer"`
	Email     string `json:"email"`
	City      string `json:"city"`
	State     string `json:"state"`
}

func GetAccountByAge(client *elasticapi.Client, age int) ([]Account, error) {

	// Search with a term query
	query := elasticapi.NewTermQuery("age", age)

	searchResult, err := client.Search().
		Index("bank").
		Type("account").
		Query(query).
		Sort("account_number", true). //Sort("balance", false).
		Size(3).
		Do(context.Background())

	if err != nil {
		return nil, err
	}

	fmt.Printf("Query took %d milliseconds\n", searchResult.TookInMillis)
	fmt.Printf("Found %d accounts matching\n", searchResult.TotalHits())

	// Each is a convenience function that iterates over hits in a search result.
	// It makes sure you don't need to check for nil values in the response.
	// However, it ignores errors in serialization. If you want full control
	// over iterating the hits, see below.
	var atyp Account
	accounts := []Account{}
	for _, item := range searchResult.Each(reflect.TypeOf(atyp)) {
		if a, ok := item.(Account); ok {
			accounts = append(accounts, a)
		}
	}

	return accounts, nil
}
