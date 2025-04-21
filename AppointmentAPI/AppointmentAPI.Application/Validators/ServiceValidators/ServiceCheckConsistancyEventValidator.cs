using CommonLibrary.RabbitMQEvents.ServiceEvents;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.ServiceValidators;

public class ServiceCheckConsistancyEventValidator : AbstractValidator<ServiceCheckConsistancyEvent>
{
    public ServiceCheckConsistancyEventValidator()
    {
        RuleFor(s => s)
            .NotNull()
            .NotEmpty()
            .WithMessage("Service Shouldn't be null!");

        RuleFor(s => s.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Service should have Title!");

        RuleFor(s => s.Price)
            .NotEmpty()
            .NotNull()
            .GreaterThan(0M)
            .WithMessage("Service's price should be more than 0!");
    }
}
