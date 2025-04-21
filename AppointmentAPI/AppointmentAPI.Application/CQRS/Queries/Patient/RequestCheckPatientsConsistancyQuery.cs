using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.Patient;

public class RequestCheckPatientsConsistancyQuery : IRequest<ResponseMessage>
{
}
