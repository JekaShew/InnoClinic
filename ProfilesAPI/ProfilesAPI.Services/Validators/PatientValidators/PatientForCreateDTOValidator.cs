using FluentValidation;
using ProfilesAPI.Shared.DTOs.PatientDTOs;

namespace ProfilesAPI.Services.Validators.PatientValidators;

public class PatientForCreateDTOValidator : AbstractValidator<PatientForCreateDTO>
{
    public PatientForCreateDTOValidator()
    {
        
    }
}
