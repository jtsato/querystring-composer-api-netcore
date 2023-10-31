# querystring-composer-api-netcore
This is a sample project to demonstrate the Clean Architecture principles using .NET Core 7.0

![CI](https://github.com/jtsato/querystring-composer-api-netcore/actions/workflows/continuous-integration.yml/badge.svg)
[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=jtsato_querystring-composer-api-netcore&metric=alert_status)](https://sonarcloud.io/summary/new_code?id=jtsato_querystring-composer-api-netcore)

[![Lines of Code](https://sonarcloud.io/api/project_badges/measure?project=jtsato_querystring-composer-api-netcore&metric=ncloc)](https://sonarcloud.io/summary/new_code?id=jtsato_querystring-composer-api-netcore)
[![Coverage](https://sonarcloud.io/api/project_badges/measure?project=jtsato_querystring-composer-api-netcore&metric=coverage)](https://sonarcloud.io/summary/new_code?id=jtsato_querystring-composer-api-netcore)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=jtsato_querystring-composer-api-netcore&metric=security_rating)](https://sonarcloud.io/summary/new_code?id=jtsato_querystring-composer-api-netcore)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=jtsato_querystring-composer-api-netcore&metric=reliability_rating)](https://sonarcloud.io/summary/new_code?id=jtsato_querystring-composer-api-netcore)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=jtsato_querystring-composer-api-netcore&metric=sqale_rating)](https://sonarcloud.io/summary/new_code?id=jtsato_querystring-composer-api-netcore)

**Table of Contents**

* [Technology stack](#technology-stack)
* [Prerequisites](#prerequisites)
* [Solution Structure](#solution-structure)
* [Testing Strategy](#testing-strategy)
* [Mutation Reports](#mutation-reports)
* [Building and Running the solution](#building-and-running-the-solution)
* [Resources](#resources)

***
## Technology stack

![.Net](https://img.shields.io/badge/.NET-5C2D91?style=for-the-badge&logo=.net&logoColor=white)
![c-sharp](https://img.shields.io/badge/C%23-239120?style=for-the-badge&logo=c-sharp&logoColor=white)
![Web Api](https://img.shields.io/badge/Web%20Api-grey?style=for-the-badge&logo=dotnet&logoColor=white)
![swagger](https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=Swagger&logoColor=white)
![Shell Script](https://img.shields.io/badge/shell_script-%23121011.svg?style=for-the-badge&logo=gnu-bash&logoColor=white)
![GitHub Actions](https://img.shields.io/badge/githubactions-%232671E5.svg?style=for-the-badge&logo=githubactions&logoColor=white)
![Docker](https://img.shields.io/badge/Docker-9ECAFA.svg?logo=docker&style=for-the-badge)

## Prerequisites

* [.NET 7](https://dotnet.microsoft.com/download)
* [Docker](https://docs.docker.com/get-docker)
* [Docker compose](https://docs.docker.com/compose/install/)

## Solution Structure

##### Core: Entities
* Represent your domain object
* Apply only logic that is applicable in general to the whole entity

##### Core: Use Cases
* Represent your business actions, it’s what you can do with the application. Expect one use case for each business action.
* Pure business logic
* Define interfaces for the data that they need in order to apply some logic. One or more providers will implement the interface, but the use case doesn’t know where the data is coming from.
* The use case doesn't know who triggered it and how the results are going to be presented.
* Throws business exceptions.

##### Providers
* Retrieve and store data from and to a number of sources (database, network devices, file system, 3rd parties, etc.)
* Implement the interfaces defined by the use case
* Use whatever framework is most appropriate (they are going to be isolated here anyway).
* Note: if using an ORM for database access, here you'd have another set of objects in order to represent the mapping to the tables (don't use the core entities as they might be very different).

##### Entrypoints
* They are ways of interacting with the application, and typically involve a delivery mechanism (e.g. REST APIs, scheduled jobs, GUI, other systems).
* Trigger a use case and convert the result to the appropriate format for the delivery mechanism
* A GUI would use MVC (or MVP) in here; the controller would trigger a use case

##### Configuration
* Wires everything together.
* Frameworks (e.g. for dependency injection) are isolated here
* Has the "dirty details" like Main class, web server configuration, datasource configuration, etc.

## Testing Strategy
##### Unit Tests
* for TDD (a.k.a. Tests first, to drive design).
* Cover every little detail, aim for 100% coverage.
* “Dev to dev” documentation: What should this class do?
* Test individual classes in isolation, very fast.

##### Integration Tests
* Test integration with slow parts (http, database, etc.)
* “Dev” documentation: Does this work as expected?
* Test one layer in isolation (e.g. only rest endpoint, or only data provider). Slow
* Use whatever library makes it easy.

## Mutation Reports
* [Core](https://jtsato.github.io/querystring-composer-api-netcore/Core/mutation-report.html)
* [EntryPoint.WebApi](https://jtsato.github.io/querystring-composer-api-netcore/EntryPoint.WebApi/mutation-report.html)
* [Infra.MongoDB](https://jtsato.github.io/querystring-composer-api-netcore/Infra.MongoDB/mutation-report.html)

***

## Building and Running the solution
* cleaning the solution:
```
dotnet clean
```
* building the solution:
```
dotnet build
```
* running unit tests:
```
dotnet test --nologo -v q
```
* running mutation tests:
```
cd UnitTest.Core
dotnet stryker
```
* starting the solution:
```
cd EntryPoint.WebApi/bin/Debug/net7.0
dotnet EntryPoint.WebApi.dll
```
***

## Resources
##### Blogs & Articles
* The Clean Architecture https://blog.8thlight.com/uncle-bob/2012/08/13/the-clean-architecture.html
* Screaming Architecture http://blog.8thlight.com/uncle-bob/2011/09/30/Screaming-Architecture.html
* NODB https://blog.8thlight.com/uncle-bob/2012/05/15/NODB.html
* Hexagonal Architecture http://alistair.cockburn.us/Hexagonal+architecture

##### Videos & Presentations
* Clean Architecture https://www.youtube.com/results?search_query=clean+architecture
* Screaming Architecture http://www.infoq.com/presentations/Screaming-Architecture

***
