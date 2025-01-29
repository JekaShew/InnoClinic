using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries
{
    public class IsRTokenCorrectByRTokenIdQuery :IRequest<CustomResponse>
    {
        public Guid RTokenId { get; set; }  
    }
}
