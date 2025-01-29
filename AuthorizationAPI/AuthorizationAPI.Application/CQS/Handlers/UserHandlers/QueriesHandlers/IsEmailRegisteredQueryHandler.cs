using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;

namespace AuthorizationAPI.Application.CQS.Handlers.UserHandlers.QueriesHandlers
{
    public class IsEmailRegisteredQueryHandler : IRequestHandler<IsEmailRegisteredQuery, CustomResponse>
    {
        private readonly IUser _userRepository;
        public IsEmailRegisteredQueryHandler(IUser userRepository)
        {
            _userRepository = userRepository;
        }
        public async Task<CustomResponse> Handle(IsEmailRegisteredQuery request, CancellationToken cancellationToken)
        {
            var check = await _userRepository.TakeUserWithPredicate(u => u.Email.Equals(request.EnteredEmail));
            if (check is not null)
                return new CustomResponse(false, "This Email has been already Registered!");
            return new CustomResponse(true, "Empty Email!");
        
        }
    }
}
