# DMS.Api

An ASP.NET Core Web API application for Document Management System.

## Project Structure

- **Controllers/**: API controllers
- **Models/**: Data models
- **Services/**: Business logic services
- **Program.cs**: Application entry point
- **appsettings.json**: Configuration settings

## Getting Started

### Prerequisites
- .NET 8.0 SDK

### Running the Application

```bash
dotnet run
```

The API will be available at:
- HTTPS: `https://localhost:7001`
- HTTP: `http://localhost:5001`

### API Documentation

When running in development mode, Swagger UI is available at:
`https://localhost:7001/swagger`

## Development

### Build
```bash
dotnet build
```

### Test
```bash
dotnet test
```

## Contributing

1. Fork the repository
2. Create a feature branch
3. Make your changes
4. Submit a pull request