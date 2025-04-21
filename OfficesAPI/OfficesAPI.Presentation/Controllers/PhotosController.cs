using CommonLibrary.Response;
using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Services.Abstractions.Interfaces;
using OfficesAPI.Shared.DTOs.PhotoDTOs;

namespace OfficesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class PhotosController : ControllerBase
{
    private readonly IPhotoService _photoServices;

    public PhotosController(IPhotoService photoServices)
    {
        _photoServices = photoServices;
    }

    /// <summary>
    /// Gets the list of all Photos
    /// </summary>
    /// <returns>The Photos list</returns>
    [HttpGet()]
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
    /// Gets selected Photo
    /// </summary>
    /// <returns>Single Photo</returns>
    [HttpGet("{photoId}")]
    [ProducesResponseType(typeof(PhotoInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    public async Task<IActionResult> GetPhotoById(Guid photoId)
    {
        var result = await _photoServices.GetPhotoById(photoId);
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }
}
