using AutoMapper;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.Mappers;

public class ServiceCategorySpecializationMapper : Profile
{
    public ServiceCategorySpecializationMapper()
    {
        CreateMap<ServiceCategorySpecialization, ServiceCategoryInfoDTO>();

        CreateMap<ServiceCategorySpecializationForUpdateDTO, ServiceCategorySpecialization>();
    }
}
