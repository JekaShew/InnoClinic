using InnoClinic.CommonLibrary.Response;
using MediatR;

namespace AppointmentAPI.Application.CQRS.Queries.DoctorSpecialization;

public class RequestCheckDoctorSpecializationsConsistancyQuery : IRequest<ResponseMessage>
{
}
