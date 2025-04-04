using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Queries.ServiceQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.ServiceQueryHandlers;

public class GetServiceByIdQueryHandler : IRequestHandler<GetServiceByIdQuery, ResponseMessage<ServiceInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public GetServiceByIdQueryHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceInfoDTO>> Handle(GetServiceByIdQuery request, CancellationToken cancellationToken)
    {
        // add lazy or Exiplicit loading for service category and maybe specizaaliztions
        var service = await _repositoryManager.Service.GetByIdAsync(request.Id);
        if (service is null)
        {
            return new ResponseMessage<ServiceInfoDTO>("Service not Found!", 404);
        }
        var serviceInfoDTO = _mapper.Map<ServiceInfoDTO>(service);

        return new ResponseMessage<ServiceInfoDTO>(serviceInfoDTO);
    }
}
