using AutoMapper;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Mappers.DoctorMappers;

public class DoctorMappers : Profile
{
    public DoctorMappers()
    {
        CreateMap<Doctor, DoctorInfoDTO>()
            .ForMember(dest => dest.DoctorSpecializations, opt => opt.MapFrom(src => src.DoctorSpecializations));

        CreateMap<Doctor, DoctorTableInfoDTO>();
            //.ForMember(dest => dest.Photo, opt => opt.Ignore())
            //.ForMember(dest => dest.PhotoId, opt => opt.Ignore());

        //CreateMap<List<Doctor>, List<DoctorTableInfoDTO>>();
            
        CreateMap<DoctorForCreateDTO, Doctor>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore())
            .ForMember(dest => dest.DoctorSpecializations, opt => opt.MapFrom(src => src.DoctorSpecializations));
        
        CreateMap<DoctorForUpdateDTO, Doctor>()
            .ForMember(dest => dest.Photo, opt => opt.Ignore());

        CreateMap<SpecializationsOfDoctorInfoDTO, DoctorSpecialization>().ReverseMap();
        //CreateMap<List<SpecializationsOfDoctorInfoDTO>, List<DoctorSpecialization>>();

        CreateMap<DoctorSpecializationForCreateDTO, DoctorSpecialization>();
        CreateMap<DoctorSpecializationForUpdateDTO, DoctorSpecialization>();
        //CreateMap<List<DoctorSpecializationForCreateDTO>, List<DoctorSpecialization>>();
    }
}
