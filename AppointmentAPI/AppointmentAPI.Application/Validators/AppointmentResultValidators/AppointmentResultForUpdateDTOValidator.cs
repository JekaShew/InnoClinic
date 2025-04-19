using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.AppointmentResultValidators;

public class AppointmentResultForUpdateDTOValidator : AbstractValidator<AppointmentResultForUpdateDTO>
{
    public AppointmentResultForUpdateDTOValidator()
    {
        
    }
}
