using IAMRS.Core.Entities;

namespace IAMRS.Core.Interfaces;

/// <summary>
/// Unit of Work pattern interface for managing transactions across repositories.
/// </summary>
public interface IUnitOfWork : IDisposable
{
    /// <summary>
    /// Repository for Machine entities.
    /// </summary>
    IRepository<Machine> Machines { get; }

    /// <summary>
    /// Repository for TelemetryData entities.
    /// </summary>
    IRepository<TelemetryData> TelemetryData { get; }

    /// <summary>
    /// Repository for Alert entities.
    /// </summary>
    IRepository<Alert> Alerts { get; }

    /// <summary>
    /// Repository for MaintenanceTicket entities.
    /// </summary>
    IRepository<MaintenanceTicket> MaintenanceTickets { get; }

    /// <summary>
    /// Repository for AuditLog entities.
    /// </summary>
    IRepository<AuditLog> AuditLogs { get; }

    /// <summary>
    /// Saves all pending changes to the database.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    /// <returns>Number of state entries written to the database.</returns>
    Task<int> SaveChangesAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Begins a new database transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task BeginTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Commits the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task CommitTransactionAsync(CancellationToken cancellationToken = default);

    /// <summary>
    /// Rolls back the current transaction.
    /// </summary>
    /// <param name="cancellationToken">Cancellation token.</param>
    Task RollbackTransactionAsync(CancellationToken cancellationToken = default);
}
