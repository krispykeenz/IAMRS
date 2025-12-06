namespace IAMRS.Core.Enums;

/// <summary>
/// Represents the priority level of a maintenance ticket.
/// </summary>
public enum TicketPriority
{
    /// <summary>
    /// Low priority, can be addressed when convenient.
    /// </summary>
    Low = 0,

    /// <summary>
    /// Medium priority, should be addressed soon.
    /// </summary>
    Medium = 1,

    /// <summary>
    /// High priority, needs prompt attention.
    /// </summary>
    High = 2,

    /// <summary>
    /// Urgent priority, requires immediate action.
    /// </summary>
    Urgent = 3
}
