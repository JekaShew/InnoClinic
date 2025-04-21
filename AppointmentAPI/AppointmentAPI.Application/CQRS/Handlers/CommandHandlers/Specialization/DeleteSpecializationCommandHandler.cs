using AppointmentAPI.Application.CQRS.Commands.Specialization;
using AppointmentAPI.Domain.IRepositories;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Specialization;

public class DeleteSpecializationCommandHandler : IRequestHandler<DeleteSpecializationCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public DeleteSpecializationCommandHandler(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task Handle(DeleteSpecializationCommand request, CancellationToken cancellationToken)
    {
        var specializationToDelete = await _repositoryManager.Specialization.GetByIdAsync(request.Id);

        if (specializationToDelete is not null)
        {
            await _repositoryManager.Specialization.SoftDeleteAsync(specializationToDelete);
            _logger.Information($"Succesfully deleted Specialization with Id: {request.Id} !");
        }
    }
}
