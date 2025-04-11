using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;
using Serilog;

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
    /// Administrator's Request to check and add Specializations From ProfilesAPI
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("checkconsistancy")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RequestCheckSpecializationConsistancy()
    {
        var result = await _specializationService.RequestCheckSpecializationConsistancyAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }
}
