using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;
using System.Runtime.InteropServices;
using Microsoft.EntityFrameworkCore;

namespace Sistema_Produccion_3_Backend.Services.Automatizacion
{
    public class ReconciliacionOfService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<ReconciliacionOfService> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public ReconciliacionOfService(IServiceProvider serviceProvider, ILogger<ReconciliacionOfService> logger, IHttpClientFactory httpClientFactory)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _httpClientFactory = httpClientFactory;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            _logger.LogWarning("StartAsync SI fue llamado por .NET Host");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            _logger.LogInformation("Servicio de Reconciliación OF iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                SAPbobsCOM.Recordset? oRecordSet = null;

                try
                {
                    using (var scope = _serviceProvider.CreateScope())
                    {
                        var context = scope.ServiceProvider.GetRequiredService<base_nuevaContext>();

                        // 1. Obtener Tarjetas OF locales activas (No archivadas y no cerradas)
                        var ofsLocales = await context.tarjetaOf
                            .AsNoTracking()                      
                            .ToListAsync(stoppingToken);

                        if (ofsLocales.Any())
                        {
                            // 2. Conectar a SAP
                            HANAConnection.sapConn();

                            if (HANAConnection.RetVal != 0)
                            {
                                string errMsg = HANAConnection.OCompany.GetLastErrorDescription();
                                _logger.LogError($"Error al conectar a SAP: {errMsg}");
                            }
                            else
                            {
                                // 3. Crear el Recordset y llamar a TU PROCEDIMIENTO ALMACENADO
                                oRecordSet = (SAPbobsCOM.Recordset)HANAConnection.OCompany.GetBusinessObject(SAPbobsCOM.BoObjectTypes.BoRecordset);

                                // Llamamos al SP tal cual lo hace tu Add-on
                                string queryHana = "{call _IM_PROC_NEXO_RECONCILIACION_OF}";
                                oRecordSet.DoQuery(queryHana);

                                var discrepancias = new List<logSincronizacionOf>();

                                // Convertimos la lista de OFs locales en un Diccionario para búsquedas ultrarrápidas
                                var dictLocales = ofsLocales.ToDictionary(o => o.oF);

                                // 4. Recorrer el Recordset del SP y comparar
                                while (!oRecordSet.EoF)
                                {
                                    int docNumSap = (int)oRecordSet.Fields.Item("DocNum").Value;

                                    // ¿Tenemos esta OF de SAP en nuestra lista de tarjetas activas locales?
                                    if (dictLocales.TryGetValue(docNumSap, out var local))
                                    {
                                        // --- EXTRAER USANDO TUS ALIAS DEL SP ---
                                        string ovNumSapStr = oRecordSet.Fields.Item("OVNum").Value?.ToString()?.Trim() ?? "";
                                        string nombreOfSap = oRecordSet.Fields.Item("CardName").Value?.ToString()?.Trim() ?? "";
                                        string codArtSap = oRecordSet.Fields.Item("CodArt").Value?.ToString()?.Trim() ?? "";
                                        string productoOfSap = oRecordSet.Fields.Item("ProdName").Value?.ToString()?.Trim() ?? "";
                                        string lNegocioSap = oRecordSet.Fields.Item("Lnegocio").Value?.ToString()?.Trim() ?? "";
                                        string clienteOfSap = oRecordSet.Fields.Item("Cliente").Value?.ToString()?.Trim() ?? "";
                                        string descripcionOfSap = oRecordSet.Fields.Item("ProdDescripcion").Value?.ToString()?.Trim() ?? "";
                                        string vendedorOfSap = oRecordSet.Fields.Item("Vendedor").Value?.ToString()?.Trim() ?? "";
                                        string cantidadOfSapStr = oRecordSet.Fields.Item("ProdCantidad").Value?.ToString()?.Trim() ?? "";
                                        DateTime? fechaEntregaSap = oRecordSet.Fields.Item("Fecha_entrega").Value as DateTime?;
                                        string tipoOrdenSap = oRecordSet.Fields.Item("U_TipoOrden").Value?.ToString()?.Trim() ?? "";
                                        string unidadSap = oRecordSet.Fields.Item("Unidad").Value?.ToString()?.Trim() ?? "";
                                        string serieSapRaw = oRecordSet.Fields.Item("SERIE").Value?.ToString()?.Trim() ?? "";
                                        string razonSocialSap = oRecordSet.Fields.Item("Razon_Social").Value?.ToString()?.Trim() ?? "";

                                        // --- APLICAR TUS REGLAS ---
                                        string serieFixSap = serieSapRaw;
                                        if (serieSapRaw == "OF_2026") serieFixSap = "OF";
                                        if (serieSapRaw == "OF_IN26") serieFixSap = "IN";

                                        int? ovSap = int.TryParse(ovNumSapStr, out int ovParsed) ? ovParsed : null;
                                        decimal? cantidadSap = decimal.TryParse(cantidadOfSapStr, out decimal cantParsed) ? cantParsed : null;

                                        // --- LÓGICA DE COMPARACIÓN ---
                                        List<string> diferencias = new List<string>();

                                        if (local.oV != ovSap) diferencias.Add($"OV|{local.oV}|{ovSap}");
                                        if ((local.nombreOf ?? "") != nombreOfSap) diferencias.Add($"Nombre|{local.nombreOf}|{nombreOfSap}");
                                        if ((local.codArticulo ?? "") != codArtSap) diferencias.Add($"Cód. Artículo: NEXO({local.codArticulo}) vs SAP({codArtSap})");
                                        if ((local.productoOf ?? "") != productoOfSap) diferencias.Add($"Producto: NEXO({local.productoOf}) vs SAP({productoOfSap})");
                                        if ((local.lineaDeNegocio ?? "") != lNegocioSap) diferencias.Add($"Línea Negocio: NEXO({local.lineaDeNegocio}) vs SAP({lNegocioSap})");
                                        if ((local.clienteOf ?? "") != clienteOfSap) diferencias.Add($"Cliente: NEXO({local.clienteOf}) vs SAP({clienteOfSap})");
                                        if ((local.descipcionOf ?? "") != descripcionOfSap) diferencias.Add($"Descripción: NEXO({local.descipcionOf}) vs SAP({descripcionOfSap})");
                                        if ((local.vendedorOf ?? "") != vendedorOfSap) diferencias.Add($"Vendedor: NEXO({local.vendedorOf}) vs SAP({vendedorOfSap})");
                                        if (local.cantidadOf != cantidadSap) diferencias.Add($"Cantidad: NEXO({local.cantidadOf}) vs SAP({cantidadSap})");
                                        if ((local.tipoDeOrden ?? "") != tipoOrdenSap) diferencias.Add($"Tipo Orden: NEXO({local.tipoDeOrden}) vs SAP({tipoOrdenSap})");
                                        if ((local.unidadMedida ?? "") != unidadSap) diferencias.Add($"Unidad: NEXO({local.unidadMedida}) vs SAP({unidadSap})");
                                        if ((local.seriesOf ?? "") != serieFixSap) diferencias.Add($"Serie: NEXO({local.seriesOf}) vs SAP({serieFixSap})");
                                        if ((local.razonSocial ?? "") != razonSocialSap) diferencias.Add($"Razón Social: NEXO({local.razonSocial}) vs SAP({razonSocialSap})");
                                        if (local.fechaVencimiento?.Date != fechaEntregaSap?.Date) diferencias.Add($"Fecha Entrega: NEXO({local.fechaVencimiento?.ToString("yyyy-MM-dd")}) vs SAP({fechaEntregaSap?.ToString("yyyy-MM-dd")})");

                                        string descNexo = local.descipcionOf ?? "";
                                        double similitudDesc = CalcularSimilitud(descNexo, descripcionOfSap);

                                        // Tolerancia: Solo es diferencia si la similitud baja del 95%)
                                        if (similitudDesc < 95.0)
                                        {
                                            diferencias.Add($"Descripción ({similitudDesc:F1}% similar)|{descNexo}|{descripcionOfSap}");
                                        }

                                        // --- REGISTRAR EL ERROR ---
                                        if (diferencias.Any())
                                        {
                                            discrepancias.Add(new logSincronizacionOf
                                            {
                                                oF = local.oF,
                                                fechaDeteccion = DateTime.Now,
                                                fechaUltimaRevision = DateTime.Now,
                                                detalleDiferencia = string.Join("\n", diferencias),
                                                estado = "Descuadrado"
                                            });
                                        }
                                    }

                                    oRecordSet.MoveNext(); // Siguiente fila de SAP
                                }

                                // 5. GUARDAR DISCREPANCIAS (Upsert y Resolución Automática)
                                if (discrepancias.Any() || context.logSincronizacionOf.Any(l => l.estado == "Descuadrado"))
                                {
                                    // Traemos los errores que actualmente están "activos" en la base de datos local
                                    var logsActivos = await context.logSincronizacionOf
                                        .Where(l => l.estado == "Descuadrado")
                                        .ToListAsync(stoppingToken);

                                    // a) Procesar las diferencias encontradas HOY
                                    foreach (var disc in discrepancias)
                                    {
                                        var existente = logsActivos.FirstOrDefault(l => l.oF == disc.oF);

                                        if (existente != null)
                                        {
                                            // El error ya existía. Actualizamos el texto y la fecha
                                            existente.detalleDiferencia = disc.detalleDiferencia;
                                            existente.fechaUltimaRevision = DateTime.Now;
                                        }
                                        else
                                        {
                                            // Es un error nuevo. Lo preparamos para insertar.
                                            disc.fechaDeteccion = DateTime.Now;
                                            disc.fechaUltimaRevision = DateTime.Now;
                                            context.logSincronizacionOf.Add(disc);

                                            string urlWebhook = "";
                                            await EnviarAlertaDiscrepancia(disc, urlWebhook);
                                        }
                                    }

                                    // b) Resolver los que ya se arreglaron
                                    var idsConErrorHoy = discrepancias.Select(d => d.oF).ToList();
                                    var logsArreglados = logsActivos.Where(l => !idsConErrorHoy.Contains(l.oF));

                                    foreach (var arreglado in logsArreglados)
                                    {
                                        arreglado.estado = "Resuelto";
                                        arreglado.fechaUltimaRevision = DateTime.Now;
                                    }

                                    // Guardar cambios en la DB
                                    await context.SaveChangesAsync(stoppingToken);

                                    _logger.LogWarning($"Reconciliación OF: {discrepancias.Count} activas. {logsArreglados.Count()} resueltas.");
                                }
                                else
                                {
                                    _logger.LogInformation("Todo en orden. No se encontraron diferencias entre NEXO y SAP para las OFs.");
                                }
                            }
                        }
                    }
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "❌ Error fatal en el servicio de Reconciliación SAP.");
                }
                finally
                {
                    // 🚀 Liberación de memoria crítica para SAP DI API
                    if (oRecordSet != null)
                    {
                        Marshal.ReleaseComObject(oRecordSet);
                        oRecordSet = null;
                    }

                    if (HANAConnection.OCompany != null && HANAConnection.OCompany.Connected)
                    {
                        HANAConnection.OCompany.Disconnect();
                    }

                    // Forzar al Garbage Collector a limpiar restos COM
                    GC.Collect();
                    GC.WaitForPendingFinalizers();
                }

                // Ejecutar cada 12 horas (Ajusta este tiempo a lo que tu operación necesite)
                await Task.Delay(TimeSpan.FromMinutes(1), stoppingToken);
            }
        }

        private async Task EnviarAlertaDiscrepancia(logSincronizacionOf disc, string webhookUrl)
        {
            if (string.IsNullOrWhiteSpace(webhookUrl)) return;

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Parsear las diferencias para construir los widgets dinámicamente
                var widgetsDiferencias = new List<object>();
                var lineas = disc.detalleDiferencia.Split('\n', StringSplitOptions.RemoveEmptyEntries);

                foreach (var linea in lineas)
                {
                    var partes = linea.Split('|');
                    if (partes.Length == 3)
                    {
                        widgetsDiferencias.Add(new
                        {
                            columns = new
                            {
                                columnItems = new object[]
                                {
                            new { horizontalSizeStyle = "FILL_AVAILABLE_SPACE", widgets = new object[] { new { decoratedText = new { topLabel = $"NEXO: {partes[0]}", text = partes[1], wrapText = true } } } },
                            new { horizontalSizeStyle = "FILL_AVAILABLE_SPACE", widgets = new object[] { new { decoratedText = new { topLabel = $"SAP: {partes[0]}", text = $"<font color=\"#FF0000\">{partes[2]}</font>", wrapText = true } } } }
                                }
                            }
                        });
                    }
                }

                var payload = new
                {
                    text = $"🚨 ALERTA DE RECONCILIACIÓN: OF {disc.oF}",
                    cardsV2 = new object[] {
                new {
                    cardId = $"alerta-reconciliacion-{disc.oF}",
                    card = new {
                        header = new {
                            title = $"⚠️ Descuadre Detectado",
                            subtitle = $"Orden de Fabricación: {disc.oF}",
                            imageUrl = "https://i.imgur.com/Sm19RjX.png"
                        },
                        sections = new object[] {
                            new {
                                header = "<b>DIFERENCIAS ENCONTRADAS</b>",
                                widgets = widgetsDiferencias
                            }
                        }
                    }
                }
            }
                };

                var content = new StringContent(System.Text.Json.JsonSerializer.Serialize(payload), System.Text.Encoding.UTF8, "application/json");
                await client.PostAsync(webhookUrl, content);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"❌ Error al enviar webhook para la OF {disc.oF}");
            }
        }

        private double CalcularSimilitud(string source, string target)
        {
            if (string.IsNullOrEmpty(source) && string.IsNullOrEmpty(target)) return 100.0;
            if (string.IsNullOrEmpty(source) || string.IsNullOrEmpty(target)) return 0.0;

            int n = source.Length, m = target.Length;
            int[,] d = new int[n + 1, m + 1];

            for (int i = 0; i <= n; d[i, 0] = i++) { }
            for (int j = 0; j <= m; d[0, j] = j++) { }

            for (int i = 1; i <= n; i++)
            {
                for (int j = 1; j <= m; j++)
                {
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;
                    d[i, j] = Math.Min(Math.Min(d[i - 1, j] + 1, d[i, j - 1] + 1), d[i - 1, j - 1] + cost);
                }
            }

            int maxLen = Math.Max(n, m);
            return (1.0 - ((double)d[n, m] / maxLen)) * 100.0;
        }
    }
}
