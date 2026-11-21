using Microsoft.AspNetCore.StaticFiles;
using Sistema_Produccion_3_Backend.Services.Correo;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;

namespace Sistema_Produccion_3_Backend.Services.Automatizacion
{
    public enum EstadoAdjunto
    {
        Adjuntado,
        NoEncontrado,
        SinAcceso,
        ExcedeTamano,
        NoPermitido
    }

    // Archivo del borrador con su estado; Archivo solo existe si se adjuntó
    // Ruta = ruta final que se intentó leer (después del reemplazo); null si la ruta de SAP no es válida
    public record AdjuntoPreparado(string Nombre, EstadoAdjunto Estado, string? Ruta, ArchivoCorreo? Archivo = null);

    // Adjuntos de un borrador listos para el correo. ConsultaFallida = no se pudo leer ATC1
    public class AdjuntosCorreo
    {
        public bool ConsultaFallida { get; init; }
        public List<AdjuntoPreparado> Archivos { get; init; } = new();

        public List<ArchivoCorreo> ParaEnviar => Archivos.Where(a => a.Archivo != null).Select(a => a.Archivo!).ToList();
    }

    // Lee del NAS los archivos anexos de un borrador (ver Docs/SPEC_adjuntos_alertas_aprobacion.md).
    // Nunca lanza por un problema con un archivo: el motivo queda en el estado y en el log
    public class AdjuntosAprobacion
    {
        private const string MimePorDefecto = "application/octet-stream";
        private static readonly FileExtensionContentTypeProvider TiposMime = new();

        private readonly ILogger<AdjuntosAprobacion> _logger;

        public AdjuntosAprobacion(ILogger<AdjuntosAprobacion> logger)
        {
            _logger = logger;
        }

        // Los archivos se adjuntan en orden de Line mientras quepan en MaxAdjuntosMB
        public async Task<AdjuntosCorreo> PrepararAsync(SolicitudPendienteSap s, AlertasAprobacionOptions opt, CancellationToken ct)
        {
            if (s.Adjuntos == null) return new AdjuntosCorreo { ConsultaFallida = true };

            var resultado = new AdjuntosCorreo();
            long cupo = Math.Max(0, opt.MaxAdjuntosMB) * 1024L * 1024L;

            foreach (var a in s.Adjuntos.OrderBy(a => a.Line))
            {
                string nombre = a.NombreArchivo;
                string? rutaSap = RutaCompleta(a.Carpeta, nombre);
                // La validación de seguridad se hace sobre la ruta final, ya reemplazada y normalizada
                string? ruta = rutaSap == null ? null : Normalizar(Reemplazar(rutaSap, opt.ReemplazosRutaAdjuntos));
                _logger.LogDebug($"Aprobación {s.WddCode} (borrador {s.Folio}): adjunto {rutaSap ?? $"{a.Carpeta} | {nombre}"} -> {ruta ?? "(ruta inválida)"}");

                if (ruta == null || !EstaPermitida(ruta, opt.RutasAdjuntosPermitidas))
                {
                    _logger.LogWarning($"Aprobación {s.WddCode} (borrador {s.Folio}): el adjunto {ruta ?? rutaSap ?? $"{a.Carpeta} | {nombre}"} está fuera de AlertasAprobacion:RutasAdjuntosPermitidas; no se lee.");
                    resultado.Archivos.Add(new AdjuntoPreparado(nombre, EstadoAdjunto.NoPermitido, ruta));
                    continue;
                }

                try
                {
                    // Sin File.Exists: devuelve false también cuando falta el permiso, y se confundiría con "no encontrado"
                    await using var stream = new FileStream(ruta, FileMode.Open, FileAccess.Read, FileShare.Read, 81920, useAsync: true);

                    if (stream.Length > cupo)
                    {
                        _logger.LogInformation($"Aprobación {s.WddCode} (borrador {s.Folio}): {ruta} ({stream.Length / 1024} KB) excede el cupo de adjuntos restante.");
                        resultado.Archivos.Add(new AdjuntoPreparado(nombre, EstadoAdjunto.ExcedeTamano, ruta));
                        continue;
                    }

                    var contenido = new byte[stream.Length];
                    await stream.ReadExactlyAsync(contenido, ct);
                    cupo -= contenido.Length;

                    resultado.Archivos.Add(new AdjuntoPreparado(nombre, EstadoAdjunto.Adjuntado, ruta, new ArchivoCorreo(nombre, TipoMime(nombre), contenido)));
                }
                catch (Exception ex) when (ex is FileNotFoundException or DirectoryNotFoundException)
                {
                    _logger.LogWarning($"Aprobación {s.WddCode} (borrador {s.Folio}): no se encontró el adjunto {ruta}.");
                    resultado.Archivos.Add(new AdjuntoPreparado(nombre, EstadoAdjunto.NoEncontrado, ruta));
                }
                catch (Exception ex) when (ex is not OperationCanceledException)
                {
                    // Permiso denegado, credenciales o recurso de red inaccesible. En el servidor suele ser
                    // la identidad del grupo de aplicaciones de IIS sin permiso de lectura en el NAS
                    _logger.LogWarning(ex, $"Aprobación {s.WddCode} (borrador {s.Folio}): sin acceso a la ruta {ruta}.");
                    resultado.Archivos.Add(new AdjuntoPreparado(nombre, EstadoAdjunto.SinAcceso, ruta));
                }
            }

            return resultado;
        }

        public static string TextoEstado(EstadoAdjunto estado) => estado switch
        {
            EstadoAdjunto.Adjuntado => "Adjuntado",
            EstadoAdjunto.NoEncontrado => "No encontrado",
            EstadoAdjunto.SinAcceso => "Sin acceso",
            EstadoAdjunto.ExcedeTamano => "Excede el tamaño, revísalo en SAP",
            EstadoAdjunto.NoPermitido => "No permitido",
            _ => estado.ToString()
        };

        // trgtPath puede venir con o sin '\' final
        private static string? RutaCompleta(string? carpeta, string nombre)
        {
            if (string.IsNullOrWhiteSpace(carpeta) || string.IsNullOrWhiteSpace(nombre)) return null;

            try
            {
                return Normalizar(Path.Combine(carpeta.Trim(), nombre));
            }
            catch (ArgumentException)
            {
                return null;
            }
        }

        // GetFullPath resuelve '..' y unifica separadores; null = ruta inválida
        private static string? Normalizar(string ruta)
        {
            try
            {
                return Path.GetFullPath(ruta.Trim());
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                return null;
            }
        }

        // Carpeta normalizada con separador final, para comparar prefijos por carpeta completa:
        // \\srv\anexos no coincide con \\srv\anexos2
        private static string? Carpeta(string ruta)
        {
            if (string.IsNullOrWhiteSpace(ruta)) return null;
            string? normalizada = Normalizar(ruta);
            if (normalizada == null) return null;
            return normalizada.TrimEnd(Path.DirectorySeparatorChar, Path.AltDirectorySeparatorChar) + Path.DirectorySeparatorChar;
        }

        // Primer par cuyo Desde es prefijo de la ruta (sin distinguir mayúsculas); sin coincidencia, la ruta de SAP
        private static string Reemplazar(string ruta, IEnumerable<ReemplazoRuta> reemplazos)
        {
            foreach (var r in reemplazos)
            {
                string? desde = Carpeta(r.Desde);
                string? hacia = Carpeta(r.Hacia);
                if (desde == null || hacia == null) continue;

                if (ruta.StartsWith(desde, StringComparison.OrdinalIgnoreCase))
                    return hacia + ruta[desde.Length..];
            }

            return ruta;
        }

        private static bool EstaPermitida(string ruta, IEnumerable<string> raices) =>
            raices.Select(Carpeta).Any(raiz => raiz != null && ruta.StartsWith(raiz, StringComparison.OrdinalIgnoreCase));

        private static string TipoMime(string nombre) =>
            TiposMime.TryGetContentType(nombre, out var tipo) ? tipo : MimePorDefecto;
    }
}
