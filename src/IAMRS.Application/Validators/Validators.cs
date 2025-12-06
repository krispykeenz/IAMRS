using FluentValidation;
using IAMRS.Application.DTOs;

namespace IAMRS.Application.Validators;

/// <summary>
/// Validator for MachineUpsertDto ensuring required fields and valid ranges.
/// </summary>
public class MachineUpsertDtoValidator : AbstractValidator<MachineUpsertDto>
{
    public MachineUpsertDtoValidator()
    {
        RuleFor(x => x.MachineCode).NotEmpty().MaximumLength(50);
        RuleFor(x => x.Name).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Location).NotEmpty().MaximumLength(200);
        RuleFor(x => x.TemperatureWarningThreshold).InclusiveBetween(-50, 200);
        RuleFor(x => x.TemperatureCriticalThreshold)
            .GreaterThan(x => x.TemperatureWarningThreshold)
            .WithMessage("Critical threshold must be greater than warning threshold");
        RuleFor(x => x.VibrationThreshold).GreaterThanOrEqualTo(0);
    }
}

/// <summary>
/// Validator for TelemetryIngestDto ensuring required fields.
/// </summary>
public class TelemetryIngestDtoValidator : AbstractValidator<TelemetryIngestDto>
{
    public TelemetryIngestDtoValidator()
    {
        RuleFor(x => x.MachineId).NotEmpty();
        RuleFor(x => x.Timestamp).NotEmpty();
        RuleFor(x => x.Temperature).InclusiveBetween(-1000, 2000).When(x => x.Temperature.HasValue);
        RuleFor(x => x.Vibration).GreaterThanOrEqualTo(0).When(x => x.Vibration.HasValue);
        RuleFor(x => x.Pressure).GreaterThanOrEqualTo(0).When(x => x.Pressure.HasValue);
    }
}

/// <summary>
/// Validator for TicketUpsertDto ensuring required fields.
/// </summary>
public class TicketUpsertDtoValidator : AbstractValidator<TicketUpsertDto>
{
    public TicketUpsertDtoValidator()
    {
        RuleFor(x => x.MachineId).NotEmpty();
        RuleFor(x => x.Title).NotEmpty().MaximumLength(200);
        RuleFor(x => x.Description).NotEmpty();
    }
}
