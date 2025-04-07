using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Application.CQRS.Commands.ServiceCategoryCommands;
using ServicesAPI.Application.CQRS.Queries.ServiceCategoryQueries;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceCategoriesController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceCategoriesController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Gets selected Service Category
    /// </summary>
    /// <returns>Single Service Category</returns>
    [HttpGet("{serviceCategoryId}")]
    [ProducesResponseType(typeof(ServiceCategoryInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetServiceCategoryById(Guid serviceCategoryId)
    {
        var result = await _mediator.Send(new GetServiceCategoryByIdQuery(){ Id = serviceCategoryId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Service Categories
    /// </summary>
    /// <returns>The Service Category list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ServiceCategoryTableInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllServiceCategories([FromBody] ServiceCategoryParameters? serviceCategoryParameters)
    {
        var result = await _mediator.Send(new GetAllServiceCategoriesQuery() { ServiceCategoryParameters = serviceCategoryParameters });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Creates new Service Category
    /// </summary>
    /// <returns>Created Service Category</returns>
    [HttpPost]
    [ProducesResponseType(typeof(ServiceCategoryInfoDTO), 201)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> AddServiceCategory([FromBody] ServiceCategoryForCreateDTO serviceCategoryForCreateDTO)
    {
        var result = await _mediator.Send(new CreateServiceCategoryCommand() { ServiceCategoryForCreateDTO = serviceCategoryForCreateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return CreatedAtAction(nameof(GetServiceCategoryById), new { serviceCategoryId = result.Value.Id }, result.Value);
    }

    /// <summary>
    /// Updates selected Service Category 
    /// </summary>
    /// <returns>Updated Service Category</returns>
    [HttpPut("{serviceCategoryId}")]
    [ProducesResponseType(typeof(ServiceCategoryInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateServiceCategory(Guid serviceCategoryId, [FromBody] ServiceCategoryForUpdateDTO serviceCategoryForUpdateDTO)
    {
        var result =await _mediator.Send(new UpdateServiceCategoryCommand() { ServiceCategoryId = serviceCategoryId, ServiceCategoryForUpdateDTO = serviceCategoryForUpdateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Deletes Service Category By Id
    /// </summary>
    /// <returns>Message</returns>
    [HttpDelete("{serviceCategoryId}")]
    [ProducesResponseType(204)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> DeleteServiceCategoryById(Guid serviceCategoryId)
    {
        var result = await _mediator.Send(new DeleteServiceCategoryCommand() { Id = serviceCategoryId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return NoContent();
    }
}
