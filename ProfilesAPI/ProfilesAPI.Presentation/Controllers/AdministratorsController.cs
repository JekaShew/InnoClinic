using CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class AdministratorsController : ControllerBase
{
    private readonly IAdministratorService _administratorService;
    public AdministratorsController(IAdministratorService administratorService)
    {
        _administratorService = administratorService;
    }

    /// <summary>
    /// Gets selected Adminitrator's Profile
    /// </summary>
    /// <returns>Single Administrator's Profile</returns>
    [HttpGet("{administratorId}")]
    [ProducesResponseType(typeof(AdministratorInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAdminitratorById(Guid administratorId)
    {
        var result = await _administratorService.GetAdministratorByIdAsync(administratorId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Administrator's Profiles
    /// </summary>
    /// <returns>The Administrator's Profiles list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<AdministratorTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllAdminitrators()
    {
        var result = await _administratorService.GetAllAdministratorsAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Administrator's Profile
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
    public async Task<IActionResult> AddAdminitrator([FromForm] AdministratorForCreateDTO administratorForCreateDTO)
    {
        var result = await _administratorService.AddAdministratorAsync(administratorForCreateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Created();
    }

    /// <summary>
    /// Updates selected Administrator's Profile 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{administratorId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateAdminitrator(Guid administratorId, [FromForm] AdministratorForUpdateDTO administratorForUpdateDTO)
    {
        var result = await _administratorService.UpdateAdministratorAsync(administratorId, administratorForUpdateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes Administrator's Profile By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{administratorId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteAdminitratorById(Guid administratorId)
    {
        var result = await _administratorService.DeleteAdministratorByIdAsync(administratorId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
