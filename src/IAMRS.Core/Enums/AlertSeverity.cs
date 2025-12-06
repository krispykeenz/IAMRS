namespace IAMRS.Core.Enums;

/// <summary>
/// Represents the severity level of an alert.
/// </summary>
public enum AlertSeverity
{
    /// <summary>
    /// Informational alert, no action required.
    /// </summary>
    Info = 0,

    /// <summary>
    /// Warning alert, attention recommended.
    /// </summary>
    Warning = 1,

    /// <summary>
    /// Critical alert, immediate action required.
    /// </summary>
    Critical = 2
}
