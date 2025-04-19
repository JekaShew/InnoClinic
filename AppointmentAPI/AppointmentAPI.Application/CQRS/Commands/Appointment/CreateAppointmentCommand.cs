using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.Appointment;

public class CreateAppointmentCommand : IRequest<ResponseMessage<AppointmentInfoDTO>>
{
    public AppointmentForCreateDTO? AppointmentForCreateDTO { get; set; }
}
