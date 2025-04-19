using AppointmentAPI.Shared.DTOs.AppointmentDTOs;
using FluentValidation;

namespace AppointmentAPI.Application.Validators.AppointmentValidators;

public class AppointmentForCreateDTOValidator : AbstractValidator<AppointmentForCreateDTO>
{
    public AppointmentForCreateDTOValidator()
    {
        
    }
}
