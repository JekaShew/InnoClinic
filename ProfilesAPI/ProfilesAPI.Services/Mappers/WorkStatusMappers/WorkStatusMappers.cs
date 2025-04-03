using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Services.Mappers.WorkStatusMappers;

public class WorkStatusMappers : Profile
{
    public WorkStatusMappers()
    {
        CreateMap<WorkStatus, WorkStatusInfoDTO>();
        CreateMap<WorkStatus, WorkStatusTableInfoDTO>();
        CreateMap<WorkStatusForCreateDTO, WorkStatus>();
        CreateMap<WorkStatusForUpdateDTO, WorkStatus>();
    }
}
