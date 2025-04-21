using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.Office;

public class RequestCheckOfficesConsistancyQuery : IRequest<ResponseMessage>
{
}
