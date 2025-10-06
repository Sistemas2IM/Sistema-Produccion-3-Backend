/*using Google.Apis.Auth;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json.Linq;
using Sistema_Produccion_3_Backend.Controllers.GoogleChatBot.ChatModels;
using System.Text.Json;


namespace Sistema_Produccion_3_Backend.Controllers.GoogleChatBot
{
    [ApiController]
    [Route("api/[controller]")]
    public class ChatBotController : ControllerBase
    {
        [HttpPost]
        public async Task<IActionResult> Post()
        {
            // Leemos el cuerpo crudo para tener el JSON completo.
            string rawJsonBody;
            using (var reader = new StreamReader(Request.Body))
            {
                rawJsonBody = await reader.ReadToEndAsync();
            }

            Console.WriteLine("--- JSON CRUDO RECIBIDO ---");
            Console.WriteLine(rawJsonBody);
            Console.WriteLine("--- FIN DEL JSON CRUDO ---");

            var body = JsonDocument.Parse(rawJsonBody).RootElement;

            // --- 1. AUTENTICACIÓN PARA ADD-ONS (Esto ya funciona) ---
            if (!body.TryGetProperty("authorizationEventObject", out var authObject) ||
                !authObject.TryGetProperty("systemIdToken", out var tokenElement))
            {
                return Unauthorized("Falta el token de autorización.");
            }

            try
            {
                var token = tokenElement.GetString();
                var botUrl = "https://pennie-octaval-francine.ngrok-free.dev/api/ChatBot";

                var payload = await GoogleJsonWebSignature.ValidateAsync(token, new GoogleJsonWebSignature.ValidationSettings
                {
                    Audience = new[] { botUrl }
                });
            }
            catch (InvalidJwtException e)
            {
                Console.WriteLine(e);
                return Unauthorized("Token inválido.");
            }

            // --- 2. LÓGICA DEL BOT CON LA RUTA CORREGIDA (Esto ya funciona) ---
            string replyText;
            if (body.TryGetProperty("chat", out var chatObject) &&
                chatObject.TryGetProperty("appCommandPayload", out var appCommandPayloadObject) &&
                appCommandPayloadObject.TryGetProperty("message", out var messageObject))
            {
                if (messageObject.TryGetProperty("slashCommand", out _))
                {
                    string commandArgs = messageObject.TryGetProperty("argumentText", out var argTextEl)
                                         ? argTextEl.GetString()?.Trim()
                                         : "";

                    replyText = string.IsNullOrEmpty(commandArgs)
                                ? "Comando '/hola' recibido."
                                : $"Recibí tu consulta: '{commandArgs}'";
                }
                else
                {
                    string userText = messageObject.TryGetProperty("argumentText", out var argTextEl)
                                      ? argTextEl.GetString()?.Trim()
                                      : "";
                    replyText = $"Mensaje recibido: '{userText}'";
                }
            }
            else
            {
                replyText = "¡Hola! La conexión con el Add-on de NEXO es correcta.";
            }

            // --- 3. RESPUESTA DEFINITIVA BASADA EN LA DOCUMENTACIÓN ---
            // Esta es la estructura que encontraste, traducida a C#.
            // No usamos "actionResponse", solo el objeto "cardsV2" en la raíz.
            var response = new
            {
                cardsV2 = new[]
                {
            new
            {
                cardId = "nexoResponseCard",
                card = new
                {
                    header = new
                    {
                       title = "NEXO",
                       subtitle = "Asistente del Sistema",
                       imageUrl = "https://developers.google.com/workspace/chat/images/quickstart-app-avatar.png",
                       imageType = "CIRCLE",
                       imageAltText = "Avatar de NEXO Bot"
                    },
                    sections = new[]
                    {
                       new
                       {
                         header = "Resultado de la Consulta",
                         collapsible = false,
                         widgets = new object[] // <-- ¡Este es el cambio!
                                            {
                                              // Este es el widget que mostrará tu texto de respuesta
                                              new
                                              {
                                                textParagraph = new
                                                {
                                                  text = replyText
                                                }
                                              },
                                              // Puedes añadir más widgets aquí si quieres, como un botón
                                              new
                                              {
                                                   buttonList = new
                                                   {
                                                       buttons = new[]
                                                       {
                                                           new
                                                           {
                                                               text = "Visitar Sitio Web",
                                                               onClick = new
                                                               {
                                                                   openLink = new { url = "https://im.com.sv/" }
                                                               }
                                                           }
                                                       }
                                                   }
                                              }
                         }
                       }
                    }
                }
            }
                }
            };

            return Ok(response);
        }
    }
}*/
