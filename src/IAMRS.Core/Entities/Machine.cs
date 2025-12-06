using IAMRS.Core.Common;
using IAMRS.Core.Enums;

namespace IAMRS.Core.Entities;

/// <summary>
/// Represents an industrial machine being monitored by the system.
/// </summary>
public class Machine : BaseEntity
{
    /// <summary>
    /// Unique machine identifier (e.g., "MX-01", "CNC-003").
    /// </summary>
    public string MachineCode { get; set; } = string.Empty;

    /// <summary>
    /// Display name of the machine.
    /// </summary>
    public string Name { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the machine.
    /// </summary>
    public string? Description { get; set; }

    /// <summary>
    /// Type/category of the machine.
    /// </summary>
    public MachineType Type { get; set; }

    /// <summary>
    /// Physical location of the machine (e.g., "Building A, Floor 2").
    /// </summary>
    public string Location { get; set; } = string.Empty;

    /// <summary>
    /// Current operational status.
    /// </summary>
    public MachineStatus Status { get; set; } = MachineStatus.Offline;

    /// <summary>
    /// Manufacturer of the machine.
    /// </summary>
    public string? Manufacturer { get; set; }

    /// <summary>
    /// Model number/name.
    /// </summary>
    public string? Model { get; set; }

    /// <summary>
    /// Serial number for the machine.
    /// </summary>
    public string? SerialNumber { get; set; }

    /// <summary>
    /// Date when the machine was installed.
    /// </summary>
    public DateTime? InstallDate { get; set; }

    /// <summary>
    /// Date of last maintenance performed.
    /// </summary>
    public DateTime? LastMaintenanceDate { get; set; }

    /// <summary>
    /// Date of next scheduled maintenance.
    /// </summary>
    public DateTime? NextMaintenanceDate { get; set; }

    /// <summary>
    /// Timestamp of last telemetry received.
    /// </summary>
    public DateTime? LastTelemetryAt { get; set; }

    /// <summary>
    /// Warning temperature threshold in Celsius.
    /// </summary>
    public double TemperatureWarningThreshold { get; set; } = 80.0;

    /// <summary>
    /// Critical temperature threshold in Celsius.
    /// </summary>
    public double TemperatureCriticalThreshold { get; set; } = 90.0;

    /// <summary>
    /// Maximum acceptable vibration level in mm/s.
    /// </summary>
    public double VibrationThreshold { get; set; } = 10.0;

    /// <summary>
    /// Whether the machine is actively being monitored.
    /// </summary>
    public bool IsMonitored { get; set; } = true;

    // Navigation properties
    /// <summary>
    /// Collection of telemetry data points for this machine.
    /// </summary>
    public virtual ICollection<TelemetryData> TelemetryData { get; set; } = new List<TelemetryData>();

    /// <summary>
    /// Collection of alerts generated for this machine.
    /// </summary>
    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();

    /// <summary>
    /// Collection of maintenance tickets for this machine.
    /// </summary>
    public virtual ICollection<MaintenanceTicket> MaintenanceTickets { get; set; } = new List<MaintenanceTicket>();
}
