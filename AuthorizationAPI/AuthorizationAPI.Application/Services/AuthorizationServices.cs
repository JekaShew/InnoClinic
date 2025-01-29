using AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands;
using AuthorizationAPI.Application.CQS.Commands.UserCommands;
using AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries;
using AuthorizationAPI.Application.CQS.Queries.RoleQueries;
using AuthorizationAPI.Application.CQS.Queries.UserQueries;
using AuthorizationAPI.Application.DTOs;
using AuthorizationAPI.Application.Interfaces;
using InnoShop.CommonLibrary.Response;
using MediatR;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationAPI.Application.Services
{
    public class AuthorizationServices : IAuthorizationServices
    {
        private readonly IMediator _mediator;
        private readonly IConfiguration _configuration;
        public AuthorizationServices(IMediator mediator, IConfiguration configuration)
        {
            _mediator = mediator;
            _configuration = configuration;
        }

        private async Task<string> GenerateJwtTokenStringByUserId(Guid userId)
        {
            var userDTO = await _mediator.Send(new TakeUserDTOByIdQuery() { Id = userId });
            var role = await _mediator.Send(new TakeRoleDTOByIdQuery() { Id = userDTO.RoleId });

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, userDTO.Email),
                new Claim(ClaimTypes.Name, userDTO.FIO),
                new Claim(ClaimTypes.NameIdentifier, userId.ToString()),
                new Claim(ClaimTypes.Role, role.Title),
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

        public async Task<(CustomResponse, string, string)> SignIn(LoginInfoDTO loginInfoDTO)
        {
            //Check Email Registered
            var isEmailRegistered = await _mediator.Send(new IsEmailRegisteredQuery() { EnteredEmail = loginInfoDTO.Email});
            if(isEmailRegistered.Flag == false)
                return (isEmailRegistered, null, null);
            //Check Email Password pair
            var checkEmailPasswordPair = await _mediator.Send( new CheckEmailPasswordPairQuery() { LoginInfoDTO = loginInfoDTO});
            if (checkEmailPasswordPair.Flag == false)
                return (checkEmailPasswordPair, null, null);
            //Generate A&R Tokens
            var userId = await _mediator.Send(new TakeUserIdByEmailQuery() {  Email = loginInfoDTO.Email });
            var tokens = await GenerateTokenPair(userId);

            return (new CustomResponse(true, "Success!"), tokens.Item1, tokens.Item2);
        }

        public async Task<CustomResponse> SignOut(Guid rTokenId)
        {
            return await _mediator.Send(new DeleteRefreshTokenByRTokenIdCommand() { Id = rTokenId });
        }

        public async Task<(CustomResponse, string, string)> SignUp(RegistrationInfoDTO registrationInfoDTO)
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

            return (new CustomResponse(true, "Success!"), tokens.Item1, tokens.Item2);
        }

        public async Task<(CustomResponse,string, string)> Refresh(Guid rTokenId)
        {
            //Check is RefreshToken Correct
            var isRTokenCorrect = await _mediator.Send(new IsRTokenCorrectByRTokenIdQuery() { RTokenId = rTokenId });
            if (isRTokenCorrect.Flag == false)
                return (isRTokenCorrect, null, null);         
            //Generate A&R Tokens
            var userId = await _mediator.Send(new TakeUserIdByRTokenIdQuery() { RTokenId = rTokenId});
            var tokens = await GenerateTokenPair(userId);

            return (new CustomResponse(true, "Success!"), tokens.Item1, tokens.Item2);
        }

        public async Task<CustomResponse> RevokeTokenByRTokenId(Guid rTokenId)
        {
            //Check is RefreshToken Correct
            var isRTokenCorrect = await _mediator.Send(new IsRTokenCorrectByRTokenIdQuery() { RTokenId = rTokenId });
            if (isRTokenCorrect.Flag == false)
                return isRTokenCorrect;
            //revoke Token
            return await _mediator.Send(new RevokeRTokenByRTokenIdCommand() { RTokenId= rTokenId });
        }
    }
}
