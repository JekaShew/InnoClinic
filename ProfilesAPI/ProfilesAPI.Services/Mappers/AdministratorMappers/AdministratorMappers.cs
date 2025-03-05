using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Mappers.AdministratorMappers;

public class AdministratorMappers : Profile
{
    public AdministratorMappers()
    {
        CreateMap<Administrator, AdministratorInfoDTO>();
        CreateMap<Administrator, AdministratorTableInfoDTO>();
        CreateMap<AdministratorForCreateDTO, Administrator>();
        CreateMap<AdministratorForUpdateDTO, Administrator>();
    }
}
