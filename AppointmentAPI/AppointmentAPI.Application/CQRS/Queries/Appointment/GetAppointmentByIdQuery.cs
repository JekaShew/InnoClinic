using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.Appointment;

public class GetAppointmentByIdQuery : IRequest<ResponseMessage<AppointmentInfoDTO>>
{
    public Guid Id { get; set; }
}
