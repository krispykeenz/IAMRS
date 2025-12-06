# IAMRS Deployment Guide

## Prerequisites

- .NET 8 SDK
- Docker Desktop
- SQL Server (or Docker container)

## Local Development Setup

### 1. Clone the Repository
```bash
git clone https://github.com/your-org/IAMRS.git
cd IAMRS
```

### 2. Start SQL Server
```bash
docker-compose up -d sqlserver
```

Wait for SQL Server to be ready (about 30 seconds):
```bash
docker logs iamrs-sqlserver
```

### 3. Apply Database Migrations
```bash
cd src/IAMRS.Api
dotnet ef database update
```

### 4. Run the API
```bash
cd src/IAMRS.Api
dotnet run --urls http://localhost:5000
```

API available at: http://localhost:5000
Swagger UI: http://localhost:5000/swagger

### 5. Run the Web Dashboard
```bash
cd src/IAMRS.Web
dotnet run --urls http://localhost:5001
```

Dashboard available at: http://localhost:5001

### 6. Run the Telemetry Simulator
```bash
cd src/IAMRS.Simulator
dotnet run -- http://localhost:5000 5 2000
```

Arguments: `<api_url> <machine_count> <interval_ms>`

## Docker Deployment

### Build and Run All Services
```bash
docker-compose --profile full up -d
```

This starts:
- SQL Server on port 1433
- API on port 5000
- Web on port 5001

### Build Individual Images
```bash
# API
docker build -t iamrs-api -f src/IAMRS.Api/Dockerfile .

# Web
docker build -t iamrs-web -f src/IAMRS.Web/Dockerfile .
```

## Production Deployment

### Environment Variables

| Variable | Description | Example |
|----------|-------------|---------|
| `ConnectionStrings__DefaultConnection` | SQL Server connection string | `Server=...` |
| `ASPNETCORE_ENVIRONMENT` | Environment name | `Production` |
| `Jwt__Key` | JWT signing key (min 32 chars) | `YourSecureKey...` |
| `Jwt__Issuer` | JWT issuer | `IAMRS` |
| `Jwt__Audience` | JWT audience | `IAMRS-Clients` |

### Azure App Service

1. Create Azure SQL Database
2. Create App Service Plan (Linux, .NET 8)
3. Create two App Services (API and Web)
4. Configure connection strings in App Service Configuration
5. Deploy using GitHub Actions or Azure DevOps

### Kubernetes

Sample deployment manifest:
```yaml
apiVersion: apps/v1
kind: Deployment
metadata:
  name: iamrs-api
spec:
  replicas: 2
  selector:
    matchLabels:
      app: iamrs-api
  template:
    metadata:
      labels:
        app: iamrs-api
    spec:
      containers:
      - name: api
        image: your-registry/iamrs-api:latest
        ports:
        - containerPort: 8080
        env:
        - name: ConnectionStrings__DefaultConnection
          valueFrom:
            secretKeyRef:
              name: iamrs-secrets
              key: db-connection
        - name: ASPNETCORE_ENVIRONMENT
          value: "Production"
```

## Database Management

### Create New Migration
```bash
cd src/IAMRS.Api
dotnet ef migrations add MigrationName -p ../IAMRS.Infrastructure -s .
```

### Apply Migrations
```bash
dotnet ef database update -p ../IAMRS.Infrastructure -s .
```

### Generate SQL Script
```bash
dotnet ef migrations script -p ../IAMRS.Infrastructure -s . -o migration.sql
```

## Health Checks

### API Health
```bash
curl http://localhost:5000/health
```

### Database Connectivity
The API will fail to start if database connection fails.

## Logging

Logs are written to:
- Console (all environments)
- File: `../logs/api-{date}.log`

Configure log levels in `appsettings.json`:
```json
{
  "Serilog": {
    "MinimumLevel": {
      "Default": "Information",
      "Override": {
        "Microsoft": "Warning"
      }
    }
  }
}
```

## Troubleshooting

### SQL Server Connection Refused
```bash
# Check if container is running
docker ps

# Check container logs
docker logs iamrs-sqlserver

# Verify port is open
nc -zv localhost 1433
```

### EF Core Migration Errors
```bash
# Ensure tools are installed
dotnet tool install --global dotnet-ef

# Rebuild solution
dotnet build
```

### Background Services Not Running
Check application logs for startup errors. Background services log to the same Serilog sinks.

## Performance Tuning

### Database Indexes
Indexes are created by migrations. Verify with:
```sql
SELECT * FROM sys.indexes WHERE object_id = OBJECT_ID('TelemetryData')
```

### Telemetry Data Retention
Consider implementing a cleanup job to purge old telemetry data:
```sql
DELETE FROM TelemetryData WHERE Timestamp < DATEADD(day, -90, GETUTCDATE())
```

### Connection Pooling
SQL Server connection pooling is enabled by default. Monitor with:
```sql
SELECT * FROM sys.dm_exec_connections
```
