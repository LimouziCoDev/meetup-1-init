# Devops

Little presentation from a go program build and deployed to docker
Then exposed via minikube (local kubernetes cluster)

## Presentation

In french:
https://docs.google.com/presentation/d/18h9jIbZljG4rtMNMrFIw2Q-QUJ2ZcqqyjnIOmVLk3PM/edit?usp=sharing

## Docker compose

The docker-compose is configured to use external storage to keep track of indices and data. To not refill the dataset each time we launch it.

After the first `docker-compose up` please run:

```bash
curl -H 'Content-Type: application/x-ndjson' -XPOST 'http://localhost:9200/bank/account/_bulk?pretty' --data-binary @accounts.json
```

## Minikube

### Basic commands

```shell
# start
minikube start
#cstop
minikube stop
# open dashboard
minikube dashboard
```

### Run Elasticsearch

This part is mandatory to make Elasticsearch run with enough memory:

```shell
minikube start
minikube ssh 'echo "sysctl -w vm.max_map_count=262144" | sudo tee -a /var/lib/boot2docker/bootlocal.sh'
minikube stop && minikube start
```

Then push the image and expose a service

```shell
kubectl run elasticsearch --image=docker.elastic.co/elasticsearch/elasticsearch:5.6.10 --image-pull-policy=IfNotPresent --env="xpack.security.enabled=false" --env="discovery.type=single-node" --env="ES_JAVA_OPTS=-Xms512m -Xmx512m" --port=9200
kubectl expose deployment elasticsearch --type=LoadBalancer
minikube service elasticsearch
```

Finally fill with data

`curl -H 'Content-Type: application/x-ndjson' -XPOST 'http://192.168.99.100:31342/bank/account/_bulk?pretty' --data-binary @accounts.json`

### Deploy the app

```shell
kubectl config use-context minikube
kubectl delete deploy meetup-devops
kubectl run meetup-devops --image=fsilberstein/meetup-devops:latest --image-pull-policy=Always --port=8080
kubectl expose deployment meetup-devops --type=NodePort
minikube service meetup-devops
```
