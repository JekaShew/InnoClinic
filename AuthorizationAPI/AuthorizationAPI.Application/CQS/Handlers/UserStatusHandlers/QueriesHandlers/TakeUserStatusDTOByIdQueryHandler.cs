using AuthorizationAPI.Application.CQS.Queries.UserStatusQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserStatusHandlers.QueriesHandlers
{
    public class TakeUserStatusDTOByIdQueryHandler : IRequestHandler<TakeUserStatusDTOByIdQuery, UserStatusDTO>
    {
        private readonly IUserStatus _userStatusRepository;
        public TakeUserStatusDTOByIdQueryHandler(IUserStatus userStatusRepository)
        {
            _userStatusRepository = userStatusRepository;
        }
        public async Task<UserStatusDTO> Handle(TakeUserStatusDTOByIdQuery request, CancellationToken cancellationToken)
        {
            return await _userStatusRepository.TakeUserStatusById(request.Id);
        }
    }
}
