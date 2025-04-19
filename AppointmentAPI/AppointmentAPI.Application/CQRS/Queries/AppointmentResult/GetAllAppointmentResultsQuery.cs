using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.AppointmentResult;

public class GetAllAppointmentResultsQuery : IRequest<ResponseMessage<IEnumerable<AppointmentResultTableInfoDTO>>>
{
    public AppointmentResultParameters AppointmentResultParameters { get; set; } = new AppointmentResultParameters();
}
