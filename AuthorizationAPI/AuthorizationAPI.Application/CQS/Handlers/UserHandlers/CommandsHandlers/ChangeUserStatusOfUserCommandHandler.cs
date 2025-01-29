using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.CommandsHandlers
{
    public class ChangeUserStatusOfUserCommandHandler : IRequestHandler<ChangeUserStatusOfUserCommand, CustomResponse>
    {
        private readonly IUser _userRepository;
        private readonly IUserStatus _userStatusRepository;
        public ChangeUserStatusOfUserCommandHandler(IUser userRepository, IUserStatus userStatusRepository)
        {
            _userRepository = userRepository;
            _userStatusRepository = userStatusRepository;
        }
        public async Task<CustomResponse> Handle(ChangeUserStatusOfUserCommand request, CancellationToken cancellationToken)
        {
            var userStatusDTO = await _userStatusRepository.TakeUserStatusById(request.UserIdUserStatusIdPairDTO.UserStatusId);
            if (userStatusDTO is null)
                return new CustomResponse(false, "No such User Statuses!");

            var userDTO = await _userRepository.TakeUserById(request.UserIdUserStatusIdPairDTO.UserId);
            if (userDTO is null)
                return new CustomResponse(false, "No such Users!");
            
            userDTO.UserStatusId = request.UserIdUserStatusIdPairDTO.UserStatusId;

           return await _userRepository.UpdateUser(userDTO);
        }
    }
}
