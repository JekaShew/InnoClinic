using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthorizationController : ControllerBase
    {
        private readonly IAuthorizationService _authorizationService;
        public AuthorizationController(IAuthorizationService authorizationService)
        {
            _authorizationService = authorizationService;
        }

        [HttpPost("signin")]
        public async Task<IActionResult> SignIn([FromBody] LoginInfoDTO loginInfoDTO)
        {
            //Rework
            var response = await _authorizationService.SignIn(loginInfoDTO);
            if (response.Item1.Flag == false || (response.Item2.IsNullOrEmpty() || response.Item3.IsNullOrEmpty()))
                return BadRequest($"Tokens refresh Failed! {response.Item1.Message}");
            return Ok(new { AccessToken = response.Item2, RefreshToken = response.Item3 });
        }

        [HttpPost("signout")]
        public async Task<IActionResult> SignOut([FromBody] Guid rTokenId)
        {
            var signOut = await _authorizationService.SignOut(rTokenId);
            if (signOut.Flag == false)
                return BadRequest(signOut.Message);

            return Ok(signOut.Message);
        }

        [HttpPost("signup")]
        public async Task<IActionResult> SignUp([FromBody] RegistrationInfoDTO registrationInfoDTO)
        {
            var response = await _authorizationService.SignUp(registrationInfoDTO);
            if (response.Item1.Flag == false || (response.Item2.IsNullOrEmpty() || response.Item3.IsNullOrEmpty()))
                return BadRequest($"Tokens refresh Failed! {response.Item1.Message}");

            return Ok(new { AccessToken = response.Item2, RefreshToken = response.Item3 });
        }

        [HttpPost("/refresh")]
        public async Task<IActionResult> Refresh([FromBody] Guid rTokenId)
        {
            var response = await _authorizationService.Refresh(rTokenId);
            if (response.Item1.Flag == false || (response.Item2.IsNullOrEmpty() || response.Item3.IsNullOrEmpty()))
                return BadRequest($"Tokens refresh Failed! {response.Item1.Message}");

            return Ok(new { AccessToken = response.Item2, RefreshToken = response.Item3 });
        }

        [HttpPatch("/revoke")]
        public async Task<IActionResult> RevokeTokenByRTokenId([FromBody] Guid rTokenId)
        {
            var revoke = await _authorizationService.RevokeTokenByRefreshTokenId(rTokenId);
            if (revoke.Flag == false)
                return BadRequest(revoke.Message);

            return Ok(revoke.Message);
        }
    }
}
