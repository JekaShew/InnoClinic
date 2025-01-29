using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands
{
    public class RevokeRTokenByRTokenIdCommand : IRequest<CustomResponse>
    {
        public Guid RTokenId { get; set; }
    }
}
