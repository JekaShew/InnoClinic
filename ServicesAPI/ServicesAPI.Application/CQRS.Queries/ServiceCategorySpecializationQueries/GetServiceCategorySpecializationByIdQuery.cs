using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Queries.ServiceCategorySpecializationQueries;

public class GetServiceCategorySpecializationByIdQuery : IRequest<ResponseMessage<ServiceCategorySpecializationInfoDTO>>
{
    public Guid Id { get; set; }
}
