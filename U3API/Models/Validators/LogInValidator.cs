using FluentValidation;
using U3API.Models.DTOs;

namespace U3API.Models.Validators
{
    public class LogInValidator : AbstractValidator<LogInDto>
    {
        public LogInValidator()
        {
            RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese su usuario.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Ingrese su contraseña.");
        }
    }
}
