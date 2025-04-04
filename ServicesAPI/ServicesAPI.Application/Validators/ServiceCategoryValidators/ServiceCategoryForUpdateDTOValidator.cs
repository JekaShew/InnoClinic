using FluentValidation;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.Validators.ServiceCategoryValidators;

public class ServiceCategoryForUpdateDTOValidator : AbstractValidator<ServiceCategoryForUpdateDTO>
{
    public ServiceCategoryForUpdateDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Service Category's Title is required!")
            .MaximumLength(60)
            .WithMessage("Service Category's Title should be less than 60 symbols!");
    }
}
