using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Mappers.ReceptionistMapper;

public class ReceptionistMappers : Profile
{
    public ReceptionistMappers()
    {
        CreateMap<Receptionist, ReceptionistInfoDTO>();
        CreateMap<Receptionist, ReceptionistTableInfoDTO>();
        CreateMap<ReceptionistForCreateDTO, Receptionist>();
        CreateMap<ReceptionistForUpdateDTO, Receptionist>();
    }
}
