using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Queries.ServiceQueries;

public class GetAllServicesQuery : IRequest<ResponseMessage<IEnumerable<ServiceTableInfoDTO>>>
{
    public ServiceParameters ServiceParameters { get; set; } = new ServiceParameters();
}
