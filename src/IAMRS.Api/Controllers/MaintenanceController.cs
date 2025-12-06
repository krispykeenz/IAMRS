using AutoMapper;
using IAMRS.Application.DTOs;
using IAMRS.Core.Entities;
using IAMRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IAMRS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MaintenanceController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MaintenanceController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all maintenance tickets.
    /// </summary>
    [HttpGet]
    public ActionResult<IEnumerable<TicketSummaryDto>> GetAll()
    {
        var items = _uow.MaintenanceTickets.Query().OrderByDescending(t => t.CreatedAt).Take(200).ToList();
        return Ok(items.Select(_mapper.Map<TicketSummaryDto>));
    }

    /// <summary>
    /// Creates a maintenance ticket.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<TicketSummaryDto>> Create([FromBody] TicketUpsertDto dto, CancellationToken cancellationToken)
    {
        var ticket = _mapper.Map<MaintenanceTicket>(dto);
        ticket.TicketNumber = $"MT-{DateTime.UtcNow:yyyyMMdd-HHmmss}";
        await _uow.MaintenanceTickets.AddAsync(ticket, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetAll), null, _mapper.Map<TicketSummaryDto>(ticket));
    }

    /// <summary>
    /// Updates a maintenance ticket.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] TicketUpsertDto dto, CancellationToken cancellationToken)
    {
        var entity = await _uow.MaintenanceTickets.GetByIdAsync(id, cancellationToken);
        if (entity == null) return NotFound();
        _mapper.Map(dto, entity);
        _uow.MaintenanceTickets.Update(entity);
        await _uow.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
