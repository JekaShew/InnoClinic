using FluentValidation;
using ProfilesAPI.Shared.DTOs.DoctorDTOs;

namespace ProfilesAPI.Services.Validators.DoctorValidators;

public class DoctorForCreateDTOValidator : AbstractValidator<DoctorForCreateDTO>
{
    public DoctorForCreateDTOValidator()
    {
        
    }
}
