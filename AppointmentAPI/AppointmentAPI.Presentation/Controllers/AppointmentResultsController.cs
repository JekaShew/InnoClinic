using AppointmentAPI.Application.CQRS.Commands.AppointmentResult;
using AppointmentAPI.Application.CQRS.Commands.Service;
using AppointmentAPI.Application.CQRS.Queries.AppointmentResult;
using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using AppointmentAPI.Shared.DTOs.ServiceDTOs;
using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentResultsController : ControllerBase
{
    private readonly IMediator _mediator;

    public AppointmentResultsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets selected Appointment Result
    /// </summary>
    /// <returns>Single Appointment Result</returns>
    [HttpGet("{appointmentResultId}")]
    [ProducesResponseType(typeof(AppointmentResultInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAppointmentResultById(Guid appointmentResultId)
    {
        var result = await _mediator.Send(new GetAppointmentResultByIdQuery() { Id = appointmentResultId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Appointment Results
    /// </summary>
    /// <returns>The Appointment Result list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentResultTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllAppointmentResults([FromBody] AppointmentResultParameters? appointmentResultParameters)
    {
        var result = await _mediator.Send(new GetAllAppointmentResultsQuery() { AppointmentResultParameters = appointmentResultParameters });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Appointment Result
    /// </summary>
    /// <returns>Created Appointment Result</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentResultInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddAppointmentResult([FromBody] AppointmentResultForCreateDTO appointmentResultForCreateDTO)
    {
        var result = await _mediator.Send(new CreateAppointmentResultCommand() { AppointmentResultForCreateDTO = appointmentResultForCreateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return CreatedAtAction(nameof(GetAppointmentResultById), new { appointmentResultId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates selected Appointment Result
    /// </summary>
    /// <returns>Updated Appointment Result</returns>
    [HttpPut("{appointmentResultId}")]
    [ProducesResponseType(typeof(AppointmentResultInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateAppointmentResult(Guid appointmentResultId, [FromBody] AppointmentResultForUpdateDTO appointmentResultForUpdateDTO)
    {
        var result = await _mediator.Send(new UpdateAppointmentResultCommand() { AppointmentResultId = appointmentResultId, AppointmentResultForUpdateDTO = appointmentResultForUpdateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes Appointment Result By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{appointmentResultId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAppointmentResultById(Guid appointmentResultId)
    {
        var result = await _mediator.Send(new DeleteAppointmentResultCommand() { Id = appointmentResultId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
