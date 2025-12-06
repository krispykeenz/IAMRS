# Entity Relationship Diagram

## Mermaid ERD

```mermaid
erDiagram
    Machine ||--o{ TelemetryData : "generates"
    Machine ||--o{ Alert : "triggers"
    Machine ||--o{ MaintenanceTicket : "requires"
    MaintenanceTicket ||--o{ Alert : "resolves"
    
    Machine {
        guid Id PK
        string MachineCode UK
        string Name
        string Description
        int Type
        string Location
        int Status
        string Manufacturer
        string Model
        string SerialNumber
        datetime InstallDate
        datetime LastMaintenanceDate
        datetime NextMaintenanceDate
        datetime LastTelemetryAt
        double TemperatureWarningThreshold
        double TemperatureCriticalThreshold
        double VibrationThreshold
        bool IsMonitored
        datetime CreatedAt
        datetime UpdatedAt
        bool IsDeleted
    }
    
    TelemetryData {
        guid Id PK
        guid MachineId FK
        double Temperature
        double Vibration
        double Pressure
        double Humidity
        double Current
        double Rpm
        double PowerConsumption
        datetime Timestamp
        int DataQuality
        string Metadata
        datetime CreatedAt
    }
    
    Alert {
        guid Id PK
        guid MachineId FK
        int Type
        int Severity
        string Message
        string Details
        double TriggerValue
        double ThresholdValue
        bool IsAcknowledged
        datetime AcknowledgedAt
        string AcknowledgedBy
        string AcknowledgementNotes
        bool IsResolved
        datetime ResolvedAt
        guid MaintenanceTicketId FK
        datetime CreatedAt
    }
    
    MaintenanceTicket {
        guid Id PK
        string TicketNumber UK
        guid MachineId FK
        string Title
        string Description
        int Priority
        int Status
        string MaintenanceType
        string CreatedBy
        string AssignedTo
        datetime StartedAt
        datetime CompletedAt
        double EstimatedHours
        double ActualHours
        decimal EstimatedCost
        decimal ActualCost
        string PartsUsed
        string ResolutionNotes
        datetime DueDate
        datetime CreatedAt
    }
    
    AuditLog {
        guid Id PK
        string UserId
        string UserName
        string Action
        string EntityType
        string EntityId
        string OldValues
        string NewValues
        string IpAddress
        string UserAgent
        string Details
        datetime Timestamp
    }
    
    AspNetUsers {
        string Id PK
        string UserName
        string Email
        string PasswordHash
        string FirstName
        string LastName
        string JobTitle
        string Department
        bool IsActive
        datetime LastLoginAt
    }
    
    AspNetRoles {
        string Id PK
        string Name
    }
    
    AspNetUserRoles {
        string UserId FK
        string RoleId FK
    }
```

## Table Descriptions

### Machines
Stores information about industrial machines being monitored. Each machine has configurable thresholds for alerting.

### TelemetryData
Time-series data collected from machine sensors. Indexed on (MachineId, Timestamp) for efficient range queries.

### Alerts
System-generated notifications when anomalies are detected. Supports acknowledgment workflow and linking to maintenance tickets.

### MaintenanceTickets
Work orders for machine maintenance. Tracks the full lifecycle from creation to completion.

### AuditLogs
Immutable record of all system changes for compliance and debugging.

### ASP.NET Identity Tables
Standard Identity tables for user authentication and role-based authorization.

## Indexes

| Table | Index | Columns |
|-------|-------|---------|
| Machines | IX_Machines_MachineCode | MachineCode (Unique) |
| TelemetryData | IX_TelemetryData_MachineId_Timestamp | MachineId, Timestamp |
| Alerts | IX_Alerts_MachineId_Severity_CreatedAt | MachineId, Severity, CreatedAt |
| MaintenanceTickets | IX_MaintenanceTickets_TicketNumber | TicketNumber (Unique) |
| AuditLogs | IX_AuditLogs_Timestamp | Timestamp |
