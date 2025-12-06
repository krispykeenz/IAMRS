using AutoMapper;
using IAMRS.Application.DTOs;
using IAMRS.Core.Entities;
using IAMRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IAMRS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class AlertsController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public AlertsController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all alerts (optionally filter by machine).
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<AlertDto>> GetAll([FromQuery] Guid? machineId)
    {
        var query = _uow.Alerts.Query();
        if (machineId.HasValue)
            query = query.Where(a => a.MachineId == machineId.Value);

        var items = query.OrderByDescending(a => a.CreatedAt).Take(200).ToList();
        return Ok(items.Select(_mapper.Map<AlertDto>));
    }

    /// <summary>
    /// Acknowledges an alert.
    /// </summary>
    [HttpPut("{id:guid}/acknowledge")]
    public async Task<ActionResult> Acknowledge(Guid id, [FromBody] AlertAcknowledgeDto dto, CancellationToken cancellationToken)
    {
        var alert = await _uow.Alerts.GetByIdAsync(id, cancellationToken);
        if (alert == null) return NotFound();
        alert.IsAcknowledged = true;
        alert.AcknowledgedAt = DateTime.UtcNow;
        alert.AcknowledgementNotes = dto.Notes;
        _uow.Alerts.Update(alert);
        await _uow.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
