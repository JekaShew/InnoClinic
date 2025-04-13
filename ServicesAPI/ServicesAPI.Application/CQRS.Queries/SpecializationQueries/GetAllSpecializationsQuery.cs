using MediatR;
using ServicesAPI.Domain.Data.Models;

namespace ServicesAPI.Application.CQRS.Queries.SpecializationQueries;

public class GetAllSpecializationsQuery : IRequest<IEnumerable<Specialization>>
{
}
