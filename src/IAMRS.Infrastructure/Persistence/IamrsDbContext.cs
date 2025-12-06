using IAMRS.Core.Entities;
using IAMRS.Core.Enums;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace IAMRS.Infrastructure.Persistence;

/// <summary>
/// Entity Framework Core database context for the IAMRS application.
/// Includes ASP.NET Identity schema.
/// </summary>
public class IamrsDbContext : IdentityDbContext<IdentityUser>
{
    public IamrsDbContext(DbContextOptions<IamrsDbContext> options) : base(options)
    {
    }

    public DbSet<Machine> Machines => Set<Machine>();
    public DbSet<TelemetryData> TelemetryData => Set<TelemetryData>();
    public DbSet<Alert> Alerts => Set<Alert>();
    public DbSet<MaintenanceTicket> MaintenanceTickets => Set<MaintenanceTicket>();
    public DbSet<AuditLog> AuditLogs => Set<AuditLog>();

    protected override void OnModelCreating(ModelBuilder builder)
    {
        base.OnModelCreating(builder);

        // Machine configuration
        builder.Entity<Machine>(entity =>
        {
            entity.ToTable("Machines");
            entity.HasIndex(e => e.MachineCode).IsUnique();
            entity.Property(e => e.MachineCode).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Name).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Location).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Manufacturer).HasMaxLength(200);
            entity.Property(e => e.Model).HasMaxLength(100);
            entity.Property(e => e.SerialNumber).HasMaxLength(100);

            entity.HasMany(e => e.TelemetryData)
                .WithOne(t => t.Machine)
                .HasForeignKey(t => t.MachineId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.Alerts)
                .WithOne(a => a.Machine)
                .HasForeignKey(a => a.MachineId)
                .OnDelete(DeleteBehavior.Cascade);

            entity.HasMany(e => e.MaintenanceTickets)
                .WithOne(t => t.Machine)
                .HasForeignKey(t => t.MachineId)
                .OnDelete(DeleteBehavior.Cascade);
        });

        // Telemetry configuration
        builder.Entity<TelemetryData>(entity =>
        {
            entity.ToTable("TelemetryData");
            entity.HasIndex(e => new { e.MachineId, e.Timestamp });
            entity.Property(e => e.Metadata).HasColumnType("nvarchar(max)");
        });

        // Alert configuration
        builder.Entity<Alert>(entity =>
        {
            entity.ToTable("Alerts");
            entity.HasIndex(e => new { e.MachineId, e.Severity, e.CreatedAt });
            entity.Property(e => e.Message).HasMaxLength(500).IsRequired();
            entity.Property(e => e.Details).HasColumnType("nvarchar(max)");
        });

        // MaintenanceTicket configuration
        builder.Entity<MaintenanceTicket>(entity =>
        {
            entity.ToTable("MaintenanceTickets");
            entity.HasIndex(e => e.TicketNumber).IsUnique();
            entity.Property(e => e.TicketNumber).HasMaxLength(50).IsRequired();
            entity.Property(e => e.Title).HasMaxLength(200).IsRequired();
            entity.Property(e => e.Description).HasColumnType("nvarchar(max)");
        });

        // AuditLog configuration
        builder.Entity<AuditLog>(entity =>
        {
            entity.ToTable("AuditLogs");
            entity.HasIndex(e => e.Timestamp);
            entity.Property(e => e.Action).HasMaxLength(100);
            entity.Property(e => e.EntityType).HasMaxLength(100);
            entity.Property(e => e.OldValues).HasColumnType("nvarchar(max)");
            entity.Property(e => e.NewValues).HasColumnType("nvarchar(max)");
            entity.Property(e => e.Details).HasColumnType("nvarchar(max)");
        });
    }
}
