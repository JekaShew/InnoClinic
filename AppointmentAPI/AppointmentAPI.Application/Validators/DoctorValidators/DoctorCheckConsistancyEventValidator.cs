using CommonLibrary.RabbitMQEvents.DoctorEvents;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.DoctorValidators;

public class DoctorCheckConsistancyEventValidator : AbstractValidator<DoctorCheckConsistancyEvent>
{
    public DoctorCheckConsistancyEventValidator()
    {
        RuleFor(d => d)
           .NotNull()
           .WithMessage("Doctor Shouldn't be null!");

        RuleFor(d => d.UserId)
            .NotNull()
            .NotEmpty()
            .NotEqual(Guid.Empty)
            .WithMessage("Doctor's UserId shouldn't be null!");

        RuleFor(d => d.FirstName)
           .NotNull()
           .NotEmpty()
           .WithMessage("Doctor's First Name shouldn't be null!");

        RuleFor(d => d.LastName)
           .NotNull()
           .NotEmpty()
           .WithMessage("Doctor's Last Name shouldn't be null!");
    }
}
