using AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.CommandsHandlers
{
    public class AddRefreshTokenCommandHandler : IRequestHandler<AddRefreshTokenCommand, CustomResponse>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public AddRefreshTokenCommandHandler(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<CustomResponse> Handle(AddRefreshTokenCommand request, CancellationToken cancellationToken)
        {
            request.RefreshTokenDTO.Id = Guid.NewGuid();

            return await _refreshTokenRepository.AddRefreshToken(request.RefreshTokenDTO);
        }
    }
}
