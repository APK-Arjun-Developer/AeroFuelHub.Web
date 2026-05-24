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

2. Update the connection string in `appsettings.json` if needed:

```json
"ConnectionStrings": {
  "DefaultConnection": "Server=localhost\\SQLEXPRESS;Database=AeroFuelHubDb;Trusted_Connection=True;TrustServerCertificate=True;"
}
```

3. Apply migrations and run the app:

```bash
dotnet ef database update
dotnet run
```

On first run, the app seeds roles, demo users, and master data (airlines, airports, fuel companies, aircraft).

4. Open the app in a browser (typically `https://localhost:7xxx` — see launch output).

## Demo Credentials

| Role | Email | Password |
|------|-------|----------|
| Admin | admin@aerofuelhub.com | Admin@123 |
| Airline Executive | airline@aerofuelhub.com | Password@123 |
| Fuel Supply Executive | fuel@aerofuelhub.com | Password@123 |
| Fuel Coordinator | coordinator@aerofuelhub.com | Password@123 |

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
