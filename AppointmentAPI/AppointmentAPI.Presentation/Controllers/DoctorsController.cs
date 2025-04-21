using AppointmentAPI.Application.CQRS.Queries.Doctor;
using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{

    private readonly IMediator _mediator;

    public DoctorsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Administrator's Request to check and add Doctors From ProfilesAPI
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("checkconsistancy")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RequestCheckDoctorsConsistancy()
    {
        var result = await _mediator.Send(new RequestCheckDoctorsConsistancyQuery() { });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }
}
