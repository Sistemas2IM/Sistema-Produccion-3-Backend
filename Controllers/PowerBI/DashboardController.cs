using Microsoft.AspNetCore.Mvc;
using System.Net.Http;
using System.Threading.Tasks;
using System.Collections.Generic;
using System.Text.Json.Serialization;
using System.Text.Json;

namespace Sistema_Produccion_3_Backend.Controllers.PowerBI
{
    [Route("api/[controller]")]
    [ApiController]
    public class DashboardController : ControllerBase
    {
        private readonly HttpClient _httpClient;

        public DashboardController(IHttpClientFactory httpClientFactory)
        {
            _httpClient = httpClientFactory.CreateClient();
        }

        [HttpGet("getToken")]
        public async Task<IActionResult> GetToken()
        {
            // Configuración de Azure AD
            var clientId = "13195310-ed46-4fbc-a3ae-820b9fecc038"; // Reemplaza con tu Client ID
            var clientSecret = "oGu8Q~J1i3gdS9VG.OYRt4sMSW~ofHFsNtOy_c0Q"; // Reemplaza con tu Client Secret
            var tenantId = "df92de6b-0963-4f60-ae89-f741299367b0"; // Reemplaza con tu Tenant ID
            var scope = "https://analysis.windows.net/powerbi/api/.default";

            // URL de Azure AD
            var url = $"https://login.microsoftonline.com/{tenantId}/oauth2/v2.0/token";

            // Parámetros de la solicitud
            var parameters = new Dictionary<string, string>
    {
        { "grant_type", "client_credentials" },
        { "client_id", clientId },
        { "client_secret", clientSecret },
        { "scope", scope }
    };

            // Realiza la solicitud HTTP
            var response = await _httpClient.PostAsync(url, new FormUrlEncodedContent(parameters));

            if (response.IsSuccessStatusCode)
            {
                // Lee el contenido de la respuesta como una cadena JSON
                var responseContent = await response.Content.ReadAsStringAsync();

                // Deserializa la respuesta JSON en un objeto
                var tokenResponse = JsonSerializer.Deserialize<TokenResponse>(responseContent);

                // Devuelve el token en la respuesta
                return Ok(new { token = tokenResponse.AccessToken });
            }
            else
            {
                // Si la solicitud falla, devuelve el mensaje de error
                var errorResponse = await response.Content.ReadAsStringAsync();
                return StatusCode((int)response.StatusCode, $"Error al obtener el token: {errorResponse}");
            }
        }

        private class TokenResponse
        {
            [JsonPropertyName("access_token")]
            public string AccessToken { get; set; }

            [JsonPropertyName("token_type")]
            public string TokenType { get; set; }

            [JsonPropertyName("expires_in")]
            public int ExpiresIn { get; set; }

            [JsonPropertyName("scope")]
            public string Scope { get; set; }
        }
    }
}
