using FluentValidation;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Validators.PatientValidators;

public class PatientForUpdateDTOValidator : AbstractValidator<PatientForUpdateDTO>
{
    public PatientForUpdateDTOValidator()
    {
        
    }
}
