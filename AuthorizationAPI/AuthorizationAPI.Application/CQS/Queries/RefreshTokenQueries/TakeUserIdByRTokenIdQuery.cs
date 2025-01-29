using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries
{
    public class TakeUserIdByRTokenIdQuery : IRequest<Guid>
    {
        public Guid RTokenId { get; set; }
    }
}
