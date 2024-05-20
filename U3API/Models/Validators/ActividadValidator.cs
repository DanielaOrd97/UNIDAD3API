using FluentValidation;
using U3API.Models.DTOs;

namespace U3API.Models.Validators
{
    public class ActividadValidator : AbstractValidator<ActividadDTO>
    {
        public ActividadValidator()
        {
            RuleFor(x => x.Titulo).NotEmpty().WithMessage("Ingrese el titulo de la actividad.");
            RuleFor(x => x.Descripcion).NotEmpty().WithMessage("Ingrese la descripcion de la actividad.");
            RuleFor(x => x.FechaDeRealizacion).NotEmpty().WithMessage("Ingrese la fecha en la que se realizo la actividad.");
          //  RuleFor(x => x.IdDepartamento).NotEmpty().WithMessage("Indique el departamento al que pertenece la actividad.");
            //FOTOS
        }
    }
}
