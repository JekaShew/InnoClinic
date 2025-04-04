using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCommands;
using ServicesAPI.Domain.Data.IRepositories;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCommandHandlers;

public class ChangeServiceStatusCommandHandler : IRequestHandler<ChangeServiceStatusCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;

    public ChangeServiceStatusCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ResponseMessage> Handle(ChangeServiceStatusCommand request, CancellationToken cancellationToken)
    {
        var service = await _repositoryManager.Service.GetByIdAsync(request.ServiceId);
        if (service is null)
        {
            return new ResponseMessage("Service not Found!", 404);
        }

        service.IsActive = !service.IsActive;
        await _repositoryManager.Service.UpdateAsync(request.ServiceId, service);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }
}

