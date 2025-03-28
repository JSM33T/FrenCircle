
# FrenCirlce

A image sharing web space for the frens by the frens.

# FrenCircle Solution

This solution is structured using Clean Architecture principles to separate concerns, improve testability, and enable future scalability.

---

## 🗂 Project Structure

### `FrenCircle.Api`

- ASP.NET Core Web API
- Contains Controllers, middlewares, filters
- Entry point for HTTP requests

---

### `FrenCircle.Application`

- Contains use cases, services, and business logic
- Implements handlers (e.g., `CreateUserHandler`)
- Depends on `Contracts`, references `Domain`

---

### `FrenCircle.Contracts`

- Contains:
  - DTOs for data transfer between layers
  - Interfaces for services/repositories (used in Application, implemented in Infra)

---

### `FrenCircle.Domain`

- Pure domain models and business rules
- Contains Entities, Enums, and Value Objects
- No dependencies on infrastructure or frameworks

---

### `FrenCircle.Infra`

- Infrastructure logic:
  - Logging
  - File storage
  - Email service, etc.
- Implements interfaces from `Contracts`

---

### `FrenCircle.Repositories`

- Database-specific implementation (Dapper/EF Core)
- Handles direct SQL/stored procedure access
- Implements repository interfaces from `Contracts`

---

### `FrenCircle.Shared`

- Common utilities, constants, helpers
- AutoMapper Profiles (`Shared/Helpers/Mapper`)
- Can be used across all layers

---

## 🚀 Tech Stack

- ASP.NET Core Web API
- Dapper for data access
- AutoMapper for object mapping
- MSSQL (SQL Server)
- Clean Architecture + DI

---

## 📌 Notes

- DTOs and interfaces live in `Contracts` to keep Application and Infra decoupled.
- Domain is kept pure and self-contained.
- Infra and Repositories are swappable; only contracts matter to Application.
