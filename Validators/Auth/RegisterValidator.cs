using FluentValidation;
using Sistema_Produccion_3_Backend.DTO.LoginAuth;

namespace Sistema_Produccion_3_Backend.Validators.Auth
{
    public class RegisterValidator : AbstractValidator<RegisterDto>
    {
        public RegisterValidator() {

            RuleFor(x => x.User).NotEmpty().WithMessage("{PropertyName} es requerido");
            RuleFor(x => x.Password)
                .NotEmpty().WithMessage("{PropertyName} es requerido")
                .Must(HaveValidPassword).WithMessage("La contraseña debe tener al menos una letra mayúscula, una minúscula y un número"); ;
            RuleFor(x => x.Nombres).NotEmpty().WithMessage("{PropertyName} es requerido");
            RuleFor(x => x.Apellidos).NotEmpty().WithMessage("{PropertyName} es requerido");
        }


        private bool HaveValidPassword(string password)
        {
            return password.Any(char.IsUpper) && password.Any(char.IsLower) && password.Any(char.IsDigit);
        }
    }
}
