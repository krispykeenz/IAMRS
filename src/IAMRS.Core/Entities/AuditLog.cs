using IAMRS.Core.Common;

namespace IAMRS.Core.Entities;

/// <summary>
/// Represents an audit log entry tracking system changes.
/// </summary>
public class AuditLog : BaseEntity
{
    /// <summary>
    /// User ID who performed the action (null for system actions).
    /// </summary>
    public string? UserId { get; set; }

    /// <summary>
    /// Username for display purposes.
    /// </summary>
    public string? UserName { get; set; }

    /// <summary>
    /// The action performed (Create, Update, Delete, Login, etc.).
    /// </summary>
    public string Action { get; set; } = string.Empty;

    /// <summary>
    /// Type of entity affected (Machine, Alert, Ticket, etc.).
    /// </summary>
    public string EntityType { get; set; } = string.Empty;

    /// <summary>
    /// ID of the affected entity.
    /// </summary>
    public string? EntityId { get; set; }

    /// <summary>
    /// JSON representation of old values (for updates).
    /// </summary>
    public string? OldValues { get; set; }

    /// <summary>
    /// JSON representation of new values.
    /// </summary>
    public string? NewValues { get; set; }

    /// <summary>
    /// IP address of the client.
    /// </summary>
    public string? IpAddress { get; set; }

    /// <summary>
    /// User agent string from the request.
    /// </summary>
    public string? UserAgent { get; set; }

    /// <summary>
    /// Additional details or context.
    /// </summary>
    public string? Details { get; set; }

    /// <summary>
    /// Timestamp of the action.
    /// </summary>
    public DateTime Timestamp { get; set; } = DateTime.UtcNow;
}
