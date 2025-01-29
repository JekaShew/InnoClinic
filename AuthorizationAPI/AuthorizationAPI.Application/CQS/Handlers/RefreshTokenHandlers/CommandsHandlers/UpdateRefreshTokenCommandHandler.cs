using AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.CommandsHandlers
{
    public class UpdateRefreshTokenCommandHandler : IRequestHandler<UpdateRefreshTokenCommand, CustomResponse>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public UpdateRefreshTokenCommandHandler(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<CustomResponse> Handle(UpdateRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            return await _refreshTokenRepository.UpdateRefreshToken(request.RefreshTokenDTO);
        }
    }
}
