using AuthorizationAPI.Application.CQS.Queries.RoleQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RoleHandlers.QueriesHandlers
{
    public class TakeRoleDTOListQueryHandler : IRequestHandler<TakeRoleDTOListQuery, List<RoleDTO>>
    {
        private readonly IRole _roleRepository;
        public TakeRoleDTOListQueryHandler(IRole roleRepository)
        {
            _roleRepository = roleRepository;
        }
        public async Task<List<RoleDTO>> Handle(TakeRoleDTOListQuery request, CancellationToken cancellationToken)
        {
            return await _roleRepository.TakeAllRoles();
        }
    }
}
