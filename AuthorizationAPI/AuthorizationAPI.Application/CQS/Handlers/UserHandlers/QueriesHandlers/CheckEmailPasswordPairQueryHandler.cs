using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class CheckEmailPasswordPairQueryHandler : IRequestHandler<CheckEmailPasswordPairQuery, CustomResponse>
    {
        private readonly IUser _userRepository;
        private readonly IUserServices _userServices;
        public CheckEmailPasswordPairQueryHandler(IUser userRepository, IUserServices userServices)
        {
            _userRepository = userRepository;
            _userServices = userServices;
        }
        public async Task<CustomResponse> Handle(CheckEmailPasswordPairQuery request, CancellationToken cancellationToken)
        {
            var authorizationInfoDTO = await _userRepository
                   .TakeAuthorizationInfoByEmail(request.LoginInfoDTO.Email);
            var enteredPasswordHash = await _userServices
                    .GetHashString($"{request.LoginInfoDTO.Password}{authorizationInfoDTO.SecurityStamp}");
            if (!enteredPasswordHash.Equals(authorizationInfoDTO.SecretPhraseHash))
                return new CustomResponse(false, "Check credetials you have entered! Wrong Email or Password!");
            return new CustomResponse(true, "Entered Credentials are correct!");
        }
    }
}
