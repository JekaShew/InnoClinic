using AppointmentAPI.Application.CQRS.Commands.Appointment;
using AppointmentAPI.Domain.IRepositories;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Appointment;

public class DeleteAppointmentCommandHandler : IRequestHandler<DeleteAppointmentCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;

    public DeleteAppointmentCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ResponseMessage> Handle(DeleteAppointmentCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _repositoryManager.Appointment.GetByIdAsync(request.Id);
        if (appointment is null)
        {
            return new ResponseMessage("Appointment not Found!", 404);
        }

        await _repositoryManager.Appointment.DeleteAsync(appointment);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }
}
