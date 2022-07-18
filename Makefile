sln = ./src/Archaeopteryx.sln
app = ./src/Archaeopteryx.Application/Archaeopteryx.Application.csproj

.PHONY: build

clean-all:
		dotnet clean $(sln)

build-all: clean-all
		dotnet build $(sln)

clean-app:
		dotnet clean $(app)

build-app: clean-app
		dotnet build $(app)

run-app: build-app
		dotnet run --project $(app)

clean-container-image:
		docker rmi archaeopteryx	

build-container-image: build-app
		@echo TODO

start-arangodb: 
		@docker run -e ARANGO_NO_AUTH=1 -p 8529:8529 -d --rm --name arangodb arangodb

stop-arangodb:
		@docker stop arangodb

restart-arangodb: stop-arangodb start-arangodb