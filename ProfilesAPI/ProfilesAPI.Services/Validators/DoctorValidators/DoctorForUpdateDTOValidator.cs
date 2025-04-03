using FluentValidation;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorForUpdateDTOValidator : AbstractValidator<DoctorForUpdateDTO>
{
    public DoctorForUpdateDTOValidator()
    {
        RuleFor(x => x.FirstName)
          .NotEmpty()
          .NotNull()
          .WithMessage("Doctor's First Name is required!");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Doctor's Last Name is required!");

        RuleFor(x => x.WorkEmail)
           .NotEmpty()
           .NotNull()
           .EmailAddress()
           .WithMessage("Doctor's Work Email is required and should be valid!");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .NotNull()
            .WithMessage("Doctor's Phone is required!");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Doctor's Birth Date is required!");

        RuleFor(x => x.CareerStartDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Doctor's Career Start Date is required!");
    }
}
