namespace IAMRS.Infrastructure.Configurations;

/// <summary>
/// Strongly-typed configuration for alert thresholds and monitoring settings.
/// </summary>
public class AlertSettings
{
    public double TemperatureWarningThreshold { get; set; } = 80.0;
    public double TemperatureCriticalThreshold { get; set; } = 90.0;
    public int ConsecutiveReadingsForWarning { get; set; } = 3;
    public int OfflineTimeoutMinutes { get; set; } = 5;
}
