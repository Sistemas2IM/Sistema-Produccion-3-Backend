using FluentValidation;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;

namespace Sistema_Produccion_3_Backend.Validators.ProductoTerminado
{
    public class PTerminadoValidator : AbstractValidator<AddProductoTerminadoDto>
    {
        public PTerminadoValidator() 
        {
            RuleFor(x => x.fechaEntrega)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.areaEntrega)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.areaRecibe)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.numeroDeCorrugados)
                .NotEmpty().WithMessage("{PropertyName} es requerido");

            RuleFor(x => x.numeroDeTarimas)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.creadoPor)
                .NotEmpty().WithMessage("{PropertyName} es requerido");

            RuleFor(x => x.recibidoPor)
                .NotEmpty().WithMessage("{PropertyName} es requerido");

            RuleFor(x => x.entregaParcial)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.reportaSobrantes)
                .NotEmpty().WithMessage("{PropertyName} es requerido");

            RuleFor(x => x.cantidadSobrante)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.cantidadEntregadaTotal)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.fechaCreacion)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.actualizadoPor)
                .NotEmpty().WithMessage("{PropertyName} es requerida");

            RuleFor(x => x.ultimaActualizacion)
                .NotEmpty().WithMessage("{PropertyName} es requerida");               

            RuleFor(x => x.tipoObjeto)
                .NotEmpty().WithMessage("{PropertyName} es requerido");
        }
    }
}
