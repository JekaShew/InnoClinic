using FluentValidation;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Validators.ReceptionistValidators;

public class ReceptionistForUpdateDTOValidator : AbstractValidator<ReceptionistForUpdateDTO>
{
    public ReceptionistForUpdateDTOValidator()
    {
        
    }
}
