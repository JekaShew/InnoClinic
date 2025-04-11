using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs.OfficeDTOs;
using Serilog;


namespace OfficesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class OfficesController : ControllerBase
{
    private readonly IOfficeService _officeService;
    private readonly ICacheService _cache;
    private readonly CacheKeyConstants _cacheKeyConstants = new CacheKeyConstants("Offices");

    public OfficesController(
            IOfficeService officeService,
            ILogger logger,
            ICacheService cache)
    {
        _officeService = officeService;
        _cache = cache;
    }

    /// <summary>
    /// Gets the list of all Offices
    /// </summary>
    /// <returns>The Offices list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<OfficeTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async ValueTask<IActionResult> GetAllOffices()
    {
        var officeTableInfoDTOs = _cache.GetData<IEnumerable<OfficeTableInfoDTO>>(_cacheKeyConstants.GetAll);
        if(officeTableInfoDTOs is not null)
        {
            return Ok(officeTableInfoDTOs);
        }

        var result = await _officeService.GetAllOfficesAsync();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        _cache.SetData(_cacheKeyConstants.GetAll, result.Value, TimeSpan.FromMinutes(5));

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets selected Office
    /// </summary>
    /// <returns>Single Office</returns>
    [HttpGet("{officeId}")]
    [ProducesResponseType(typeof(OfficeInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> GetOfficeByid(string officeId)
    {
        var cacheKey = $"{_cacheKeyConstants.GetById}{officeId}";
        var officeTableInfoDTOs = _cache.GetData<OfficeInfoDTO>(cacheKey);
        if (officeTableInfoDTOs is not null)
        {
            return Ok(officeTableInfoDTOs);
        }

        var result = await _officeService.GetOfficeByIdAsync(officeId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        _cache.SetData(cacheKey, result.Value, TimeSpan.FromMinutes(5));

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Office
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost]
    [ProducesResponseType(typeof(OfficeInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddOffice(
            [FromForm] OfficeForCreateDTO officeForCreateDTO,
            [FromForm] ICollection<IFormFile> files)
    {
        var result = await _officeService.CreateOfficeAsync(officeForCreateDTO, files);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        var cacheKey = $"{_cacheKeyConstants.GetById}{result.Value.Id}";
        _cache.SetData(cacheKey, result.Value, TimeSpan.FromMinutes(3));

        return CreatedAtAction(nameof(GetOfficeByid), new { officeId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates selected Office 
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{officeId}")]
    [ProducesResponseType(typeof(OfficeInfoDTO), 200)]
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
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        _cache.RemoveData(_cacheKeyConstants.GetAll);
        var cacheKey = $"{_cacheKeyConstants.GetById}{result.Value.Id}";
        _cache.RemoveData(cacheKey);
        _cache.SetData(cacheKey, result.Value, TimeSpan.FromMinutes(3));

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes Office By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{officeId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteOfficeById(string officeId)
    {
        var result = await _officeService.DeleteOfficeByIdAsync(officeId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        _cache.RemoveData(_cacheKeyConstants.GetAll);
        var cacheKey = $"{_cacheKeyConstants.GetById}{officeId}";
        _cache.RemoveData(cacheKey);

        return NoContent();
    }

    /// <summary>
    /// Changes Office's status By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPut("{officeId}/changestatusofofficebyid")]
    [ProducesResponseType(200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> ChangeStatusOfOfficeByIdAsync(string officeId)
    {
        var result = await _officeService.ChangeStatusOfOfficeByIdAsync(officeId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        _cache.RemoveData(_cacheKeyConstants.GetAll);
        var cacheKey = $"{_cacheKeyConstants.GetById}{officeId}";
        _cache.RemoveData(cacheKey);

        return Ok();
    }
}
