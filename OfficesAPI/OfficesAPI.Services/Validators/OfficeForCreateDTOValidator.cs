using FluentValidation;
using OfficesAPI.Shared.DTOs;

namespace OfficesAPI.Services.Validators
{
    public class OfficeForCreateDTOValidator : AbstractValidator<OfficeForCreateDTO>
    {
        public OfficeForCreateDTOValidator()
        {
            RuleFor(o => o.IsActive)
               .NotNull()
               .NotEmpty()
               .WithMessage("Office's Status is required!");

            RuleFor(o => o.City)
                .NotNull()
                .NotEmpty()
                .WithMessage("Office's City is required!");

            RuleFor(o => o.Street)
                .NotNull()
                .NotEmpty()
                .WithMessage("Office's Street is required!");

            RuleFor(o => o.HouseNumber)
                .NotNull()
                .NotEmpty()
                .WithMessage("Office's House Number is required!");
        }
    }
}
