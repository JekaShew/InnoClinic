using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCommands;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCommandHandlers;

public class UpdateServiceCommandHandler : IRequestHandler<UpdateServiceCommand, ResponseMessage<ServiceInfoDTO>>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public UpdateServiceCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage<ServiceInfoDTO>> Handle(UpdateServiceCommand request, CancellationToken cancellationToken)
    {
        // LAzya loading service category
        var service = await _repositoryManager.Service.GetByIdAsync(request.ServiceId);
        if (service is null)
        {
            return new ResponseMessage<ServiceInfoDTO>("Service not Found!", 404);
        }

        service = _mapper.Map<Service>(request.ServiceForUpdateDTO);
        await _repositoryManager.Service.UpdateAsync(request.ServiceId, service);
        await _repositoryManager.CommitAsync();
        var serviceInfoDTO = _mapper.Map<ServiceInfoDTO>(service);

        return new ResponseMessage<ServiceInfoDTO>(serviceInfoDTO);
    }
}
