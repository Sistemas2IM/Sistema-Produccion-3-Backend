using FluentValidation;
using Sistema_Produccion_3_Backend.DTO.LoginAuth;

namespace Sistema_Produccion_3_Backend.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator() {


            RuleFor(x => x.User)
                .NotEmpty().WithMessage("{PropertyName} es requerido")
                .MinimumLength(3).WithMessage("{PropertyName} debe tener al menos 3 caracteres")
                .MaximumLength(50).WithMessage("{PropertyName} no debe tener una longitud mayor a 50")
                .Matches(@"^\S*$").WithMessage("{PropertyName} no debe contener espacios");

            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("La Contraseña es requerida")
                .MinimumLength(6).WithMessage("La Contraseña debe tener al menos 6 caracteres")
                .Matches(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[@$!%*?&])[A-Za-z\d@$!%*?&]*$").WithMessage(
                    "La contraseña debe contener al menos una letra mayúscula, una minúscula, un número y un carácter especial"
                );

            RuleFor(x => x.Nombres)
                .NotEmpty().WithMessage("{PropertyName} son requeridos")
                .MaximumLength(200).WithMessage("{PropertyName} no debe tener una longitud mayor a 200")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("{PropertyName} solo puede contener letras y espacios");

            RuleFor(x => x.Apellidos)
                .NotEmpty().WithMessage("{PropertyName} son requeridos")
                .MaximumLength(200).WithMessage("{PropertyName} no debe tener una longitud mayor a 200")
                .Matches(@"^[a-zA-Z\s]+$").WithMessage("{PropertyName} solo puede contener letras y espacios");

            // Validación opcional para el Email
            RuleFor(x => x.Email)
                .EmailAddress().WithMessage("{PropertyName} no tiene un formato válido")
                .When(x => !string.IsNullOrEmpty(x.Email)); // Solo valida si el campo no está vacío o es nulo
        }
    }
}
