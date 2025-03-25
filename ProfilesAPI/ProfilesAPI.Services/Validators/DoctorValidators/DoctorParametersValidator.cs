using FluentValidation;
using ProfilesAPI.Domain.Data.Models;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorParametersValidator : AbstractValidator<DoctorParameters>
{
    public DoctorParametersValidator()
    {
        RuleFor(x => x.Offices)
          .Must(offices => offices == null
           || offices.All(office => office is string))
          .WithMessage("Incorrect Office value!");

        RuleFor(x => x.Specializations)
          .Must(specializations => specializations == null
           || specializations.All(specialization => Guid.TryParse(specialization.ToString(), out _) && !specialization.Equals(Guid.Empty) ))
          .WithMessage("Incorrect Specialization value!");
    }
}
