using AutoMapper;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.Mappers;

public class ServiceMappers : Profile
{
    public ServiceMappers()
    {
        CreateMap<Service, ServiceInfoDTO>();

        CreateMap<Service, ServiceTableInfoDTO>();

        CreateMap<ServiceForCreateDTO, Service>();

        CreateMap<ServiceForUpdateDTO, Service>();
    }
}
