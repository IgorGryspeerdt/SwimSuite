
# SwimSuite

SwimSuite is a modern, SaaS-ready web application designed to help swimming clubs manage their day-to-day operations more efficiently.

The project started from a real problem: trainers and officials currently register their activities manually, making reimbursement calculations and bookkeeping time-consuming and error-prone. SwimSuite aims to centralize these processes in a single, easy-to-use application.

## Vision

The long-term goal is to build a modular platform that can be used by one or many swimming clubs.

Although development starts with a single club, the application is designed from day one to support multiple clubs (multi-tenancy), making it suitable as a commercial SaaS product in the future.

## Initial Features

The MVP focuses on the following functionality:

- Secure authentication and role-based access
- Management of clubs and users
- Trainer attendance registration
- Official duty registration
- Monthly reimbursement overview
- Export to Excel for bookkeeping

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

- Docker
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

## Repository Structure

```text
/
├── SwimSuite.Web/        MVC application
├── docs/                 Project documentation
├── .github/              GitHub & AI instructions
├── SwimSuite.sln
└── README.md
```

## Documentation

Additional documentation can be found in the `docs` folder:

- AGENTS.md
- ARCHITECTURE.md
- DATABASE.md
- DECISIONS.md
- DOMAIN.md
- ROADMAP.md

## Development Philosophy

The project follows a simple principle:

> Build the simplest solution that solves today's problem, while keeping tomorrow's expansion in mind.

Rather than overengineering the first version, SwimSuite will evolve iteratively through well-defined milestones.
