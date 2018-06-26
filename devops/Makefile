BASEREPO = fsilberstein
IMAGE = meetup-devops

.PHONY: build
build:
	CGO_ENABLED=0 GOOS=linux go build -i -o ./devops -ldflags '-s' -installsuffix cgo main.go

.PHONY: test
test-unit:
	go test -v ./...

.PHONY: docker
docker:
	echo $(DOCKER_PASSWORD) | docker login -u $(DOCKER_USERNAME) --password-stdin && \
	docker build --no-cache -t $(BASEREPO)/$(IMAGE):latest . && \
	docker push $(BASEREPO)/$(IMAGE):latest