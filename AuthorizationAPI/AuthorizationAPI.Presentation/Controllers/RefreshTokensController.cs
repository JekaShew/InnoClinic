using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RefreshTokensController : ControllerBase
{
    private readonly IRefreshTokenService _refreshTokenService;
    public RefreshTokensController(IRefreshTokenService refreshTokenService)
    {
        _refreshTokenService = refreshTokenService;
    }

    /// <summary>
    /// Gets selected Refresh Token
    /// </summary>
    /// <returns>Single Refresh Token</returns>
    [HttpGet("{refreshTokenId}")]
    [ProducesResponseType(typeof(RefreshTokenInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.GetRefreshTokenInfoByRefreshTokenId(refreshTokenId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Logged In Users
    /// </summary>
    /// <returns>The Logged In Users list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<UserLoggedInInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllLoggedInUsers()
    {
        var result = await _refreshTokenService.GetAllLoggedInUsers();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes Refresh Token By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{refreshTokenId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteRefreshTokenByRTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.DeleteRefreshTokenByRTokenId(refreshTokenId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return NoContent();
    }

    /// <summary>
    /// Switches Revoke status of Refresh Token by Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{refreshTokenId}/revoke")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RevokeRefreshTokenByRefreshTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.RevokeRefreshTokenByRefreshTokenId(refreshTokenId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok();
    }
}
