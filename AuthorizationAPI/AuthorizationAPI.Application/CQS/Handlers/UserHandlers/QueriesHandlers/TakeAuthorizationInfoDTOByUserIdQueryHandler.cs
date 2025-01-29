using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class TakeAuthorizationInfoDTOByUserIdQueryHandler : IRequestHandler<TakeAuthorizationInfoDTOByUserIdQuery, AuthorizationInfoDTO>
    {
        private readonly IUser _userRepository;
        public TakeAuthorizationInfoDTOByUserIdQueryHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<AuthorizationInfoDTO> Handle(TakeAuthorizationInfoDTOByUserIdQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.TakeAuthorizationInfoByUserId(request.UserId);
        }
    }
}
