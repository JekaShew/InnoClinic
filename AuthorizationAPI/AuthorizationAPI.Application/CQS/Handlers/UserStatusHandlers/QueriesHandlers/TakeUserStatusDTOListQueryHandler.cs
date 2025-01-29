using AuthorizationAPI.Application.CQS.Queries.UserStatusQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserStatusHandlers.QueriesHandlers
{
    public class TakeUserStatusDTOListQueryHandler : IRequestHandler<TakeUserStatusDTOListQuery, List<UserStatusDTO>>
    {
        private readonly IUserStatus _userStatusRepository;
        public TakeUserStatusDTOListQueryHandler(IUserStatus userStatusRepository)
        {
            _userStatusRepository = userStatusRepository;
        }
        public async Task<List<UserStatusDTO>> Handle(TakeUserStatusDTOListQuery request, CancellationToken cancellationToken)
        {
            return await _userStatusRepository.TakeAllUserStatuses();
        }
    }
}
