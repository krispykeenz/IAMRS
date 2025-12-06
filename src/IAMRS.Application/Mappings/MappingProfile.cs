using AutoMapper;
using IAMRS.Application.DTOs;
using IAMRS.Core.Entities;

namespace IAMRS.Application.Mappings;

/// <summary>
/// AutoMapper profile mapping domain entities to DTOs and vice versa.
/// </summary>
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Machine, MachineSummaryDto>();
        CreateMap<Machine, MachineDetailDto>();
        CreateMap<MachineUpsertDto, Machine>();

        CreateMap<TelemetryData, TelemetryDto>();
        CreateMap<TelemetryIngestDto, TelemetryData>()
            .ForMember(dest => dest.MachineId, opt => opt.Ignore()); // MachineId set manually after lookup

        CreateMap<Alert, AlertDto>();

        CreateMap<MaintenanceTicket, TicketSummaryDto>();
        CreateMap<TicketUpsertDto, MaintenanceTicket>();
    }
}
