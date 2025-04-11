using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Application.CQRS.Commands.ServiceCommands;
using ServicesAPI.Application.CQRS.Queries.ServiceQueries;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServicesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServicesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets selected Service
    /// </summary>
    /// <returns>Single Service</returns>
    [HttpGet("{serviceId}")]
    [ProducesResponseType(typeof(ServiceInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetServiceById(Guid serviceId)
    {
        var result = await _mediator.Send(new GetServiceByIdQuery() { Id = serviceId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Services
    /// </summary>
    /// <returns>The Service list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ServiceTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllServices([FromBody] ServiceParameters? serviceParameters)
    {
        var result = await _mediator.Send(new GetAllServicesQuery() { ServiceParameters = serviceParameters });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Service
    /// </summary>
    /// <returns>Created Service</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ServiceInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddService([FromBody] ServiceForCreateDTO serviceForCreateDTO)
    {
        var result = await _mediator.Send(new CreateServiceCommand() { serviceForCreateDTO = serviceForCreateDTO});
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return CreatedAtAction(nameof(GetServiceById), new { serviceId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates selected Service
    /// </summary>
    /// <returns>Updated Service</returns>
    [HttpPut("{serviceId}")]
    [ProducesResponseType(typeof(ServiceInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateService(Guid serviceId, [FromBody] ServiceForUpdateDTO serviceForUpdateDTO)
    {
        var result = await _mediator.Send(new UpdateServiceCommand() { ServiceId = serviceId, ServiceForUpdateDTO = serviceForUpdateDTO});
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Change Service Status
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{serviceId}/changeservicestatus")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeServiceStatus(Guid serviceId)
    {
        var result = await _mediator.Send(new ChangeServiceStatusCommand() { ServiceId = serviceId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes Service By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{serviceId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteServiceById(Guid serviceId)
    {
        var result = await _mediator.Send(new DeleteServiceCommand() { Id = serviceId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
