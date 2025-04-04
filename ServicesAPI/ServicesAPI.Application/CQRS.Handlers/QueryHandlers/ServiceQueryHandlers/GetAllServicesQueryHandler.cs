using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.ServiceQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.ServiceQueryHandlers;

internal class GetAllServicesQueryHandler : IRequestHandler<GetAllServicesQuery, ResponseMessage<IEnumerable<ServiceTableInfoDTO>>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetAllServicesQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<IEnumerable<ServiceTableInfoDTO>>> Handle(GetAllServicesQuery request, CancellationToken cancellationToken)
    {
        var services = await _repositoryManager.Service.GetAllWithParametersAsync(request.ServiceParameters);
        var serviceTableInfoDTOs = _mapper.Map<IEnumerable<ServiceTableInfoDTO>>(services);

        return new ResponseMessage<IEnumerable<ServiceTableInfoDTO>>(serviceTableInfoDTOs);
    }
}
