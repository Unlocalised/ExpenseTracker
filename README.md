# ExpenseTracker 🧾

**ExpenseTracker** is a modular, event-sourced microservices system built with **.NET 8**, leveraging **Clean Architecture**, **DDD**, and **MartenDB** for efficient command/query separation and event persistence.

## 🧱 Architecture

This project is composed of several services and shared layers:

- **ExpenseService** – Manages account creation, updates, deposits, withdrawals, and deletions.
- **AuditService** – Provides read access to account data for audit purposes.
- **ExpenseTracker** – Contains shared domain models, application interfaces, and event sourcing utilities.

Each service follows Clean Architecture:

- `Api/`: REST endpoints, Swagger docs
- `Application/`: Use cases, MediatR handlers, validation
- `Domain/`: Aggregates, events, enums, exceptions
- `Infrastructure/`: Persistence, Redis, Marten configuration

## 📦 Features

- Event Sourcing via MartenDB
- CQRS with MediatR
- Docker-based microservices setup
- Kong API Gateway for centralized routing
- Redis integration for caching (optional)
- Full validation pipeline via FluentValidation
- OpenAPI documentation via NSwag

## 🧰 Tech Stack

- **.NET 8**, **MediatR**, **FluentValidation**
- **Marten** (event store)
- **PostgreSQL**, **Redis**
- **Docker**, **Kong** (API Gateway)

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/)
- [Docker & Docker Compose](https://docs.docker.com/compose/)
- [Node.js](https://nodejs.org/) (for NSwag codegen)

### Run All Services

```bash
docker-compose up --build
```

Services run on:

- `localhost:80/expense/api` → ExpenseService
- `localhost:80/audit/api` → AuditService
- `localhost:8001` → Kong Admin API

### Access Swagger UI

Each service generates a Swagger spec using NSwag and serves it at:

- `http://localhost:80/expense/api/swagger`
- `http://localhost:80/audit/api/swagger`

## 📂 Folder Structure

```
src/
├── AuditService/
│   ├── Api/ | Application/ | Infrastructure/
├── ExpenseService/
│   ├── Api/ | Application/ | Infrastructure/
├── ExpenseTracker/
│   ├── Api/ | Application/ | Domain/
Kong/
  └── declarative/kong.yml
Postgres/
  ├── init.sql | postgresql-databases.sh
docker-compose.yml
```

## 🧪 Development Tips

- All write operations (create, update, deposit, withdraw, delete) are in `ExpenseService.Application`
- All read models and projections are updated via Marten's async daemon
- `AccountAggregate` and `TransactionAggregate` define the domain logic
- API controllers are in `*.Api/Controllers` with MediatR commands

## ✅ Roadmap

- [x] Event-sourced aggregate state
- [x] AuditService for projections and read models
- [x] Kong Gateway routing
- [ ] Advanced authorization and user handling
- [ ] Frontend (Angular or Blazor)
- [ ] Deployment pipeline

## 🤝 Contributing

Feel free to fork the repo and submit PRs. All feedback is welcome!

## 🪪 License

Licensed under the MIT License.
