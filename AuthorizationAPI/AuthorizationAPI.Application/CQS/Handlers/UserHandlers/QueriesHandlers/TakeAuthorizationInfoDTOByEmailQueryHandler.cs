using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class TakeAuthorizationInfoDTOByEmailQueryHandler : IRequestHandler<TakeAuthorizationInfoDTOByEmailQuery, AuthorizationInfoDTO>
    {
        private readonly IUser _userRepository;
        public TakeAuthorizationInfoDTOByEmailQueryHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<AuthorizationInfoDTO> Handle(TakeAuthorizationInfoDTOByEmailQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.TakeAuthorizationInfoByEmail(request.Email);
        }
    }
}
