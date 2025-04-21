using AppointmentAPI.Application.CQRS.Queries.Patient;
using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IMediator _mediator;

    public PatientsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Administrator's Request to check and add Patients From ProfilesAPI
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("checkconsistancy")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RequestCheckPatientsConsistancy()
    {
        var result = await _mediator.Send(new RequestCheckPatientsConsistancyQuery() { });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }
}
