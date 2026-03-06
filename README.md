# JinjaMonogatari API

JinjaMonogatari API is a .NET Web API that serves shrine discovery and cultural content for the JinjaMonogatari platform. It provides read-optimized endpoints for map/list/detail views, a citation-backed etiquette module, and authenticated user features such as saved-shrine collections with refresh-token session support.

---

## ✨ Overview

This repository contains the backend API used by the JinjaMonogatari clients. The API is structured for predictable change and long-term maintainability: feature-scoped CQRS handlers, explicit DTOs, and Infrastructure services that keep database concerns out of controllers and request handlers.

Mobile client repository:
- Mobile: https://github.com/KobenjiSan/jinjamonogatari-mobile

## 🧠 Problem & Motivation

Shrine discovery is only useful if it can scale beyond hardcoded fixtures and support real-world queries (map points, nearby browsing, detail hydration) while preserving research transparency (citations and attribution). This API exists to deliver structured cultural content efficiently, enforce consistent shapes to clients via DTOs, and centralize authentication + user collections without leaking persistence concerns into the UI.

## 🏗 Architecture

### Architecture overview

The API follows a layered `src/` structure with CQRS feature boundaries:

- `src/Api`
  - Thin controllers grouped by feature (`Controllers/Etiquette`, `Controllers/Shrines`, `Controllers/Users`)
  - Cross-cutting middleware (`ExceptionHandlingMiddleware`)
  - Identity extraction helpers (`ClaimsPrincipalExtensions`)
- `src/Application`
  - Feature-scoped DTOs (read models)
  - Queries + handlers (read use cases)
  - Commands + handlers (write use cases)
  - Service interfaces (contracts the Infrastructure layer implements)
  - Common exceptions and shared interfaces
- `src/Domain`
  - Entities and relationships (Shrine, Tag, Kami, History, Folklore, Image, Citations, User, RefreshToken, UserCollection, etc.)
  - Timestamp contracts (`IHasCreatedAt`, `IHasTimestamps`)
- `src/Infrastructure`
  - EF Core persistence (`AppDbContext`, configurations, migrations, seeding via `DevDataSeeder`)
  - Service implementations (read/write services per feature)
  - Auth services (password hashing, token generation, token options)

This structure keeps request orchestration (controllers/handlers) separate from persistence details (EF Core), while domain modeling stays independent of transport concerns (DTOs).

### Major layers/services

- **Controllers (Api)**
  - Route and authorize requests
  - Delegate to application queries/commands
  - Do not perform database work directly

- **CQRS Handlers (Application)**
  - Queries: read flows like map points, list view, previews, shrine detail tabs, etiquette topics
  - Commands: write flows like register/login/logout, refresh tokens, profile update, add/remove saved shrines
  - Each handler calls a feature service interface

- **Services (Infrastructure)**
  - Encapsulate EF Core queries and writes
  - Centralize PostGIS usage (distance, map points, and location-aware responses)
  - Ensure write logic and timestamps are consistent across the system

### Key technical decisions

- **CQRS by feature**: handlers are grouped under `Application/Features/<Feature>/Queries|Commands` so each use case has an obvious home.
- **DTO-first API boundary**: the API returns explicit DTOs (e.g., `ShrinePreviewDto`, `ShrineMetaDto`, `ShrineCardDto`, `EtiquetteTopicDto`) to keep clients stable even if domain entities evolve.
- **Spatial database support (PostGIS + NetTopologySuite)**: shrine locations are stored as spatial points; distance and map-related queries are handled by the database rather than re-implementing geospatial math in the app layer.
- **Centralized exception middleware**: errors are normalized through `ExceptionHandlingMiddleware` so controllers stay minimal and behavior is consistent.
- **JWT claims as identity source**: user identity is extracted from the authenticated principal rather than trusting client-supplied user IDs.

## 🚀 Features

- Shrine discovery read endpoints:
  - Map points feed for rendering markers
  - List view and preview endpoints
  - Detail endpoints split by content areas (meta, kami, history, folklore, gallery)
- Etiquette module:
  - Topic list + topic detail queries
  - Steps, citations, and image attribution models
- Authentication and user session:
  - Register and login flows with hashed passwords
  - JWT access tokens
  - Refresh-token rotation support
  - Logout flow
- User profile:
  - Retrieve current user (`/me`)
  - Update profile (first name, last name, phone)
- Saved shrines collection:
  - Add/remove saved shrine
  - Read saved IDs and saved cards for client rendering
- Development tooling:
  - Swagger/OpenAPI for endpoint discovery
  - Seeding and migrations for local bootstrap

## 🛠 Tech Stack

### Backend
- .NET Web API (C#)
- CQRS-style request handling (queries/commands + handlers)

### Database
- PostgreSQL
- PostGIS (spatial queries)
- EF Core + NetTopologySuite

### Authentication / Security
- JWT access tokens
- Refresh tokens (rotation)
- Claims-based user identity extraction
- Password hashing via a dedicated service

### Infrastructure
- EF Core migrations
- Local seeding via `DevDataSeeder`
- Centralized exception middleware

### Tools
- Swagger/OpenAPI (development)
- pgAdmin (local DB management)

## 📸 Screenshots / Demo

> Screenshots coming soon.

## ⚙️ Local Development

### Prerequisites

- .NET SDK (matching the repo’s target framework)
- PostgreSQL with PostGIS enabled
- pgAdmin (optional but recommended)

### Setup

1) Clone

```bash
git clone https://github.com/KobenjiSan/jinjamonogatari-api.git
cd jinjamonogatari-api
```

2) Configure environment

Create an `.env` (or update `appsettings.Development.json`) with your local connection string and JWT settings.

Typical values you will need:
- Database connection string (PostgreSQL)
- JWT settings (issuer, audience, signing key)
- Token options (access token TTL, refresh token TTL)

3) Apply migrations

```bash
dotnet ef database update
```

4) Run the API

```bash
dotnet run --project src/Api
```

5) Open Swagger

Once running, use the Swagger UI to explore endpoints.

Notes:
- Local bootstrapping uses a development seeder (`DevDataSeeder`) to ensure the API has data to serve during early integration.
- If you change domain entities/configurations, create a new migration and re-apply.

## 🔐 Authentication / Security

- Auth is token-based:
  - Short-lived JWT access tokens for API requests
  - Refresh tokens for session continuation without re-authentication
- User identity is derived from JWT claims via `ClaimsPrincipalExtensions`, not from client-provided identifiers.
- Passwords are stored as hashes and handled through a dedicated password hashing service.

## 🧩 Key Engineering Decisions

- **Read models aligned to client needs**: separate DTOs for map points, list cards, previews, and tab-specific detail reduces over-fetching and keeps responses small and predictable.
- **PostGIS for location-aware behavior**: distance and spatial filtering are database responsibilities; this keeps logic consistent across clients and avoids duplicated math.
- **Service layer boundaries**: handlers orchestrate use cases; services own persistence. This avoids “EF Core inside handlers” and keeps refactors tractable.
- **Consistent timestamps**: timestamp fields are handled centrally via DbContext behavior so entities stay correct without manual repetition.
- **Exception middleware as a contract**: a single place to translate exceptions into HTTP responses reduces controller branching and makes error behavior uniform.

## 📈 Future Improvements

- Bounding-box search and richer filtering on shrine list/map endpoints
- Standardized response envelopes for paging and metadata (where needed)
- Expanded authorization rules (admin/editor roles for future CMS integration)
- Observability: structured logging, request tracing, and metrics
- Harden refresh-token rotation edge cases (reuse detection, token family invalidation)

## 🧪 Testing

- Automated test coverage is not yet established.
- Current validation is integration-focused:
  - Swagger-driven endpoint verification
  - Postman validation for auth/session flows
  - Mobile-client integration for shrine discovery, saved state, and refresh behavior

## 🚢 Deployment

- Deployment is not finalized in this repository.
- Intended path:
  - Containerized API deployment
  - Managed Postgres + PostGIS
  - Environment-based configuration for JWT/token settings and connection strings

## 👤 Author

Samuel Keller  
B.S. Information Technology (Software Development + Digital Media) — Georgia Gwinnett College
