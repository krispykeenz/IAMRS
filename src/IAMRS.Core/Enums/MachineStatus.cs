namespace IAMRS.Core.Enums;

/// <summary>
/// Represents the operational status of a machine.
/// </summary>
public enum MachineStatus
{
    /// <summary>
    /// Machine is offline or not responding.
    /// </summary>
    Offline = 0,

    /// <summary>
    /// Machine is operating normally.
    /// </summary>
    Online = 1,

    /// <summary>
    /// Machine is operating but showing warning signs.
    /// </summary>
    Warning = 2,

    /// <summary>
    /// Machine is in critical condition requiring immediate attention.
    /// </summary>
    Critical = 3,

    /// <summary>
    /// Machine is undergoing maintenance.
    /// </summary>
    Maintenance = 4
}
