using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class DoctorsController : ControllerBase
{
    private readonly IDoctorService _doctorService;
    public DoctorsController(IDoctorService doctorService)
    {
        _doctorService = doctorService;
    }

    /// <summary>
    /// Gets selected Doctor's Profile
    /// </summary>
    /// <returns>Single Doctor's Profile</returns>
    [HttpGet("{doctorId}")]
    [ProducesResponseType(typeof(DoctorInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetDoctorById(Guid doctorId)
    {
        var result = await _doctorService.GetDoctorByIdAsync(doctorId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Doctor's Profiles
    /// </summary>
    /// <returns>The Doctor's Profiles list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<DoctorTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllDoctors([FromBody] DoctorParameters? doctorParameters)
    {
        var result = await _doctorService.GetAllDoctorsAsync(doctorParameters);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Doctor's Profile
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost]
    [ProducesResponseType(typeof(Guid), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddDoctor([FromForm] DoctorForCreateDTO doctorForCreateDTO)
    {
        var result = await _doctorService.CreateDoctorAsync(doctorForCreateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return CreatedAtAction(nameof(GetDoctorById), new { doctorId = result.Value }, result.Value);
    }

    /// <summary>
    /// Updates selected Doctor's Profile 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{doctorId}")]
    [ProducesResponseType(typeof(DoctorInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateDoctor(Guid doctorId, [FromForm] DoctorForUpdateDTO doctorForUpdateDTO)
    {
        var result = await _doctorService.UpdateDoctorAsync(doctorId, doctorForUpdateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Updates selected Doctor's Specializations 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{doctorId}/updatedoctorspecializations")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateDoctorSpecializations(Guid doctorId, [FromBody]IEnumerable<DoctorSpecializationForUpdateDTO> doctorSpecializationForUpdateDTOs)
    {
        var result = await _doctorService.UpdateDoctorSpecializationsAsync(doctorId, doctorSpecializationForUpdateDTOs);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes Doctor's Profile By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{doctorId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteDoctorById(Guid doctorId)
    {
        var result = await _doctorService.DeleteDoctorByIdAsync(doctorId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
// for each workers profile change workStatus 