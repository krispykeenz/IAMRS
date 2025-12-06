using System.Data;
using IAMRS.Core.Entities;
using IAMRS.Core.Interfaces;
using IAMRS.Infrastructure.Persistence;
using Microsoft.EntityFrameworkCore.Storage;

namespace IAMRS.Infrastructure.Repositories;

/// <summary>
/// Unit of Work implementation for managing transactions across repositories.
/// </summary>
public class UnitOfWork : IUnitOfWork
{
    private readonly IamrsDbContext _context;
    private IDbContextTransaction? _currentTransaction;

    public IRepository<Machine> Machines { get; }
    public IRepository<TelemetryData> TelemetryData { get; }
    public IRepository<Alert> Alerts { get; }
    public IRepository<MaintenanceTicket> MaintenanceTickets { get; }
    public IRepository<AuditLog> AuditLogs { get; }

    public UnitOfWork(IamrsDbContext context)
    {
        _context = context;
        Machines = new Repository<Machine>(context);
        TelemetryData = new Repository<TelemetryData>(context);
        Alerts = new Repository<Alert>(context);
        MaintenanceTickets = new Repository<MaintenanceTicket>(context);
        AuditLogs = new Repository<AuditLog>(context);
    }

    public async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
        => await _context.SaveChangesAsync(cancellationToken);

    public async Task BeginTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction != null)
            return;

        _currentTransaction = await _context.Database.BeginTransactionAsync(cancellationToken);
    }

    public async Task CommitTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.CommitAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public async Task RollbackTransactionAsync(CancellationToken cancellationToken = default)
    {
        if (_currentTransaction == null)
            return;

        await _currentTransaction.RollbackAsync(cancellationToken);
        await _currentTransaction.DisposeAsync();
        _currentTransaction = null;
    }

    public void Dispose()
    {
        _currentTransaction?.Dispose();
        _context.Dispose();
    }
}
