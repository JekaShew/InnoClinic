using AppointmentAPI.Application.CQRS.Commands.Appointment;
using AppointmentAPI.Application.CQRS.Commands.Service;
using AppointmentAPI.Application.CQRS.Queries.Appointment;
using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using AppointmentAPI.Shared.DTOs.ServiceDTOs;
using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace AppointmentAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AppointmentsController : ControllerBase
{
    private readonly IMediator _mediator;
    public AppointmentsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets selected Appointment
    /// </summary>
    /// <returns>Single Appointment</returns>
    [HttpGet("{appointmentId}")]
    [ProducesResponseType(typeof(AppointmentInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAppointmentById(Guid appointmentId)
    {
        var result = await _mediator.Send(new GetAppointmentByIdQuery() { Id = appointmentId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Appointments
    /// </summary>
    /// <returns>The Appointment list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<AppointmentTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllAppointments([FromBody] AppointmentParameters? appointmentParameters)
    {
        var result = await _mediator.Send(new GetAllAppointmentsQuery() { AppointmentParameters = appointmentParameters });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Appointment
    /// </summary>
    /// <returns>Created Appointment</returns>
    [HttpPost]
    [ProducesResponseType(typeof(AppointmentInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddAppointment([FromBody] AppointmentForCreateDTO appointmentForCreateDTO)
    {
        var result = await _mediator.Send(new CreateAppointmentCommand() { AppointmentForCreateDTO = appointmentForCreateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return CreatedAtAction(nameof(GetAppointmentById), new { appointmentId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates selected Appointment
    /// </summary>
    /// <returns>Updated Appointment</returns>
    [HttpPut("{appointmentId}")]
    [ProducesResponseType(typeof(AppointmentInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateAppointment(Guid appointmentId, [FromBody] AppointmentForUpdateDTO appointmentForUpdateDTO)
    {
        var result = await _mediator.Send(new UpdateAppointmentCommand() { AppointmentId = appointmentId, AppointmentForUpdateDTO = appointmentForUpdateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Change Appointment Status
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{appointmentId}/changeappointmentstatus")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeAppointmentStatus(Guid appointmentId)
    {
        var result = await _mediator.Send(new ChangeAppointmentStatusCommand() { AppointmentId = appointmentId , AppointmentStatus = AppointmentStatus });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes Appointment By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{appointmentId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAppointmentById(Guid appointmentId)
    {
        var result = await _mediator.Send(new DeleteAppointmentCommand() { Id = appointmentId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
