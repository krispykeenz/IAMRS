# IAMRS Architecture Document

## Overview

The Industrial Asset Monitoring & Reporting System (IAMRS) is a full-stack .NET 8 solution designed for monitoring industrial machinery, detecting anomalies, generating alerts, and managing maintenance workflows.

## System Architecture

```
┌─────────────────────────────────────────────────────────────────────────┐
│                           External Clients                               │
│     ┌─────────────┐    ┌─────────────┐    ┌─────────────────────────┐  │
│     │   Blazor    │    │  REST API   │    │  Telemetry Simulator    │  │
│     │  Dashboard  │    │   Clients   │    │  (Console App)          │  │
│     └──────┬──────┘    └──────┬──────┘    └───────────┬─────────────┘  │
└────────────┼─────────────────────┼────────────────────┼─────────────────┘
             │                     │                    │
             ▼                     ▼                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                         Presentation Layer                               │
│  ┌───────────────────────────┐  ┌───────────────────────────────────┐  │
│  │      IAMRS.Web            │  │         IAMRS.Api                 │  │
│  │   (Blazor Server)         │  │       (ASP.NET Core)              │  │
│  │   - Dashboard             │  │   - REST Controllers             │  │
│  │   - Machine Views         │  │   - Swagger/OpenAPI              │  │
│  │   - Alert Management      │  │   - Authentication               │  │
│  │   - Maintenance Module    │  │   - Request Logging              │  │
│  └───────────────────────────┘  └───────────────────────────────────┘  │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                         Application Layer                                │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                    IAMRS.Application                             │   │
│  │  ┌─────────────┐ ┌─────────────┐ ┌─────────────────────────────┐│   │
│  │  │   DTOs      │ │  Services   │ │   Background Workers        ││   │
│  │  │  Mappings   │ │  Validators │ │  - TelemetryHealthMonitor   ││   │
│  │  │  (AutoMap)  │ │  (Fluent)   │ │  - PredictiveProcessor      ││   │
│  │  └─────────────┘ └─────────────┘ └─────────────────────────────┘│   │
│  └─────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                        Infrastructure Layer                              │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                   IAMRS.Infrastructure                           │   │
│  │  ┌─────────────┐ ┌─────────────┐ ┌───────────────────────────┐ │   │
│  │  │  DbContext  │ │ Repositories│ │    ASP.NET Identity       │ │   │
│  │  │  (EF Core)  │ │ Unit of Work│ │    (Users, Roles)         │ │   │
│  │  └─────────────┘ └─────────────┘ └───────────────────────────┘ │   │
│  └─────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
┌─────────────────────────────────────────────────────────────────────────┐
│                           Domain Layer                                   │
│  ┌─────────────────────────────────────────────────────────────────┐   │
│  │                       IAMRS.Core                                 │   │
│  │  ┌─────────────┐ ┌─────────────┐ ┌───────────────────────────┐ │   │
│  │  │  Entities   │ │    Enums    │ │       Interfaces          │ │   │
│  │  │  Machine    │ │ MachineStatus│ │   IRepository<T>          │ │   │
│  │  │  Telemetry  │ │ AlertSeverity│ │   IUnitOfWork             │ │   │
│  │  │  Alert      │ │ TicketStatus │ │   IMachineRepository      │ │   │
│  │  │  Ticket     │ │              │ │                           │ │   │
│  │  └─────────────┘ └─────────────┘ └───────────────────────────┘ │   │
│  └─────────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────────┘
                                    │
                                    ▼
                         ┌───────────────────┐
                         │    SQL Server     │
                         │    (Docker)       │
                         └───────────────────┘
```

## Design Patterns

### Repository Pattern
Abstracts data access behind interfaces, enabling:
- Testability through mocking
- Consistent data access API
- Separation of concerns

### Unit of Work Pattern
Manages transactions across multiple repositories:
- Atomic operations
- Transaction management
- Single SaveChanges call

### Clean Architecture
Dependency flow points inward:
- Core has no external dependencies
- Infrastructure depends on Core
- Application depends on Core and Infrastructure
- Presentation depends on Application

## Key Components

### Background Services

**TelemetryHealthMonitor**
- Runs every 30 seconds
- Detects machines without recent telemetry
- Generates offline alerts
- Updates machine status

**PredictiveMaintenanceProcessor**
- Runs every minute
- Analyzes telemetry trends
- Computes rolling averages and standard deviation
- Generates predictive maintenance alerts

### Alert Generation Rules

| Condition | Alert Type | Severity |
|-----------|------------|----------|
| Temperature > 90°C | HighTemperature | Critical |
| Temperature > 80°C (3+ readings) | HighTemperature | Warning |
| No telemetry for 5 minutes | MachineOffline | Warning |
| Vibration > threshold | AbnormalVibration | Warning |
| Statistical anomaly (>3σ) | PredictiveMaintenance | Warning |

## Technology Stack

| Layer | Technology |
|-------|------------|
| Frontend | Blazor Server, Bootstrap 5 |
| API | ASP.NET Core 8, Swagger |
| Business Logic | C#, AutoMapper, FluentValidation |
| Data Access | Entity Framework Core 8 |
| Database | SQL Server 2022 |
| Authentication | ASP.NET Identity |
| Logging | Serilog |
| Container | Docker |

## Security Considerations

- SQL injection prevention via EF Core parameterized queries
- XSS protection through Blazor's built-in encoding
- HTTPS enforcement in production
- Role-based access control (Admin, Operator, Viewer)
- Audit logging for all data modifications
