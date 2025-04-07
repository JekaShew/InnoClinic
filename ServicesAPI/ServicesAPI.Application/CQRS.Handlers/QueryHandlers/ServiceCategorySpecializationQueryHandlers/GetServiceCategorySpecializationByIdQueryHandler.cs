using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.ServiceCategorySpecializationQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.ServiceCategorySpecializationQueryHandlers;

public class GetServiceCategorySpecializationByIdQueryHandler : IRequestHandler<GetServiceCategorySpecializationByIdQuery, ResponseMessage<ServiceCategorySpecializationInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetServiceCategorySpecializationByIdQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceCategorySpecializationInfoDTO>> Handle(GetServiceCategorySpecializationByIdQuery request, CancellationToken cancellationToken)
    {
        var serviceCattegorySpecialization = await _repositoryManager.ServiceCategorySpecialization.GetByIdAsync(request.Id, s => s.Specialization, sc => sc.ServiceCategory); 
        if(serviceCattegorySpecialization is null)
        {
            return new ResponseMessage<ServiceCategorySpecializationInfoDTO>("Related Service Category and Specialization not Found!", 404);
        }

        var serviceCategorySpecializationInfoDTO = _mapper.Map<ServiceCategorySpecializationInfoDTO>(serviceCattegorySpecialization);

        return new ResponseMessage<ServiceCategorySpecializationInfoDTO>(serviceCategorySpecializationInfoDTO);
    }
}
