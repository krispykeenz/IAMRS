using IAMRS.Core.Entities;
using IAMRS.Core.Enums;
using IAMRS.Core.Interfaces;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace IAMRS.Application.Background;

/// <summary>
/// Periodically analyzes telemetry data to compute rolling averages and standard deviation,
/// generating predictive maintenance alerts when anomalies are detected.
/// </summary>
public class PredictiveMaintenanceProcessor : BackgroundService
{
    private readonly IServiceProvider _serviceProvider;
    private readonly ILogger<PredictiveMaintenanceProcessor> _logger;

    public PredictiveMaintenanceProcessor(IServiceProvider serviceProvider, ILogger<PredictiveMaintenanceProcessor> logger)
    {
        _serviceProvider = serviceProvider;
        _logger = logger;
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        _logger.LogInformation("PredictiveMaintenanceProcessor started");

        while (!stoppingToken.IsCancellationRequested)
        {
            try
            {
                using var scope = _serviceProvider.CreateScope();
                var uow = scope.ServiceProvider.GetRequiredService<IUnitOfWork>();

                var machines = uow.Machines.Query().Where(m => m.IsMonitored).ToList();

                foreach (var machine in machines)
                {
                    // Analyze last 50 readings for temperature and vibration
                    var recent = uow.TelemetryData.Query()
                        .Where(t => t.MachineId == machine.Id && t.Temperature.HasValue)
                        .OrderByDescending(t => t.Timestamp)
                        .Take(50)
                        .Select(t => t.Temperature!.Value)
                        .ToList();

                    if (recent.Count >= 10)
                    {
                        var avg = recent.Average();
                        var variance = recent.Select(v => Math.Pow(v - avg, 2)).Average();
                        var std = Math.Sqrt(variance);

                        var latest = recent.First();

                        // If latest value is more than avg + 3*std, raise a predictive alert
                        if (std > 0 && latest > avg + 3 * std)
                        {
                            var alert = new Alert
                            {
                                MachineId = machine.Id,
                                Type = AlertType.PredictiveMaintenance,
                                Severity = AlertSeverity.Warning,
                                Message = $"Predictive anomaly: Temperature {latest:F1}°C deviates significantly (avg {avg:F1}°C, std {std:F1})"
                            };
                            await uow.Alerts.AddAsync(alert, stoppingToken);
                        }
                    }
                }

                await uow.SaveChangesAsync(stoppingToken);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error in PredictiveMaintenanceProcessor");
            }

            await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
        }
    }
}
