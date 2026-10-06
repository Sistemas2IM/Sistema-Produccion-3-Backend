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

        public async Task EnviarNotificacionCasoCalidad(casoCalidad caso, string tipoEvento, string vendedorOf, string lineaDeNegocio)
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


                // 🚀 2. RUTAS DE ENVÍO (Calidad siempre por defecto)
                var urlsDestino = new List<string> {
            "https://chat.googleapis.com/v1/spaces/AAQAZCnXf3s/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=iAkmD-PzeExi3MQnMueFKDDm1xI_XTkD__lbsl8UozE"
        };

                // Normalizamos vendedor y línea de negocio
                string vendedor = vendedorOf?.Trim().ToLower() ?? "";
                string linea = lineaDeNegocio?.Trim().ToLower() ?? "";
                bool esFlexo = linea.Contains("flexo");

                // Evaluamos vendedor para agregar su webhook correspondiente (Offset vs Flexo)
                if (vendedor.Contains("hugo campos"))
                {
                    urlsDestino.Add(esFlexo
                        ? "https://chat.googleapis.com/v1/spaces/AAQA_pwneUY/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=9FX0MwqytenAJAD7OXodHxhKcS7DPc763nVzGZZAl68"
                        : "https://chat.googleapis.com/v1/spaces/AAQAilWl2Jo/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=A_yWs6EE-IfQBU5w_USJjShwA2kAxPRSbESFH2DrYjk");
                }
                else if (vendedor.Contains("javier toledo"))
                {
                    urlsDestino.Add(esFlexo
                        ? "https://chat.googleapis.com/v1/spaces/AAQALh_MjAA/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=MhqS8I35I1hNWaWKYZzOE6XGbP9a2W1RsIBsnSldh_M"
                        : "https://chat.googleapis.com/v1/spaces/AAQA6GMP9tw/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=l8nS8pLNykzWIw4XLfb6J-r9wvt5UKrnwfliR50UpC0");
                }
                else if (vendedor.Contains("juan mónico") || vendedor.Contains("juan monico"))
                {
                    urlsDestino.Add(esFlexo
                        ? "https://chat.googleapis.com/v1/spaces/AAQAKIkMsgE/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=bxAiuETKksuXpwVR0i89DB5Dr10SRCl8SuGqI9OKP0I"
                        : "https://chat.googleapis.com/v1/spaces/AAQAvl06ADc/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=iHTcm8qgEwBL9oFHaRmxCJvmnvyJiW25DbLvKijtJZc");
                }
                else if (vendedor.Contains("marisol osegueda"))
                {
                    if (!esFlexo)
                        urlsDestino.Add("https://chat.googleapis.com/v1/spaces/AAQAnCd5iFM/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=fTcbUJFLc4JDJSVprm95wEfRJp6COGnZE3DSl8RiuK4");
                }
                else if (vendedor.Contains("claudia ruano"))
                {
                    urlsDestino.Add(esFlexo
                        ? "https://chat.googleapis.com/v1/spaces/AAQAl0RbCc4/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=CEOT9v4DyAd7yq2FyAVGyLYPffHaSBKtMZu3POmBOr8"
                        : "https://chat.googleapis.com/v1/spaces/AAQAF1YqrtA/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=HhIwSaU11IIeqpa0HkyL3Ma8yildVUDXt4aiAdA46lc");
                }
                else if (vendedor.Contains("jenny galvez") || vendedor.Contains("jenny gálvez"))
                {
                    urlsDestino.Add(esFlexo
                        ? "https://chat.googleapis.com/v1/spaces/AAQA50GvVkQ/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=LkJttJbKruQYmw6L2OG6kubfveOLf-QYl0kLepegWJA"
                        : "https://chat.googleapis.com/v1/spaces/AAQAGR8CzsU/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=2roIwXvkK6QLLmm46Fzxb5hLHZtvWpvHJrOMY2BlSuQ");
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
                string tipoCasoTexto = caso.idTipoCasoNavigation?.nombre ?? $"Tipo ID {caso.idTipoCaso}";

                string registradoPorNombre = caso.registradoPorNavigation != null
                    ? $"{caso.registradoPorNavigation.nombres} {caso.registradoPorNavigation.apellidos}"
                    : (caso.registradoPor ?? "*No especificado*");

                bool esApertura = tipoEvento.Equals("Apertura", StringComparison.OrdinalIgnoreCase);
                string colorHeader = esApertura ? "#d32f2f" : "#f57c00";
                string tituloAlerta = esApertura ? "🚨 APERTURA DE CASO DE CALIDAD" : "📝 SEGUIMIENTO DE CASO";

                // 🚀 5. ARMADO DE LA TARJETA (Uso de HTML **, *)
                var widgetsDetalles = new List<object>();
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Número de Caso", text = $"**{caso.idCasoCalidad}**" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Estado Actual", text = $"**{nombreVisual}**" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Orden de Fabricación (OF)", text = caso.oF.ToString() } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Vendedor", text = !string.IsNullOrWhiteSpace(vendedorOf) ? vendedorOf : "*No especificado*" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Tipo y Origen", text = $"{tipoCasoTexto} {caso.origen}" } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Fecha y Hora", text = (caso.fechaActualizacion ?? caso.fechaRegistro).ToString("dd/MM/yyyy HH:mm") } });

                if (!string.IsNullOrWhiteSpace(caso.descripcion))
                {
                    string descLimpia = caso.descripcion.Replace("\n", "<br>");
                    widgetsDetalles.Add(new { decoratedText = new { topLabel = "Descripción del Caso", text = descLimpia, wrapText = true } });
                }

                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Registrado Por", text = registradoPorNombre } });
                widgetsDetalles.Add(new { decoratedText = new { topLabel = "Responsable Asignado", text = caso.responsable ?? "*Sin asignar*" } });

                if (!esApertura && caso.fechaCierre.HasValue && !string.IsNullOrWhiteSpace(caso.justificacionCierre))
                {
                    widgetsDetalles.Add(new { decoratedText = new { topLabel = "Justificación de Cierre", text = caso.justificacionCierre, wrapText = true } });
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
                        title = $"**{tituloAlerta}**",
                        subtitle = $"OF: {caso.oF} | {tipoCasoTexto} {caso.origen}"
                    },
                    sections = new dynamic[]
                    {
                        new
                        {
                            header = "**📄 INFORMACIÓN PRINCIPAL**",
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

                    var content = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");
                    await client.PostAsync(finalUrl, content);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al enviar webhooks de calidad para el caso {caso.idCasoCalidad}");
            }
        }
    }
}
