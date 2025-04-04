using AutoMapper;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.Mappers;

public class ServiceCategoryMappers : Profile
{
    public ServiceCategoryMappers()
    {
        CreateMap<ServiceCategory, ServiceCategoryInfoDTO>();

        CreateMap<ServiceCategory, ServiceCategoryTableInfoDTO>();

        CreateMap<ServiceCategoryForCreateDTO, ServiceCategory>();

        CreateMap<ServiceCategoryForUpdateDTO, ServiceCategory>();
    }
}
