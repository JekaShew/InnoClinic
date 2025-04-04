using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCommandHandlers;

public class CreateServiceCommandHandler : IRequestHandler<CreateServiceCommand, ResponseMessage<ServiceInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public CreateServiceCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceInfoDTO>> Handle(CreateServiceCommand request, CancellationToken cancellationToken)
    {
        var service = _mapper.Map<Service>(request.serviceForCreateDTO);
        await _repositoryManager.Service.CreateAsync(service);
        await _repositoryManager.CommitAsync();
        var serviceInfoDTO = _mapper.Map<ServiceInfoDTO>(service);

        return new ResponseMessage<ServiceInfoDTO>(serviceInfoDTO);
    }
}
