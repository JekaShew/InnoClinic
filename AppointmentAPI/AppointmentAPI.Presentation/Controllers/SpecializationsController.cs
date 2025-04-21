using AppointmentAPI.Application.CQRS.Queries.Specialization;
using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecializationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public SpecializationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Administrator's Request to check and add Specializations From ServicesAPI
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("checkconsistancy")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RequestCheckSpecializationsConsistancy()
    {
        var result = await _mediator.Send(new RequestCheckSpecializationsConsistancyQuery() { });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }
}
