using FluentValidation;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorSpecializationsForCreateDTOValidator : AbstractValidator<DoctorSpecializationForCreateDTO>
{
    public DoctorSpecializationsForCreateDTOValidator()
    {
        
    }
}
