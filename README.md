# ExpenseTracker

**ExpenseTracker** is a distributed, event-driven financial microservices system built with modern .NET technologies, designed for scalability, maintainability, and clear separation of concerns.

## 🧠 Core Concepts

- **Microservices**: Split between `ExpenseService` (write side) and `AuditService` (read side)
- **Event Sourcing**: Persist events using **Marten** over PostgreSQL
- **CQRS**: Clear segregation of command and query responsibilities
- **Async Messaging**: Built with **Wolverine** and **RabbitMQ** for robust, durable inter-service communication
- **API Gateway**: Centralized routing using **Kong Gateway**
- **Dockerized**: All services can be run in containers using Docker Compose

## 📁 Project Structure

```
src/
├── ExpenseService/       # Command side with full CRUD logic and event publishing
├── AuditService/         # Read side with Marten projections and async daemon
├── ExpenseTracker/       # Shared contracts, common abstractions, and API base
└── Kong/                 # Kong gateway declarative config
```

## ⚙️ Technologies

- [.NET 8](https://dotnet.microsoft.com)
- [Marten](https://martendb.io)
- [Wolverine](https://wolverine.netlify.app)
- [RabbitMQ](https://www.rabbitmq.com)
- [PostgreSQL](https://www.postgresql.org)
- [Redis](https://redis.io)
- [Kong Gateway](https://konghq.com/kong/)
- [Docker Compose](https://docs.docker.com/compose/)

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/en-us/download/dotnet/8.0)
- [Docker](https://www.docker.com/)
- [Kong CLI](https://docs.konghq.com/gateway/latest/kong-enterprise/kong-manager/kong-manager-cli/)

### Run with Docker

```bash
git clone https://github.com/aekoky/ExpenseTracker.git
cd ExpenseTracker
docker compose up --build
```

This starts:
- Kong Gateway
- PostgreSQL
- Redis
- RabbitMQ
- ExpenseService (command API)
- AuditService (query API)

### Test the APIs

Once up, access:

- ExpenseService: http://localhost:8080/expense/api/docs
- AuditService: http://localhost:8080/audit/api/docs

You can create/update/delete accounts from the `ExpenseService`, and verify read projections from the `AuditService`.

## 🧪 Development & Debugging

To run locally via Visual Studio or CLI:

```bash
dotnet build
dotnet run --project src/ExpenseService/ExpenseService.Api
dotnet run --project src/AuditService/AuditService.Api
```

Make sure PostgreSQL, Redis, and RabbitMQ are running (or use the docker setup).

## ✨ Features

- Strongly typed integration events
- Durable inbox for message processing
- Event projections using Marten's async daemon
- FluentValidation with Wolverine
- Kong routing via declarative YAML

## 📜 License

This project is open-sourced under the MIT License.

---

Feel free to open issues, fork the repo, and suggest improvements. Contributions welcome! 🚀
