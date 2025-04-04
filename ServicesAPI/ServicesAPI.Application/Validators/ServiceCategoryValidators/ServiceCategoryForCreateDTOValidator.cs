using FluentValidation;
using ServicesAPI.Shared.DTOs.ServiceCategoryDTOs;

namespace ServicesAPI.Application.Validators.ServiceCategoryValidators;

public class ServiceCategoryForCreateDTOValidator : AbstractValidator<ServiceCategoryForCreateDTO>
{
    public ServiceCategoryForCreateDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Service Category's Title is required!")
            .MaximumLength(60)
            .WithMessage("Service Category's Title should be less than 60 symbols!");
    }
}
