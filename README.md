# AeroFuel Hub

AeroFuel Hub is an enterprise-style ASP.NET Core MVC web application for managing aviation fuel operations, including master data, transaction lifecycle tracking, role-based dashboards, and reporting exports.

## Features

- Role-based access control with scoped dashboards for:
  - **Admin**
  - **AirlineExecutive**
  - **FuelSupplyExecutive**
  - **FuelCoordinator**
- Authentication and authorization using ASP.NET Core Identity.
- Fuel transaction workflow:
  - Create, view, search, and soft-delete transactions
  - Transaction history and details
  - PDF invoice generation
  - Excel report export
- Master data management:
  - Airlines
  - Aircraft
  - Airports
  - Fuel companies
- User administration and profile management.
- Seeded initial roles, users, and sample master data for quick onboarding.
- Toast notifications and responsive UI.

## Tech Stack

| Category | Technology |
|---|---|
| Backend | ASP.NET Core MVC (.NET 10, `net10.0`) |
| ORM / Data Access | Entity Framework Core 10 (SQL Server provider) |
| Database | SQL Server / SQL Server Express (configurable via connection string) |
| AuthN/AuthZ | ASP.NET Core Identity + role-based authorization |
| Reporting | QuestPDF (PDF), ClosedXML (Excel) |
| UI | Razor Views, Bootstrap 5, jQuery, jQuery Validation, AdminLTE layout styling |
| Notifications | AspNetCoreHero.ToastNotification (Notyf) |
| CI/CD | GitHub Actions + FTP deployment (MonsterASP target) |

## Architecture

The project follows a layered MVC + service/repository pattern:

```text
Controller -> Service -> Repository -> DbContext (EF Core)
```

Key patterns used:

- **Separation of concerns** between web, business, and data layers.
- **Dependency injection** for repository/service abstractions.
- **Soft delete strategy** for fuel transactions using query filters.
- **Role-driven data access behavior** in dashboard/user flows.

## Project Structure

```text
AeroFuelHub.Web/
├─ .github/
│  └─ workflows/
│     └─ deploy.yml
├─ Constants/
│  └─ Roles.cs
├─ Controllers/
│  ├─ AccountController.cs
│  ├─ DashboardController.cs
│  ├─ FuelTransactionController.cs
│  ├─ UserController.cs
│  ├─ ProfileController.cs
│  ├─ AirlinesController.cs
│  ├─ AircraftsController.cs
│  ├─ AirportsController.cs
│  └─ FuelCompaniesController.cs
├─ Data/
│  ├─ ApplicationDbContext.cs
│  └─ Seeders/
│     ├─ DbSeeder.cs
│     └─ MasterDataSeeder.cs
├─ Enums/
├─ Migrations/
├─ Models/
│  ├─ Common/
│  └─ Entities/
├─ Repositories/
│  ├─ Interfaces/
│  └─ Implementations/
├─ Services/
│  ├─ Interfaces/
│  └─ Implementations/
├─ ViewModels/
├─ Views/
├─ wwwroot/
├─ appsettings.json
├─ appsettings.Development.json
├─ appsettings.Production.example.json
├─ Program.cs
├─ AeroFuelHub.Web.csproj
└─ AeroFuelHub.Web.sln
```

## Prerequisites

- **.NET SDK**: .NET 10 SDK (required by `TargetFramework: net10.0`).
- **Database**: SQL Server instance (e.g., SQL Server Express / Local SQL Server).
- **IDE**: Visual Studio 2022+ (or VS Code + C# extension).
- **EF Core CLI (recommended)**:
  ```bash
  dotnet tool install --global dotnet-ef
  ```
- **Node/npm**: Not required for default build/run workflow in this repository.

## Installation

1. **Clone the repository**
   ```bash
   git clone <REPOSITORY_URL>
   cd AeroFuelHub.Web
   ```

2. **Restore dependencies**
   ```bash
   dotnet restore AeroFuelHub.Web.sln
   ```

3. **Configure settings**
   - Development connection string is in `appsettings.Development.json`.
   - For production-like setup, copy `appsettings.Production.example.json` into a local `appsettings.Production.json` and fill secrets locally.

4. **Apply database migrations**
   ```bash
   dotnet ef database update
   ```

5. **Run the application**
   ```bash
   dotnet run --project AeroFuelHub.Web.csproj
   ```

6. **Open in browser**
   - HTTPS profile default: `https://localhost:7161`
   - HTTP profile default: `http://localhost:5029`

## Environment Variables / Configuration

### Core configuration

| Key | Required | Description |
|---|---|---|
| `ConnectionStrings:DefaultConnection` | Yes | SQL Server connection string used by EF Core. |
| `ASPNETCORE_ENVIRONMENT` | Yes | `Development` / `Production`; controls config source and middleware behavior. |
| `Logging:LogLevel:*` | Optional | Application and framework log verbosity. |

### GitHub Actions secrets (for deployment workflow)

| Secret | Required | Description |
|---|---|---|
| `FTP_SERVER` | Yes | FTP host for deployment target. |
| `FTP_USERNAME` | Yes | FTP username. |
| `FTP_PASSWORD` | Yes | FTP password. |
| `PRODUCTION_CONNECTION_STRING` | Yes | Production SQL Server connection string used to generate `appsettings.Production.json` during CI. |

> Do not commit real production secrets to source control.

## Database Setup

- Provider: **Microsoft.EntityFrameworkCore.SqlServer**.
- DbContext: `ApplicationDbContext` (inherits from `IdentityDbContext<ApplicationUser>`).
- Automatic migration on startup: `Database.MigrateAsync()` is executed at app boot.
- Existing migrations are under `Migrations/`.
- Seed data process at startup:
  - Role seeding
  - Admin user seeding
  - Demo role users seeding
  - Master data seeding (airlines, airports, fuel companies, aircraft)

### Default seeded accounts

| Role | Email | Password |
|---|---|---|
| Admin | `admin@aerofuelhub.com` | `Admin@123` |
| AirlineExecutive | `airline@aerofuelhub.com` | `Password@123` |
| FuelSupplyExecutive | `fuel@aerofuelhub.com` | `Password@123` |
| FuelCoordinator | `coordinator@aerofuelhub.com` | `Password@123` |

> TODO: Rotate seeded credentials and enforce secure secrets policy before production use.

## Build and Run

```bash
dotnet restore AeroFuelHub.Web.sln
dotnet build AeroFuelHub.Web.csproj --configuration Debug
dotnet run --project AeroFuelHub.Web.csproj
```

## GitHub Actions / CI-CD

This repository includes one workflow:

- **`.github/workflows/deploy.yml`**
  - Triggers on push to `main`
  - Restores, builds, publishes the app
  - Generates `appsettings.Production.json` from `PRODUCTION_CONNECTION_STRING`
  - Deploys publish output to `/wwwroot/` via `SamKirkland/FTP-Deploy-Action`

### Deployment target

- Designed for MonsterASP-style FTP deployment.
- Production environment should set `ASPNETCORE_ENVIRONMENT=Production`.

## Screenshots

> TODO: Add screenshots for the following pages:

- Login page
- Role-specific dashboard (Admin / Airline / Fuel Supply / Coordinator)
- Fuel transaction create form
- Fuel transaction history/report page

## API Endpoints

This is an MVC web app (primarily Razor Views), not a standalone REST API. Key HTTP endpoints are controller actions:

| Method | Route | Description |
|---|---|---|
| GET | `/Account/Login` | Render login page. |
| POST | `/Account/Login` | Authenticate user session. |
| POST | `/Account/Logout` | End authenticated session. |
| GET | `/Dashboard/Admin` | Admin dashboard. |
| GET | `/FuelTransaction/Create` | Render create transaction form. |
| POST | `/FuelTransaction/Create` | Submit new fuel transaction. |
| GET | `/FuelTransaction/History` | List/search transactions. |
| GET | `/FuelTransaction/Details/{id}` | View transaction details. |
| GET | `/FuelTransaction/Invoice/{id}` | Generate/download invoice PDF. |
| GET | `/FuelTransaction/ExportExcel` | Export filtered report to Excel. |
| POST | `/FuelTransaction/Delete/{id}` | Soft delete transaction. |

## Testing

- No dedicated test project is currently present in the repository.
- Current validation approach is build + runtime verification.

> TODO: Add unit/integration test projects (e.g., xUnit + WebApplicationFactory) and include in CI pipeline.

## Deployment

### GitHub Actions (recommended)

- Push to `main` triggers automated build and FTP deploy workflow.
- Configure required repository secrets before first deployment.

### Visual Studio publish (manual)

- Use included publish profile templates under `Properties/PublishProfiles/` as a starting point.
- Ensure production connection string is supplied in environment-specific settings.

> TODO: Add Azure App Service and/or Docker deployment profiles if required by your platform strategy.

## Contributing

1. Create a feature branch from `main`.
2. Follow existing layered architecture conventions (Controller -> Service -> Repository).
3. Keep controllers thin and move business logic into services.
4. Add/update migrations for schema changes.
5. Submit a pull request with:
   - Change summary
   - Validation steps
   - Screenshots (for UI changes)

## License

No license file is currently present.

> TODO: Add a `LICENSE` file (for example MIT, Apache-2.0, or company-internal proprietary license) and update this section.

## Author / Maintainers

- **Primary repository maintainer(s):** TODO
- **Contact / team alias:** TODO

---

If you maintain this project in production, consider adding:

- Branch protection rules
- Required PR checks
- Security scanning (CodeQL/Dependabot)
- Secrets scanning and credential rotation policy
