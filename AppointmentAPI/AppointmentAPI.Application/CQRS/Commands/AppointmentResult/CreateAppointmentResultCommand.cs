using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.AppointmentResult;

public class CreateAppointmentResultCommand : IRequest<ResponseMessage<AppointmentResultInfoDTO>>
{
    public AppointmentResultForCreateDTO? AppointmentResultForCreateDTO { get; set; }
}
