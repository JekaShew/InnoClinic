using CommonLibrary.RabbitMQEvents.PatientEvents;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.PatientValidators;

public class PatientCreatedEventValidator : AbstractValidator<PatientCreatedEvent>
{
    public PatientCreatedEventValidator()
    {
        RuleFor(p => p)
           .NotNull()
           .WithMessage("Patient Shouldn't be null!");

        RuleFor(p => p.UserId)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Patient's UserId shouldn't be null!");

        RuleFor(p => p.Address)
            .NotNull()
            .NotEmpty()
            .WithMessage("Patient's Adress shouldn't be null!");

        RuleFor(p => p.FirstName)
           .NotNull()
           .NotEmpty()
           .WithMessage("Patient's First Name shouldn't be null!");

        RuleFor(p => p.LastName)
           .NotNull()
           .NotEmpty()
           .WithMessage("Patient's Last Name shouldn't be null!");
    }
}
