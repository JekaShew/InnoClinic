using CommonLibrary.RabbitMQEvents.OfficeEvents;
using FluentValidation;

namespace ProfilesAPI.Services.Validators.OfficeValidators;

public class OfficeUpdatedEventValidator : AbstractValidator<OfficeUpdatedEvent>
{
    public OfficeUpdatedEventValidator()
    {
        RuleFor(c => c)
             .NotNull()
             .WithMessage("Office shouldn't be null!");

        RuleFor(c => c.Id)
              .NotEmpty()
              .NotNull()
              .WithMessage("Office's ID shouldn't be null!");

        RuleFor(c => c.City)
           .NotEmpty()
           .NotNull()
           .WithMessage("Office's city shouldn't be null!");

        RuleFor(c => c.Street)
          .NotEmpty()
          .NotNull()
          .WithMessage("Office's street shouldn't be null!");

        RuleFor(c => c.HouseNumber)
          .NotEmpty()
          .NotNull()
          .WithMessage("Office's house number shouldn't be null!");
    }
}
