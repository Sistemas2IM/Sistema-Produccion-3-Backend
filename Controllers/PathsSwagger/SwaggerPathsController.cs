using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Sistema_Produccion_3_Backend.Controllers.PathsSwagger
{
    using Microsoft.AspNetCore.Mvc;
    using System.Text.Json;
    using System.Threading.Tasks;
    using System.Net.Http;
    using System.Collections.Generic;

    [Route("api/[controller]")]
    [ApiController]
    public class SwaggerPathsController : ControllerBase
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public SwaggerPathsController(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        // GET: api/SwaggerPaths/onlypaths
        [HttpGet("onlypaths")]
        public async Task<IActionResult> GetOnlyPaths()
        {
            var swaggerUrl = "http://192.168.2.62:667/swagger/v1/swagger.json"; // Cambia esta URL si es necesario
            var client = _httpClientFactory.CreateClient();

            // Obtener el JSON completo de Swagger
            var response = await client.GetStringAsync(swaggerUrl);
            var jsonDocument = JsonDocument.Parse(response);

            // Extraer solo la parte de 'paths'
            if (jsonDocument.RootElement.TryGetProperty("paths", out var paths))
            {
                var structuredEndpoints = new Dictionary<string, Dictionary<string, List<string>>>();

                // Recorrer los paths y organizar la información
                foreach (var path in paths.EnumerateObject())
                {
                    foreach (var method in path.Value.EnumerateObject())
                    {
                        var resourceName = path.Name.Split('/')[2]; // Extrae el nombre del recurso (OF, Asignacion, etc.)
                        var methodType = method.Name.ToUpper(); // Método HTTP (GET, POST, etc.)

                        // Crear un diccionario para el recurso si no existe
                        if (!structuredEndpoints.ContainsKey(resourceName))
                        {
                            structuredEndpoints[resourceName] = new Dictionary<string, List<string>>();
                        }

                        // Asegurarse de que el método HTTP tenga una lista para almacenar múltiples endpoints
                        if (!structuredEndpoints[resourceName].ContainsKey(methodType))
                        {
                            structuredEndpoints[resourceName][methodType] = new List<string>();
                        }

                        // Agregar el endpoint tal cual, pero identificando duplicados
                        var endpoint = path.Name; // Usar solo el endpoint sin modificaciones
                        var existingEndpoints = structuredEndpoints[resourceName][methodType];

                        // Generar un nombre único usando un sufijo numérico (no visible)
                        var uniqueEndpoint = existingEndpoints.Count == 0 ? endpoint : $"{endpoint}";
                        existingEndpoints.Add(uniqueEndpoint);
                    }
                }

                // Devolver la estructura en formato JSON
                return Ok(structuredEndpoints);
            }

            return NotFound("No se encontraron paths en el Swagger JSON.");
        }
    }
}
