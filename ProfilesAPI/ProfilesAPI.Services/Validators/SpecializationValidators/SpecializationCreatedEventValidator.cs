using CommonLibrary.RabbitMQEvents.SpecializationEvents;
using FluentValidation;
using ProfilesAPI.Shared.DTOs.SpecializationDTOs;

namespace ProfilesAPI.Services.Validators.SpecializationValidators;

public class SpecializationCreatedEventValidator : AbstractValidator<SpecializationCreatedEvent>
{
    public SpecializationCreatedEventValidator()
    {
        RuleFor(c => c)
              .NotNull()
              .WithMessage("Specialization shouldn't be null!");

        RuleFor(c => c.Id)
              .NotEmpty()
              .NotNull()
              .WithMessage("Specialization's ID shouldn't be null!");
        
        RuleFor(c => c.Title)
           .NotEmpty()
           .NotNull()
           .WithMessage("Specialization's title shouldn't be null!");
    }
}
