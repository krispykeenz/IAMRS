using AutoMapper;
using FluentValidation;
using IAMRS.Application.Mappings;
using IAMRS.Application.Validators;
using IAMRS.Application.Services;
using IAMRS.Infrastructure.Configurations;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;

namespace IAMRS.Application.Extensions;

/// <summary>
/// Extension methods for registering application services.
/// </summary>
public static class ServiceCollectionExtensions
{
    /// <summary>
    /// Adds the application services to the DI container.
    /// </summary>
    public static IServiceCollection AddApplication(this IServiceCollection services, IConfiguration configuration)
    {
        // Register AutoMapper manually to avoid ambiguous AddAutoMapper extension overloads
        var mapperConfig = new MapperConfiguration(cfg =>
        {
            cfg.AddProfile<MappingProfile>();
        });
        services.AddSingleton(mapperConfig.CreateMapper());

        services.AddValidatorsFromAssemblyContaining<MachineUpsertDtoValidator>();

        services.Configure<AlertSettings>(configuration.GetSection("AlertSettings"));

        services.AddScoped<ITelemetryService, TelemetryService>();

        services.AddHostedService<IAMRS.Application.Background.TelemetryHealthMonitor>();
        services.AddHostedService<IAMRS.Application.Background.PredictiveMaintenanceProcessor>();
        return services;
    }
}
