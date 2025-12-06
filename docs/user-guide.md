# IAMRS User Guide

## Introduction

The Industrial Asset Monitoring & Reporting System (IAMRS) provides real-time monitoring of industrial machinery, automatic anomaly detection, alerting, and maintenance management.

## Accessing the System

### Web Dashboard
Navigate to: `http://localhost:5001`

### API (for integrations)
Base URL: `http://localhost:5000/api`
Swagger Documentation: `http://localhost:5000/swagger`

## Dashboard Overview

The main dashboard displays:

1. **Summary Cards** - Quick statistics showing:
   - Total number of machines
   - Online machines (green)
   - Machines in warning state (yellow)
   - Machines in critical state (red)

2. **Machine Status Grid** - Visual cards for each machine showing:
   - Machine code and name
   - Location
   - Current status badge

3. **Recent Alerts** - Latest system alerts with severity indicators

## Managing Machines

### Viewing Machines

1. Click **Machines** in the navigation menu
2. View the list of all registered machines
3. Click any row to see detailed information

### Machine Details

The detail page shows:
- Machine specifications (type, manufacturer, model)
- Current thresholds (temperature warning/critical, vibration)
- Recent telemetry data table
- Telemetry trend chart
- Machine-specific alerts

## Understanding Alerts

### Alert Severity Levels

| Level | Color | Description |
|-------|-------|-------------|
| **Critical** | Red | Immediate attention required |
| **Warning** | Yellow | Attention recommended |
| **Info** | Blue | Informational only |

### Alert Types

- **High Temperature** - Temperature exceeds configured thresholds
- **Abnormal Vibration** - Vibration levels outside normal range
- **Machine Offline** - No telemetry received for configured timeout period
- **Predictive Maintenance** - Statistical anomaly detected

### Acknowledging Alerts

1. Go to **Alerts** page
2. Find the alert you want to acknowledge
3. Click **Acknowledge** button
4. Optionally add notes about the action taken

## Maintenance Management

### Creating a Maintenance Ticket

1. Go to **Maintenance** page
2. Click **+ Create Ticket**
3. Fill in the form:
   - Select the affected machine
   - Choose priority level
   - Enter title and description
4. Click **Save**

### Ticket Priority Levels

| Priority | Response Time |
|----------|---------------|
| **Urgent** | Immediate |
| **High** | Within hours |
| **Medium** | Within days |
| **Low** | When convenient |

### Ticket Status Workflow

```
Open → In Progress → Resolved → Closed
                  ↘ Cancelled
```

## Telemetry Data

### Data Collection

Telemetry is collected from machines and includes:
- Temperature (°C)
- Vibration (mm/s)
- Pressure (bar)
- Additional sensor data as configured

### Automatic Alerting

The system automatically generates alerts when:
1. **Temperature > 90°C** → Critical alert immediately
2. **Temperature > 80°C for 3+ readings** → Warning alert
3. **No data for 5 minutes** → Offline warning
4. **Statistical anomaly (>3σ deviation)** → Predictive maintenance alert

## Best Practices

### Daily Monitoring
- Check dashboard for any critical or warning states
- Review unacknowledged alerts
- Verify all monitored machines are online

### Responding to Alerts
1. **Critical**: Investigate immediately
2. **Warning**: Plan investigation within shift
3. **Predictive**: Schedule preventive maintenance

### Maintenance Planning
- Create tickets for all planned maintenance
- Update ticket status as work progresses
- Document resolution in ticket notes

## Keyboard Shortcuts

| Shortcut | Action |
|----------|--------|
| `D` | Go to Dashboard |
| `M` | Go to Machines |
| `A` | Go to Alerts |
| `T` | Go to Maintenance (Tickets) |

## Getting Help

### API Documentation
Access full API documentation at `/swagger` for integration needs.

### Technical Support
Contact your system administrator for:
- User account issues
- Configuration changes
- System errors

### Reporting Bugs
Include the following information:
- Steps to reproduce
- Expected vs actual behavior
- Browser and OS information
- Any error messages displayed
