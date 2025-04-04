using FluentValidation;
using ServicesAPI.Shared.DTOs.ServiceDTOs;

namespace ServicesAPI.Application.Validators.ServiceValidators;

public class ServiceForCreateDTOValidator : AbstractValidator<ServiceForCreateDTO>
{
    public ServiceForCreateDTOValidator()
    {
        RuleFor(x => x.Title)
            .NotEmpty()
            .NotNull()
            .WithMessage("Service's Title is required!")
            .MaximumLength(60)
            .WithMessage("Service's Title should be less than 60 symbols!");

        RuleFor(x => x.Price)
            .NotEmpty()
            .NotNull()
            .WithMessage("Service's Price is required!")
            .GreaterThan(0)
            .WithMessage("Service's Price should be greater than 0!");

        RuleFor(x => x.ServiceCategoryId)
            .NotEmpty()
            .NotNull()
            .WithMessage("Service's ServiceCategoryId is required!");
    }
}
