using AppointmentAPI.Domain.Data.Models;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AutoMapper;

namespace AppointmentAPI.Application.Mappers;

public class AppointmentMappers : Profile
{
    public AppointmentMappers()
    {
        CreateMap<Appointment, AppointmentTableInfoDTO>();

        CreateMap<Appointment, AppointmentInfoDTO>();

        CreateMap<AppointmentForCreateDTO, Appointment>();

        CreateMap<AppointmentForUpdateDTO, Appointment>();
    }
}
