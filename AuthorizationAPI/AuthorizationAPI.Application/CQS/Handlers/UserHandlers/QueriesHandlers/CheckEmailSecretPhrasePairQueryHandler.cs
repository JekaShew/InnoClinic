using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class CheckEmailSecretPhrasePairQueryHandler : IRequestHandler<CheckEmailSecretPhrasePairQuery, CustomResponse>
    {
        private readonly IUser _userRepository;
        private readonly IUserServices _userServices;
        public CheckEmailSecretPhrasePairQueryHandler(IUser userRepository, IUserServices userServices)
        {
            _userRepository = userRepository;
            _userServices = userServices;
        }
        public async Task<CustomResponse> Handle(CheckEmailSecretPhrasePairQuery request, CancellationToken cancellationToken)
        {
            var authorizationInfoDTO = await _userRepository
                    .TakeAuthorizationInfoByEmail(request.EmailSecretPhrasePairDTO.Email);
            var enteredSecretPhraseHash = await _userServices
                    .GetHashString($"{request.EmailSecretPhrasePairDTO.SecretPhrase}{authorizationInfoDTO.SecurityStamp}");
            if (!enteredSecretPhraseHash.Equals(authorizationInfoDTO.SecretPhraseHash))
                return new CustomResponse(false, "Check credetials you have entered! Wrong Email or Secret Phrase!");
            return new CustomResponse(true, "Entered Credentials are correct!");
        }
    }
}
