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
            string apiKey = string.Empty;

            // Primero buscar en el encabezado "apiKey"
            if (context.HttpContext.Request.Headers.TryGetValue("apiKey", out var extractedApiKey))
            {
                apiKey = extractedApiKey.ToString();
            }
            // Si no se encuentra en el encabezado, buscar en el query string
            else if (context.HttpContext.Request.Query.TryGetValue("apiKey", out var queryApiKey))
            {
                apiKey = queryApiKey.ToString();
            }

            // Si no se encuentra la API Key en ningún lugar
            if (string.IsNullOrWhiteSpace(apiKey))
            {
                context.Result = new UnauthorizedResult(); // La clave API es obligatoria
                return;
            }

            // Validar la API Key
            bool isValid = _apiKeyValidation.IsValidApiKey(apiKey);

            if (!isValid)
            {
                context.Result = new UnauthorizedResult(); // La clave API no es válida
            }
        }
    }



}
