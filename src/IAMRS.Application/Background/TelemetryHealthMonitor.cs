using IAMRS.Core.Entities;
using IAMRS.Core.Enums;
using IAMRS.Core.Interfaces;
using IAMRS.Infrastructure.Configurations;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;

namespace IAMRS.Application.Background;

/// <summary>
/// Periodically checks for machines that haven't reported telemetry and raises offline alerts.
/// </summary>
public class TelemetryHealthMonitor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<TelemetryHealthMonitor> _logger;
    private readonly AlertSettings _settings;

    public TelemetryHealthMonitor(IServiceProvider serviceProvider, ILogger<TelemetryHealthMonitor> logger, IOptions<AlertSettings> settings)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
        _settings = settings.Value;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("TelemetryHealthMonitor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();
                var now = DateTime.UtcNow;
                var threshold = now.AddMinutes(-_settings.OfflineTimeoutMinutes);

                var offlineMachines = uow.Machines.Query()
                    .Where(m => m.IsMonitored && (m.LastTelemetryAt == null || m.LastTelemetryAt < threshold))
                    .ToList();

                foreach (var machine in offlineMachines)
                {
                    // Avoid duplicate frequent alerts by checking recent ones
                    var recentOfflineAlert = uow.Alerts.Query()
                        .Where(a => a.MachineId == machine.Id && a.Type == AlertType.MachineOffline && !a.IsResolved)
                        .OrderByDescending(a => a.CreatedAt)
                        .FirstOrDefault();

                    if (recentOfflineAlert == null || (now - recentOfflineAlert.CreatedAt).TotalMinutes > _settings.OfflineTimeoutMinutes)
                    {
                        var alert = new Alert
                        {
                            MachineId = machine.Id,
                            Type = AlertType.MachineOffline,
                            Severity = AlertSeverity.Warning,
                            Message = $"Machine {machine.MachineCode} is offline (no telemetry since {machine.LastTelemetryAt:O})"
                        };
                        await uow.Alerts.AddAsync(alert, stoppingToken);

                        machine.Status = MachineStatus.Offline;
                        uow.Machines.Update(machine);
                    }
                }

                await uow.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in TelemetryHealthMonitor");
            }

            await Task.Delay(TimeSpan.FromSeconds(30), stoppingToken);
        }
    }
}
