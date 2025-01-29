using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class TakeUserIdByEmailQuery : IRequest<Guid>
    {
        public string Email {  get; set; }  
    }
}
