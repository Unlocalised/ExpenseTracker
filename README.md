# ExpenseTracker

**ExpenseTracker** is a modular, event-sourced .NET Core application designed to manage personal finances. It uses **Clean Architecture**, **Domain-Driven Design (DDD)**, and **Event Sourcing** to deliver a scalable and maintainable system.

## 🏗️ Architecture

This project follows **Clean Architecture** principles and is organized into the following layers:

- **Domain**: Contains core business logic, aggregates, and events.
- **Application**: Defines use cases, command/query handlers, and application-level services.
- **Infrastructure**: Handles database access, event storage, and external integrations.
- **API**: RESTful HTTP interface for client interaction.

## 📦 Features

- Account creation, update, and soft deletion
- Deposit and withdrawal functionality
- Event-sourced account state reconstruction
- CQRS with MediatR (planned)
- MartenDB event storage (planned/in-progress)
- Modular and extensible project structure

## 🧱 Domain Highlights

- **AccountAggregate** is the aggregate root, applying and enqueuing events such as `AccountCreatedEvent`, `AccountDepositEvent`, and `AccountWithdrawalEvent`.
- Events are immutable and drive state transitions.
- Built-in validation logic ensures consistency (e.g., no update after soft deletion).

## 🚀 Getting Started

### Prerequisites

- [.NET 8 SDK](https://dotnet.microsoft.com/download)
- [PostgreSQL](https://www.postgresql.org/) (optional for event store)
- [Docker](https://www.docker.com/) (optional for local dev)

### Running the Application

1. Clone the repo:

```bash
git clone https://github.com/aekoky/ExpenseTracker.git
cd ExpenseTracker
```

2. Restore dependencies and build:

```bash
dotnet restore
dotnet build
```

3. Run the application:

```bash
dotnet run --project src/ExpenseTracker.API
```

## ✅ Roadmap

- [x] Domain-driven event-based account system
- [ ] Integration with MartenDB for event persistence
- [ ] CQRS pattern for separation of read/write models
- [ ] UI interface with Angular or Blazor
- [ ] Dockerized deployment

## 🧪 Testing

Unit tests for aggregates and application logic are planned using xUnit and FluentAssertions.

## 🧠 Design Principles

- Separation of concerns
- Single responsibility
- High cohesion, low coupling
- Immutable events
- Testability and maintainability

## 📂 Project Structure

```
ExpenseTracker/
├── src/
│   ├── ExpenseTracker.API/          # API endpoints
│   ├── ExpenseTracker.Application/  # Application layer
│   ├── ExpenseTracker.Domain/       # Domain models and events
│   └── ExpenseTracker.Infrastructure/ # Persistence, services
├── tests/
│   └── ExpenseTracker.Tests/        # Unit tests
```

## 🤝 Contributing

Contributions are welcome! Feel free to open an issue or submit a pull request.

## 📄 License

This project is licensed under the MIT License. See the [LICENSE](LICENSE) file for details.
