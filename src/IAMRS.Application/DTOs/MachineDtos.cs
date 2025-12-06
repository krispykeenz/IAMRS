using IAMRS.Core.Enums;

namespace IAMRS.Application.DTOs;

/// <summary>
/// Data transfer object for machine summary.
/// </summary>
public record MachineSummaryDto(
    Guid Id,
    string MachineCode,
    string Name,
    MachineType Type,
    string Location,
    MachineStatus Status,
    DateTime? LastTelemetryAt
);

/// <summary>
/// Data transfer object for machine details.
/// </summary>
public class MachineDetailDto
{
    public Guid Id { get; set; }
    public string MachineCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public MachineType Type { get; set; }
    public string Location { get; set; } = string.Empty;
    public MachineStatus Status { get; set; }
    public string? Manufacturer { get; set; }
    public string? Model { get; set; }
    public string? SerialNumber { get; set; }
    public DateTime? InstallDate { get; set; }
    public DateTime? LastMaintenanceDate { get; set; }
    public DateTime? NextMaintenanceDate { get; set; }
    public DateTime? LastTelemetryAt { get; set; }

    public double TemperatureWarningThreshold { get; set; }
    public double TemperatureCriticalThreshold { get; set; }
    public double VibrationThreshold { get; set; }
}

/// <summary>
/// Data transfer object for creating/updating a machine.
/// </summary>
public class MachineUpsertDto
{
    public string MachineCode { get; set; } = string.Empty;
    public string Name { get; set; } = string.Empty;
    public string? Description { get; set; }
    public MachineType Type { get; set; }
    public string Location { get; set; } = string.Empty;
    public double TemperatureWarningThreshold { get; set; } = 80;
    public double TemperatureCriticalThreshold { get; set; } = 90;
    public double VibrationThreshold { get; set; } = 10;
    public bool IsMonitored { get; set; } = true;
}
