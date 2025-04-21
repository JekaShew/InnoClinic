using AppointmentAPI.Application.CQRS.Commands.Office;
using AppointmentAPI.Domain.IRepositories;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Office;

public class DeleteOfficeCommandHandler : IRequestHandler<DeleteOfficeCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public DeleteOfficeCommandHandler(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task Handle(DeleteOfficeCommand request, CancellationToken cancellationToken)
    {
        var officeToDelete = await _repositoryManager.Office.GetByIdAsync(request.Id);

        if (officeToDelete is not null)
        {
            await _repositoryManager.Office.SoftDeleteAsync(officeToDelete);
            _logger.Information($"Succesfully deleted Office with Id: {request.Id} !");
        }
    }
}
