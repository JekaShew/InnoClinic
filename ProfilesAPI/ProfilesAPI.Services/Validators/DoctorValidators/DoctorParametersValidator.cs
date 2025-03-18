using FluentValidation;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorParametersValidator : AbstractValidator<DoctorParameters>
{
    public DoctorParametersValidator()
    {
        
    }
}
