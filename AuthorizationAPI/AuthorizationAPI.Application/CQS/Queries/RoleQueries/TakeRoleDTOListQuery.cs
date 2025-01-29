using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.RoleQueries
{
    public class TakeRoleDTOListQuery : IRequest<List<RoleDTO>>
    {
    }
}
