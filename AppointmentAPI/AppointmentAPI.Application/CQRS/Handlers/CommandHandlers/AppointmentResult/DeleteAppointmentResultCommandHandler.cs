using AppointmentAPI.Application.CQRS.Commands.AppointmentResult;
using AppointmentAPI.Domain.IRepositories;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.AppointmentResult;

public class DeleteAppointmentResultCommandHandler : IRequestHandler<DeleteAppointmentResultCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;

    public DeleteAppointmentResultCommandHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<ResponseMessage> Handle(DeleteAppointmentResultCommand request, CancellationToken cancellationToken)
    {
        var appointmentResult = await _repositoryManager.AppointmentResult.GetByIdAsync(request.Id);
        if (appointmentResult is null)
        {
            return new ResponseMessage("AppointmentResult not Found!", 404);
        }

        await _repositoryManager.AppointmentResult.DeleteAsync(appointmentResult);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }

}
