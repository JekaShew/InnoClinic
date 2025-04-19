using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using AutoMapper;

namespace AppointmentAPI.Application.Mappers;

public class AppointmentResultMappers : Profile
{
    public AppointmentResultMappers()
    {
        CreateMap<AppointmentResult, AppointmentResultTableInfoDTO>();

        CreateMap<AppointmentResult, AppointmentResultInfoDTO>();

        CreateMap<AppointmentResultForCreateDTO, AppointmentResult>();

        CreateMap<AppointmentResultForUpdateDTO, AppointmentResult>();
    }
}
