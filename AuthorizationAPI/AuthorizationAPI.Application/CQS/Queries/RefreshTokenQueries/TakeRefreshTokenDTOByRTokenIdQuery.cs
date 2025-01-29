using AuthorizationAPI.Application.DTOs;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries
{
    public class TakeRefreshTokenDTOByRTokenIdQuery :IRequest<RefreshTokenDTO>
    {
        public Guid  Id { get; set; }
    }
}
