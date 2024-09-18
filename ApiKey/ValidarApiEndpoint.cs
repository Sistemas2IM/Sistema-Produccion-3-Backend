using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Sistema_Produccion_3_Backend.ApiKey
{
    public class ValidarApiEndpoint : ActionFilterAttribute
    {
        private readonly IApiKeyValidation _apiKeyValidation;

        public ValidarApiEndpoint(IApiKeyValidation apiKeyValidation)
        {
            _apiKeyValidation = apiKeyValidation;
        }

        public override void OnActionExecuting(ActionExecutingContext context)
        {
            if (!context.ActionArguments.ContainsKey("apiKey") || string.IsNullOrWhiteSpace(context.ActionArguments["apiKey"]?.ToString()))
            {
                context.Result = new UnauthorizedResult(); // La clave API es obligatoria
                return;
            }

            string apiKey = context.ActionArguments["apiKey"]?.ToString();
            bool isValid = _apiKeyValidation.IsValidApiKey(apiKey);

            if (!isValid)
            {
                context.Result = new UnauthorizedResult(); // La clave API no es válida
            }
        }
    }
}
