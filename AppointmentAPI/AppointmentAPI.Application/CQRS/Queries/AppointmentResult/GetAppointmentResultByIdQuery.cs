using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.AppointmentResult;

public class GetAppointmentResultByIdQuery : IRequest<ResponseMessage<AppointmentResultInfoDTO>>
{
    public Guid Id { get; set; }
}
