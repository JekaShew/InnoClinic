using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.ServiceCategoryQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.ServiceCategoryQueryHandlers;

public class GetAllServiceCategoriesQueryHandler : IRequestHandler<GetAllServiceCategoriesQuery, ResponseMessage<IEnumerable<ServiceCategoryTableInfoDTO>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAllServiceCategoriesQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<IEnumerable<ServiceCategoryTableInfoDTO>>> Handle(GetAllServiceCategoriesQuery request, CancellationToken cancellationToken)
    {
        var serviceCategories = await _repositoryManager.ServiceCategory.GetAllWithParametersAsync(request.ServiceCategoryParameters);
        var serviceCategoryTableInfoDTOs = _mapper.Map<IEnumerable<ServiceCategoryTableInfoDTO>>(serviceCategories);

        return new ResponseMessage<IEnumerable<ServiceCategoryTableInfoDTO>>(serviceCategoryTableInfoDTOs);
    }
}
