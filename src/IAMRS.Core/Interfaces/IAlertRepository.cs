using IAMRS.Core.Entities;
using IAMRS.Core.Enums;

namespace IAMRS.Core.Interfaces;

/// <summary>
/// Specialized repository interface for Alert entities.
/// </summary>
public interface IAlertRepository : IRepository<Alert>
{
    /// <summary>
    /// Gets alerts for a specific machine.
    /// </summary>
    /// <param name="machineId">The machine ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of alerts for the machine.</returns>
    Task<IReadOnlyList<Alert>> GetByMachineAsync(Guid machineId, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets unacknowledged alerts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of unacknowledged alerts.</returns>
    Task<IReadOnlyList<Alert>> GetUnacknowledgedAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets alerts by severity level.
    /// </summary>
    /// <param name="severity">The severity level.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of alerts with the specified severity.</returns>
    Task<IReadOnlyList<Alert>> GetBySeverityAsync(AlertSeverity severity, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets alerts by type.
    /// </summary>
    /// <param name="type">The alert type.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of alerts of the specified type.</returns>
    Task<IReadOnlyList<Alert>> GetByTypeAsync(AlertType type, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets active (unresolved) alerts.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of active alerts.</returns>
    Task<IReadOnlyList<Alert>> GetActiveAlertsAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets alerts within a time range.
    /// </summary>
    /// <param name="from">Start of time range.</param>
    /// <param name="to">End of time range.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of alerts in the time range.</returns>
    Task<IReadOnlyList<Alert>> GetByTimeRangeAsync(
        DateTime from, 
        DateTime to, 
        CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets the count of alerts by severity.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Dictionary of severity to count.</returns>
    Task<Dictionary<AlertSeverity, int>> GetCountBySeverityAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets recent alerts for a machine of a specific type (for duplicate detection).
    /// </summary>
    /// <param name="machineId">The machine ID.</param>
    /// <param name="type">The alert type.</param>
    /// <param name="withinMinutes">Time window in minutes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of recent similar alerts.</returns>
    Task<IReadOnlyList<Alert>> GetRecentByMachineAndTypeAsync(
        Guid machineId, 
        AlertType type, 
        int withinMinutes, 
        CancellationToken cancellationToken = default);
}
