using AutoMapper;
using IAMRS.Application.DTOs;
using IAMRS.Core.Entities;
using IAMRS.Core.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace IAMRS.Api.Controllers;

[ApiController]
[Route("api/[controller]")]
public class MachinesController : ControllerBase
{
    private readonly IUnitOfWork _uow;
    private readonly IMapper _mapper;

    public MachinesController(IUnitOfWork uow, IMapper mapper)
    {
        _uow = uow;
        _mapper = mapper;
    }

    /// <summary>
    /// Gets all machines.
    /// </summary>
    [HttpGet]
    public async Task<ActionResult<IEnumerable<MachineSummaryDto>>> GetAll(CancellationToken cancellationToken)
    {
        var items = await _uow.Machines.GetAllAsync(cancellationToken);
        return Ok(items.Select(_mapper.Map<MachineSummaryDto>));
    }

    /// <summary>
    /// Gets a machine by ID.
    /// </summary>
    [HttpGet("{id:guid}")]
    public async Task<ActionResult<MachineDetailDto>> GetById(Guid id, CancellationToken cancellationToken)
    {
        var item = await _uow.Machines.GetByIdAsync(id, cancellationToken);
        if (item == null) return NotFound();
        return Ok(_mapper.Map<MachineDetailDto>(item));
    }

    /// <summary>
    /// Creates a new machine.
    /// </summary>
    [HttpPost]
    public async Task<ActionResult<MachineDetailDto>> Create([FromBody] MachineUpsertDto dto, CancellationToken cancellationToken)
    {
        var entity = _mapper.Map<Machine>(dto);
        await _uow.Machines.AddAsync(entity, cancellationToken);
        await _uow.SaveChangesAsync(cancellationToken);
        return CreatedAtAction(nameof(GetById), new { id = entity.Id }, _mapper.Map<MachineDetailDto>(entity));
    }

    /// <summary>
    /// Updates an existing machine.
    /// </summary>
    [HttpPut("{id:guid}")]
    public async Task<ActionResult> Update(Guid id, [FromBody] MachineUpsertDto dto, CancellationToken cancellationToken)
    {
        var entity = await _uow.Machines.GetByIdAsync(id, cancellationToken);
        if (entity == null) return NotFound();
        _mapper.Map(dto, entity);
        _uow.Machines.Update(entity);
        await _uow.SaveChangesAsync(cancellationToken);
        return NoContent();
    }

    /// <summary>
    /// Deletes a machine.
    /// </summary>
    [HttpDelete("{id:guid}")]
    public async Task<ActionResult> Delete(Guid id, CancellationToken cancellationToken)
    {
        var entity = await _uow.Machines.GetByIdAsync(id, cancellationToken);
        if (entity == null) return NotFound();
        _uow.Machines.Remove(entity);
        await _uow.SaveChangesAsync(cancellationToken);
        return NoContent();
    }
}
