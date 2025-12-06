using IAMRS.Application.DTOs;
using IAMRS.Application.Services;
using Microsoft.AspNetCore.Mvc;

namespace IAMRS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class TelemetryController : ControllerBase
{
    private readonly ITelemetryService _telemetryService;

    public TelemetryController(ITelemetryService telemetryService)
    {
        _telemetryService = telemetryService;
    }

    /// <summary>
    /// Ingests telemetry data for a machine.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TelemetryDto>> Post([FromBody] TelemetryIngestDto dto, CancellationToken cancellationToken)
    {
        var result = await _telemetryService.IngestTelemetryAsync(dto, cancellationToken);
        return Ok(result);
    }
}
