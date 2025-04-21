using AppointmentAPI.Application.CQRS.Commands.Patient;
using AppointmentAPI.Domain.IRepositories;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Paient;

public class DeletePatientCommandHandler : IRequestHandler<DeletePatientCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;

    public DeletePatientCommandHandler(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task Handle(DeletePatientCommand request, CancellationToken cancellationToken)
    {
        var patientToDelete = await _repositoryManager.Patient.GetByIdAsync(request.Id);

        if (patientToDelete is not null)
        {
            await _repositoryManager.Patient.SoftDeleteAsync(patientToDelete);
            _logger.Information($"Succesfully deleted Patient with Id: {request.Id} !");
        }
    }
}
