using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries
{
    public class TakeRefreshTokenDTOListQuery : IRequest<List<RefreshTokenDTO>>
    {
    }
}
