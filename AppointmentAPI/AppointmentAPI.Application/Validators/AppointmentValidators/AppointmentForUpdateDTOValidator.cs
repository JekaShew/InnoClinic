using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.AppointmentValidators;

public class AppointmentForUpdateDTOValidator : AbstractValidator<AppointmentForUpdateDTO>
{
    public AppointmentForUpdateDTOValidator()
    {
        
    }
}
