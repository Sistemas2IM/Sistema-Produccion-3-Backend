using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Microsoft.Extensions.Caching.Memory;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.GoogleChat
{
    public class rechazosReclamosWebhook
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<rechazosReclamosWebhook> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public rechazosReclamosWebhook(
            base_nuevaContext context,
            IMapper mapper,
            ILogger<rechazosReclamosWebhook> logger,
            IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public async Task EnviarNotificacionCasoCalidad(casoCalidad caso, string tipoEvento, string webhookUrl)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl)) return;

            try
            {
                var client = _httpClientFactory.CreateClient();

                // 🚀 1. PREPARAMOS LA URL PARA SOPORTAR HILOS
                string finalUrl = webhookUrl;
                if (!finalUrl.Contains("messageReplyOption"))
                {
                    finalUrl += finalUrl.Contains("?") ? "&" : "?";
                    finalUrl += "messageReplyOption=REPLY_MESSAGE_FALLBACK_TO_NEW_THREAD";
                }

                // 2. Configuramos el aspecto visual según el tipo de evento
                bool esApertura = tipoEvento.Equals("Apertura", StringComparison.OrdinalIgnoreCase);
                string colorHeader = esApertura ? "#d32f2f" : "#f57c00";
                string tituloAlerta = esApertura ? "🚨 APERTURA DE CASO DE CALIDAD" : "📝 SEGUIMIENTO DE CASO";

                string tipoCasoTexto = caso.idTipoCaso == 1 ? "Rechazo" : (caso.idTipoCaso == 2 ? "Reclamo" : $"Tipo ID {caso.idTipoCaso}");

                // 3. Armamos los campos del caso
                var widgetsDetalles = new List<dynamic>();

                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Número de Caso", text = $"<b>{caso.idCasoCalidad}</b>" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Orden de Fabricación (OF)", text = caso.oF.ToString() } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Tipo y Origen", text = $"{tipoCasoTexto} {caso.origen}" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Fecha y Hora", text = (caso.fechaActualizacion ?? caso.fechaRegistro).ToString("dd/MM/yyyy HH:mm") } });

                if (!string.IsNullOrWhiteSpace(caso.descripcion))
                {
                    string descLimpia = caso.descripcion.Replace("\n", "<br>");
                    widgetsDetalles.Add(new { decoratedText = new { topLabel = "Descripción del Caso", text = descLimpia, wrapText = true } });
                }

                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Registrado Por", text = caso.registradoPor ?? "<i>No especificado</i>" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Responsable Asignado", text = caso.responsable ?? "<i>Sin asignar</i>" } });

                if (!esApertura && caso.fechaCierre.HasValue && !string.IsNullOrWhiteSpace(caso.justificacionCierre))
                {
                    widgetsDetalles.Add(new { decoratedText = new { topLabel = "Justificación de Cierre", text = caso.justificacionCierre, wrapText = true } });
                }

                // 4. Ensamblamos la tarjeta JSON
                var payload = new
                {
                    text = $"{tituloAlerta}: Caso {caso.idCasoCalidad} - OF {caso.oF}",
                    // 2. ASIGNAMOS LA CLAVE DEL HILO PARA AGRUPAR
                    thread = new { threadKey = $"caso-calidad-{caso.idCasoCalidad}" },
                    cardsV2 = new dynamic[]
                    {
                new
                {
                    cardId = $"caso-calidad-{caso.idCasoCalidad}-{Guid.NewGuid()}",
                    card = new
                    {
                        header = new
                        {
                            title = $"<font color=\"{colorHeader}\"><b>{tituloAlerta}</b></font>",
                            subtitle = $"OF: {caso.oF} | {tipoCasoTexto} {caso.origen}"
                        },
                        sections = new dynamic[]
                        {
                            new
                            {
                                header = "<b>📄 INFORMACIÓN PRINCIPAL</b>",
                                widgets = widgetsDetalles
                            }
                        }
                    }
                }
                    }
                };

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync(finalUrl, content); // 🚀 Usamos la finalUrl modificada
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al enviar webhook de calidad para el caso {caso.idCasoCalidad}");
            }
        }
    }
}
