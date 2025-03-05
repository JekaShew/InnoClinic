using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Mappers.DoctorMappers;

public class DoctorMappers : Profile
{
    public DoctorMappers()
    {
        CreateMap<Doctor, DoctorInfoDTO>();
        CreateMap<Doctor, DoctorTableInfoDTO>();
        CreateMap<DoctorForCreateDTO, Doctor>();
        CreateMap<DoctorForUpdateDTO, Doctor>();
    }
}
