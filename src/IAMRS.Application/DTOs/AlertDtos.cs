using IAMRS.Core.Enums;

namespace IAMRS.Application.DTOs;

/// <summary>
/// DTO representing an alert.
/// </summary>
public class AlertDto
{
    public Guid Id { get; set; }
    public Guid MachineId { get; set; }
    public AlertType Type { get; set; }
    public AlertSeverity Severity { get; set; }
    public string Message { get; set; } = string.Empty;
    public DateTime CreatedAt { get; set; }
    public bool IsAcknowledged { get; set; }
    public DateTime? AcknowledgedAt { get; set; }
}

/// <summary>
/// DTO for acknowledging an alert.
/// </summary>
public class AlertAcknowledgeDto
{
    public string? Notes { get; set; }
}
