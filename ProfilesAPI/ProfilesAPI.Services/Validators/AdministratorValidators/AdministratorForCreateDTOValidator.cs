using FluentValidation;
using ProfilesAPI.Shared.DTOs.AdministratorDTOs;

namespace ProfilesAPI.Services.Validators.AdministratorValidators;

public class AdministratorForCreateDTOValidator : AbstractValidator<AdministratorForCreateDTO>
{
    public AdministratorForCreateDTOValidator()
    {
        RuleFor(x => x.FirstName)
           .NotEmpty()
           .NotNull()
           .WithMessage("Administrator's First Name is required!");

        RuleFor(x => x.LastName)
            .NotEmpty()
            .NotNull()
            .WithMessage("Administrator's Last Name is required!");

        RuleFor(x => x.WorkEmail)
           .NotEmpty()
           .NotNull()
           .EmailAddress()
           .WithMessage("Administrator's Work Email is required and should be valid!");

        RuleFor(x => x.Phone)
            .NotEmpty()
            .NotNull()
            .WithMessage("Administrator's Phone is required!");

        RuleFor(x => x.BirthDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Administrator's Birth Date is required!");

        RuleFor(x => x.CareerStartDate)
            .NotEmpty()
            .NotNull()
            .WithMessage("Administrator's Career Start Date is required!");
    }
}
