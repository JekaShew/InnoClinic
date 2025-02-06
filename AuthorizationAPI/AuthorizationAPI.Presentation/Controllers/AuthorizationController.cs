using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
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

    [HttpPost("signin")]
    public async Task<IActionResult> SignIn([FromBody] LoginInfoDTO loginInfoDTO)
    {
        var result = await _authorizationService.SignIn(loginInfoDTO);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }

    [HttpPost("signout")]
    //[Authorize]
    public async Task<IActionResult> SignOut([FromBody] Guid rTokenId)
    {
        var result = await _authorizationService.SignOut(rTokenId);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }

    [HttpPost("signup")]
    public async Task<IActionResult> SignUp([FromBody] RegistrationInfoDTO registrationInfoDTO)
    {
        var result = await _authorizationService.SignUp(registrationInfoDTO);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }

    [HttpPost("refresh")]
    //[Authorize]
    public async Task<IActionResult> Refresh([FromBody] Guid rTokenId)
    {
        var result = await _authorizationService.Refresh(rTokenId);
        if (result.Flag == false)
            return HandleResponseMessage(result);
        return Ok(result);
    }
}
