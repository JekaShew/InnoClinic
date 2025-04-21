using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.Service;

public class RequestCheckServicesConsistancyQuery : IRequest<ResponseMessage>
{
}
