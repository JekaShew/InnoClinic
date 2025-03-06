using FluentValidation;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorForUpdateDTOValidator : AbstractValidator<DoctorForUpdateDTO>
{
    public DoctorForUpdateDTOValidator()
    {
        
    }
}
