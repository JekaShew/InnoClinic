using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RefreshTokensController : ResponseMessageHandler
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
    [HttpGet("{refreshTokenId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage<RefreshTokenInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.GetRefreshTokenInfoByRefreshTokenId(refreshTokenId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<RefreshTokenInfoDTO>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Gets the list of all Logged In Users
    /// </summary>
    /// <returns>The Logged In Users list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<UserLoggedInInfoDTO>>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllLoggedInUsers()
    {
        var result = await _refreshTokenService.GetAllLoggedInUsers();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<IEnumerable<UserLoggedInInfoDTO>>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Deletes Refresh Token By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{refreshTokenId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteRefreshTokenByRTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.DeleteRefreshTokenByRTokenId(refreshTokenId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 204);
    }

    /// <summary>
    /// Switches Revoke status of Refresh Token by Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{refreshTokenId:guid}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RevokeRefreshTokenByRefreshTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.RevokeRefreshTokenByRefreshTokenId(refreshTokenId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }
}
