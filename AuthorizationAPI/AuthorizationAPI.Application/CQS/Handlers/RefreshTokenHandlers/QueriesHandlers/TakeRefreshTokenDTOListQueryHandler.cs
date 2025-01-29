using AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.QueriesHandlers
{
    public class TakeRefreshTokenDTOListQueryHandler : IRequestHandler<TakeRefreshTokenDTOListQuery, List<RefreshTokenDTO>>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public TakeRefreshTokenDTOListQueryHandler(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<List<RefreshTokenDTO>> Handle(TakeRefreshTokenDTOListQuery request, CancellationToken cancellationToken)
        {
            return await _refreshTokenRepository.TakeAllRefreshTokens();
        }
    }
}
