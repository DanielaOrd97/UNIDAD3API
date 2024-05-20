using FluentValidation;
using U3API.Models.DTOs;

namespace U3API.Models.Validators
{
    public class DepartamentoValidator: AbstractValidator<DepartamentoDTO>
    {
        public DepartamentoValidator()
        {
            RuleFor(x => x.NombreDepartamento).NotEmpty().WithMessage("Ingrese el nombre del departamento.");
            RuleFor(x => x.Username).NotEmpty().WithMessage("Ingrese un nombre de usuario.");
            RuleFor(x => x.Password).NotEmpty().WithMessage("Ingrese una contraseña.");
        }
    }
}
