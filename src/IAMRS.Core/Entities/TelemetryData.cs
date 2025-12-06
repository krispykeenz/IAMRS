using IAMRS.Core.Common;

namespace IAMRS.Core.Entities;

/// <summary>
/// Represents a telemetry data point collected from a machine sensor.
/// </summary>
public class TelemetryData : BaseEntity
{
    /// <summary>
    /// Foreign key to the associated machine.
    /// </summary>
    public Guid MachineId { get; set; }

    /// <summary>
    /// Temperature reading in Celsius.
    /// </summary>
    public double? Temperature { get; set; }

    /// <summary>
    /// Vibration reading in mm/s (RMS velocity).
    /// </summary>
    public double? Vibration { get; set; }

    /// <summary>
    /// Pressure reading in bar/PSI.
    /// </summary>
    public double? Pressure { get; set; }

    /// <summary>
    /// Humidity percentage (0-100).
    /// </summary>
    public double? Humidity { get; set; }

    /// <summary>
    /// Current draw in amperes.
    /// </summary>
    public double? Current { get; set; }

    /// <summary>
    /// Rotational speed in RPM.
    /// </summary>
    public double? Rpm { get; set; }

    /// <summary>
    /// Power consumption in kW.
    /// </summary>
    public double? PowerConsumption { get; set; }

    /// <summary>
    /// Timestamp when the reading was taken at the sensor.
    /// </summary>
    public DateTime Timestamp { get; set; }

    /// <summary>
    /// Quality indicator for the reading (0-100, 100 being highest quality).
    /// </summary>
    public int? DataQuality { get; set; }

    /// <summary>
    /// Additional JSON metadata from the sensor.
    /// </summary>
    public string? Metadata { get; set; }

    // Navigation property
    /// <summary>
    /// Reference to the associated machine.
    /// </summary>
    public virtual Machine Machine { get; set; } = null!;
}
