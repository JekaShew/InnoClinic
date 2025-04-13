using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.SpecializationQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.SpecializationQueryHandlers;

public class GetAllSpecializationWithParametersQueryHandler : IRequestHandler<GetAllSpecializationsWithParametersQuery, ResponseMessage<IEnumerable<SpecializationTableInfoDTO>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAllSpecializationWithParametersQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<IEnumerable<SpecializationTableInfoDTO>>> Handle(GetAllSpecializationsWithParametersQuery request, CancellationToken cancellationToken)
    {
        var specializations = await _repositoryManager.Specialization.GetAllWithParametersAsync(request.SpecializationParameters);
        var specializationTableInfoDTOs = _mapper.Map<IEnumerable<SpecializationTableInfoDTO>?>(specializations);

        return new ResponseMessage<IEnumerable<SpecializationTableInfoDTO>>(specializationTableInfoDTOs);
    }
}
