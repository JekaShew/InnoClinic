using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Queries.ServiceQueries;

public class GetServiceByIdQuery : IRequest<ResponseMessage<ServiceInfoDTO>>
{
    public Guid Id { get; set; }
}
