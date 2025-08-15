# E-Commerce Customer Management Backend

Microservice for customer and user management with Clean Architecture, CQRS, and Event Sourcing.

## Features

- Customer CRUD Operations
- User Account Management
- Address Management
- Customer Preferences
- Multi-Tenancy Support
- Event Sourcing
- RESTful API with Versioning

## API Endpoints

### v1 Endpoints
- `GET /api/v1/customers` - List customers
- `GET /api/v1/customers/{id}` - Get customer by ID
- `POST /api/v1/customers` - Create customer
- `PUT /api/v1/customers/{id}` - Update customer
- `DELETE /api/v1/customers/{id}` - Delete customer
- `GET /api/v1/customers/{id}/addresses` - Get customer addresses
- `POST /api/v1/customers/{id}/addresses` - Add customer address

## Architecture

```
├── Domain Layer (Entities, Value Objects, Domain Events)
├── Application Layer (Commands, Queries, Handlers)
├── Infrastructure Layer (Persistence, External Services)
└── API Layer (Minimal API Endpoints)
```

## Run Locally

```bash
dotnet run
```

Access: https://localhost:7001
