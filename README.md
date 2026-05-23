# 🤝 Donation Platform API

A RESTful API for a community donation platform built with **ASP.NET Core**. Users can post items they want to donate, browse foundations and organizations, and foundations can create profiles listing what they need. Think of it as a Facebook-style social layer for donations.

![C#](https://img.shields.io/badge/C%23-ASP.NET_Core-512BD4?style=flat-square&logo=dotnet&logoColor=white)
![JWT](https://img.shields.io/badge/Auth-JWT-black?style=flat-square&logo=jsonwebtokens)
![Architecture](https://img.shields.io/badge/Architecture-Layered_MVC-blue?style=flat-square)

---

## What it does

- **Donors** can register, post items they want to donate, and browse foundations by category
- **Foundations** can create organizational profiles, list what they need, and receive donations
- **Social features**: comments on publications, star ratings, follower system
- **Auth**: JWT-based authentication with role separation (donor / foundation)
- **Notifications**: system-level notifications for key events (new donation match, comment, etc.)

## Architecture

Follows a strict layered architecture separating concerns across 4 layers:

```
Controllers  →  Services  →  Repositories  →  Database (SQL)
    │               │               │
  HTTP           Business        Data
 routing          logic          access
```

| Layer | Responsibility |
|-------|---------------|
| Controllers | HTTP routing, request/response handling |
| Services | Business logic, validation, orchestration |
| Repositories | Data access, query abstraction |
| Models | Domain entities and DTOs |
| Migrations | Database schema version control |

## Key features

- JWT authentication with role-based access control
- Publication system (create, read, update, delete donations)
- Foundation profiles with star ratings
- Comment threads on publications
- Notification system
- SQL database with migration-based schema management
- Swagger API documentation

## Git workflow

The repository follows a structured branching strategy:

| Branch | Purpose |
|--------|---------|
| `master` | Stable production-ready code |
| `Desarrollo` | Active feature development |
| `Pruebas` | QA and integration testing |

## Stack

| Component | Technology |
|-----------|-----------|
| Framework | ASP.NET Core |
| Language | C# |
| Auth | JWT Bearer tokens |
| Database | SQL Server |
| ORM | Entity Framework Core |
| API Docs | Swagger / OpenAPI |

## Author

**Jaider Romero** · Systems Engineering · Universidad de Cundinamarca  
[LinkedIn](https://www.linkedin.com/in/jaiderromeroudec/) · [jaiderromero8@gmail.com](mailto:jaiderromero8@gmail.com)
