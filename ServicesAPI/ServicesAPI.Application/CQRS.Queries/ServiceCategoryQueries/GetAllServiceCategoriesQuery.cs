using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.CQRS.Queries.ServiceCategoryQueries;

public class GetAllServiceCategoriesQuery : IRequest<ResponseMessage<IEnumerable<ServiceCategoryTableInfoDTO>>>
{
    public ServiceCategoryParameters ServiceCategoryParameters { get; set; } = new ServiceCategoryParameters();
}
