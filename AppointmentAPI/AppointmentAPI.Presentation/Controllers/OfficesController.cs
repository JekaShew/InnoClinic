using AppointmentAPI.Application.CQRS.Queries.Office;
using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfficesController : ControllerBase
{
    private readonly IMediator _mediator;

    public OfficesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Administrator's Request to check and add Offices From OfficeAPI
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("checkconsistancy")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RequestCheckOfficesConsistancy()
    {
        var result = await _mediator.Send(new RequestCheckOfficesConsistancyQuery() { });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }
}
