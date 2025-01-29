using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class TakeUserEmailByUserIdQuery : IRequest<string>
    {
        public Guid UserId { get; set; }
    }
}
