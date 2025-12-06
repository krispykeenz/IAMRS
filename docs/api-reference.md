# IAMRS API Reference

Base URL: `http://localhost:5000/api`

## Authentication

The API uses JWT Bearer tokens for authentication. Include the token in the `Authorization` header:
```
Authorization: Bearer <token>
```

## Machines

### List All Machines
```http
GET /api/machines
```

**Response** `200 OK`
```json
[
  {
    "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
    "machineCode": "MX-001",
    "name": "CNC Machine 1",
    "type": 0,
    "location": "Building A, Floor 1",
    "status": 1,
    "lastTelemetryAt": "2024-12-06T10:30:00Z"
  }
]
```

### Get Machine by ID
```http
GET /api/machines/{id}
```

**Parameters**
| Name | Type | Description |
|------|------|-------------|
| id | guid | Machine unique identifier |

**Response** `200 OK`
```json
{
  "id": "3fa85f64-5717-4562-b3fc-2c963f66afa6",
  "machineCode": "MX-001",
  "name": "CNC Machine 1",
  "description": "5-axis CNC milling machine",
  "type": 0,
  "location": "Building A, Floor 1",
  "status": 1,
  "manufacturer": "Haas",
  "model": "VF-2",
  "serialNumber": "SN12345",
  "installDate": "2020-01-15",
  "lastMaintenanceDate": "2024-11-01",
  "nextMaintenanceDate": "2025-02-01",
  "lastTelemetryAt": "2024-12-06T10:30:00Z",
  "temperatureWarningThreshold": 80,
  "temperatureCriticalThreshold": 90,
  "vibrationThreshold": 10
}
```

### Create Machine
```http
POST /api/machines
```

**Request Body**
```json
{
  "machineCode": "MX-002",
  "name": "Robot Arm 1",
  "type": 1,
  "location": "Building B",
  "temperatureWarningThreshold": 75,
  "temperatureCriticalThreshold": 85,
  "vibrationThreshold": 8,
  "isMonitored": true
}
```

**Response** `201 Created`

### Update Machine
```http
PUT /api/machines/{id}
```

**Response** `204 No Content`

### Delete Machine
```http
DELETE /api/machines/{id}
```

**Response** `204 No Content`

---

## Telemetry

### Ingest Telemetry Data
```http
POST /api/telemetry
```

**Request Body**
```json
{
  "machineId": "MX-001",
  "temperature": 77.5,
  "vibration": 3.2,
  "pressure": 6.1,
  "timestamp": "2024-12-06T10:35:00Z"
}
```

Note: `machineId` can be either a GUID or the machine code string.

**Response** `200 OK`
```json
{
  "id": "a1b2c3d4-...",
  "machineId": "3fa85f64-...",
  "temperature": 77.5,
  "vibration": 3.2,
  "pressure": 6.1,
  "timestamp": "2024-12-06T10:35:00Z"
}
```

---

## Alerts

### List Alerts
```http
GET /api/alerts?machineId={machineId}
```

**Query Parameters**
| Name | Type | Required | Description |
|------|------|----------|-------------|
| machineId | guid | No | Filter by machine |

**Response** `200 OK`
```json
[
  {
    "id": "...",
    "machineId": "...",
    "type": 0,
    "severity": 2,
    "message": "Temperature CRITICAL: 95.0°C exceeds 90°C",
    "createdAt": "2024-12-06T10:36:00Z",
    "isAcknowledged": false,
    "acknowledgedAt": null
  }
]
```

### Acknowledge Alert
```http
PUT /api/alerts/{id}/acknowledge
```

**Request Body**
```json
{
  "notes": "Investigating issue"
}
```

**Response** `204 No Content`

---

## Maintenance

### List Maintenance Tickets
```http
GET /api/maintenance
```

**Response** `200 OK`
```json
[
  {
    "id": "...",
    "ticketNumber": "MT-20241206-103500",
    "machineId": "...",
    "title": "Replace cooling fan",
    "priority": 2,
    "status": 0,
    "createdAt": "2024-12-06T10:35:00Z"
  }
]
```

### Create Maintenance Ticket
```http
POST /api/maintenance
```

**Request Body**
```json
{
  "machineId": "3fa85f64-...",
  "title": "Scheduled maintenance",
  "description": "Quarterly inspection and lubrication",
  "priority": 1,
  "maintenanceType": "Preventive",
  "dueDate": "2024-12-15"
}
```

**Response** `201 Created`

### Update Maintenance Ticket
```http
PUT /api/maintenance/{id}
```

**Response** `204 No Content`

---

## Enums

### MachineType
| Value | Name |
|-------|------|
| 0 | CNC |
| 1 | Robot |
| 2 | Conveyor |
| 3 | Press |
| 4 | Pump |
| 5 | Compressor |
| 6 | Motor |
| 7 | Generator |
| 8 | HeatExchanger |
| 9 | Welder |
| 99 | Other |

### MachineStatus
| Value | Name |
|-------|------|
| 0 | Offline |
| 1 | Online |
| 2 | Warning |
| 3 | Critical |
| 4 | Maintenance |

### AlertSeverity
| Value | Name |
|-------|------|
| 0 | Info |
| 1 | Warning |
| 2 | Critical |

### AlertType
| Value | Name |
|-------|------|
| 0 | HighTemperature |
| 1 | AbnormalVibration |
| 2 | MachineOffline |
| 3 | PressureAnomaly |
| 4 | PredictiveMaintenance |
| 5 | ThresholdBreach |
| 6 | CommunicationFailure |
| 7 | SensorMalfunction |

### TicketPriority
| Value | Name |
|-------|------|
| 0 | Low |
| 1 | Medium |
| 2 | High |
| 3 | Urgent |

### TicketStatus
| Value | Name |
|-------|------|
| 0 | Open |
| 1 | InProgress |
| 2 | Resolved |
| 3 | Closed |
| 4 | Cancelled |

---

## Error Responses

### 400 Bad Request
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.1",
  "title": "One or more validation errors occurred.",
  "status": 400,
  "errors": {
    "MachineCode": ["The MachineCode field is required."]
  }
}
```

### 404 Not Found
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.5.4",
  "title": "Not Found",
  "status": 404
}
```

### 500 Internal Server Error
```json
{
  "type": "https://tools.ietf.org/html/rfc7231#section-6.6.1",
  "title": "An error occurred while processing your request.",
  "status": 500
}
```
