using AuthorizationAPI.Domain.Data.Models;
using AuthorizationAPI.Domain.IRepositories;
using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.Constants;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using InnoClinic.CommonLibrary.Response;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace AuthorizationAPI.Services.Services
{
    public class AuthorizationService : IAuthorizationService
    {
        private readonly IRepositoryManager _repositoryManager;
        private readonly IUserService _userService;
        private readonly IConfiguration _configuration;
        public AuthorizationService(
                IConfiguration configuration,
                IRepositoryManager repositoryManager,
                IUserService userService)
        {
            _configuration = configuration;
            _repositoryManager = repositoryManager;
            _userService = userService;
        }
        // Add methods ActivateUserByEmail-> default UserStatus change to "Not Activated"
        private async Task<string> GenerateJwtTokenStringByUserId(Guid userId)
        {
            var user = (await _repositoryManager.User.GetUsersWithExpressionAsync(u => u.Id.Equals(userId), false)).FirstOrDefault();
            var role = (await _repositoryManager.Role.GetRolesWithExpressionAsync(r => r.Id.Equals(user.RoleId), false)).FirstOrDefault();

            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Email, user.Email),
                new Claim(ClaimTypes.Name, $"{user.FirstName} {user.LastName}"),
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
            var refreshToken = new RefreshToken()
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                IsRevoked = false,
                ExpireDate = DateTime.UtcNow.AddMinutes(120),
            };

            _repositoryManager.RefreshToken.CreateRefreshToken(refreshToken);
            await _repositoryManager.SaveChangesAsync();
            
            return refreshToken.Id.ToString();
        }

        private async Task<TokensDTO> GenerateTokenPair(Guid userId)
        {
            var jwt = await GenerateJwtTokenStringByUserId(userId);
            var rt = await GenerateRefreshTokenByUserId(userId);
            return new TokensDTO() { AccessToken = jwt, RefreshToken = rt };
        }

        public async Task<CommonResponse<TokensDTO>> SignIn(LoginInfoDTO loginInfoDTO)
        {
            //Check Email Registered
            var userDetailedDTO = await _userService.IsEmailRegistered(loginInfoDTO.Email, false);
            if(userDetailedDTO is null)
                return new CommonResponse<TokensDTO>(false, MessageConstants.CheckCredsMessage);
            //Check Email Password pair
            var enteredPasswordHash = await _userService.GetHashString($"{loginInfoDTO.Password}{userDetailedDTO.SecurityStamp}");
            if (!enteredPasswordHash.Equals(userDetailedDTO.PasswordHash))
                return new CommonResponse<TokensDTO>(false, MessageConstants.CheckCredsMessage);

            //Generate A&R Tokens
            var tokens = await GenerateTokenPair(userDetailedDTO.Id);

            return new CommonResponse<TokensDTO>(true, MessageConstants.SuccessMessage, tokens);
        }

        public async Task<CommonResponse> SignOut(Guid rTokenId)
        {
            var refreshToken = (await _repositoryManager.RefreshToken.GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(rTokenId), false)).FirstOrDefault();
            if (refreshToken is null)
                return new CommonResponse(false, MessageConstants.NotFoundMessage);
            _repositoryManager.RefreshToken.DeleteRefreshToken(refreshToken);
            await _repositoryManager.SaveChangesAsync();

            return new CommonResponse(true, MessageConstants.SuccessMessage);
        }

        public async Task<CommonResponse<TokensDTO>> SignUp(RegistrationInfoDTO registrationInfoDTO)
        {
            //Check Email Registered
            var userDetailedDTO = await _userService.IsEmailRegistered(registrationInfoDTO.Email, false);
            if (userDetailedDTO is not null)
                return new CommonResponse<TokensDTO>(false, MessageConstants.EmailRegisteredMessage);
            //Create and Generate A&R Tokens
            var userId = await _userService.CreateUserAsync(registrationInfoDTO);
            if (userId.Equals(Guid.Empty))
                return new CommonResponse<TokensDTO>(false, MessageConstants.FailedCreateMessage);
            var tokens = await GenerateTokenPair(userId);

            return new CommonResponse<TokensDTO>(true, MessageConstants.SuccessMessage, tokens); ;
        }
        private async Task<RefreshToken> IsRefreshTokeCorrect(Guid rTokenId, bool trackChanges)
        {
            //Check is RefreshToken Correct
            var refreshToken = (await _repositoryManager.RefreshToken
                    .GetRefreshTokensWithExpressionAsync(rt => rt.Id.Equals(rTokenId), trackChanges))
                    .FirstOrDefault();
            if (refreshToken is null)
                return null;

            if (refreshToken.IsRevoked == true)
                return null;

            if (refreshToken.ExpireDate <= DateTime.UtcNow || refreshToken.ExpireDate.Equals(DateTime.MinValue))
                return null;

            return refreshToken;
        }
        public async Task<CommonResponse<TokensDTO>> Refresh(Guid rTokenId)
        {
            var refreshToken = await IsRefreshTokeCorrect(rTokenId, false);
            //Generate A&R Tokens
            var tokens = await GenerateTokenPair(refreshToken.UserId);

            return new CommonResponse<TokensDTO>(true, MessageConstants.SuccessMessage, tokens);
        }        
    }
}
