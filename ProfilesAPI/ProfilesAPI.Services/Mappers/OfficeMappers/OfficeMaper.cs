using AutoMapper;
using CommonLibrary.RabbitMQEvents;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Mappers.OfficeMappers;

public class OfficeMaper : Profile
{
    public OfficeMaper()
    {
        CreateMap<OfficeCreatedEvent, Office>();
        CreateMap<OfficeUpdatedEvent, Office>();
        CreateMap<OfficeCheckConsistancyEvent, Office>();
    }
}
