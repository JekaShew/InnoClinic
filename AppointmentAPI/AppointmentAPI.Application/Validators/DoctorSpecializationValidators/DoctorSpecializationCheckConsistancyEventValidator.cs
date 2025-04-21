using CommonLibrary.RabbitMQEvents.DoctorSpecializationEvents;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.DoctorSpecializationValidators;

public class DoctorSpecializationCheckConsistancyEventValidator : AbstractValidator<DoctorSpecializationCheckConsistancyEvent>
{
    public DoctorSpecializationCheckConsistancyEventValidator()
    {
        RuleFor(ds => ds)
            .NotNull()
            .WithMessage("Doctor's Specialization shouldn't be null!");

        RuleFor(ds => ds.DoctorId)
            .NotNull()
            .WithMessage("Doctor's Specialization Id shouldn't be null!");

        RuleFor(ds => ds.SpecializationId)
            .NotNull()
            .WithMessage("Doctor's Id shouldn't be null!");
    }
}
