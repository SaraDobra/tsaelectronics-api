# TsaElectronics

Backend API for an electronics ecommerce site (phones, laptops, etc.), built with ASP.NET Core (.NET 10) and SQL Server. A separate Next.js app consumes this API.

## Architecture

Single Web API project (`TsaElectronics.Api`) organized in layers:

- `Controllers/` — HTTP endpoints
- `Services/` — business logic, one folder + interface per feature (e.g. `ProductServices/IProductService`)
- `Data/Entities/` — EF Core entity classes, grouped by feature
- `Data/AppDbContext.cs` — EF Core + ASP.NET Core Identity database context
- `Mapper/` — AutoMapper profiles (entity ↔ model)
- `Models/` — request/response models, grouped by feature
- `Helpers/` — cross-cutting utilities (JWT issuing, slug generation, migration bootstrapping)

Auth: ASP.NET Core Identity issues JWTs from `/api/auth/login` and `/api/auth/register` as plain JSON. The Next.js frontend's Auth.js Credentials provider consumes that token server-side and owns the actual browser session cookie — this API never sets a cookie itself.

## Local development setup

### 1. Install Docker Desktop

SQL Server has no native macOS build, so local dev runs it in a container. Install Docker Desktop for Mac: https://www.docker.com/products/docker-desktop

### 2. Start SQL Server

```bash
docker compose up -d
```

This starts a SQL Server 2022 container on `localhost:1433` (SA password is dev-only, set in `docker-compose.yml`).

### 3. Apply database migrations

Local connection string and JWT signing key are already configured via .NET User Secrets (not committed to git). The API applies pending migrations automatically on startup in the Development environment.

To apply them manually instead:

```bash
cd src/TsaElectronics.Api
dotnet ef database update
```

### 4. Run the API

```bash
cd src/TsaElectronics.Api
dotnet run
```

Or press F5 in VS Code (launch configuration already set up in `.vscode/`).

Swagger UI is available at `https://localhost:<port>/swagger` in Development.

### Secrets

Non-secret config lives in `appsettings.json`. Local secrets (DB connection string, JWT key) are stored via `dotnet user-secrets` and never committed. Deployed environments (Azure App Service) get secrets via environment variables in the App Service configuration — see `docs/ARCHITECTURE.md`.

## Tests

```bash
dotnet test
```
