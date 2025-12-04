using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Sistema_Produccion_3_Backend.Models;
using System;
using System.Linq;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace Sistema_Produccion_3_Backend.Services.Automatizacion
{
    // Agrega tus usings de Data y Models aquí

    public class VigilanteVencimientosService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<VigilanteVencimientosService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        // URLs de los Webhooks
        const string UrlClaudia = "https://chat.googleapis.com/v1/spaces/sYGT9CAAAAE/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=J66jBKxAB0SxqKV7igGtV5g3qLAhQhuPZBzXuY-tK1M";
        const string UrlJavier = "https://chat.googleapis.com/v1/spaces/9dQbXUAAAAE/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=NErWICVNwIEyHaWSbK8ffpFu0-5f03sTV87Yus9jdoA";

        public VigilanteVencimientosService(
            IServiceProvider serviceProvider,
            ILogger<VigilanteVencimientosService> logger,
            IHttpClientFactory httpClientFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("👁️ Vigilante de Vencimientos iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                try
                {
                    // 1. Crear un scope para poder usar la Base de Datos (DbContext es Scoped)
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        // Reemplaza 'TuDbContext' con el nombre real de tu contexto
                        var context = scope.ServiceProvider.GetRequiredService<base_nuevaContext>();

                        // 2. Definir el rango de fecha (8 días desde hoy)
                        var hoy = DateTime.Now;
                        var fechaLimite = hoy.AddDays(8).Date;

                        // 3. Consultar tarjetas que vencen en 8 días o menos (y que no estén cerradas/archivadas)
                        // ⚠️ Ajusta la condición '!t.archivada' o el estado según tu lógica de "tarjeta activa"
                        var tarjetasPorVencer = await context.tarjetaOf
                            .Where(t => t.fechaVencimiento != null
                                        && t.fechaVencimiento.Value.Date >= hoy.Date // Que no haya vencido hace mucho (opcional)
                                        && t.fechaVencimiento.Value.Date <= fechaLimite // Que venza en los próximos 8 días
                                        && t.archivada != true // Importante: no notificar tarjetas viejas
                                        && t.cerrada != true)  // Importante: no notificar si ya se cerró
                            .Include(t => t.idEstadoOfNavigation) // Para mostrar el estado actual
                            .ToListAsync(stoppingToken);

                        _logger.LogInformation($"Se encontraron {tarjetasPorVencer.Count} tarjetas por vencer.");

                        // 4. Iterar y notificar
                        foreach (var tarjeta in tarjetasPorVencer)
                        {
                            string urlWebhook = null;

                            // Validar vendedor
                            if (tarjeta.vendedorOf?.Equals("Claudia Ruano", StringComparison.OrdinalIgnoreCase) == true)
                                urlWebhook = UrlClaudia;
                            else if (tarjeta.vendedorOf?.Equals("Javier Toledo", StringComparison.OrdinalIgnoreCase) == true)
                                urlWebhook = UrlJavier;

                            // Si hay webhook asignado, enviamos la alerta
                            if (!string.IsNullOrEmpty(urlWebhook))
                            {
                                await EnviarAlertaVencimiento(tarjeta, urlWebhook);
                                // Pequeña pausa para no saturar a Google si son muchas
                                await Task.Delay(500);
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error en el Vigilante de Vencimientos.");
                }

                // 5. Esperar 24 horas antes de volver a revisar
                //    (O calcula el tiempo para que corra siempre a las 8:00 AM)
                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }
        }

        private async Task EnviarAlertaVencimiento(tarjetaOf tarjeta, string webhookUrl)
        {
            try
            {
                var client = _httpClientFactory.CreateClient();

                // Calcular días exactos
                int diasRestantes = (tarjeta.fechaVencimiento.Value.Date - DateTime.Now.Date).Days;
                string textoAlerta = diasRestantes == 0 ? "¡HOY!" : $"{diasRestantes} días";
                string colorAlerta = diasRestantes <= 2 ? "#FF0000" : "#FFA500"; // Rojo si es urgente, Naranja si no

                var payload = new
                {
                    text = $"⚠️ ALERTA DE VENCIMIENTO: OF {tarjeta.oF}",
                    cardsV2 = new object[] {
                    new {
                        cardId = $"alerta-vencimiento-{tarjeta.oF}",
                        card = new {
                            header = new {
                                title = $"⚠️ Vence en {textoAlerta}",
                                subtitle = $"OF {tarjeta.oF} - OV {tarjeta.oV}",
                                imageUrl = "https://i.imgur.com/Sm19RjX.png" // Ícono de alerta
                            },
                            sections = new object[] {
                                new {
                                    widgets = new object[] {
                                        new {
                                            decoratedText = new {
                                                topLabel = "CLIENTE",
                                                text = tarjeta.clienteOf,
                                                wrapText = true
                                            }
                                        },
                                        new {
                                            decoratedText = new {
                                                topLabel = "PRODUCTO",
                                                text = $"{tarjeta.codArticulo} {tarjeta.productoOf}",
                                                wrapText = true
                                            }
                                        },
                                        new {
                                            columns = new {
                                                columnItems = new object[] {
                                                    new { widgets = new object[] { new { decoratedText = new { topLabel = "ESTADO ACTUAL", text = tarjeta.idEstadoOfNavigation?.nombreEstado ?? "N/A" } } } },
                                                    new { widgets = new object[] { new { decoratedText = new { topLabel = "FECHA LÍMITE", text = tarjeta.fechaVencimiento.Value.ToString("dd-MMM") } } } }
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

                var json = System.Text.Json.JsonSerializer.Serialize(payload);
                var content = new StringContent(json, System.Text.Encoding.UTF8, "application/json");

                // Para agrupar por OV, podrías añadir &threadKey=... si usas la API completa, 
                // pero con webhooks simples, Google a veces agrupa solo.
                await client.PostAsync(webhookUrl, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error enviando alerta para OF {tarjeta.oF}");
            }
        }
    }
}

