using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services.Correo;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;

namespace Sistema_Produccion_3_Backend.Services.Automatizacion
{
    public class AlertasAprobacionOptions
    {
        public const string Seccion = "AlertasAprobacion";

        public bool Habilitado { get; set; }
        // Primera ejecución: registra las pendientes actuales como enviadas sin mandar correo
        public bool Seed { get; set; } = true;
        public int IntervaloMinutos { get; set; } = 5;
        public int MaxIntentos { get; set; } = 5;
        // Avisar aunque el aprobador sea también el solicitante
        public bool AvisarAutoaprobacion { get; set; } = true;
        // Aviso de resultado (aprobada/rechazada/cancelada) al solicitante
        public bool AvisarSolicitante { get; set; }
        // ObjType -> plantilla de URL del Web Client; {DraftEntry} se sustituye por OWDD.DraftEntry.
        // La plantilla se usa tal cual (p. ej. %252C debe quedar doblemente codificado)
        public Dictionary<string, string> UrlWebClientPorTipo { get; set; } = new();
        // Textos de los correos; vacíos = no se muestran
        public string? InstruccionesAprobacion { get; set; }
        public string? NotaAprobada { get; set; }
    }

    // Avisa por correo de solicitudes de aprobación de SAP B1 (ver Docs/SPEC_alertas_aprobacion_sap.md).
    // Solo lee SAP; la tabla AprobacionAviso deduplica los avisos entre ciclos.
    public class AlertasAprobacionSapService : BackgroundService
    {
        private const string TipoPendiente = "PENDIENTE";
        private const string TipoResultado = "RESULTADO";
        private const string StatusPendiente = "W";
        private const string ErrorSinCorreo = "Usuario SAP sin correo (OUSR.E_Mail)";

        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<AlertasAprobacionSapService> _logger;
        private readonly IOptionsMonitor<AlertasAprobacionOptions> _options;

        public AlertasAprobacionSapService(
            IServiceProvider serviceProvider,
            ILogger<AlertasAprobacionSapService> logger,
            IOptionsMonitor<AlertasAprobacionOptions> options)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
            _options = options;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            await Task.Yield();

            _logger.LogInformation("Servicio de Alertas de Aprobación SAP iniciado.");

            while (!stoppingToken.IsCancellationRequested)
            {
                // Se relee en cada ciclo: Habilitado, Seed e IntervaloMinutos se pueden cambiar sin reiniciar
                var opt = _options.CurrentValue;

                if (opt.Habilitado)
                {
                    try
                    {
                        using var scope = _serviceProvider.CreateScope();
                        await EjecutarCicloAsync(scope.ServiceProvider, opt, stoppingToken);
                    }
                    catch (OperationCanceledException) when (stoppingToken.IsCancellationRequested)
                    {
                        break;
                    }
                    catch (Exception ex)
                    {
                        _logger.LogError(ex, "❌ Error en el ciclo de Alertas de Aprobación SAP.");
                    }
                }

                // El ciclo es secuencial: una ejecución nunca se solapa con la siguiente
                await Task.Delay(TimeSpan.FromMinutes(Math.Max(1, opt.IntervaloMinutos)), stoppingToken);
            }
        }

        private async Task EjecutarCicloAsync(IServiceProvider sp, AlertasAprobacionOptions opt, CancellationToken ct)
        {
            var context = sp.GetRequiredService<base_nuevaContext>();
            var sap = sp.GetRequiredService<AprobacionesSapRepository>();
            var correo = sp.GetRequiredService<ICorreoService>();

            int nuevos = 0, enviados = 0, fallidos = 0, cerradas = 0;

            // 1. Aprobadores de la etapa actual de cada solicitud pendiente
            var pendientes = await sap.ObtenerPendientesAsync(ct);

            // Una solicitud decidida no vuelve a 'W', así que los avisos abiertos cubren todas las pendientes ya registradas
            var avisosAbiertos = await context.AprobacionAviso
                .Where(a => a.TipoAviso == TipoPendiente && a.StatusSap == StatusPendiente)
                .ToListAsync(ct);
            var porLlave = avisosAbiertos.ToDictionary(a => (a.WddCode, a.StepCode, a.UserID));

            foreach (var p in pendientes)
            {
                if (!opt.AvisarAutoaprobacion && p.AprobadorID == p.SolicitanteID) continue;

                var llave = (p.WddCode, p.CurrStep, p.AprobadorID);
                if (!porLlave.TryGetValue(llave, out var aviso))
                {
                    aviso = await RegistrarAvisoAsync(context, p.WddCode, p.CurrStep, p.AprobadorID, TipoPendiente, StatusPendiente, p.AprobadorEmail, opt.Seed, ct);
                    if (aviso == null) continue; // ya lo registró otra instancia

                    porLlave[llave] = aviso;
                    nuevos++;

                    if (aviso.Email == null)
                        _logger.LogWarning($"Aprobación {p.WddCode}: el aprobador {p.Aprobador} ({p.AprobadorID}) no tiene correo en SAP.");
                }

                if (opt.Seed || aviso.Enviado || aviso.Intentos >= opt.MaxIntentos) continue;

                var (asunto, cuerpo) = PlantillasAprobacion.Pendiente(p, opt);
                if (await IntentarEnvioAsync(context, correo, aviso, p.AprobadorEmail, asunto, cuerpo, ct)) enviados++;
                else if (aviso.Email != null) fallidos++;
            }

            // 2. Solicitudes avisadas que ya no están pendientes: actualizar estado y (opcional) avisar al solicitante
            var wddPendientes = pendientes.Select(p => p.WddCode).ToHashSet();
            var avisosPorCerrar = porLlave.Values.Where(a => !wddPendientes.Contains(a.WddCode)).ToList();

            bool avisarSolicitante = opt.AvisarSolicitante && !opt.Seed;
            var resultadosPorReintentar = avisarSolicitante
                ? await context.AprobacionAviso
                    .Where(a => a.TipoAviso == TipoResultado && !a.Enviado && a.Intentos < opt.MaxIntentos)
                    .ToListAsync(ct)
                : new List<AprobacionAviso>();

            var wddConsultar = avisosPorCerrar.Select(a => a.WddCode)
                .Concat(resultadosPorReintentar.Select(a => a.WddCode))
                .Distinct()
                .ToList();

            if (wddConsultar.Count > 0)
            {
                var estados = await sap.ObtenerEstadosAsync(wddConsultar, ct);

                foreach (var e in estados.Where(e => e.Status != StatusPendiente))
                {
                    foreach (var a in avisosPorCerrar.Where(a => a.WddCode == e.WddCode))
                        a.StatusSap = e.Status;
                    cerradas++;

                    if (!avisarSolicitante) continue;

                    var aviso = resultadosPorReintentar.FirstOrDefault(a => a.WddCode == e.WddCode)
                        ?? await RegistrarAvisoAsync(context, e.WddCode, e.CurrStep, e.SolicitanteID, TipoResultado, e.Status, e.SolicitanteEmail, false, ct);
                    if (aviso == null || aviso.Enviado || aviso.Intentos >= opt.MaxIntentos) continue;

                    var (asunto, cuerpo) = PlantillasAprobacion.Resultado(e, opt);
                    if (await IntentarEnvioAsync(context, correo, aviso, e.SolicitanteEmail, asunto, cuerpo, ct)) enviados++;
                    else if (aviso.Email != null) fallidos++;
                }

                await context.SaveChangesAsync(ct);
            }

            if (nuevos > 0 || enviados > 0 || fallidos > 0 || cerradas > 0)
            {
                _logger.LogInformation($"Alertas de Aprobación SAP{(opt.Seed ? " (seed)" : "")}: {pendientes.Count} aprobadores pendientes, {nuevos} avisos nuevos, {enviados} enviados, {fallidos} fallidos, {cerradas} solicitudes cerradas.");
            }
        }

        // Inserta el aviso antes de enviar: la llave única evita duplicados si dos instancias corren a la vez
        private async Task<AprobacionAviso?> RegistrarAvisoAsync(
            base_nuevaContext context, int wddCode, int stepCode, int userId, string tipoAviso,
            string statusSap, string? email, bool seed, CancellationToken ct)
        {
            var aviso = new AprobacionAviso
            {
                WddCode = wddCode,
                StepCode = stepCode,
                UserID = userId,
                TipoAviso = tipoAviso,
                StatusSap = statusSap,
                Email = Truncar(email, 200),
                Enviado = seed,
                Intentos = 0,
                UltimoError = string.IsNullOrWhiteSpace(email) ? ErrorSinCorreo : null,
                FechaCreacion = DateTime.Now
            };

            context.AprobacionAviso.Add(aviso);

            try
            {
                await context.SaveChangesAsync(ct);
                return aviso;
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2627 or 2601 })
            {
                context.Entry(aviso).State = EntityState.Detached;
                return null;
            }
        }

        private async Task<bool> IntentarEnvioAsync(
            base_nuevaContext context, ICorreoService correo, AprobacionAviso aviso,
            string? email, string asunto, string cuerpo, CancellationToken ct)
        {
            // Sin correo no cuenta como intento: si luego se agrega el correo en SAP, el aviso sale en el siguiente ciclo
            if (string.IsNullOrWhiteSpace(email))
            {
                if (aviso.Email != null || aviso.UltimoError != ErrorSinCorreo)
                {
                    aviso.Email = null;
                    aviso.UltimoError = ErrorSinCorreo;
                    await context.SaveChangesAsync(ct);
                }
                return false;
            }

            aviso.Email = Truncar(email, 200);
            aviso.Intentos++;

            try
            {
                await correo.EnviarAsync(email, asunto, cuerpo, ct);
                aviso.Enviado = true;
                aviso.FechaEnvio = DateTime.Now;
                aviso.UltimoError = null;
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                aviso.UltimoError = Truncar(ex.Message, 1000);
                _logger.LogWarning(ex, $"No se pudo enviar el aviso {aviso.TipoAviso} de la aprobación {aviso.WddCode} a {email} (intento {aviso.Intentos}).");
            }

            // Sin token: si el correo ya salió, el registro debe quedar guardado aunque el host se esté deteniendo
            await context.SaveChangesAsync(CancellationToken.None);
            return aviso.Enviado;
        }

        private static string? Truncar(string? texto, int largo) =>
            texto == null ? null : texto.Length <= largo ? texto : texto[..largo];
    }
}
