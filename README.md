# 🏛️ Huwiyati (هويتي) - Digital Identity & Civil Services Platform

[![.NET 9.0](https://img.shields.io/badge/.NET-9.0-512BD4?logo=dotnet&logoColor=white)](https://dotnet.microsoft.com/)
[![Architecture](https://img.shields.io/badge/Architecture-Clean%20Architecture-blue.svg)](https://learn.microsoft.com/en-us/dotnet/architecture/modern-web-apps-azure/common-web-application-architectures#clean-architecture)
[![Entity Framework Core](https://img.shields.io/badge/EF%20Core-9.0-purple.svg)](https://learn.microsoft.com/en-us/ef/core/)
[![License](https://img.shields.io/badge/License-Proprietary-red.svg)](#license)

**Huwiyati (هويتي)** is a sovereign national digital identity and unified civil registry platform engineered with **.NET 9** following the principles of **Clean Architecture**, **Domain-Driven Design (DDD)**, and enterprise-grade security standards.

The platform provides digitized identity management, multi-role governance (Citizens, Staff, Branch Admins, and SuperAdmins), physical card issuance (National ID, Family Card, Passport, Birth/Death Certificates), and auditable civil service workflows.

---

## 🌟 Key Capabilities & Features

### 1. 🔐 Identity & Authentication (Zero-Trust Security)
- **National Number Sovereign Identity**: Accounts strictly tethered to civil registry records (`Person`).
- **Device Trust & Fingerprinting**: Automated detection of unrecognized hardware with mandatory OTP challenges.
- **Stateless Two-Factor Email/Phone Lifecycle**:
  - Unverified registration typo correction (safe account re-binding).
  - Secure self-service email and phone updates via dual-phase challenge (`request` & `confirm`).
- **Auditable Role-Based Access Control (RBAC)**: Distinct permissions for `Citizen`, `Employee`, `Admin`, and `SuperAdmin`.
- **Administrative SuperAdmin Role Governance**: Secure role handover with dual-step cryptographic token/OTP verification.

### 2. 🗂️ Civil Registry & Document Management
- **National Identification Card**: Issue and renewal with biometric photo capture, occupation, and blood type tracking.
- **Family Status Record (Family Card)**: Marriage recording, multiple wives support, dependent child additions, and renewals.
- **Digital Passport & Border Records**: Passport issuance/renewal and entry/exit travel logging.
- **Vital Statistics (Birth & Death)**: Digital birth certificate provisioning (with maternal mortality tolerance) and automated system-wide deceased status propagation upon death record registration.

### 3. 🏢 Sovereign Organization & Branch Hierarchy
- **Branch-Level Isolation**: One active administrative manager per physical branch enforcement.
- **Dynamic Routing**: Public lookup of civil authority branches and healthcare facilities for citizen service fulfillment.

---

## 🏗️ Architecture & Project Structure

Huwiyati adheres strictly to the four-layer **Clean Architecture**:

```
Huwiyati/
├── 📁 Huwiyati.Domain            # Enterprise Core (Entities, Enums, Value Objects, Domain Events)
├── 📁 Huwiyati.Application       # Business Logic (Use Cases, CQRS-style Handlers, DTOs, Validators)
├── 📁 Huwiyati.Infrastructure    # External Concerns (EF Core, SQL Server, ASP.NET Core Identity, Email)
├── 📁 Huwiyati.API               # Presentation Layer (Controllers, Middlewares, Filters, OpenAPI)
└── 📁 Docs                       # Comprehensive Architectural Plans & Design Documents
```

### Dependency Graph (Clean Architecture Rule)
```
      [ Huwiyati.API ]
             │
             ▼
   [ Huwiyati.Infrastructure ] ──┐
             │                   │
             ▼                   ▼
    [ Huwiyati.Application ] ────┘
             │
             ▼
       [ Huwiyati.Domain ]
```

---

## 🛠️ Technology Stack

| Layer | Technologies & Libraries |
|---|---|
| **Framework** | .NET 9 (C# 13) |
| **API** | ASP.NET Core Web API, RESTful, OpenAPI / Swagger |
| **Data Access** | Entity Framework Core 9, SQL Server, Fluent Configuration |
| **Security & Auth** | ASP.NET Core Identity, JWT (JSON Web Tokens), BCrypt / PBKDF2 |
| **Validation** | FluentValidation with Automatic Pipeline Filters |
| **Architecture** | Clean Architecture, Repository/Service Abstractions, Domain Events |

---

## 🚀 Getting Started

### Prerequisites
- [.NET 9.0 SDK](https://dotnet.microsoft.com/download/dotnet/9.0)
- [Microsoft SQL Server](https://www.microsoft.com/en-us/sql-server/) (or LocalDB / Docker container)
- [Visual Studio 2022](https://visualstudio.microsoft.com/) (v17.12+) or [VS Code](https://code.visualstudio.com/) with C# Dev Kit

### Installation & Setup

1. **Clone the Repository**
   ```bash
   git clone https://github.com/YourUsername/Huwiyati.git
   cd Huwiyati
   ```

2. **Configure Database Connection**
   Open `Huwiyati.API/appsettings.json` and adjust the connection string:
   ```json
   "ConnectionStrings": {
     "DefaultConnection": "Server=localhost;Database=HuwiyatiDb;Trusted_Connection=True;TrustServerCertificate=True;"
   }
   ```

3. **Apply Database Migrations**
   ```bash
   dotnet ef database update --project Huwiyati.Infrastructure --startup-project Huwiyati.API
   ```

4. **Run the Application**
   ```bash
   dotnet run --project Huwiyati.API
   ```
   Navigate to `https://localhost:5001/swagger` to explore the interactive API documentation.

---

## 📡 API Endpoints Overview

### 🔑 Authentication (`api/v1/auth`)
- `POST /api/v1/auth/register` - Register a new citizen account with civil registry verification.
- `POST /api/v1/auth/verify-otp` - Verify activation code for a newly registered citizen account.
- `POST /api/v1/auth/login` - Authenticate citizen, employee, admin, or superadmin credentials.
- `POST /api/v1/auth/verify-device` - Verify unrecognized hardware device via OTP challenge.
- `POST /api/v1/auth/refresh-token` - Renew session token for administrative staff.

### 👤 Profile & Self-Service (`api/v1/profile`)
- `POST /api/v1/profile/change-email/request` - Initiate email update challenge.
- `POST /api/v1/profile/change-email/confirm` - Confirm email update with received OTP.
- `POST /api/v1/profile/change-phone/request` - Initiate phone update challenge.
- `POST /api/v1/profile/change-phone/confirm` - Confirm phone update with received OTP.
- `POST /api/v1/profile/deactivate-account` - Self-deactivate account.
- `POST /api/v1/profile/reactivate-account` - Reactivate deactivated account via OTP.
- `POST /api/v1/profile/remove-device` - Revoke trusted client device.

### 🏢 Organizations & Branches (`api/v1/branches`)
- `GET /api/v1/branches/organizations` - Retrieve all main government ministries & authorities.
- `GET /api/v1/branches` - Query active regional offices and branches filtered by authority.
- `POST /api/v1/branches` - Provision a new branch (SuperAdmin only).
- `PUT /api/v1/branches/{id}` - Update branch attributes (SuperAdmin only).
- `DELETE /api/v1/branches/{id}` - Soft-delete a branch (SuperAdmin only).

### 👥 Staff & User Management (`api/v1/users-management`)
- `GET /api/v1/users-management/citizen-status/{nationalNumber}` - Check online onboarding status.
- `POST /api/v1/users-management/activate-citizen` - In-branch staff activation of citizen accounts.
- `GET /api/v1/users-management/all-users` - Platform-wide user catalog audit (SuperAdmin only).

---

## 🔒 Security Best Practices Implemented
- **Data Protection**: Strict encapsulation of Domain entities; external modification restricted through dedicated use cases.
- **Deceased Citizen Protection**: Systematic rejection of any issuance, registration, renewal, or activation against deceased individuals.
- **Clean Separation of Concerns**: Strict decoupled modules adhering to the Single Responsibility Principle (SRP).

---

## 📄 License
This software and its documentation are proprietary and confidential. Unauthorized copying, distribution, modification, or commercial exploitation is strictly prohibited.
