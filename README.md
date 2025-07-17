# Feature Flag Service
## Project prerequisites
- .NET SDK 9.0
- Postman (for API testing, optional)

## Getting Started

* Go to the folder `feature-flag-service/FeatureFlag.Api`
* Execute the command `dotnet restore`
* Execute the command `dotnet run`

## API Testing with PostMam
* Open PostMan app
* Import the PostMan collection from `feature-flag-service/FeatureFlags Rute.postman_collection.json`
* I left some request to tests all the methods for example with the GUID `11111111-1111-1111-1111-111111111111` which is by default loaded in memory

## Tests with FeatureFlag.Api.http file
* Open the file and execute the requests here [Tests http file](/FeatureFlag.Api/FeatureFlag.Api.http)
* It contains tests for the main bussines rules

## Comments

* No database or external service. Since the task said 4 hour and I desided left the service with the data inmemory, so no persistence
* Easy transition to persistent storage via IFeatureToggleRepository, creating new concrete implementation with the DB logic
* I didn´t have enough time to implement Unit Test, github copilot would help but I left it out of scope

### Time Spend

- 2 hour Analysis, Scaffolding and environment configuration (Git Repo, references...etc)
- 3 hour implementation of business logic
- 1 hour Documentation