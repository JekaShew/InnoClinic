using AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.RefreshTokenHandlers.QueriesHandlers
{
    public class IsRTokenCorrectByRTokenIdQueryHandlers : IRequestHandler<IsRTokenCorrectByRTokenIdQuery, CustomResponse>
    {
        private readonly IRefreshToken _refreshTokenRepository;
        public IsRTokenCorrectByRTokenIdQueryHandlers(IRefreshToken refreshTokenRepository)
        {
            _refreshTokenRepository = refreshTokenRepository;
        }
        public async Task<CustomResponse> Handle(IsRTokenCorrectByRTokenIdQuery request, CancellationToken cancellationToken)
        {
            var rTokenDTO = await _refreshTokenRepository.TakeRefreshTokenByRTokenId(request.RTokenId);

            if (rTokenDTO is null)
                return new CustomResponse(false, "Refresh Token Not Found!");

            if (rTokenDTO.IsRevoked == true)
                return new CustomResponse(false, "Refresh Token is Revoked!");

            if (rTokenDTO.ExpireDate <= DateTime.UtcNow || rTokenDTO.ExpireDate == null)
                return new CustomResponse(false, "Refresh Token Expired!");

            return new CustomResponse(true, "Refresh Token is Correct!");
        }
    }
}
