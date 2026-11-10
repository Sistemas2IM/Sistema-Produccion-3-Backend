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

        public async Task EnviarNotificacionCasoCalidad(casoCalidad caso, string tipoEvento, string? vendedorOf, string? lineaDeNegocio)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // 🚀 1. DEFINICIÓN DE GAIA IDs (Google User IDs para menciones)
                string idJenny = "102786782527552753135";
                string idHugo = "117748976701427013386";
                string idJavier = "114614693010125690335";
                string idJuan = "110342200159826898480";
                string idMarisol = "107391663729700326308";
                string idClaudia = "111502846367859573871";

                // Normalizamos vendedor y línea de negocio
                string vendedor = vendedorOf?.Trim().ToLower() ?? "";
                string linea = lineaDeNegocio?.Trim().ToLower() ?? "";
                bool esFlexo = linea.Contains("flexo");


                // 🚀 2. RUTAS DE ENVÍO (Calidad dinámico: Offset vs Flexo)
                var urlsDestino = new List<string>
                {
                    esFlexo
                        ? "" // Calidad Flexo
                        : "" // Calidad Offset
                };
                ;
                // Evaluamos vendedor para agregar su webhook correspondiente (Offset vs Flexo)
                if (vendedor.Contains("hugo campos"))
                {
                    urlsDestino.Add(esFlexo
                        ? "" // Calidad Flexo
                        : "" // Calidad Offset
                    );
                }
                else if (vendedor.Contains("javier toledo"))
                {
                    urlsDestino.Add(esFlexo
                        ? "" // Calidad Flexo
                        : "" // Calidad Offset
                    );
                }
                else if (vendedor.Contains("juan mónico") || vendedor.Contains("juan monico"))
                {
                    urlsDestino.Add(esFlexo
                        ? "" // Calidad Flexo
                        : "" // Calidad Offset
                    );
                }
                else if (vendedor.Contains("marisol osegueda"))
                {
                    if (!esFlexo)
                        urlsDestino.Add("");
                }
                else if (vendedor.Contains("claudia ruano"))
                {
                    urlsDestino.Add(esFlexo
                        ? "" // Calidad Flexo
                        : "" // Calidad Offset
                    );
                }
                else if (vendedor.Contains("jenny galvez") || vendedor.Contains("jenny gálvez"))
                {
                    urlsDestino.Add(esFlexo
                        ? "" // Calidad Flexo
                        : "" // Calidad Offset
                    );
                }

                // 🚀 3. CONFIGURAR MENCIONES Y TEXTO VISUAL SEGÚN EL ESTADO
                string menciones = "";
                string nombreVisual = "";

                switch (caso.idEstado)
                {
                    case 27: // Pendiente
                        menciones = "";
                        nombreVisual = "🆕 Nuevo caso requiere revisión";
                        break;

                    case 28: // En Análisis
                        menciones = $"";
                        nombreVisual = "🔍 En Análisis";
                        break;

                    case 29: // Pendiente de Información
                        menciones = $"";
                        nombreVisual = "⚠️ Se requiere información adicional de ventas";
                        break;

                    case 30: // Acción Solicitada
                        menciones = "";
                        nombreVisual = "⚡ Se ha solicitado una acción correctiva";
                        break;

                    case 31: // Cerrado
                        menciones = "";
                        nombreVisual = "✅ El caso ha sido cerrado satisfactoriamente";
                        break;

                    case 32: // No Procede
                        menciones = "";
                        nombreVisual = "❌ Se ha determinado que el caso no procede";
                        break;

                    default:
                        menciones = $"";
                        nombreVisual = $"Estado ID {caso.idEstado}";
                        break;
                }

                // 🚀 4. DATOS COMPLEMENTARIOS Y FORMATO
                // Las tarjetas interpretan HTML: todo texto libre se escapa y "No especificado" va en <i>, no en markdown
                const string noEspecificado = "<i>No especificado</i>";

                string tipoCasoTexto = Html(caso.idTipoCasoNavigation?.nombre ?? $"Tipo ID {caso.idTipoCaso}");
                string origenTexto = Html(caso.origen);

                string registradoPorNombre = caso.registradoPorNavigation != null
                    ? Html($"{caso.registradoPorNavigation.nombres} {caso.registradoPorNavigation.apellidos}")
                    : (string.IsNullOrWhiteSpace(caso.registradoPor) ? noEspecificado : Html(caso.registradoPor));

                string responsableNombre = caso.responsableNavigation != null
                    ? Html($"{caso.responsableNavigation.nombres} {caso.responsableNavigation.apellidos}")
                    : (string.IsNullOrWhiteSpace(caso.responsable) ? noEspecificado : Html(caso.responsable));

                bool esApertura = tipoEvento.Equals("Apertura", StringComparison.OrdinalIgnoreCase);
                string colorHeader = esApertura ? "#d32f2f" : "#f57c00";
                string tituloAlerta = esApertura ? "🚨 APERTURA DE CASO DE CALIDAD" : "📝 SEGUIMIENTO DE CASO";

                // 🚀 5. ARMADO DE LA TARJETA (Uso de HTML , *)
                var widgetsDetalles = new List<object>();
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Número de Caso", text = $"{caso.idCasoCalidad}" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Estado Actual", text = $"{nombreVisual}" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Orden de Fabricación (OF)", text = caso.oF.ToString() } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Vendedor", text = !string.IsNullOrWhiteSpace(vendedorOf) ? Html(vendedorOf) : noEspecificado } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Tipo y Origen", text = $"{tipoCasoTexto} {origenTexto}" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Fecha y Hora", text = (caso.fechaActualizacion ?? caso.fechaRegistro).ToString("dd/MM/yyyy HH:mm") } });

                if (!string.IsNullOrWhiteSpace(caso.descripcion))
                {
                    widgetsDetalles.Add(new { decoratedText = new { topLabel = "Descripción del Caso", text = HtmlMultilinea(caso.descripcion), wrapText = true } });
                }

                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Registrado Por", text = registradoPorNombre } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Responsable Asignado", text = responsableNombre } });

                if (!esApertura && caso.fechaCierre.HasValue && !string.IsNullOrWhiteSpace(caso.justificacionCierre))
                {
                    widgetsDetalles.Add(new { decoratedText = new { topLabel = "Justificación de Cierre", text = HtmlMultilinea(caso.justificacionCierre), wrapText = true } });
                }

                // 🚀 6. ENSAMBLAJE DEL PAYLOAD
                string textoRaiz = string.IsNullOrWhiteSpace(menciones)
                    ? $"{tituloAlerta}: Caso {caso.idCasoCalidad} - OF {caso.oF}"
                    : $"{menciones}\n\n{tituloAlerta}: Caso {caso.idCasoCalidad} - OF {caso.oF}";

                var payload = new
                {
                    text = textoRaiz,
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
                        title = $"{tituloAlerta}",
                        subtitle = $"OF: {caso.oF} | {caso.idTipoCasoNavigation?.nombre ?? $"Tipo ID {caso.idTipoCaso}"} {caso.origen}"
                    },
                    sections = new dynamic[]
                    {
                        new
                        {
                            header = "📄 INFORMACIÓN PRINCIPAL",
                            widgets = widgetsDetalles
                        }
                    }
                }
            }
                    }
                };

                var jsonContent = System.Text.Json.JsonSerializer.Serialize(payload);

                // 🚀 7. BUCLE DE ENVÍO A CADA WEBHOOK
                foreach (var webhookUrl in urlsDestino)
                {
                    if (string.IsNullOrWhiteSpace(webhookUrl)) continue;

                    string finalUrl = webhookUrl;
                    if (!finalUrl.Contains("messageReplyOption"))
                    {
                        finalUrl += finalUrl.Contains("?") ? "&" : "?";
                        finalUrl += "messageReplyOption=REPLY_MESSAGE_FALLBACK_TO_NEW_THREAD";
                    }

                    // try por webhook: si falla uno, los demás destinos igual reciben la notificación
                    try
                    {
                        var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                        var response = await client.PostAsync(finalUrl, content);

                        if (!response.IsSuccessStatusCode)
                        {
                            string cuerpo = await response.Content.ReadAsStringAsync();
                            _logger.LogError($"❌ Google Chat rechazó la notificación del caso {caso.idCasoCalidad} (espacio {ObtenerEspacio(webhookUrl)}): {(int)response.StatusCode} {response.ReasonPhrase}. {cuerpo}");
                        }
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, $"❌ Error al enviar webhook de calidad del caso {caso.idCasoCalidad} (espacio {ObtenerEspacio(webhookUrl)})");
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al preparar la notificación de calidad para el caso {caso.idCasoCalidad}");
            }
        }

        private static string Html(string? texto) => System.Net.WebUtility.HtmlEncode(texto?.Trim() ?? "");

        // Se escapa primero y luego se convierten los saltos, para que el <br> no quede escapado
        private static string HtmlMultilinea(string texto) =>
            Html(texto).Replace("\r\n", "<br>").Replace("\n", "<br>");

        // Solo el ID del espacio para el log, sin exponer key ni token
        private static string ObtenerEspacio(string webhookUrl)
        {
            var partes = webhookUrl.Split('/');
            int idx = Array.IndexOf(partes, "spaces");
            return idx >= 0 && idx + 1 < partes.Length ? partes[idx + 1] : "desconocido";
        }
    }
}
