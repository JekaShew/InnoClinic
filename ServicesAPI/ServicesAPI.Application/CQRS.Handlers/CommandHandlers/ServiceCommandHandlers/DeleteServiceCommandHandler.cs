using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;
using ServicesAPI.Application.CQRS.Commands.ServiceCommands;
using ServicesAPI.Domain.Data.IRepositories;

namespace ServicesAPI.Application.CQRS.Handlers.CommandHandlers.ServiceCommandHandlers;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;

    public DeleteServiceCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ResponseMessage> Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var service = await _repositoryManager.Service.GetByIdAsync(request.Id);
        if (service is null)
        {
            return new ResponseMessage("Service not Found!", 404);
        }

        await _repositoryManager.Service.DeleteAsync(service);

        return new ResponseMessage();
    }
}
