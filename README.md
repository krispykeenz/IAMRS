# Industrial Asset Monitoring & Reporting System (IAMRS)

A full-stack .NET 8 solution for industrial machine telemetry monitoring, anomaly detection, alerting, and maintenance management.

## 🏗️ Architecture

```
┌─────────────────────────────────────────────────────────────────────┐
│                         Presentation Layer                          │
│  ┌─────────────────┐  ┌─────────────────┐  ┌─────────────────────┐ │
│  │   IAMRS.Web     │  │   IAMRS.Api     │  │  IAMRS.Simulator    │ │
│  │  Blazor Server  │  │  REST API       │  │  Console App        │ │
│  └────────┬────────┘  └────────┬────────┘  └──────────┬──────────┘ │
└───────────┼────────────────────┼────────────────────────┼───────────┘
            │                    │                        │
┌───────────┴────────────────────┴────────────────────────┴───────────┐
│                        Application Layer                            │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │                    IAMRS.Application                         │   │
│  │  Services │ DTOs │ Validators │ Background Workers │ CQRS   │   │
│  └─────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────┬───────────────────────────────────┘
                                  │
┌─────────────────────────────────┴───────────────────────────────────┐
│                       Infrastructure Layer                          │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │                   IAMRS.Infrastructure                       │   │
│  │   DbContext │ Repositories │ EF Core │ Identity             │   │
│  └─────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────┬───────────────────────────────────┘
                                  │
┌─────────────────────────────────┴───────────────────────────────────┐
│                          Domain Layer                               │
│  ┌─────────────────────────────────────────────────────────────┐   │
│  │                       IAMRS.Core                             │   │
│  │          Entities │ Enums │ Interfaces │ Events             │   │
│  └─────────────────────────────────────────────────────────────┘   │
└─────────────────────────────────────────────────────────────────────┘
                                  │
                          ┌───────┴───────┐
                          │  SQL Server   │
                          │   Database    │
                          └───────────────┘
```

## 🚀 Quick Start

### Prerequisites
- .NET 8 SDK
- Docker Desktop
- Node.js (for frontend tooling)

### 1. Start SQL Server
```bash
docker-compose up -d sqlserver
```

### 2. Apply Database Migrations
```bash
cd src/IAMRS.Api
dotnet ef database update
```

### 3. Run the API
```bash
cd src/IAMRS.Api
dotnet run
```
API available at: `https://localhost:5001` | Swagger: `https://localhost:5001/swagger`

### 4. Run the Web Dashboard
```bash
cd src/IAMRS.Web
dotnet run
```
Dashboard available at: `https://localhost:5002`

### 5. Run the Telemetry Simulator
```bash
cd src/IAMRS.Simulator
dotnet run -- --machines 5 --interval 2000
```

## 📊 Features

### Machine Monitoring
- Real-time telemetry ingestion (temperature, vibration, pressure)
- Machine status tracking (Online, Offline, Warning, Critical)
- Historical data visualization

### Anomaly Detection
- Temperature threshold monitoring (80°C warning, 90°C critical)
- Consecutive reading analysis for trend detection
- Machine offline detection

### Alerting System
- Automated alert generation
- Severity levels (Info, Warning, Critical)
- Alert acknowledgment workflow

### Maintenance Management
- Ticket creation and assignment
- Priority-based tracking
- Status workflow (Open → In Progress → Resolved → Closed)

### Security
- ASP.NET Identity authentication
- Role-based authorization (Admin, Operator, Viewer)
- Audit logging

## 🧪 Testing

```bash
cd src/IAMRS.Tests
dotnet test
```

## 📁 Project Structure

```
IAMRS/
├── src/
│   ├── IAMRS.Api/              # REST API endpoints
│   ├── IAMRS.Web/              # Blazor Server dashboard
│   ├── IAMRS.Core/             # Domain entities and interfaces
│   ├── IAMRS.Infrastructure/   # Data access and repositories
│   ├── IAMRS.Application/      # Business logic and services
│   ├── IAMRS.Simulator/        # Telemetry data generator
│   └── IAMRS.Tests/            # Unit and integration tests
├── docs/                       # Documentation
├── design/                     # Architecture diagrams
└── docker-compose.yml          # Container orchestration
```

## 🔧 Configuration

### Connection String
Set in `appsettings.json` or environment variable:
```json
{
  "ConnectionStrings": {
    "DefaultConnection": "Server=localhost;Database=IAMRS;User Id=sa;Password=YourStrong@Passw0rd;TrustServerCertificate=True"
  }
}
```

### Alert Thresholds
```json
{
  "AlertSettings": {
    "TemperatureWarningThreshold": 80,
    "TemperatureCriticalThreshold": 90,
    "ConsecutiveReadingsForWarning": 3,
    "OfflineTimeoutMinutes": 5
  }
}
```

## 📄 API Endpoints

| Method | Endpoint | Description |
|--------|----------|-------------|
| GET | /api/machines | List all machines |
| GET | /api/machines/{id} | Get machine details |
| POST | /api/machines | Create machine |
| PUT | /api/machines/{id} | Update machine |
| DELETE | /api/machines/{id} | Delete machine |
| POST | /api/telemetry | Ingest telemetry data |
| GET | /api/telemetry/{machineId} | Get telemetry history |
| GET | /api/alerts | List alerts |
| PUT | /api/alerts/{id}/acknowledge | Acknowledge alert |
| GET | /api/maintenance | List maintenance tickets |
| POST | /api/maintenance | Create ticket |
| PUT | /api/maintenance/{id} | Update ticket |

## 📜 License

MIT License - See LICENSE file for details.

## 👥 Contributing

1. Fork the repository
2. Create a feature branch (`git checkout -b feature/amazing-feature`)
3. Commit changes (`git commit -m 'Add amazing feature'`)
4. Push to branch (`git push origin feature/amazing-feature`)
5. Open a Pull Request
