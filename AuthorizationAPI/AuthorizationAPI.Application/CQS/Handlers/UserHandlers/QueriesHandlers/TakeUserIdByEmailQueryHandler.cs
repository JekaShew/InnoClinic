using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class TakeUserIdByEmailQueryHandler : IRequestHandler<TakeUserIdByEmailQuery, Guid>
    {
        private readonly IUser _userRepository;
        public TakeUserIdByEmailQueryHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<Guid> Handle(TakeUserIdByEmailQuery request, CancellationToken cancellationToken)
        {
            var userDTO = await _userRepository.TakeUserWithPredicate(u => u.Email.Equals(request.Email));
            return userDTO.Id;
        }
    }
}
