using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.ServiceCategoryQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.ServiceCategoryQueryHandlers;

public class GetServiceCategoryByIdQueryHandler : IRequestHandler<GetServiceCategoryByIdQuery, ResponseMessage<ServiceCategoryInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetServiceCategoryByIdQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceCategoryInfoDTO>> Handle(GetServiceCategoryByIdQuery request, CancellationToken cancellationToken)
    {
        var serviceCategory = await _repositoryManager.ServiceCategory.GetByIdAsync(request.Id);
        if (serviceCategory is null)
        {
            return new ResponseMessage<ServiceCategoryInfoDTO>("Service Category not Found!", 404);
        }
        var serviceCategoryInfoDTO = _mapper.Map<ServiceCategoryInfoDTO>(serviceCategory);

        return new ResponseMessage<ServiceCategoryInfoDTO>(serviceCategoryInfoDTO);
    }
}
