# AeroFuel Hub

Aircraft fuel transaction management system built with ASP.NET Core MVC for interview assessment.

## Tech Stack

- ASP.NET Core MVC (.NET 10)
- Entity Framework Core + SQL Server
- ASP.NET Identity (role-based authentication)
- AdminLTE 3 + Bootstrap 5
- Chart.js, QuestPDF, ClosedXML
- Toast notifications (Notyf)

## Features

- Authentication and role-based authorization
- Roles: Admin, AirlineExecutive, FuelSupplyExecutive, FuelCoordinator
- Role-based sidebar navigation and data filtering
- Fuel transaction CRUD with soft delete
- Role-scoped dashboards with Chart.js analytics
- User and profile management
- PDF invoice generation and Excel export
- Dark mode and toast notifications

## Architecture

```
Controller → Service → Repository → DbContext
```

## Prerequisites

- [.NET 10 SDK](https://dotnet.microsoft.com/download)
- SQL Server (LocalDB or SQL Server Express)

## Setup

1. Clone the repository and open the solution folder.
2. **Development** — connection string is in `appsettings.Development.json` (local SQL Express). Update it if your SQL instance differs.
3. Apply migrations and run the app:

```bash
dotnet ef database update
dotnet run
```

On first run, the app seeds roles, demo users, and master data (airlines, airports, fuel companies, aircraft).

1. Open the app in a browser (typically `https://localhost:7xxx` — see launch output).

## Production (MonsterASP.NET)

**Do not commit** `appsettings.Production.json` (passwords). It is gitignored. Use `appsettings.Production.example.json` as a template for local testing only.

### Auto-deploy (GitHub Actions on `main`)

Add these **repository secrets** (Settings → Secrets and variables → Actions):

| Secret | Value |
|--------|--------|
| `FTP_SERVER` | MonsterASP host, e.g. `site12345.siteasp.net` (no `ftp://`) |
| `FTP_USERNAME` | FTP username from Deploy page |
| `FTP_PASSWORD` | FTP password |
| `PRODUCTION_CONNECTION_STRING` | Full SQL connection string from MonsterASP MSSQL page |

On each push to `main`, the workflow builds in **Release**, creates `appsettings.Production.json` from the secret, and uploads to `/wwwroot/` via FTP.

In MonsterASP, set **`ASPNETCORE_ENVIRONMENT`** = `Production` (recommended).

Migrations run automatically on startup (`Database.MigrateAsync()`).

### Manual publish (Visual Studio)

Copy `appsettings.Production.example.json` to `appsettings.Production.json`, fill in your connection string, then Publish. That file stays on your machine only.

## Demo Credentials


| Role                  | Email                                                             | Password     |
| --------------------- | ----------------------------------------------------------------- | ------------ |
| Admin                 | [admin@aerofuelhub.com](mailto:admin@aerofuelhub.com)             | Admin@123    |
| Airline Executive     | [airline@aerofuelhub.com](mailto:airline@aerofuelhub.com)         | Password@123 |
| Fuel Supply Executive | [fuel@aerofuelhub.com](mailto:fuel@aerofuelhub.com)               | Password@123 |
| Fuel Coordinator      | [coordinator@aerofuelhub.com](mailto:coordinator@aerofuelhub.com) | Password@123 |


## Project Structure

```
Controllers/          MVC controllers (thin)
Services/             Business logic and authorization scoping
Repositories/         Data access (EF Core)
Models/Entities/      Domain entities
ViewModels/           Form and display models
Views/                Razor views
Data/                 DbContext and seeders
Constants/            Role name constants
```

