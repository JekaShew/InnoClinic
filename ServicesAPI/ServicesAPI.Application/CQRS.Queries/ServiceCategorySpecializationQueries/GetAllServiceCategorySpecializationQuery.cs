using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Queries.ServiceCategorySpecializationQueries;

public class GetAllServiceCategorySpecializationQuery : IRequest<ResponseMessage<IEnumerable<ServiceCategorySpecializationInfoDTO>>>
{
    public ServiceCategorySpecializationParameters ServiceCategorySpecializationParameters { get; set; } = new ServiceCategorySpecializationParameters();
}
