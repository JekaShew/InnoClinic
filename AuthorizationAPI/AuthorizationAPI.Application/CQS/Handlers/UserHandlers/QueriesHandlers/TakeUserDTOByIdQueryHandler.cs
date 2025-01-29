using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class TakeUserDTOByIdQueryHandler : IRequestHandler<TakeUserDTOByIdQuery, UserDTO>
    {
        private readonly IUser _userRepository;
        public TakeUserDTOByIdQueryHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<UserDTO> Handle(TakeUserDTOByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userRepository.TakeUserById(request.Id);
        }
    }
}
