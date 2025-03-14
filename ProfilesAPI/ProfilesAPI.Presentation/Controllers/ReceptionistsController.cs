using CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ReceptionistsController : ControllerBase
{
    private readonly IReceptionistService _receptionistService;
    public ReceptionistsController(IReceptionistService receptionistService)
    {
        _receptionistService = receptionistService;
    }

    /// <summary>
    /// Gets selected Receptionist's Profile
    /// </summary>
    /// <returns>Single Receptionist's Profile</returns>
    [HttpGet("{receptionistId}")]
    [ProducesResponseType(typeof(ReceptionistInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetReceptionistById(Guid receptionistId)
    {
        var result = await _receptionistService.GetReceptionistByIdAsync(receptionistId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Receptionist's Profiles
    /// </summary>
    /// <returns>The Receptionist's Profiles list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ReceptionistTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllReceptionists()
    {
        var result = await _receptionistService.GetAllReceptionistsAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Receptionist's Profile
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost]
    [ProducesResponseType(201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddReceptionist([FromForm] ReceptionistForCreateDTO receptionistForCreateDTO)
    {
        var result = await _receptionistService.AddReceptionistAsync(receptionistForCreateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Created();
    }

    /// <summary>
    /// Updates selected Receptionist's Profile
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{receptionistId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateReceptionist(Guid receptionistId, [FromBody] ReceptionistForUpdateDTO receptionistForUpdateDTO)
    {
        var result = await _receptionistService.UpdateReceptionistAsync(receptionistId, receptionistForUpdateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes Receptionist's Profile By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{receptionistId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteReceptionistById(Guid receptionistId)
    {
        var result = await _receptionistService.DeleteReceptionistByIdAsync(receptionistId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
