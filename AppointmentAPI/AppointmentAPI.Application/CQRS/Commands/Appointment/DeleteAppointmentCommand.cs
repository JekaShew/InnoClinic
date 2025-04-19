using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Appointment;

public class DeleteAppointmentCommand : IRequest<ResponseMessage>
{
    public Guid Id { get; set; }
}
