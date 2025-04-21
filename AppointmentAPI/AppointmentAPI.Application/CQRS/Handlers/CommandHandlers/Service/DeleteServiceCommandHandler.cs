using AppointmentAPI.Application.CQRS.Commands.Service;
using AppointmentAPI.Domain.IRepositories;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Service;

public class DeleteServiceCommandHandler : IRequestHandler<DeleteServiceCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public DeleteServiceCommandHandler(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task Handle(DeleteServiceCommand request, CancellationToken cancellationToken)
    {
        var serviceToDelete = await _repositoryManager.Service.GetByIdAsync(request.Id);

        if (serviceToDelete is not null)
        {
            await _repositoryManager.Service.SoftDeleteAsync(serviceToDelete);
            _logger.Information($"Succesfully deleted Service with Id: {request.Id} !");
        }
    }
}
