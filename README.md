<p align="center">
  <img src="El-Nabaash.API/wwwroot/images/elnabaash.png" alt="El-Nabaash Logo" width="200" />
</p>

<h1 align="center">El-Nabaash API</h1>

<p align="center">
  <em>Internal research API for managing recovered artifacts & archaeological data</em>
</p>

<p align="center">
  <img src="https://img.shields.io/badge/.NET-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt=".NET 10" />
  <img src="https://img.shields.io/badge/PostgreSQL-4169E1?style=for-the-badge&logo=postgresql&logoColor=white" alt="PostgreSQL" />
  <img src="https://img.shields.io/badge/EF_Core-10.0-512BD4?style=for-the-badge&logo=dotnet&logoColor=white" alt="EF Core" />
  <img src="https://img.shields.io/badge/Swagger-85EA2D?style=for-the-badge&logo=swagger&logoColor=black" alt="Swagger" />
  <img src="https://img.shields.io/badge/Docker-2496ED?style=for-the-badge&logo=docker&logoColor=white" alt="Docker" />
</p>

---

## 📖 Overview

**El-Nabaash** is a secure, role-based REST API built with **ASP.NET Core Minimal APIs** for the El-Nabaash Research Division. It provides endpoints for managing archaeological sites, recovered artifacts, catalog records, and media files — with full authentication, authorization, and data seeding out of the box.

### Key Features

- 🏛️ **Site & Artifact Management** — Full CRUD for archaeological sites and artifacts
- 📋 **Catalog Records** — Submission, verification, and note workflows with audit trails
- 🖼️ **Media Storage** — Upload and retrieve artifact images with binary storage
- 🔐 **Identity & Auth** — ASP.NET Identity with JWT Bearer tokens and role-based policies
- 👥 **Role System** — Admin, Archivist, Researcher, and Viewer roles with claims-based permissions
- 📄 **Swagger/OpenAPI** — Interactive API documentation with security definitions
- 🌱 **Auto-Seeding** — Database migrations and seed data applied automatically on startup
- 🐳 **Docker Ready** — Multi-stage Dockerfile and Compose configuration included

---

## 🏗️ Architecture

```
El-Nabaash.API/
├── Data/                    # DbContext, migrations, seed data & utilities
│   ├── AppDbContext.cs
│   ├── DataSeeder.cs
│   ├── DataUtility.cs
│   ├── Migrations/
│   └── SeedData/            # JSON seed files & images
├── DTOs/                    # Request/Response data transfer objects
│   ├── Artifacts/
│   ├── Catalog/
│   ├── Identity/
│   ├── Site/
│   └── Welcome/
├── Endpoints/               # Minimal API endpoint definitions
│   ├── Artifacts/
│   ├── CatalogRecords/
│   ├── CustomIdentityEndpoints/
│   ├── Home/
│   └── Sites/
├── Enums/                   # ArtifactType, CatalogStatus
├── Extensions/              # Service & middleware registration extensions
│   ├── CoreServiceExtensions.cs
│   ├── InfrastructureServiceExtensions.cs
│   ├── OpenApiSwaggerExtensions.cs
│   ├── WebApiServiceExtensions.cs
│   └── WebApplicationExtensions.cs
├── Filters/                 # Swagger schema & exception filters
├── Middlewares/              # Custom middleware (blocked identity routes)
├── Models/                  # Entity models
│   ├── ApplicationUser.cs
│   ├── Artifact.cs
│   ├── ArtifactMediaFile.cs
│   ├── CatalogNote.cs
│   ├── CatalogRecord.cs
│   └── Site.cs
├── Services/                # Business logic layer
│   ├── Abstraction/         # Service interfaces
│   └── Implementation/      # Service implementations
├── wwwroot/                 # Static files (images, logos)
├── Program.cs               # Clean application entry point
├── GlobalUsings.cs
└── appsettings.json
```

### Domain Model

```mermaid
erDiagram
    Site ||--o{ Artifact : contains
    Artifact ||--o{ ArtifactMediaFile : has
    Artifact ||--o{ CatalogRecord : tracks
    CatalogRecord ||--o{ CatalogNote : has
    ApplicationUser ||--o{ CatalogRecord : submits
    ApplicationUser ||--o{ CatalogRecord : verifies

    Site {
        int Id PK
        string Name
        string Location
        string Coordinates
        double Latitude
        double Longitude
        string Description
    }

    Artifact {
        int Id PK
        string Name
        string CatalogNumber
        string Description
        string Type
        DateTime DateDiscovered
        int SiteId FK
    }

    CatalogRecord {
        int Id PK
        int ArtifactId FK
        string SubmittedById FK
        string VerifiedById FK
        string Status
        DateTime DateSubmitted
    }

    CatalogNote {
        int Id PK
        int CatalogRecordId FK
        string AuthorId FK
        string Content
        DateTime CreatedAt
    }
```

---

## 🚀 Getting Started

### Prerequisites

| Tool          | Version  | Purpose                    |
|---------------|----------|----------------------------|
| .NET SDK      | 10.0+    | Build & run the API        |
| PostgreSQL    | 14+      | Database                   |
| Docker        | 24+      | Containerized deployment   |

### 1. Clone the Repository

```bash
git clone https://github.com/AliEl-Husseiny/El-Nabaash.git
cd El-Nabaash
```

### 2. Configure the Database

Set your PostgreSQL connection string using **User Secrets** (recommended) or environment variables:

```bash
cd El-Nabaash.API
dotnet user-secrets set "ConnectionStrings:DbConnection" "Host=localhost;Port=5432;Database=elnabaash;Username=postgres;Password=yourpassword"
```

Or set the `DATABASE_URL` environment variable (used for cloud deployments):

```bash
export DATABASE_URL="postgres://user:password@host:5432/elnabaash"
```

### 3. Run the API

```bash
dotnet run --project El-Nabaash.API
```

The API will:
1. ✅ Apply any pending EF Core migrations
2. ✅ Seed roles (`Admin`, `Archivist`, `Researcher`, `Viewer`)
3. ✅ Seed demo users with claims-based permissions
4. ✅ Seed sample sites, artifacts, media files, and catalog records
5. ✅ Start listening on `https://localhost:5001`

### 4. Explore the API

Open the Swagger UI in your browser:

```
https://localhost:5001/swagger
```

---

## 🐳 Docker

### Build & Run with Docker Compose

```bash
docker compose up --build
```

### Build Manually

```bash
docker build -t el-nabaash-api -f Dockerfile .
docker run -p 8080:8080 -e DATABASE_URL="postgres://user:pass@host:5432/db" el-nabaash-api
```

---

## 🔐 Authentication & Roles

The API uses **ASP.NET Identity** with **JWT Bearer** authentication.

### Default Seeded Users

| Role         | Email                   | Password         | Permissions                                             |
|--------------|-------------------------|------------------|---------------------------------------------------------|
| Admin        | `admin@aeon.org`        | `Admin123!`      | Full access, verify records, upload media, manage users |
| Archivist    | `archivist@aeon.org`    | `Archivist123!`  | Verify catalog records                                  |
| Researcher   | `researcher@aeon.org`   | `Researcher123!` | Upload media                                            |
| Viewer       | `viewer@aeon.org`       | `Viewer123!`     | Read-only access                                        |

### Authenticating

1. **Login** via `POST /api/auth/login` with email and password
2. Copy the returned **Bearer token**
3. Click **Authorize** 🔒 in Swagger UI and paste the token
4. All subsequent requests will be authenticated

---

## 📡 API Endpoints

### Sites

| Method | Endpoint          | Description          |
|--------|-------------------|----------------------|
| GET    | `/api/sites`      | List all sites       |
| GET    | `/api/sites/{id}` | Get site by ID       |
| POST   | `/api/sites`      | Create a new site    |
| PUT    | `/api/sites/{id}` | Update a site        |
| DELETE | `/api/sites/{id}` | Delete a site        |

### Artifacts

| Method | Endpoint              | Description             |
|--------|-----------------------|-------------------------|
| GET    | `/api/artifacts`      | List all artifacts      |
| GET    | `/api/artifacts/{id}` | Get artifact by ID      |
| POST   | `/api/artifacts`      | Create a new artifact   |
| PUT    | `/api/artifacts/{id}` | Update an artifact      |
| DELETE | `/api/artifacts/{id}` | Delete an artifact      |

### Artifact Media Files

| Method | Endpoint            | Description                 |
|--------|---------------------|-----------------------------|
| GET    | `/api/media`        | List media files            |
| GET    | `/api/media/{id}`   | Get/download a media file   |
| POST   | `/api/media`        | Upload a media file         |
| DELETE | `/api/media/{id}`   | Delete a media file         |

### Catalog Records

| Method | Endpoint                | Description                |
|--------|-------------------------|----------------------------|
| GET    | `/api/catalog-records`  | List catalog records       |
| GET    | `/api/catalog-records/{id}` | Get record by ID       |
| POST   | `/api/catalog-records`  | Submit a new record        |
| PUT    | `/api/catalog-records/{id}` | Update a record        |
| DELETE | `/api/catalog-records/{id}` | Delete a record        |

### Authentication

| Method | Endpoint            | Description          |
|--------|---------------------|----------------------|
| POST   | `/api/auth/login`   | Login with credentials |

> **Note:** Registration, password reset, and account management endpoints are blocked by the `BlockIdentityEndpoints` middleware for security purposes.

---

## 🛠️ Tech Stack

| Layer              | Technology                                  |
|--------------------|---------------------------------------------|
| **Framework**      | ASP.NET Core 10.0 (Minimal APIs)            |
| **ORM**            | Entity Framework Core 10.0                  |
| **Database**       | PostgreSQL (via Npgsql)                      |
| **Auth**           | ASP.NET Identity + JWT Bearer               |
| **Documentation**  | Swagger / OpenAPI (Swashbuckle)              |
| **Containerization** | Docker with multi-stage builds            |

---

## 📦 NuGet Packages

| Package                                          | Purpose                           |
|--------------------------------------------------|-----------------------------------|
| `Microsoft.AspNetCore.Authentication.JwtBearer`  | JWT authentication                |
| `Microsoft.AspNetCore.Identity.EntityFrameworkCore` | Identity with EF Core          |
| `Microsoft.EntityFrameworkCore`                  | ORM framework                     |
| `Microsoft.EntityFrameworkCore.Design`           | EF Core migrations tooling        |
| `Microsoft.EntityFrameworkCore.Tools`            | CLI tools for migrations          |
| `Npgsql.EntityFrameworkCore.PostgreSQL`          | PostgreSQL database provider      |
| `Swashbuckle.AspNetCore`                         | Swagger/OpenAPI documentation     |

---

## 🧹 Clean Architecture Highlights

The `Program.cs` is kept minimal and readable through extension methods:

```csharp
public static class Program
{
    public static async Task Main(string[] args)
    {
        var builder = WebApplication.CreateBuilder(args);

        builder.Services.AddWebApiServices(builder.Configuration);
        builder.Services.AddOpenApiSwagger();
        builder.Services.AddInfrastructureServices(builder.Configuration);
        builder.Services.AddCoreServices(builder.Configuration);

        var app = builder.Build();

        await app.SeedDatabaseAsync();
        app.UseSwaggerMiddlewares();
        app.UseHttpsRedirection();
        app.UseStaticFiles();
        app.UseAuthentication();
        app.UseAuthorization();
        app.UseBlockIdentityEndpoints();
        app.MapEndpoints();

        await app.RunAsync();
    }
}
```

| Extension                        | Responsibility                                      |
|----------------------------------|-----------------------------------------------------|
| `AddWebApiServices()`           | Authorization, policies, validation                  |
| `AddOpenApiSwagger()`           | Swagger/OpenAPI configuration & security definitions |
| `AddInfrastructureServices()`   | Database, Identity, email sender                     |
| `AddCoreServices()`             | Application services (Site, Artifact, Catalog)       |
| `SeedDatabaseAsync()`           | Auto-migrate and seed demo data                      |
| `UseSwaggerMiddlewares()`       | Swagger UI pipeline                                  |
| `MapEndpoints()`                | All minimal API endpoint routes                      |

---

## 👤 Author

**Ali Ahmed El-Husseiny**

- 📧 [ali.ahmed.software.engineer@gmail.com](mailto:ali.ahmed.software.engineer@gmail.com)
- 🔗 [linktr.ee/ali.ahmed.software.engineer](https://linktr.ee/ali.ahmed.software.engineer)
- 💻 [github.com/AliEl-Husseiny](https://github.com/AliEl-Husseiny)

---

## 📄 License

This project is proprietary to the El-Nabaash Research Division.

---

<p align="center">
  <sub>Built with ❤️ using .NET 10 & PostgreSQL</sub>
</p>
