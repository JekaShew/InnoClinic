using AppointmentAPI.Application.CQRS.Commands.Doctor;
using AppointmentAPI.Domain.IRepositories;
using MediatR;
using Serilog;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Doctor;

public class DeleteDoctorCommandHandler : IRequestHandler<DeleteDoctorCommand>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly ILogger _logger;


    public DeleteDoctorCommandHandler(IRepositoryManager repositoryManager, ILogger logger)
    {
        _repositoryManager = repositoryManager;
        _logger = logger;
    }

    public async Task Handle(DeleteDoctorCommand request, CancellationToken cancellationToken)
    {
        var doctorToDelete = await _repositoryManager.Doctor.GetByIdAsync(request.Id);

        if (doctorToDelete is not null)
        {
            await _repositoryManager.Doctor.SoftDeleteAsync(doctorToDelete);
            _logger.Information($"Succesfully deleted Doctor with Id: {request.Id}!");
        }
    }
}
