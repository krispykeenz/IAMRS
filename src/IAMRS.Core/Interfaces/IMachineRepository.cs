using IAMRS.Core.Entities;
using IAMRS.Core.Enums;

namespace IAMRS.Core.Interfaces;

/// <summary>
/// Specialized repository interface for Machine entities.
/// </summary>
public interface IMachineRepository : IRepository<Machine>
{
    /// <summary>
    /// Gets a machine by its code.
    /// </summary>
    /// <param name="machineCode">The machine code (e.g., "MX-01").</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>The machine if found, null otherwise.</returns>
    Task<Machine?> GetByCodeAsync(string machineCode, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets machines by status.
    /// </summary>
    /// <param name="status">The machine status.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of machines with the specified status.</returns>
    Task<IReadOnlyList<Machine>> GetByStatusAsync(MachineStatus status, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets machines by location.
    /// </summary>
    /// <param name="location">The location string.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of machines in the specified location.</returns>
    Task<IReadOnlyList<Machine>> GetByLocationAsync(string location, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets machines that are being monitored.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of monitored machines.</returns>
    Task<IReadOnlyList<Machine>> GetMonitoredMachinesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets machines that haven't sent telemetry within the specified minutes.
    /// </summary>
    /// <param name="minutes">Timeout in minutes.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of offline machines.</returns>
    Task<IReadOnlyList<Machine>> GetOfflineMachinesAsync(int minutes, CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets machines with their latest telemetry data.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Collection of machines with telemetry.</returns>
    Task<IReadOnlyList<Machine>> GetWithLatestTelemetryAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Gets machine with all related data for detail view.
    /// </summary>
    /// <param name="id">Machine ID.</param>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Machine with related entities.</returns>
    Task<Machine?> GetWithDetailsAsync(Guid id, CancellationToken cancellationToken = default);
}
