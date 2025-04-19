using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Appointment;

public class UpdateAppointmentCommand : IRequest<ResponseMessage<AppointmentInfoDTO>>
{
    public Guid AppointmentId { get; set; }
    public AppointmentForUpdateDTO? AppointmentForUpdateDTO { get; set; }
}
