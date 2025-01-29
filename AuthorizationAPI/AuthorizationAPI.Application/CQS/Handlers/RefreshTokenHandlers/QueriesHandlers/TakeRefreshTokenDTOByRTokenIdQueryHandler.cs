using AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.QueriesHandlers
{
    public class TakeRefreshTokenDTOByRTokenIdQueryHandler : IRequestHandler<TakeRefreshTokenDTOByRTokenIdQuery, RefreshTokenDTO>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public TakeRefreshTokenDTOByRTokenIdQueryHandler(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<RefreshTokenDTO> Handle(TakeRefreshTokenDTOByRTokenIdQuery request, CancellationToken cancellationToken)
        {
            return await _refreshTokenRepository.TakeRefreshTokenByRTokenId(request.Id);
        }
    }
}
