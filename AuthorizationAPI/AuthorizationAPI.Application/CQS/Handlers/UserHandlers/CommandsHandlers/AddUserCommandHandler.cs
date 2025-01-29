using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.Interfaces;
using AuthorizationAPI.Application.Mappers;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.CommandsHandlers
{
    public class AddUserCommandHandler : IRequestHandler<AddUserCommand, CustomResponse>
    {
        private readonly IUser _userRepository;
        private readonly IUserServices _userServices;
        private readonly IUserStatus _userStatusRepository;
        private readonly IRole _roleRepository;
        public AddUserCommandHandler(IUserServices userServices, IUser userRepository,IUserStatus userStatusRepository, IRole roleRepository)
        {
            _userServices = userServices;
            _userRepository = userRepository;
            _userStatusRepository = userStatusRepository;
            _roleRepository = roleRepository;   
        }
        public async Task<CustomResponse> Handle(AddUserCommand request, CancellationToken cancellationToken)
        {
            var userStatusDTO = await _userStatusRepository.TakeUserStatusDTOWithPredicate(us => us.Title == "Activated");
            if (userStatusDTO is null)
                return new CustomResponse(false, "There is no Default User Status named Activated in DB!");

            var roleDTO = await _roleRepository.TakeRoleDTOWithPredicate(us => us.Title == "Patient");
            if (roleDTO is null)
                return new CustomResponse(false, "There is no Default Role named Patient in DB!");

            var securityStamp = await _userServices.GetHashString(request.RegistrationInfoDTO.SecretPhrase);
            var secretPhraseHash = await _userServices.GetHashString($"{request.RegistrationInfoDTO.SecretPhrase}{securityStamp}");
            var passwordHash = await _userServices.GetHashString($"{request.RegistrationInfoDTO.Password}{securityStamp}");

            var userDetailedDTO = UserMapper.RegistrationInfoDTOToUserDetailedDTO(request.RegistrationInfoDTO);
            userDetailedDTO.Id= Guid.NewGuid();
            userDetailedDTO.RegistrationDate = DateTime.UtcNow;

            userDetailedDTO.SecurityStamp = securityStamp;
            userDetailedDTO.SecretPhraseHash = secretPhraseHash;
            userDetailedDTO.PasswordHash = passwordHash;

            userDetailedDTO.RoleId = roleDTO.Id;
            userDetailedDTO.UserStatusId = userStatusDTO.Id;

            return await _userRepository.AddUser(userDetailedDTO);
        }
    }
}
