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

        public ReconciliacionOfService(IServiceProvider serviceProvider, ILogger<ReconciliacionOfService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        public override Task StartAsync(CancellationToken cancellationToken)
        {
            // Pon un punto de interrupción en esta línea 👇
            _logger.LogWarning("🔥 StartAsync SI fue llamado por .NET Host!");
            return base.StartAsync(cancellationToken);
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            _logger.LogInformation("🔄 Servicio de Reconciliación OF iniciado.");

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

                                        if (local.oV != ovSap) diferencias.Add($"OV: NEXO({local.oV}) vs SAP({ovSap})");
                                        if ((local.nombreOf ?? "") != nombreOfSap) diferencias.Add($"Nombre: NEXO({local.nombreOf}) vs SAP({nombreOfSap})");
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

                                        // --- REGISTRAR EL ERROR ---
                                        if (diferencias.Any())
                                        {
                                            discrepancias.Add(new logSincronizacionOf
                                            {
                                                oF = local.oF,
                                                fechaDeteccion = DateTime.Now,
                                                fechaUltimaRevision = DateTime.Now,
                                                detalleDiferencia = string.Join(" | ", diferencias),
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

                                    _logger.LogWarning($"⚠️ Reconciliación OF: {discrepancias.Count} activas. {logsArreglados.Count()} resueltas.");
                                }
                                else
                                {
                                    _logger.LogInformation("✅ Todo en orden. No se encontraron diferencias entre NEXO y SAP para las OFs.");
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
                await Task.Delay(TimeSpan.FromMinutes(5), stoppingToken);
            }
        }
    }
}
