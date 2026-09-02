
# SwimSuite

SwimSuite is a modern web application for swimming-club operations, with a future SaaS direction.

The project started from a real problem: trainers and officials currently register their activities manually, making reimbursement calculations and bookkeeping time-consuming and error-prone. SwimSuite aims to centralize these processes in a single, easy-to-use application.

## Documentation

The `/docs` directory is the source of truth for contributors and AI-assisted development. Start with:

- [Vision](docs/VISION.md)
- [Architecture](docs/ARCHITECTURE.md)
- [Coding standards](docs/coding-standards.md)
- [Business rules](docs/business-rules.md)
- [Roadmap](docs/ROADMAP.md)
- [AI-assisted development](docs/ai-development.md)

The database, permissions, domain documentation and architecture decision records are also indexed in `/docs`.

## Vision

The long-term goal is to build a modular platform that can be used by one or many swimming clubs.

Although development starts with club-bound data, the long-term direction is support for multiple clubs (multi-tenancy) and a commercial SaaS product. Full tenant isolation and club-specific user rights are not implemented yet; see the documentation for the current state.

## Current implemented functionality

The MVP focuses on the following functionality:

- Identity-based registration and login UI, with authenticated access on business controllers
- Trainer attendance registration
- Official duty registration
- Training groups and training blocks
- Management of officials and official-duty registration

## Future Modules

The platform is intended to grow over time with additional modules, such as:

- Competition management
- Member management
- Financial reporting
- Website integration
- REST API
- Email notifications
- Mobile application
- Licensing and subscriptions

## Technology Stack

### Backend

- ASP.NET Core MVC (.NET 10)
- C#
- ASP.NET Core Identity
- Entity Framework Core

### Frontend

- Razor Views
- Bootstrap 5

### Database

- PostgreSQL

### Infrastructure

- Dockerfile aanwezig (huidige projectbestandsverwijzing moet vóór een betrouwbare containerbuild worden herzien)
- Docker Compose (planned)

## Architecture

The project follows a layered architecture to keep responsibilities separated.

```text
Presentation (MVC)
        │
Application (Business Logic)
        │
Infrastructure (Data Access, Identity)
        │
Database (PostgreSQL)
```

Business logic should remain independent from controllers whenever possible.

## Project Goals

- Maintainable codebase
- Clean architecture
- Multi-tenant design
- Responsive user interface
- Future API support
- Production-ready deployment
- Commercial SaaS potential

## Development Philosophy

The project follows a simple principle:

> Build the simplest solution that solves today's problem, while keeping tomorrow's expansion in mind.

Rather than overengineering the first version, SwimSuite will evolve iteratively through well-defined milestones.
