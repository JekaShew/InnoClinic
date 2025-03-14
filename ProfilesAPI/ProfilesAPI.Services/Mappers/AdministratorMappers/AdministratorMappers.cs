using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Mappers.AdministratorMappers;

public class AdministratorMappers : Profile
{
    public AdministratorMappers()
    {
        CreateMap<Administrator, AdministratorInfoDTO>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo));
        
        CreateMap<Administrator, AdministratorTableInfoDTO>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.PhotoId, opt => opt.MapFrom(src => src.Photo));
       
        CreateMap<AdministratorForCreateDTO, Administrator>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());

        CreateMap<AdministratorForUpdateDTO, Administrator>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());
    }
}
