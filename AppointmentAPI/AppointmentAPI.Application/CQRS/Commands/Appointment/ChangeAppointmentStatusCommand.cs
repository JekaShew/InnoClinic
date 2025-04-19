using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Appointment;

public class ChangeAppointmentStatusCommand : IRequest<ResponseMessage>
{
    public Guid AppointmentId { get; set; }
    public AppointmentStatus AppointmentStatus { get; set; }
}
