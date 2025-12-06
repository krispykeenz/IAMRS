using IAMRS.Core.Common;
using IAMRS.Core.Enums;

namespace IAMRS.Core.Entities;

/// <summary>
/// Represents a maintenance work order/ticket for a machine.
/// </summary>
public class MaintenanceTicket : BaseEntity
{
    /// <summary>
    /// Human-readable ticket number (e.g., "MT-2024-001").
    /// </summary>
    public string TicketNumber { get; set; } = string.Empty;

    /// <summary>
    /// Foreign key to the associated machine.
    /// </summary>
    public Guid MachineId { get; set; }

    /// <summary>
    /// Brief title of the maintenance issue.
    /// </summary>
    public string Title { get; set; } = string.Empty;

    /// <summary>
    /// Detailed description of the maintenance required.
    /// </summary>
    public string Description { get; set; } = string.Empty;

    /// <summary>
    /// Priority level of the ticket.
    /// </summary>
    public TicketPriority Priority { get; set; }

    /// <summary>
    /// Current status of the ticket.
    /// </summary>
    public TicketStatus Status { get; set; } = TicketStatus.Open;

    /// <summary>
    /// Type of maintenance (Preventive, Corrective, Predictive).
    /// </summary>
    public string MaintenanceType { get; set; } = "Corrective";

    /// <summary>
    /// User ID who created the ticket.
    /// </summary>
    public string? CreatedBy { get; set; }

    /// <summary>
    /// User ID assigned to work on this ticket.
    /// </summary>
    public string? AssignedTo { get; set; }

    /// <summary>
    /// Timestamp when work started.
    /// </summary>
    public DateTime? StartedAt { get; set; }

    /// <summary>
    /// Timestamp when work was completed.
    /// </summary>
    public DateTime? CompletedAt { get; set; }

    /// <summary>
    /// Estimated time to complete in hours.
    /// </summary>
    public double? EstimatedHours { get; set; }

    /// <summary>
    /// Actual time spent in hours.
    /// </summary>
    public double? ActualHours { get; set; }

    /// <summary>
    /// Estimated cost for the maintenance.
    /// </summary>
    public decimal? EstimatedCost { get; set; }

    /// <summary>
    /// Actual cost incurred.
    /// </summary>
    public decimal? ActualCost { get; set; }

    /// <summary>
    /// Parts replaced or used during maintenance.
    /// </summary>
    public string? PartsUsed { get; set; }

    /// <summary>
    /// Resolution notes describing what was done.
    /// </summary>
    public string? ResolutionNotes { get; set; }

    /// <summary>
    /// Due date for completion.
    /// </summary>
    public DateTime? DueDate { get; set; }

    // Navigation properties
    /// <summary>
    /// Reference to the associated machine.
    /// </summary>
    public virtual Machine Machine { get; set; } = null!;

    /// <summary>
    /// Alerts that led to this ticket.
    /// </summary>
    public virtual ICollection<Alert> Alerts { get; set; } = new List<Alert>();
}
