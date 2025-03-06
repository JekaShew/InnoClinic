using FluentValidation;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Validators.ReceptionistValidators;

public class ReceptionistForCreateDTOValidator : AbstractValidator<ReceptionistForCreateDTO>
{
    public ReceptionistForCreateDTOValidator()
    {
        
    }
}
