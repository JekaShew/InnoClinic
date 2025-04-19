using AppointmentAPI.Shared.DTOs.AppointmentResultDTOs;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.AppointmentResultValidators;

public class AppointmentResultForCreateDTOValidator : AbstractValidator<AppointmentResultForCreateDTO>
{
    public AppointmentResultForCreateDTOValidator()
    {
        
    }
}
