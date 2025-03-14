using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class SpecializationsController : ControllerBase
{
    private readonly ISpecializationService _specializationService;
    public SpecializationsController(ISpecializationService specializationService)
    {
        _specializationService = specializationService;
    }

    /// <summary>
    /// Gets selected Specialization
    /// </summary>
    /// <returns>Single Specialization</returns>
    [HttpGet("{specializationId}")]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator, Doctor, Receptionist")]
    public async Task<IActionResult> GetSpecializationById(Guid specializationId)
    {
        var result = await _specializationService.GetSpecializationByIdAsync(specializationId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Specializations
    /// </summary>
    /// <returns>The Specializations list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator, Doctor, Receptionist")]
    public async Task<IActionResult> GetAllSpecializations()
    {
        var result = await _specializationService.GetAllSpecializationsAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Specialization
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
    public async Task<IActionResult> AddSpecialization([FromBody] SpecializationForCreateDTO specializationForCreateDTO)
    {
        var result = await _specializationService.AddSpecializationAsync(specializationForCreateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Created();
    }

    /// <summary>
    /// Updates selected Specialization 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{specializationId}")]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateSpecialization(Guid specializationId, [FromBody] SpecializationForUpdateDTO specializationForUpdateDTO)
    {
        var result = await _specializationService.UpdateSpecializationAsync(specializationId, specializationForUpdateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes Specialization By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{specializationId}")]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteSpecializationById(Guid specializationId)
    {
        var result = await _specializationService.DeleteSpecializationByIdAsync(specializationId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
