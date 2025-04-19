using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.Appointment;

public class GetAllAppointmentsQuery : IRequest<ResponseMessage<IEnumerable<AppointmentTableInfoDTO>>>
{
    public AppointmentParameters AppointmentParameters { get; set; } = new AppointmentParameters();
}
