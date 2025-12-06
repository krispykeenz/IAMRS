using IAMRS.Core.Enums;

namespace IAMRS.Application.DTOs;

/// <summary>
/// DTO for maintenance ticket summary.
/// </summary>
public class TicketSummaryDto
{
    public Guid Id { get; set; }
    public string TicketNumber { get; set; } = string.Empty;
    public Guid MachineId { get; set; }
    public string Title { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; }
    public TicketStatus Status { get; set; }
    public DateTime CreatedAt { get; set; }
}

/// <summary>
/// DTO for creating/updating a maintenance ticket.
/// </summary>
public class TicketUpsertDto
{
    public Guid MachineId { get; set; }
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public TicketPriority Priority { get; set; } = TicketPriority.Medium;
    public string MaintenanceType { get; set; } = "Corrective";
    public DateTime? DueDate { get; set; }
}
