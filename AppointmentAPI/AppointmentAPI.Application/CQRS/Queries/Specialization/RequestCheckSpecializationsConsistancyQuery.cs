using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.Specialization;

public class RequestCheckSpecializationsConsistancyQuery : IRequest<ResponseMessage>
{
}
