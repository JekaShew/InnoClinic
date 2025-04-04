using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.CQRS.Queries.ServiceCategoryQueries;

public class GetServiceCategoryByIdQuery : IRequest<ResponseMessage<ServiceCategoryInfoDTO>>
{
    public Guid Id { get; set; }
}
