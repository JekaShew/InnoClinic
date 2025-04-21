using AutoMapper;
using CommonLibrary.CommonService;
using CommonLibrary.Constants;
using CommonLibrary.RabbitMQEvents.OfficeEvents;
using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using FluentValidation;
using InnoClinic.CommonLibrary.Exceptions;
using InnoClinic.CommonLibrary.Response;
using MassTransit;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Domain.IRepositories;
using ProfilesAPI.Services.Abstractions.Interfaces;
using ProfilesAPI.Services.Validators.OfficeValidators;
using Serilog;

namespace ProfilesAPI.Services.Services;

public class SpecializationService : ISpecializationService
{
    private readonly ICommonService _commonService;
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly IRepositoryManager _repositoryManager;
    private readonly IMapper _mapper;
    private readonly ILogger _logger;
    private readonly IValidator<SpecializationCreatedEvent> _specializationCreatedEventValidator;
    private readonly IValidator<SpecializationUpdatedEvent> _specializationUpdatedEventValidator;
    private readonly IValidator<SpecializationCheckConsistancyEvent> _specializationCheckConsistancyEventValidator;

    public SpecializationService(
        ICommonService commonService,
        IPublishEndpoint publishEndpoint,
        IRepositoryManager repositoryManager,
        IMapper mapper,
        ILogger logger,
        IValidator<SpecializationCreatedEvent> specializationCreatedEventValidator,
        IValidator<SpecializationUpdatedEvent> specializationUpdatedEventValidator,
        IValidator<SpecializationCheckConsistancyEvent> specializationCheckConsistancyEventValidator)
    {
        _commonService = commonService;
        _publishEndpoint = publishEndpoint;
        _repositoryManager = repositoryManager;
        _mapper = mapper;
        _logger = logger;
        _specializationCreatedEventValidator = specializationCreatedEventValidator;
        _specializationUpdatedEventValidator = specializationUpdatedEventValidator;
        _specializationCheckConsistancyEventValidator = specializationCheckConsistancyEventValidator;
    }

    public async Task<ResponseMessage> RequestCheckSpecializationConsistancyAsync()
    {
        var currentUserInfo = _commonService.GetCurrentUserInfo();
        if (currentUserInfo is null || !currentUserInfo.Role.Equals(RoleConstants.Administrator))
        {
            return new ResponseMessage("Forbidden Action provided! You are UnAuthorizaed or have No rights to call this Action!", 403);
        }
        // Maybe  set security in this object IN FUTURE!
        var specializationRequestCheckConsistancyEvent = new SpecializationRequestCheckConsistancyEvent()
        {
            UserId = currentUserInfo.Id,
            DateTime = DateTime.UtcNow,
        };

        await _publishEndpoint.Publish(specializationRequestCheckConsistancyEvent);
        _logger.Information($"Succesfully sent request to check OfficeConsistancy by user with ID: {specializationRequestCheckConsistancyEvent.UserId} at {specializationRequestCheckConsistancyEvent.DateTime}!");
        return new ResponseMessage();
    }

    public async Task CheckSpecializationConsistancyAsync(SpecializationCheckConsistancyEvent specializationCheckConsistancyEvent)
    {
        var validationResult = await _specializationCheckConsistancyEventValidator.ValidateAsync(specializationCheckConsistancyEvent);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var specialization = await _repositoryManager.Specialization.GetByIdAsync(specializationCheckConsistancyEvent.Id);
        var consistantSpecialization = _mapper.Map<Specialization>(specializationCheckConsistancyEvent);
        if (specialization is null)
        {
            await _repositoryManager.Specialization.CreateAsync(consistantSpecialization);
            _logger.Information($"Succesfully added Specialization: {consistantSpecialization}");
        }

        if (specialization is not null && !specialization.Equals(consistantSpecialization))
        {
            await _repositoryManager.Specialization.UpdateAsync(consistantSpecialization.Id, consistantSpecialization);
            _logger.Information($"Succesfully updated Specialization: {consistantSpecialization}");
        }
    }

    public async Task CreateSpecializationAsync(SpecializationCreatedEvent specializationCreatedEvent)
    {
        var validationResult = await _specializationCreatedEventValidator.ValidateAsync(specializationCreatedEvent);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var specialziation = _mapper.Map<Specialization>(specializationCreatedEvent);
        await _repositoryManager.Specialization.CreateAsync(specialziation);
        _logger.Information($"Succesfully added Specialization: {specialziation}");
    }

    public async Task DeleteSpecializationAsync(SpecializationDeletedEvent specializationDeletedEvent)
    {
        var specializationToDelete = await _repositoryManager.Specialization.GetByIdAsync(specializationDeletedEvent.Id);

        if (specializationToDelete is not null)
        {
            await _repositoryManager.Specialization.SoftDeleteAsync(specializationToDelete);
            _logger.Information($"Succesfully deleted Specialization with Id: {specializationDeletedEvent.Id}");
        }        
    }

    public async Task UpdateSpecializationAsync(SpecializationUpdatedEvent specializationUpdatedEvent)
    {
        var validationResult = await _specializationUpdatedEventValidator.ValidateAsync(specializationUpdatedEvent);
        if (!validationResult.IsValid)
        {
            throw new ValidationAppException(validationResult.Errors.Select(e => e.ErrorMessage).ToArray());
        }

        var specialization = _mapper.Map<Specialization>(specializationUpdatedEvent);
        await _repositoryManager.Specialization.UpdateAsync(specializationUpdatedEvent.Id, specialization);
        _logger.Information($"Succesfully updated Specialization: {specialization}");
    }
}
