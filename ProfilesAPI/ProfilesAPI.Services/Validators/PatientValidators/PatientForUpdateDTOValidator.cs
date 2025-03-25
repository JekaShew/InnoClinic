using FluentValidation;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Validators.PatientValidators;

public class PatientForUpdateDTOValidator : AbstractValidator<PatientForUpdateDTO>
{
    public PatientForUpdateDTOValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty()
           .NotNull()
           .WithMessage("Patient's First Name is required!");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Patient's Last Name is required!");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .NotNull()
            .WithMessage("Patient's Phone is required!");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Patient's Birth Date is required!");
    }
}
