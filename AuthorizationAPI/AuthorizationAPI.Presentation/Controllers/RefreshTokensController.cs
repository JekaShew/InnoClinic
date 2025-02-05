using AuthorizationAPI.Services.Abstractions.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokensController : BaseManualController
    {
        private readonly IRefreshTokenService _refreshTokenService;
        public RefreshTokensController(IRefreshTokenService refreshTokenService)
        {
            _refreshTokenService = refreshTokenService;
        }

        [HttpGet("{rTokenId}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetRefreshTokenInfoByRefreshTokenId(Guid rTokenId)
        {
            var result = await _refreshTokenService.GetRefreshTokenInfoByRefreshTokenId(rTokenId);
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

        [HttpGet]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> GetAllLoggedInUsers()
        {
            var result = await _refreshTokenService.GetAllLoggedInUsers();
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

        [HttpDelete("{refreshTokenId}")]
        //[Authorize(Roles = "Administrator")]
        public async Task<IActionResult> DeleteRefreshTokenByRTokenId(Guid refreshTokenId)
        {
            var result = await _refreshTokenService.DeleteRefreshTokenByRTokenId(refreshTokenId);
            if (result.Flag == false)
                return HandlePesponseMessage(result);
            return Ok(result);
        }

        //[HttpPatch]
        ////[Authorize(Roles = "Administrator")]
        //public async Task<IActionResult> RevokeRefreshTokenByRefreshTokenId([FromBody] RefreshTokenDTO refreshTokenDTO)
        //{
        //    var result = await _mediator.Send(new UpdateRefreshTokenCommand() { RefreshTokenDTO = refreshTokenDTO });
    //        if (result.Flag == false)
    //            return HandlePesponseMessage(result);
    //        return Ok(result);
    //}
}
}
