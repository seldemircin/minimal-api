# .NET Core Minimal API

This project has been prepared to demonstrate the basic features and best practices of .NET Minimal API. Each section addresses an important topic in modern web API development.

## 🚀 Features

- Minimal API fundamentals
- Route handlers and parameters
- HTTP status codes
- Global error handling
- CORS configuration
- Data validation
- Dependency injection
- Repository pattern
- AutoMapper integration
- Entity Framework Core
- ASP.NET Core Identity
- JWT authentication

## 📋 Sections

1. **Hello World (ch_01_Hello)**
    - Introduction to Minimal API
    - Basic endpoint structure
    - Response modeling

2. **Route Handlers (ch_02_RouteHandlers)**
    - Different handler types
    - Lambda expressions
    - Local functions

3. **Route Parameters (ch_03_RouteParameters)**
    - Route parameters
    - Query string parameters
    - Model binding

4. **Status Codes (ch_04_StatusCodes)**
    - HTTP status codes
    - Results class
    - Response management

5. **Global Error Handler (ch_05_GlobalErrorHandler)**
    - Exception handling
    - Custom exceptions
    - Error response modeling

6. **CORS (ch_06_Cors)**
    - CORS configuration
    - Policy definitions
    - Middleware usage

7. **Validation (ch_07_Validation)**
    - Data annotations
    - Model validation
    - Custom validation

8. **Dependency Injection (ch_08_DependencyInjection)**
    - Service registration
    - Constructor injection
    - Lifetime management

9. **DI with Interfaces (ch_09_DI_Interfaces)**
    - Interface-based programming
    - Loose coupling
    - Service abstraction

10. **Data Access Layer (ch_10_dal)**
    - Entity Framework Core
    - DbContext configuration
    - Repository pattern

11. **Repository in Use (ch_11_repo_in_use)**
    - Generic repository
    - CRUD operations
    - Service layer

12. **AutoMapper (ch_12_auto_mapper)**
    - DTO pattern
    - Object mapping
    - Profile configuration

13. **Configuration (ch_13_configuration)**
    - Extension methods
    - Middleware configuration
    - Service configuration

14. **Relations (ch_14_relations)**
    - Entity relationships
    - Eager loading
    - Navigation properties

15. **Identity (ch_15_identity)**
    - ASP.NET Core Identity
    - User management
    - Role-based authorization

16. **JWT (ch_16_jwt)**
    - JWT authentication
    - Token management
    - Refresh token

## 🛠️ Technologies

- .NET 8.0
- Entity Framework Core
- MySQL
- AutoMapper
- ASP.NET Core Identity
- JWT

## 📦 Installation

1. Clone the project:
```bash
git clone [https://github.com/yourusername/minimal-api.git](https://github.com/yourusername/minimal-api.git)
```

2. Navigate to the project directory:
```bash
cd minimal-api
```

3. Install dependencies:
```bash
dotnet restore
```

4. Create migrations:
```bash
dotnet ef migrations add mig_01
```

5. Create the database:
```bash
dotnet ef database update
```

6. Run the application:
```bash
dotnet run
```

## 🔧 Configuration

Configure the following settings in the `appsettings.json` file:

```json
{
  "ConnectionStrings": {
    "DefaultConnection": "your_connection_string"
  },
  "JwtSettings": {
    "SecretKey": "your_secret_key",
    "Issuer": "your_issuer",
    "Audience": "your_audience"
  }
}
```
