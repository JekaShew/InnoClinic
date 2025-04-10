using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.Response;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs.PhotoDTOs;

namespace OfficesAPI.Presentation.Controllers;

[Route("api/Offices/{officeId}/[controller]")]
[ApiController]

public class PhotoController : ControllerBase
{
    private readonly IPhotoService _photoServices;
 
    public PhotoController(IPhotoService photoServices)
    {
        _photoServices = photoServices;
    }

    /// <summary>
    /// Adds new Photo to office by office Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpPost]
    [ProducesResponseType(typeof(PhotoInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddPhototoOffice(string officeId, IFormFile formFile)
    {
        var result = await _photoServices.AddPhototoOffice(officeId, formFile);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return CreatedAtAction(nameof(GetPhotoById), new { photoId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Deletes Office's photo By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{photoId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteOfficePhotoById(string officeId, string photoId)
    {
        var result = await _photoServices.DeleteOfficePhotoById(officeId, photoId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return NoContent();
    }

    /// <summary>
    /// Gets the list of all Photos
    /// </summary>
    /// <returns>The Photos list</returns>
    [HttpGet("/getallphotos")]
    [ProducesResponseType(typeof(IEnumerable<PhotoInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllPhotos()
    {
        var result = await _photoServices.GetAllPhotos();
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }

    /// <summary>
    /// Gets All office's Photos by Office Id
    /// </summary>
    /// <returns>The Photos list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(IEnumerable<PhotoInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> GetAllPhotosOfOfficeById(string officeId)
    {
        var result = await _photoServices.GetAllPhotosOfOfficeById(officeId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }
 
    /// <summary>
    /// Gets selected Photo
    /// </summary>
    /// <returns>Single Photo</returns>
    [HttpGet("/{photoId}")]
    [ProducesResponseType(typeof(PhotoInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> GetPhotoById(string photoId)
    {
        var result = await _photoServices.GetPhotoById(photoId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }
            
        return Ok(result.Value);
    }
}
