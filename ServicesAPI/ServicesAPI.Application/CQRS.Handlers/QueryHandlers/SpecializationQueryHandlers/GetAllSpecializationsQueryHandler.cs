using MediatR;
using ServicesAPI.Application.CQRS.Queries.SpecializationQueries;
using ServicesAPI.Domain.Data.IRepositories;
using ServicesAPI.Domain.Data.Models;

namespace ServicesAPI.Application.CQRS.Handlers.QueryHandlers.SpecializationQueryHandlers;

internal class GetAllSpecializationsQueryHandler : IRequestHandler<GetAllSpecializationsQuery, IEnumerable<Specialization>>
{
    private readonly IRepositoryManager _repositoryManager;

    public GetAllSpecializationsQueryHandler(IRepositoryManager repositoryManager)
    {
        _repositoryManager = repositoryManager;
    }

    public async Task<IEnumerable<Specialization>> Handle(GetAllSpecializationsQuery request, CancellationToken cancellationToken)
    {
        var specializations = await _repositoryManager.Specialization.GetAllAsync();

        return specializations;
    }   
}
