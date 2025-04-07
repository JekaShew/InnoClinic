using FluentValidation;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Validators.PatientValidators;

public class PatientParametersValidator : AbstractValidator<PatientParameters>
{
    public PatientParametersValidator()
    {
        
    }
}
