using FluentValidation;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Validators.ReceptionistValidators;

public class ReceptionistForUpdateDTOValidator : AbstractValidator<ReceptionistForUpdateDTO>
{
    public ReceptionistForUpdateDTOValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty()
           .NotNull()
           .WithMessage("Receptionist's First Name is required!");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Receptionist's Last Name is required!");

        RuleFor(x => x.WorkEmail)
           .NotEmpty()
           .NotNull()
           .EmailAddress()
           .WithMessage("Receptionist's Work Email is required and should be valid!");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .NotNull()
            .WithMessage("Receptionist's Phone is required!");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Receptionist's Birth Date is required!");

        RuleFor(x => x.CareerStartDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Receptionist's Career Start Date is required!");
    }
}
