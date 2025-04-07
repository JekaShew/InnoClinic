using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.ServiceCategorySpecializationQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.ServiceCategorySpecializationQueryHandlers;

public class GetAllServiceCategorySpecializationQueryHandler : IRequestHandler<GetAllServiceCategorySpecializationQuery, ResponseMessage<IEnumerable<ServiceCategorySpecializationInfoDTO>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAllServiceCategorySpecializationQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<IEnumerable<ServiceCategorySpecializationInfoDTO>>> Handle(GetAllServiceCategorySpecializationQuery request, CancellationToken cancellationToken)
    {
        var serviceCategorySpecializations = await _repositoryManager.ServiceCategorySpecialization.GetAllAsync(request.ServiceCategorySpecializationParameters);
        var serviceCategorySpecializationInfoDTOs = _mapper.Map<IEnumerable<ServiceCategorySpecializationInfoDTO>>(serviceCategorySpecializations);

        return new ResponseMessage<IEnumerable<ServiceCategorySpecializationInfoDTO>>(serviceCategorySpecializationInfoDTOs)
    }
}
