using CommonLibrary.Response;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using ServicesAPI.Application.CQRS.Commands.ServiceCategorySpecializationCommands;
using ServicesAPI.Application.CQRS.Queries.ServiceCategorySpecializationQueries;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Presentation.Controllers;

[Route("api/[controller]")]
[ApiController]
public class ServiceCategorySpecializationsController : ControllerBase
{
    private readonly IMediator _mediator;

    public ServiceCategorySpecializationsController(IMediator mediator)
    {
        _mediator = mediator;
    }

    /// <summary>
    /// Updates selected Service Category Specialization Pair
    /// </summary>
    /// <returns>Updated Service Category Specialization Pair</returns>
    [HttpPut("{serviceCategorySpecializationId}")]
    [ProducesResponseType(typeof(ServiceCategorySpecializationInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 422)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> UpdateServiceCategorySpecialization(Guid serviceCategorySpecializationId, [FromBody] ServiceCategorySpecializationForUpdateDTO serviceCategorySpecializationForUpdateDTO)
    {
        var result = await _mediator.Send(new UpdateServiceCategorySpecializationCommand() { ServiceCategorySpecializationId = serviceCategorySpecializationId, ServiceCategorySpecializationForUpdateDTO = serviceCategorySpecializationForUpdateDTO });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets selected Service Category Specialization Pair
    /// </summary>
    /// <returns>Single Service Category Specialization Pair</returns>
    [HttpGet("{serviceCategorySpecializationId}")]
    [ProducesResponseType(typeof(ServiceCategorySpecializationInfoDTO), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetServiceCategorySpecializationById(Guid serviceCategorySpecializationId)
    {
        var result = await _mediator.Send(new GetServiceCategorySpecializationByIdQuery() { Id = serviceCategorySpecializationId });
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }

    /// <summary>
    /// Gets the list of all Service Category Specialization Pairs
    /// </summary>
    /// <returns>The Service Category Specialization Pair list</returns>
    [HttpGet]
    [ProducesResponseType(typeof(ICollection<ServiceCategorySpecializationInfoDTO>), 200)]
    [ProducesResponseType(typeof(FailMessage), 400)]
    [ProducesResponseType(typeof(FailMessage), 403)]
    [ProducesResponseType(typeof(FailMessage), 404)]
    [ProducesResponseType(typeof(FailMessage), 408)]
    [ProducesResponseType(typeof(FailMessage), 500)]
    //[Authorize(Roles = "Administrator")]
    public async Task<IActionResult> GetAllServiceCategorySpecializations([FromBody] ServiceCategorySpecializationParameters? serviceCategorySpecializationParameters)
    {
        var result = await _mediator.Send(new GetAllServiceCategorySpecializationQuery() { ServiceCategorySpecializationParameters = serviceCategorySpecializationParameters});
        if (!result.IsComplited)
        {
            return new FailMessage(result.ErrorMessage, result.StatusCode);
        }

        return Ok(result.Value);
    }
}
