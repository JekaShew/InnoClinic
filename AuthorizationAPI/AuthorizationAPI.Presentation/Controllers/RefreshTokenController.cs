using AuthorizationAPI.Application.CQS.Commands.RefreshTokenCommands;
using AuthorizationAPI.Application.CQS.Queries.RefreshTokenQueries;
using AuthorizationAPI.Application.DTOs;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AuthorizationAPI.Presentation.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RefreshTokenController : ControllerBase
    {
        private readonly IMediator _mediator;
        public RefreshTokenController(IMediator mediator)
        {
            _mediator = mediator;
        }

        [HttpGet("{rTokenId}")]
        public async Task<IActionResult> TakeRefreshTokenByRTokenId(Guid rTokenId)
        {
            var result = await _mediator.Send(new TakeRefreshTokenDTOByRTokenIdQuery() { Id = rTokenId});
            if (result == null)
                return NotFound("Refresh Token Not found!");
            return Ok(result);
        }

        [HttpGet]
        public async Task<IActionResult> TakeAllRefreshTokens()
        {
            var result = await _mediator.Send(new TakeRefreshTokenDTOListQuery() { });
            if (!result.Any())
                return NotFound("No refresh tokens Found!");
            return Ok(result);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateRefreshToken([FromBody] RefreshTokenDTO refreshTokenDTO)
        {
            var result = await _mediator.Send(new UpdateRefreshTokenCommand() { RefreshTokenDTO = refreshTokenDTO});
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }

        [HttpDelete("{rTokenId}")]
        public async Task<IActionResult> DeleteRefreshTokenByRTokenId(Guid rTokenId)
        {
            var result = await _mediator.Send(new DeleteRefreshTokenByRTokenIdCommand() { Id = rTokenId});
            if (result.Flag == false)
                return StatusCode(500, result.Message);
            return Ok(result.Message);
        }
    }
}
