using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AuthorizationController : ResponseMessageHandler
{
    private readonly IAuthorizationService _authorizationService;
    public AuthorizationController(IAuthorizationService authorizationService)
    {
        _authorizationService = authorizationService;
    }

    /// <summary>
    /// Signing In by Email and Password
    /// </summary>
    /// <returns>Access and Refresh Tokens</returns>
    [HttpPost("signin")]
    [ProducesResponseType(typeof(SuccessMessage<TokensDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> SignIn([FromBody] LoginInfoDTO loginInfoDTO)
    {
        var result = await _authorizationService.SignIn(loginInfoDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<TokensDTO>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Signing Out by Refresh Token Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("signout")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> SignOut([FromBody] GuidValue refreshTokenId)
    {
        var result = await _authorizationService.SignOut(refreshTokenId.Value);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }

    /// <summary>
    /// Signing Up with all Registration Info
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(SuccessMessage), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> SignUp([FromBody] RegistrationInfoDTO registrationInfoDTO)
    {
        var result = await _authorizationService.SignUp(registrationInfoDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 201);
    }

    /// <summary>
    /// Refreshes Access and Refresh Tokens by Refresh Token Id
    /// </summary>
    /// <returns>Access and Refresh Tokens</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(SuccessMessage<TokensDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize]
    public async Task<IActionResult> Refresh([FromBody] GuidValue refreshTokenId)
    {
        var result = await _authorizationService.Refresh(refreshTokenId.Value);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<TokensDTO>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Resends verefication Letter to email by verifying Email and Password
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("resendemailverification")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> ResendEmailVerification([FromBody] LoginInfoDTO loginInfoDTO)
    {
        var result = await _authorizationService.ResendEmailVerification(loginInfoDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }
}
