using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.Doctor;

public class RequestCheckDoctorsConsistancyQuery : IRequest<ResponseMessage>
{
}
