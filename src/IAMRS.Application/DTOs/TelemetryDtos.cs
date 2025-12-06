namespace IAMRS.Application.DTOs;

/// <summary>
/// DTO for telemetry ingestion.
/// </summary>
public class TelemetryIngestDto
{
    public string MachineId { get; set; } = string.Empty; // Supports code or GUID via API mapping
    public double? Temperature { get; set; }
    public double? Vibration { get; set; }
    public double? Pressure { get; set; }
    public DateTime Timestamp { get; set; }
    public int? DataQuality { get; set; }
    public string? Metadata { get; set; }
}

/// <summary>
/// DTO for telemetry response.
/// </summary>
public class TelemetryDto
{
    public Guid Id { get; set; }
    public Guid MachineId { get; set; }
    public double? Temperature { get; set; }
    public double? Vibration { get; set; }
    public double? Pressure { get; set; }
    public DateTime Timestamp { get; set; }
}
