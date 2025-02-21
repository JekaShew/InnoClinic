using CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs.OfficeDTOs;
using System.Reflection.Metadata;

namespace OfficesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfficesController : ResponseMessageHandler
{
    private readonly IOfficeService _officeService;

    public OfficesController(IOfficeService officeService)
    {
        _officeService = officeService;
    }

    /// <summary>
    /// Gets the list of all Offices
    /// </summary>
    /// <returns>The Offices list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(SuccessMessage<IEnumerable<OfficeTableInfoDTO>>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> GetAllOffices()
    {
        var result = await _officeService.GetAllOfficesAsync();
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<IEnumerable<OfficeTableInfoDTO>>(result.Message.Value, result.Value);
    }

    /// <summary>
    /// Gets selected Office
    /// </summary>
    /// <returns>Single Office</returns>
    [HttpGet("{officeId}")]
    [ProducesResponseType(typeof(SuccessMessage<OfficeInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> GetOfficeByid(string officeId)
    {
        var result = await _officeService.GetOfficeByIdAsync(officeId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage<OfficeInfoDTO>(result.Message.Value, result.Value);
    }

    ///// <summary>
    ///// Creates new Office
    ///// </summary>
    ///// <returns>Message</returns>
    //[HttpPost]
    //[ProducesResponseType(typeof(SuccessMessage), 201)]
    //[ProducesResponseType(typeof(FailMessage), 400)]
    //[ProducesResponseType(typeof(FailMessage), 403)]
    //[ProducesResponseType(typeof(FailMessage), 404)]
    //[ProducesResponseType(typeof(FailMessage), 408)]
    //[ProducesResponseType(typeof(FailMessage), 422)]
    //[ProducesResponseType(typeof(FailMessage), 500)]
    ////[Authorize(Roles = "Administrator")]
    //public async Task<IActionResult> AddOffice(OfficeForCreateDTO officeForCreateDTO)
    //{
    //    var result = await _officeService.CreateOfficeAsync(officeForCreateDTO);
    //    if (!result.Flag)
    //        return HandleResponseMessage(result);
    //    return new SuccessMessage(result.Message.Value, 201);
    //}

    [HttpPost("AddOffice")]
    [Consumes("multipart/form-data")]
    [Produces("application/json")]
    public async Task<IActionResult> AddOffice(
            [FromForm] OfficeForCreateDTO officeForCreateDTO,
            [FromForm] ICollection<IFormFile> files)
    {
        var result = await _officeService.CreateOfficeAsync(officeForCreateDTO,files);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 201);
    }

    /// <summary>
    /// Updates selected Office 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{officeId}")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateOffice(string officeId, [FromBody] OfficeForUpdateDTO officeForUpdateDTO)
    {
        var result = await _officeService.UpdateOfficeInfoAsync(officeId, officeForUpdateDTO);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value);
    }

    /// <summary>
    /// Deletes Office By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{officeId}")]
    [ProducesResponseType(typeof(SuccessMessage), 204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteOfficeById(string officeId)
    {
        var result = await _officeService.DeleteOfficeByIdAsync(officeId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 204);
    }

    /// <summary>
    /// Changes Office's status By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{officeId}/changestatusofofficebyid")]
    [ProducesResponseType(typeof(SuccessMessage), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeStatusOfOfficeByIdAsync(string officeId)
    {
        var result = await _officeService.ChangeStatusOfOfficeByIdAsync(officeId);
        if (!result.Flag)
            return HandleResponseMessage(result);
        return new SuccessMessage(result.Message.Value, 200);
    }
}
