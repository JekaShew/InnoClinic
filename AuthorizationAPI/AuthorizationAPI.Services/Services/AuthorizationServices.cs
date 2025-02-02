using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs;
using InnoClinic.CommonLibrary.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationAPI.Services.Services
{
    public class AuthorizationServices : IAuthorizationServices
    {
        private readonly IUserRepository _userRepository;
        private readonly IRoleRepository _roleRepository;
        private readonly IConfiguration _configuration;
        public AuthorizationServices(IConfiguration configuration, IUserRepository userRepository,  IRoleRepository roleRepository)
        {
            _configuration = configuration;
            _userRepository = userRepository;
            _roleRepository = roleRepository;
        }

        private async Task<string> GenerateJwtTokenStringByUserId(Guid userId)
        {
            var user = await _userRepository.TakeUserById(userId);
            var role = await _roleRepository.TakeRoleById(user.Value.RoleId);

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Value.Email),
                new Claim(ClaimTypes.Name, user.Value.FIO),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role.Value.Title),
            };

            var jwtHandler = new JwtSecurityTokenHandler();
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_configuration["Authentication:SecretKey"]));

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(claims),
                Issuer = _configuration["Authentication:Issuer"],
                Audience = _configuration["Authentication:Audience"],
                Expires = DateTime.UtcNow.AddMinutes(15),
                SigningCredentials =
                    new SigningCredentials(key,
                        SecurityAlgorithms.HmacSha256Signature)
            };

            var jwtToken = jwtHandler.CreateToken(tokenDescriptor);
            var tokenString = jwtHandler.WriteToken(jwtToken);
            return tokenString;
        }

        private async Task<string> GenerateRefreshTokenByUserId(Guid userId)
        {
            var refreshToken = new RefreshTokenDTO()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsRevoked = false,
                ExpireDate = DateTime.UtcNow.AddMinutes(120),
            };

            var addRefreshToken = await _mediator.Send(new AddRefreshTokenCommand() { RefreshTokenDTO = refreshToken });
            if (addRefreshToken.Flag == false)
                return null;
            return refreshToken.Id.ToString();
        }

        private async Task<(string, string)> GenerateTokenPair(Guid userId)
        {
            var jwt = await GenerateJwtTokenStringByUserId(userId);
            var rt = await GenerateRefreshTokenByUserId(userId);
            return (jwt, rt);
        }

        public async Task<CustomResponse<(string, string)>> SignIn(LoginInfoDTO loginInfoDTO)
        {
            //Check Email Registered
            var isEmailRegistered = await _mediator.Send(new IsEmailRegisteredQuery() { EnteredEmail = loginInfoDTO.Email });
            if (isEmailRegistered.Flag == false)
                return (isEmailRegistered, null, null);
            //Check Email Password pair
            var checkEmailPasswordPair = await _mediator.Send(new CheckEmailPasswordPairQuery() { LoginInfoDTO = loginInfoDTO });
            if (checkEmailPasswordPair.Flag == false)
                return (checkEmailPasswordPair, null, null);
            //Generate A&R Tokens
            var userId = await _mediator.Send(new TakeUserIdByEmailQuery() { Email = loginInfoDTO.Email });
            var tokens = await GenerateTokenPair(userId);

            return new CustomResponse<(string,string)>(true, "Success!", tokens);
        }

        public async Task<CustomResponse> SignOut(Guid rTokenId)
        {
            return await _mediator.Send(new DeleteRefreshTokenByRTokenIdCommand() { Id = rTokenId });
        }

        public async Task<CustomResponse<(string, string)>> SignUp(RegistrationInfoDTO registrationInfoDTO)
        {
            //Check Email Registered
            var isEmailRegistered = await _mediator.Send(new IsEmailRegisteredQuery() { EnteredEmail = registrationInfoDTO.Email });
            if (isEmailRegistered.Flag == false)
                return (isEmailRegistered, null, null);

            //Activate By Email???
            //Implement

            //Add User
            await _mediator.Send(new AddUserCommand() { RegistrationInfoDTO = registrationInfoDTO });

            //Generate A&R Tokens
            var userId = await _mediator.Send(new TakeUserIdByEmailQuery() { Email = registrationInfoDTO.Email });
            var tokens = await GenerateTokenPair(userId);

            return new CustomResponse<(string, string)>(true, "Success!", tokens); ;
        }

        public async Task<CustomResponse<(string, string)>> Refresh(Guid rTokenId)
        {
            //Check is RefreshToken Correct
            var isRTokenCorrect = await _mediator.Send(new IsRTokenCorrectByRTokenIdQuery() { RTokenId = rTokenId });
            if (isRTokenCorrect.Flag == false)
                return (isRTokenCorrect, null, null);
            //Generate A&R Tokens
            var userId = await _mediator.Send(new TakeUserIdByRTokenIdQuery() { RTokenId = rTokenId });
            var tokens = await GenerateTokenPair(userId);

            return new CustomResponse<(string, string)>(true, "Success!", tokens); ;
        }

        public async Task<CustomResponse> RevokeTokenByRTokenId(Guid rTokenId)
        {
            //Check is RefreshToken Correct
            var isRTokenCorrect = await _mediator.Send(new IsRTokenCorrectByRTokenIdQuery() { RTokenId = rTokenId });
            if (isRTokenCorrect.Flag == false)
                return isRTokenCorrect;
            //revoke Token
            return await _mediator.Send(new RevokeRTokenByRTokenIdCommand() { RTokenId = rTokenId });
        }
    }
}
