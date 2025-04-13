using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Application.CQRS.Commands.SpecializationCommands;
using ServicesAPI.Application.CQRS.Queries.SpecializationQueries;
using ServicesAPI.Shared.DTOs.SpecializationDTOs;

namespace ServicesAPI.Presentation.Controllers;

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
    /// Gets selected Specialization
    /// </summary>
    /// <returns>Single Specialization</returns>
    [HttpGet("{specializationId}")]
    [ProducesResponseType(typeof(SpecializationInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetSpecializationById(Guid specializationId)
    {
        var result = await _mediator.Send(new GetSpecializationByIdQuery() { Id = specializationId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Specializations
    /// </summary>
    /// <returns>The Specialization list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<SpecializationTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllSpecializations([FromBody] SpecializationParameters? specializationParameters)
    {
        var result = await _mediator.Send(new GetAllSpecializationsWithParametersQuery() { SpecializationParameters = specializationParameters });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Specialization
    /// </summary>
    /// <returns>Created Specialization</returns>
    [HttpPost]
    [ProducesResponseType(typeof(SpecializationInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddSpecialization([FromBody] SpecializationForCreateDTO specializationForCreateDTO)
    {
        var result = await _mediator.Send(new CreateSpecializationCommand() { SpecializationForCreateDTO = specializationForCreateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return CreatedAtAction(nameof(GetSpecializationById), new { specializationId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates selected Specialization
    /// </summary>
    /// <returns>Updated Specialization</returns>
    [HttpPut("{specializationId}")]
    [ProducesResponseType(typeof(SpecializationInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateSpecialization(Guid specializationId, [FromBody] SpecializationForUpdateDTO specializationForUpdateDTO)
    {
        var result = await _mediator.Send(new UpdateSpecializationCommand() { SpecializationId = specializationId, specializationForUpdateDTO = specializationForUpdateDTO});
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes Specialization By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{specializationId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteSpecializationById(Guid specializationId)
    {
        var result = await _mediator.Send(new DeleteSpecializationCommand() { Id = specializationId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
