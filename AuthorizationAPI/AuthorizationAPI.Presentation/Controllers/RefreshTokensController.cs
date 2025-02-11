using AuthorizationAPI.Services.Abstractions.Interfaces;
using AuthorizationAPI.Shared.DTOs.RefreshTokenDTOs;
using AuthorizationAPI.Shared.DTOs.UserDTOs;
using CommonLibrary.Response.FailMesssages;
using CommonLibrary.Response.SuccessMessages;
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
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetRefreshTokenInfoByRefreshTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.GetRefreshTokenInfoByRefreshTokenId(refreshTokenId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Gets the list of all Logged In Users
    /// </summary>
    /// <returns>The Logged In Users list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<UserLoggedInInfoDTO>>), 200)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllLoggedInUsers()
    {
        var result = await _refreshTokenService.GetAllLoggedInUsers();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessMessage<object>(result.Message.Value, result.Value));
    }

    /// <summary>
    /// Deletes Refresh Token By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{refreshTokenId:guid}")]
    [ProducesResponseType(typeof(SuccessOnDeleteMessage), 204)]
    [ProducesResponseType(typeof(BadRequestMessage), 400)]
    [ProducesResponseType(typeof(ForbiddenMessage), 403)]
    [ProducesResponseType(typeof(NotFoundMessage), 404)]
    [ProducesResponseType(typeof(RequestTimeoutMessage), 408)]
    [ProducesResponseType(typeof(ValidationErrorMessage), 422)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteRefreshTokenByRTokenId(Guid refreshTokenId)
    {
        var result = await _refreshTokenService.DeleteRefreshTokenByRTokenId(refreshTokenId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return Ok(new SuccessOnDeleteMessage(result.Message.Value));
    }

    //[HttpPatch]
    ////[Authorize(Roles = "Administrator")]
    //public async Task<IActionResult> RevokeRefreshTokenByRefreshTokenId([FromBody] RefreshTokenDTO refreshTokenDTO)
    //{
    //    var result = await _mediator.Send(new UpdateRefreshTokenCommand() { RefreshTokenDTO = refreshTokenDTO });
    //        if (!result.Flag)
    //            return HandlePesponseMessage(result);
    //        return Ok(result);
    //}
}
