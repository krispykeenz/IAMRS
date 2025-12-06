using IAMRS.Core.Entities;

namespace IAMRS.Core.Interfaces;

/// <summary>
/// Specialized repository interface for TelemetryData entities.
/// </summary>
public interface ITelemetryRepository : IRepository<TelemetryData>
{
    /// <summary>
    /// Gets telemetry data for a machine within a time range.
    /// </summary>
    /// <param name="machineId">The machine ID.</param>
    /// <param name="from">Start of time range.</param>
    /// <param name="to">End of time range.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of telemetry data points.</returns>
    Task<IReadOnlyList<TelemetryData>> GetByMachineAndTimeRangeAsync(
        Guid machineId, 
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the latest telemetry reading for a machine.
    /// </summary>
    /// <param name="machineId">The machine ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The latest telemetry data point.</returns>
    Task<TelemetryData?> GetLatestByMachineAsync(Guid machineId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the last N telemetry readings for a machine.
    /// </summary>
    /// <param name="machineId">The machine ID.</param>
    /// <param name="count">Number of readings to return.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of recent telemetry data points.</returns>
    Task<IReadOnlyList<TelemetryData>> GetLastNByMachineAsync(
        Guid machineId, 
        int count, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets average telemetry values for a machine over a time period.
    /// </summary>
    /// <param name="machineId">The machine ID.</param>
    /// <param name="from">Start of time range.</param>
    /// <param name="to">End of time range.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Telemetry data with average values.</returns>
    Task<TelemetryData?> GetAveragesAsync(
        Guid machineId, 
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Deletes old telemetry data beyond retention period.
    /// </summary>
    /// <param name="olderThan">Date threshold.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of records deleted.</returns>
    Task<int> DeleteOldDataAsync(DateTime olderThan, CancellationToken cancellationToken = default);
}
