using CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PatientsController : ControllerBase
{
    private readonly IPatientService _patientService;
    public PatientsController(IPatientService patientService)
    {
        _patientService = patientService;
    }

    /// <summary>
    /// Gets selected Patient's Profile
    /// </summary>
    /// <returns>Single Patient's Profile</returns>
    [HttpGet("{patientId}")]
    [ProducesResponseType(typeof(PatientInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetPatientById(Guid patientId)
    {
        var result = await _patientService.GetPatientByIdAsync(patientId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Patient's Profiles
    /// </summary>
    /// <returns>The Patient's Profiles list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PatientTableInfoDTO>),200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllPatients()
    {
        var result = await _patientService.GetAllPatientsAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Patient's Profile
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
    public async Task<IActionResult> AddPatient([FromForm]PatientForCreateDTO patientForCreateDTO)
    {
        var result = await _patientService.AddPatientAsync(patientForCreateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Created();
    }

    /// <summary>
    /// Updates selected Patient's Profile
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{patientId}")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdatePatient(Guid patientId, [FromForm] PatientForUpdateDTO patientForUpdateDTO)
    {
        var result = await _patientService.UpdatePatientAsync(patientId, patientForUpdateDTO);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }

    /// <summary>
    /// Deletes Patient's Profile By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{patientId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeletePatientById(Guid patientId)
    {
        var result = await _patientService.DeletePatientByIdAsync(patientId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
