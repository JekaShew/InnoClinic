using FluentValidation;
using ProfilesAPI.Shared.DTOs.ReceptionistDTOs;

namespace ProfilesAPI.Services.Validators.ReceptionistValidators;

public class ReceptionistParametersValidator : AbstractValidator<ReceptionistParameters>
{
    public ReceptionistParametersValidator()
    {
        
    }
}
