using FluentValidation;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorSpecializationForUpdateDTOValidator : AbstractValidator<DoctorSpecializationForUpdateDTO>
{
    public DoctorSpecializationForUpdateDTOValidator()
    {
        
    }
}
