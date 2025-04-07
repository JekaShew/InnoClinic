using FluentValidation;
using ServicesAPI.Shared.DTOs.ServiceCategorySpecializationDTOs;

namespace ServicesAPI.Application.Validators.ServiceCategorySpecializationValidators;

public class ServiceCategorySpecializationForUpdateDTOValidator : AbstractValidator<ServiceCategorySpecializationForUpdateDTO>
{
    public ServiceCategorySpecializationForUpdateDTOValidator()
    {
        RuleFor(x => x.Description)
            .MaximumLength(1000)
            .WithMessage("Description should be less than 1000 symbols!");

    }
}
