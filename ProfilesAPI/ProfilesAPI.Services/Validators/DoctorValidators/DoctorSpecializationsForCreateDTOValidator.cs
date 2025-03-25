using FluentValidation;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorSpecializationsForCreateDTOValidator : AbstractValidator<DoctorSpecializationForCreateDTO>
{
    public DoctorSpecializationsForCreateDTOValidator()
    {
        RuleFor(x => x.SpecializationId)
            .NotNull()
            .NotEmpty()
            .Must(specializationId =>
            {
                bool isGuid = Guid.TryParse(specializationId.ToString(), out _);
                return Guid.TryParse(specializationId.ToString(), out _);
            })
            .WithMessage("Specialization is required and should be valid!");

        RuleFor(x => x.SpecialzationAchievementDate)
            .NotNull()
            .NotEmpty()
            .WithMessage("Specialization Achievement Date is required!");
    }
}
