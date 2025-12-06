namespace IAMRS.Core.Enums;

/// <summary>
/// Represents the status of a maintenance ticket.
/// </summary>
public enum TicketStatus
{
    /// <summary>
    /// Ticket is open and awaiting assignment.
    /// </summary>
    Open = 0,

    /// <summary>
    /// Ticket is assigned and work is in progress.
    /// </summary>
    InProgress = 1,

    /// <summary>
    /// Ticket work is complete, awaiting verification.
    /// </summary>
    Resolved = 2,

    /// <summary>
    /// Ticket is closed and archived.
    /// </summary>
    Closed = 3,

    /// <summary>
    /// Ticket was cancelled.
    /// </summary>
    Cancelled = 4
}
