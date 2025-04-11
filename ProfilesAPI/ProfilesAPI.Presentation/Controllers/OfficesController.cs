using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfficesController : ControllerBase
{
    private readonly IOfficeService _officeService;
    public OfficesController(IOfficeService officeService)
    {
        _officeService = officeService;
    }

    /// <summary>
    /// Administrator's Request to check and add Offices From OfficesAPI
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost("checkconsistancy")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> RequestCheckOfficeConsistancy()
    {
        var result = await _officeService.RequestCheckOfficeConsistancyAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok();
    }
}
