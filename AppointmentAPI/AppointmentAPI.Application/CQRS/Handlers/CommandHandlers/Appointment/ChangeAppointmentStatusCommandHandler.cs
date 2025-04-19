using AppointmentAPI.Application.CQRS.Commands.Appointment;
using AppointmentAPI.Domain.IRepositories;
using AutoMapper;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Handlers.CommandHandlers.Appointment;

public class ChangeAppointmentStatusCommandHandler : IRequestHandler<ChangeAppointmentStatusCommand, ResponseMessage>
{
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;

    public ChangeAppointmentStatusCommandHandler(IRepositoryManager repositoryManager, IMapper mapper)
    {
        _repositoryManager = repositoryManager;
        _mapper = mapper;
    }

    public async Task<ResponseMessage> Handle(ChangeAppointmentStatusCommand request, CancellationToken cancellationToken)
    {
        var appointment = await _repositoryManager.Appointment.GetByIdAsync(request.AppointmentId);
        if(appointment is null)
        {
            return new ResponseMessage("Appointment not Found!", 404);
        }

        appointment.Status = request.AppointmentStatus;
        await _repositoryManager.Appointment.UpdateAsync(appointment.Id, appointment);
        await _repositoryManager.CommitAsync();

        return new ResponseMessage();
    }
}
