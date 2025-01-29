using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class TakeUserEmailByUserIdQueryHandler : IRequestHandler<TakeUserEmailByUserIdQuery, string>
    {
        private readonly IUser _userRepository;
        public TakeUserEmailByUserIdQueryHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<string> Handle(TakeUserEmailByUserIdQuery request, CancellationToken cancellationToken)
        {
            var userDTO = await _userRepository.TakeUserById(request.UserId);
            return userDTO.Email;
        }
    }
}
