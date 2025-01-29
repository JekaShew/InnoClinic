using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class TakeUserDTOListQueryHandler : IRequestHandler<TakeUserDTOListQuery, List<UserDTO>>
    {
        private readonly IUser _userRepository;
        public TakeUserDTOListQueryHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<List<UserDTO>> Handle(TakeUserDTOListQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.TakeAllUsers();
        }
    }
}
