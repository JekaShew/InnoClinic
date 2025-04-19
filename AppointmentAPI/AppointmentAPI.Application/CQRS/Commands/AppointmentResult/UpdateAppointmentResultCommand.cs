using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Commands.AppointmentResult;

public class UpdateAppointmentResultCommand : IRequest<ResponseMessage<AppointmentResultInfoDTO>>
{
    public Guid AppointmentResultId { get; set; }
    public AppointmentResultForUpdateDTO? AppointmentResultForUpdateDTO { get; set; }
}
