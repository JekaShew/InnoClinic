using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Queries.SpecializationQueries;

public class GetSpecializationByIdQuery : IRequest<ResponseMessage<SpecializationInfoDTO>>
{
    public Guid Id { get; set; }
}
