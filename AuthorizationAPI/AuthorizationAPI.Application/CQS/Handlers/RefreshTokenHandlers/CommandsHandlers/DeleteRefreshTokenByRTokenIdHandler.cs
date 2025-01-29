using AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.CommandsHandlers
{
    public class DeleteRefreshTokenByRTokenIdHandler : IRequestHandler<DeleteRefreshTokenByRTokenIdCommand, CustomResponse>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public DeleteRefreshTokenByRTokenIdHandler(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;   
        }
        public async Task<CustomResponse> Handle(DeleteRefreshTokenByRTokenIdCommand request, CancellationToken cancellationToken)
        {
            return await _refreshTokenRepository.DeleteRefreshTokenByRTokenId(request.Id);
        }
    }
}
