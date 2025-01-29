using AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.QueriesHandlers
{
    public class TakeUserIdByRTokenIdQueryHandler : IRequestHandler<TakeUserIdByRTokenIdQuery, Guid>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public TakeUserIdByRTokenIdQueryHandler(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;  
        }
        public async Task<Guid> Handle(TakeUserIdByRTokenIdQuery request, CancellationToken cancellationToken)
        {
            var rTokenDTO = await _refreshTokenRepository.TakeRefreshTokenByRTokenId(request.RTokenId);
            
            return rTokenDTO.UserId;
        }
    }
}
