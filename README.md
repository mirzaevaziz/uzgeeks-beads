# Warehouse Management System (WMS)

A modern, cloud-ready Warehouse Management System built with .NET 9 using Domain-Driven Design (DDD) principles and Clean Architecture.

## Features

- **Product Management**: Create, update, and manage product catalog
- **Inventory Tracking**: Real-time inventory tracking across multiple warehouses
- **Order Management**: Handle inbound and outbound orders
- **Location Management**: Organize warehouse storage locations
- **CQRS Pattern**: Separate read and write operations for optimal performance
- **Clean Architecture**: Organized in layers (Domain, Application, Infrastructure, Presentation)
- **RESTful API**: Well-documented API endpoints with Swagger/OpenAPI

## Technology Stack

- **.NET 9**: Latest version of the .NET framework
- **ASP.NET Core**: Web API framework
- **Entity Framework Core 9**: ORM for database operations
- **SQL Server**: Primary database
- **MediatR**: CQRS and mediator pattern implementation
- **AutoMapper**: Object-to-object mapping
- **FluentValidation**: Input validation
- **Swagger/OpenAPI**: API documentation
- **xUnit**: Testing framework
- **Docker**: Containerization

## Architecture

The solution follows Clean Architecture and DDD principles:

```
WMS/
├── src/
│   ├── WMS.Domain/           # Core business logic and entities
│   │   ├── Entities/         # Domain entities
│   │   ├── ValueObjects/     # Value objects (Address, Money, Quantity)
│   │   ├── Events/           # Domain events
│   │   └── Interfaces/       # Repository interfaces
│   ├── WMS.Application/      # Application business rules
│   │   ├── Commands/         # CQRS commands
│   │   ├── Queries/          # CQRS queries
│   │   ├── DTOs/             # Data transfer objects
│   │   └── Mappings/         # AutoMapper profiles
│   ├── WMS.Infrastructure/   # External concerns
│   │   ├── Persistence/      # EF Core DbContext
│   │   └── Repositories/     # Repository implementations
│   └── WMS.WebApi/           # API presentation layer
│       └── Controllers/      # API controllers
├── tests/
│   ├── WMS.Domain.Tests/     # Domain unit tests
│   └── WMS.Application.Tests/# Application unit tests
└── docs/                     # Documentation
```

## Getting Started

### Prerequisites

- [.NET 9 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [SQL Server](https://www.microsoft.com/sql-server) or SQL Server LocalDB
- [Docker](https://www.docker.com/) (optional, for containerized deployment)

### Installation

1. Clone the repository:

```bash
git clone <repository-url>
cd beads_uzgeeks
```

2. Restore dependencies:

```bash
dotnet restore
```

3. Update database connection string in `src/WMS.WebApi/appsettings.json`:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=(localdb)\\mssqllocaldb;Database=WMSDb;Trusted_Connection=true"
  }
}
```

4. Apply database migrations:

```bash
cd src/WMS.Infrastructure
dotnet ef database update --startup-project ../WMS.WebApi
```

5. Run the application:

```bash
cd ../WMS.WebApi
dotnet run
```

6. Access Swagger UI at: `https://localhost:5001/swagger`

### Docker Deployment

Run with Docker Compose:

```bash
docker-compose up -d
```

This will start:

- SQL Server container on port 1433
- WMS API container on port 5000

Access the API at: `http://localhost:5000/swagger`

## API Endpoints

### Products

- `GET /api/products` - Get all products
- `GET /api/products/{id}` - Get product by ID
- `GET /api/products/sku/{sku}` - Get product by SKU
- `POST /api/products` - Create new product
- `PUT /api/products/{id}` - Update product

### Inventory

- `GET /api/inventory` - Get all inventory
- `GET /api/inventory/{id}` - Get inventory by ID
- `POST /api/inventory/adjust` - Adjust inventory levels
- `POST /api/inventory/reserve` - Reserve stock

### Orders

- `GET /api/orders` - Get all orders
- `GET /api/orders/{id}` - Get order by ID
- `POST /api/orders` - Create new order
- `PUT /api/orders/{id}/confirm` - Confirm order
- `PUT /api/orders/{id}/cancel` - Cancel order

## Development

### Build Solution

```bash
dotnet build
```

### Run Tests

```bash
dotnet test
```

### Code Quality

The solution enforces:

- Nullable reference types
- Code analysis (Roslyn analyzers)
- EditorConfig rules
- Central package management

## Project Structure

### Domain Layer

Contains core business logic:

- **Entities**: Product, Warehouse, Location, Inventory, Order, OrderLine
- **Value Objects**: Address, Money, Quantity
- **Domain Events**: StockIncreased, StockDecreased, etc.
- **Interfaces**: Repository contracts

### Application Layer

Contains application-specific business rules:

- **CQRS Commands**: CreateProduct, UpdateProduct, etc.
- **CQRS Queries**: GetProductById, GetAllProducts, etc.
- **DTOs**: Data transfer objects for API
- **Validators**: FluentValidation rules
- **Mappings**: AutoMapper profiles

### Infrastructure Layer

Implements external concerns:

- **EF Core DbContext**: Database configuration
- **Repositories**: Data access implementations
- **Migrations**: Database schema migrations

### Presentation Layer (WebApi)

Handles HTTP requests:

- **Controllers**: API endpoints
- **Middleware**: Request/response pipeline
- **Dependency Injection**: Service registration

## Contributing

1. Fork the repository
2. Create a feature branch
3. Commit your changes
4. Push to the branch
5. Create a Pull Request

## License

This project is licensed under the MIT License.

## Contact

For questions or support, please create an issue in the repository.
