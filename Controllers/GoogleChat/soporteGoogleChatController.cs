using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sistema_Produccion_3_Backend.Controllers.TablerosOf;
using Sistema_Produccion_3_Backend.DTO.GoogleChat.SoporteNexo;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.GoogleChat
{
    [Route("api/[controller]")]
    [ApiController]
    public class soporteGoogleChatController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<soporteGoogleChatController> _logger;
        private readonly IMemoryCache _memoryCache;

        // Un solo constructor con todas las dependencias
        public soporteGoogleChatController(
            base_nuevaContext context,
            IMapper mapper,
            IHttpClientFactory httpClientFactory,
            ILogger<soporteGoogleChatController> logger,
            IMemoryCache memoryCache)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory = httpClientFactory;
            _logger = logger;
            _memoryCache = memoryCache;
        }

        [HttpPost("log-soporte")]
        public async Task<IActionResult> PostLogSoporte(LogSoporteNexoDto logDto)
        {
            // 1. Mapeo y Guardado en DB
            var logSoporte = _mapper.Map<logSoporteNexo>(logDto);
            logSoporte.fechaRegistro = DateTime.Now;

            _context.logSoporteNexo.Add(logSoporte);
            await _context.SaveChangesAsync();

            // 2. Webhook Soporte
            const string urlSoporteGeneral = "https://chat.googleapis.com/v1/spaces/AAQA_XKMSY4/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=RZQ7_IeGm1pddB1z1YHlnNeygReUl3j4jvCh8d-g8nY";

            // 3. Lógica de severidad
            string iconoSeveridad = logDto.severidad switch
            {
                "Crítico" => "🔴 CRÍTICO",
                "Alto" => "🟠 ALTA",
                "Medio" => "🟡 MEDIA",
                "Bajo" => "🔵 BAJA"
            };

            // 1. Definición de IDs (Gaia IDs)
            string idGaby = "113216648114435613456";    // Procesos
            string idNestor = "112640148632860820770";  // Desarrollador (Futuro)
            string idWilber = "107197359071551964527";  // Equipos
            string idRicardo = "108799712826803307406"; // Equipos
            string idNelson = "108446408022178816605";  // Transferencias
            string idCarlos = "105030089368515489216";  // Datos

            // 2. Variables para el payload
            string menciones = "";
            string nombreVisual = "";

            // 3. Switch de asignación según tipoDeError
            switch (logDto.tipoDeError)
            {
                case "Error de proceso":
                    menciones = $"<users/{idGaby}>";
                    nombreVisual = "Gaby (Procesos)";
                    break;

                case "Duda operativa":
                    menciones = $"<users/{idGaby}>";
                    nombreVisual = "Gaby (Procesos)";
                    break;

                case "Problemas con el equipo": // Asumiendo que este mapea a Equipos
                    menciones = $"<users/{idWilber}> <users/{idRicardo}>";
                    nombreVisual = "Wilber / Ricardo (Mantenimiento)";
                    break;

                case "Error de transferencias": // Ajustar según el ID que mandes del front
                    menciones = $"<users/{idNelson}>";
                    nombreVisual = "Nelson (Transferencias)";
                    break;

                case "Error de datos":
                    menciones = $"<users/{idCarlos}>";
                    nombreVisual = "Carlos (Datos)";
                    break;

                case "Incidente operativo":
                    menciones = $"<users/{idCarlos}>";
                    nombreVisual = "Carlos (Datos)";
                    break;

                default:
                    // Si no cae en ninguno, te menciona a ti o a un canal general
                    menciones = $"<users/{idNestor}>";
                    nombreVisual = "Soporte Técnico";
                    break;
            }

            // 4. Construcción del Payload
            var payload = new
            {
                text = $"⚠️ {menciones}, se requiere su apoyo.",
                cardsV2 = new object[] {
                    new {
                        cardId = $"soporte-nexo-{logSoporte.idLogSoporte}",
                        card = new {
                            header = new {
                                title = $"🚨 Soporte NEXO | Incidente {logDto.severidad}",
                                subtitle = $"Reportado por: {logDto.user}",
                                imageUrl = "https://fonts.gstatic.com/s/i/short-term/release/googlesymbols/report_problem/default/48px.svg"
                            },
                            sections = new object[] {
                                new {
                                    widgets = new object[] {
                                        new { decoratedText = new { topLabel = "DESCRIPCIÓN DEL PROBLEMA", text = $"<b>{logDto.descripcion}</b>", wrapText = true } },
                                        new { divider = new { } },
                                        new {
                                            columns = new {
                                                columnItems = new object[] {
                                                    new { widgets = new object[] { new { decoratedText = new { topLabel = "MÓDULO", text = logDto.moduloOriginador } } } },
                                                    new { widgets = new object[] { new { decoratedText = new { topLabel = "REFERENCIA (ID)", text = logDto.idReferencia } } } }
                                                }
                                            }
                                        },
                                        new
                                        {
                                            columns = new {
                                                columnItems = new object[] {
                                                    new { widgets = new object[] { new { decoratedText = new { topLabel = "TIPO DE ERROR", text = logDto.tipoDeError } } } },
                                                    new { widgets = new object[] { new { decoratedText = new { topLabel = "SEVERIDAD", text = iconoSeveridad } } } },
                                                }
                                            }
                                        },
                                        new {
                                            columns = new {
                                                columnItems = new object[] {                                             
                                                    new { widgets = new object[] { new { decoratedText = new { topLabel = "FECHA/HORA", text = logSoporte.fechaRegistro?.ToString("g") } } } }
                                                }
                                            }
                                        },
                                        new {
                                            buttonList = new {
                                                buttons = new object[] {
                                                    new {
                                                        text = "VER TARJETA EN NEXO",
                                                        onClick = new { openLink = new { url = logDto.urlReferencia } }
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

            // 5. Envío
            try
            {
                await SendToGoogleChat(payload, null, urlSoporteGeneral);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error enviando notificación de soporte");
            }

            return Ok(new { id = logSoporte.idLogSoporte, status = "Log registrado y notificado" });
        }

        // --- Método SendToGoogleChat permanece igual, solo asegúrate de que esté dentro de la clase ---
        private async Task<string?> SendToGoogleChat(object messagePayload, string? currentThreadId, string webhookUrl)
        {
            var client = _httpClientFactory.CreateClient();
            string postUrl = webhookUrl;

            if (!string.IsNullOrEmpty(currentThreadId))
            {
                postUrl = $"{webhookUrl}&messageReplyOption=REPLY_MESSAGE_FALLBACK_TO_NEW_THREAD";
            }

            try
            {
                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(messagePayload);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");
                var response = await client.PostAsync(postUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error de Google Chat API: {response.StatusCode} - {errorBody}");
                    return null;
                }

                if (string.IsNullOrEmpty(currentThreadId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    var chatResponse = System.Text.Json.JsonSerializer.Deserialize<GoogleChatResponse>(responseBody);
                    return chatResponse?.Thread?.Name;
                }

                return null;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error al enviar notificación a Google Chat.");
                return null;
            }
        }

        private class GoogleChatResponse { [System.Text.Json.Serialization.JsonPropertyName("thread")] public GoogleChatThread? Thread { get; set; } }
        private class GoogleChatThread { [System.Text.Json.Serialization.JsonPropertyName("name")] public string? Name { get; set; } }
    }
}
