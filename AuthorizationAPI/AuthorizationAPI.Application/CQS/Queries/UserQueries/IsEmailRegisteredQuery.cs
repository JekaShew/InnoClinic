using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.UserQueries
{
    public class IsEmailRegisteredQuery : IRequest<CustomResponse>
    {
        public string EnteredEmail { get; set; }
    }
}
