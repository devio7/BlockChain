 ## BlockChain API

ASP.NET Core REST API built with .NET 10 that retrieves blockchain information from the BlockCypher API and stores blockchain history in a local SQLite database.

The solution follows **Clean Architecture** combined with **Vertical Slice Architecture** for application use cases.


 #### Solution Structure

The solution contains seven projects:

```text
BlockChain
│
├── BlockChain.API
│
├── BlockChain.Application
│
├── BlockChain.Domain
│
├── BlockChain.Infrastructure
│
├── BlockChain.UnitTests
│
├── BlockChain.IntegrationTests
│
└── BlockChain.FunctionalTests
```

 #### Architecture

The application is organized into four main layers:

```text
┌───────────────────────────────┐
│        BlockChain.API         │
│                               │
│ Controllers                   │
│ Middleware                    │
│ Exception handling            │
│ Application composition       │
└───────────────┬───────────────┘
                │
                ▼
┌───────────────────────────────┐
│     BlockChain.Application    │
│                               │
│ Features                      │
│ Commands / Queries            │
│ Handlers                      │
│ Validators                    │
│ DTOs                          │
│ Application abstractions      │
└───────────────┬───────────────┘
                │
                ▼
┌───────────────────────────────┐
│        BlockChain.Domain      │
│                               │
│ Entities                      │
│ Enums                         │
│ Interfaces                    │
└───────────────────────────────┘
                ▲
                │
┌───────────────┴───────────────┐
│    BlockChain.Infrastructure  │
│                               │
│ Persistence                   │
│ EF Core / SQLite              │
│ Repositories                  │
│ BlockCypher integration       │
└───────────────────────────────┘
```

The test projects are separate from the application architecture:

* BlockChain.UnitTests
* BlockChain.IntegrationTests
* BlockChain.FunctionalTests

Each project tests the application at a different level.

 #### BlockChain.API

The API project is responsible for exposing the REST API and composing the application.

```text
BlockChain.API
│
├── Controllers
├── Exceptions
├── Extensions
├── Middlewares
├── Properties
├── appsettings.json
├── Dockerfile
└── Program.cs
```

 #### Responsibilities

* REST API controllers
* HTTP request/response handling
* Middleware
* Exception handling
* Dependency Injection composition
* Application startup
* OpenAPI / Swagger
* Docker configuration
* Application configuration

Controllers are intentionally kept thin and delegate application operations to MediatR.

 #### BlockChain.Application

The Application project contains the application's use cases and application-specific logic.

```text
BlockChain.Application
│
├── Common
├── DTOs
├── ExternalServices
├── Features
└── ServiceCollectionExtensions.cs
```

The application uses Vertical Slice Architecture.

Each feature represents a specific use case and contains the components required for that operation.

For example:

```text
Features
└── Blockchain
    │
    ├── CreateBlockchainEntry
    │   ├── Command
    │   ├── Handler
    │   └── Validator
    │
    └── GetBlockchainHistory
        ├── Query
        └── Handler
```

 #### Responsibilities
* Commands
* Queries
* Handlers
* Validators
* DTOs
* Application abstractions
* Application-specific mapping
* External service abstractions

The Application layer does not contain concrete Infrastructure implementations.

 #### BlockChain.Domain

The Domain project contains the core domain model.

```text
BlockChain.Domain
│
├── Entities
├── Enums
└── Interfaces
```

 #### BlockChain.Infrastructure

Infrastructure contains the concrete implementations for persistence and external services.

```text
BlockChain.Infrastructure
│
├── ExternalServices
│   └── BlockCypher
│       ├── BlockCypherClient.cs
│       ├── BlockCypherClientOptions.cs
│       └── BlockCypherConstants.cs
│
├── Persistence
│   ├── Configurations
│   │   └── BlockchainConfiguration.cs
│   │
│   ├── Repositories
│   │   └── BlockchainRepository.cs
│   │
│   └── BlockchainDbContext.cs
│
└── ServiceCollectionExtensions.cs
```

 #### BlockCypher

BlockCypherClient.cs implements communication with the BlockCypher API.

The Application layer communicates through an abstraction, while the concrete HTTP implementation is located in Infrastructure.

BlockCypherClientOptions.cs contains configuration options for the external API.

BlockCypherConstants.cs contains constants used by the BlockCypher integration.

 #### Persistence

Entity Framework Core persistence is located under:

```text
Persistence
├── Configurations
├── Repositories
└── BlockchainDbContext.cs
```

 #### Testing Architecture

The solution separates tests into three projects:

* BlockChain.UnitTests
* BlockChain.IntegrationTests
* BlockChain.FunctionalTests

Each test project has a different purpose.

                    Testing Pyramid

                          /\
                         /  \
                        /    \
                       /      \
                      /        \
                     /Functional\
                    /    Tests   \
                   /--------------\
                  /  Integration   \
                 /      Tests       \
                /--------------------\
               /     Unit Tests       \
              /________________________\

 #### Technologies

* .NET 10
* ASP.NET Core
* C#
* Entity Framework Core
* SQLite
* MediatR
* FluentValidation
* Serilog
* OpenAPI
* Swagger UI
* Docker
* BlockCypher API

 ### Quick Start

 #### Run with Visual Studio and IIS Express

Open the solution in Visual Studio.

Set `BlockChain.API` as the startup project.

In the Visual Studio toolbar, select:

```text
IIS Express
```

 #### Run with Docker

For running the API inside a Linux Docker container, select:

```text
Container (Dockerfile)
```