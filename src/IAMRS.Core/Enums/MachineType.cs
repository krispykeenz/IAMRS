namespace IAMRS.Core.Enums;

/// <summary>
/// Represents the type of industrial machine.
/// </summary>
public enum MachineType
{
    /// <summary>
    /// Computer Numerical Control machine for precision manufacturing.
    /// </summary>
    CNC = 0,

    /// <summary>
    /// Industrial robot for automation tasks.
    /// </summary>
    Robot = 1,

    /// <summary>
    /// Conveyor belt system for material transport.
    /// </summary>
    Conveyor = 2,

    /// <summary>
    /// Press machine for forming operations.
    /// </summary>
    Press = 3,

    /// <summary>
    /// Pump for fluid transfer.
    /// </summary>
    Pump = 4,

    /// <summary>
    /// Compressor for air/gas compression.
    /// </summary>
    Compressor = 5,

    /// <summary>
    /// Electric motor for mechanical power.
    /// </summary>
    Motor = 6,

    /// <summary>
    /// Generator for electrical power generation.
    /// </summary>
    Generator = 7,

    /// <summary>
    /// Heat exchanger for thermal management.
    /// </summary>
    HeatExchanger = 8,

    /// <summary>
    /// Welding equipment.
    /// </summary>
    Welder = 9,

    /// <summary>
    /// Other unclassified machine type.
    /// </summary>
    Other = 99
}
