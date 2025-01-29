using AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.CommandsHandlers
{
    public class RevokeRTokenByRTokenIdCommandHandler : IRequestHandler<RevokeRTokenByRTokenIdCommand, CustomResponse>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public RevokeRTokenByRTokenIdCommandHandler(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;    
        }
        public async Task<CustomResponse> Handle(RevokeRTokenByRTokenIdCommand request, CancellationToken cancellationToken)
        {
            var rTokenDTO = await _refreshTokenRepository.TakeRefreshTokenByRTokenId(request.RTokenId);
            if (rTokenDTO is null)
                return new CustomResponse(false, "No Refresh token found!");
            rTokenDTO.IsRevoked = false;

            return await _refreshTokenRepository.UpdateRefreshToken(rTokenDTO);
        }
    }
}
