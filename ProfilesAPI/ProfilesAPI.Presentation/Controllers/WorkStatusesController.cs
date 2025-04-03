using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.WorkStatusDTOs;

namespace ProfilesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class WorkStatusesController : ControllerBase
{
    private readonly IWorkStatusService _workStatusService;
    public WorkStatusesController(IWorkStatusService workStatusService)
    {
        _workStatusService = workStatusService;
    }

    /// <summary>
    /// Gets selected Work Status
    /// </summary>
    /// <returns>Single Work Status</returns>
    [HttpGet("{workStatusId}")]
    [ProducesResponseType(typeof(WorkStatusInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator, Doctor, Receptionist")]
    public async Task<IActionResult> GetWorkStatusById(Guid workStatusId)
    {
        var result = await _workStatusService.GetWorkStatusByIdAsync(workStatusId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Work Statuses
    /// </summary>
    /// <returns>The Work Statuses list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<WorkStatusTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator, Doctor, Receptionist")]
    public async Task<IActionResult> GetAllWorkStatuses()
    {
        var result = await _workStatusService.GetAllWorkStatusesAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Work Status
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost]
    [ProducesResponseType(typeof(WorkStatusInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddWorkStatus([FromBody] WorkStatusForCreateDTO workStatusForCreateDTO)
    {
        var result = await _workStatusService.CreateWorkStatusAsync(workStatusForCreateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return CreatedAtAction(nameof(GetWorkStatusById), new { workStatusId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates selected Work Status 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{workStatusId}")]
    [ProducesResponseType(typeof(WorkStatusInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateWorkStatus(Guid workStatusId, [FromBody] WorkStatusForUpdateDTO workStatusForUpdateDTO)
    {
        var result = await _workStatusService.UpdateWorkStatusAsync(workStatusId, workStatusForUpdateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes Work Status By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{workStatusId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteWorkStatusById(Guid workStatusId)
    {
        var result = await _workStatusService.DeleteWorkStatusByIdAsync(workStatusId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
