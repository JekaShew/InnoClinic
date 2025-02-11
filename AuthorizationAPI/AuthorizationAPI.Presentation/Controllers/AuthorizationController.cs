using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.AdditionalDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.Response.FailMesssages;
using CommonLibrary.Response.SuccessMessages;
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
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    public async Task<IActionResult> SignIn([FromBody] LoginInfoDTO loginInfoDTO)
    {
        var result = await _authorizationService.SignIn(loginInfoDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Signing Out by Refresh Token Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("signout")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize]
    public async Task<IActionResult> SignOut([FromBody] GuidValue refreshTokenId)
    {
        var result = await _authorizationService.SignOut(refreshTokenId.Value);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage(result.Message.Value));
    }

    /// <summary>
    /// Signing Up with all Registration Info
    /// </summary>
    /// <returns>Access and Refresh Tokens</returns>
    [HttpPost("signup")]
    [ProducesResponseType(typeof(SuccessMessage<TokensDTO>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    public async Task<IActionResult> SignUp([FromBody] RegistrationInfoDTO registrationInfoDTO)
    {
        var result = await _authorizationService.SignUp(registrationInfoDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Refreshing Access and Refresh Tokens by Refresh Token Id
    /// </summary>
    /// <returns>Access and Refresh Tokens</returns>
    [HttpPost("refresh")]
    [ProducesResponseType(typeof(SuccessMessage<TokensDTO>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize]
    public async Task<IActionResult> Refresh([FromBody] GuidValue refreshTokenId)
    {
        var result = await _authorizationService.Refresh(refreshTokenId.Value);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }
}
