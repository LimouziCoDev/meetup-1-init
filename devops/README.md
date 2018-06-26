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
