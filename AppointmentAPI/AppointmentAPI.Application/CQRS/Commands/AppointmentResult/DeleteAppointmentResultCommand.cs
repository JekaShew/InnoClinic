using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.AppointmentResult;

public class DeleteAppointmentResultCommand : IRequest<ResponseMessage>
{
    public Guid Id { get; set; }
}
