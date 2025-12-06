using AutoMapper;
using IAMRS.Application.DTOs;
using IAMRS.Core.Entities;
using IAMRS.Core.Enums;
using IAMRS.Core.Interfaces;
using IAMRS.Infrastructure.Configurations;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IAMRS.Application.Services;

/// <summary>
/// Provides operations for telemetry ingestion and processing.
/// </summary>
public interface ITelemetryService
{
    Task<TelemetryDto> IngestTelemetryAsync(TelemetryIngestDto dto, CancellationToken cancellationToken = default);
    Task<IReadOnlyList<TelemetryDto>> GetRecentTelemetryAsync(Guid machineId, int count, CancellationToken cancellationToken = default);
}

/// <summary>
/// Concrete implementation of ITelemetryService.
/// </summary>
public class TelemetryService : ITelemetryService
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;
    private readonly ILogger<TelemetryService> _logger;
    private readonly AlertSettings _settings;

    public TelemetryService(IUnitOfWork uow, IMapper mapper, ILogger<TelemetryService> logger, IOptions<AlertSettings> settings)
    {
        _uow = uow;
        _mapper = mapper;
        _logger = logger;
        _settings = settings.Value;
    }

    public async Task<TelemetryDto> IngestTelemetryAsync(TelemetryIngestDto dto, CancellationToken cancellationToken = default)
    {
        // Accept both GUID and machine code
        Machine? machine = null;
        if (Guid.TryParse(dto.MachineId, out var machineGuid))
        {
            machine = await _uow.Machines.GetByIdAsync(machineGuid, cancellationToken);
        }
        else
        {
            machine = await _uow.Machines.FirstOrDefaultAsync(m => m.MachineCode == dto.MachineId, cancellationToken);
        }

        if (machine == null)
            throw new KeyNotFoundException($"Machine not found: {dto.MachineId}");

        var telemetry = _mapper.Map<TelemetryData>(dto);
        telemetry.MachineId = machine.Id;

        await _uow.TelemetryData.AddAsync(telemetry, cancellationToken);

        machine.LastTelemetryAt = telemetry.Timestamp;
        machine.Status = MachineStatus.Online;
        _uow.Machines.Update(machine);

        await EvaluateAlertsAsync(machine, telemetry, cancellationToken);

        await _uow.SaveChangesAsync(cancellationToken);

        return _mapper.Map<TelemetryDto>(telemetry);
    }

    public async Task<IReadOnlyList<TelemetryDto>> GetRecentTelemetryAsync(Guid machineId, int count, CancellationToken cancellationToken = default)
    {
        var query = _uow.TelemetryData.Query()
            .Where(t => t.MachineId == machineId)
            .OrderByDescending(t => t.Timestamp)
            .Take(count);

        var list = query.ToList();
        return list.Select(_mapper.Map<TelemetryDto>).ToList();
    }

    private async Task EvaluateAlertsAsync(Machine machine, TelemetryData telemetry, CancellationToken cancellationToken)
    {
        // Critical temperature alert
        if (telemetry.Temperature.HasValue && telemetry.Temperature.Value > _settings.TemperatureCriticalThreshold)
        {
            await CreateAlertAsync(machine, AlertType.HighTemperature, AlertSeverity.Critical,
                $"Temperature CRITICAL: {telemetry.Temperature:F1}°C exceeds {_settings.TemperatureCriticalThreshold}°C",
                telemetry.Temperature, _settings.TemperatureCriticalThreshold, cancellationToken);
            return; // Critical overrides warning
        }

        // Warning temperature alert: 3 consecutive readings above warning threshold
        if (telemetry.Temperature.HasValue && telemetry.Temperature.Value > _settings.TemperatureWarningThreshold)
        {
            var count = await _uow.TelemetryData.CountAsync(t => t.MachineId == machine.Id && t.Temperature.HasValue && t.Temperature.Value > _settings.TemperatureWarningThreshold, cancellationToken);
            if (count >= _settings.ConsecutiveReadingsForWarning - 1) // include current
            {
                await CreateAlertAsync(machine, AlertType.HighTemperature, AlertSeverity.Warning,
                    $"Temperature WARNING: {telemetry.Temperature:F1}°C exceeds {_settings.TemperatureWarningThreshold}°C in consecutive readings",
                    telemetry.Temperature, _settings.TemperatureWarningThreshold, cancellationToken);
            }
        }

        // Vibration abnormality (simple threshold)
        if (telemetry.Vibration.HasValue && telemetry.Vibration.Value > machine.VibrationThreshold)
        {
            await CreateAlertAsync(machine, AlertType.AbnormalVibration, AlertSeverity.Warning,
                $"Vibration {telemetry.Vibration:F2} mm/s exceeds threshold {machine.VibrationThreshold:F2} mm/s",
                telemetry.Vibration, machine.VibrationThreshold, cancellationToken);
        }
    }

    private async Task CreateAlertAsync(Machine machine, AlertType type, AlertSeverity severity, string message, double? value, double? threshold, CancellationToken cancellationToken)
    {
        var alert = new Alert
        {
            MachineId = machine.Id,
            Type = type,
            Severity = severity,
            Message = message,
            TriggerValue = value,
            ThresholdValue = threshold
        };

        await _uow.Alerts.AddAsync(alert, cancellationToken);
    }
}
