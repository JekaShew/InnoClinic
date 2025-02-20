using Microsoft.AspNetCore.Mvc;
using OfficesAPI.Services.Abstractions.Interfaces;

namespace OfficesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]

public class PhotoController : ResponseMessageHandler
{
    private readonly IPhotoService _photoServices;
    public PhotoController(IPhotoService photoServices)
    {
        _photoServices = photoServices;
    }

    // add Photo to office   POST
    // delete Photo from office  DELETE
    // Get all photoes  GETALL
    // Get all photoes of office GET ALLwID
    // Get photo of office by id GET BY ID

}
