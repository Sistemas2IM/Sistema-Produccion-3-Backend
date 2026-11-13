using Sistema_Produccion_3_Backend.Services.SAP.HANA;
using System.Globalization;
using System.Net;
using System.Text;

namespace Sistema_Produccion_3_Backend.Services.Automatizacion
{
    // HTML de los correos de aprobación. Todo texto que viene de SAP se escapa.
    public static class PlantillasAprobacion
    {
        public static (string Asunto, string Cuerpo) Pendiente(SolicitudPendienteSap s, AlertasAprobacionOptions opt)
        {
            string asunto = $"Aprobación pendiente: {s.TipoDoc} #{s.Folio}";

            var filas = new List<(string, string)>
            {
                ("Tipo de documento", Html(s.TipoDoc)),
                ("Folio", s.Folio.ToString()),
                ("Fecha del documento", FormatearFecha(s.DocDate ?? s.CreateDate)),
                ("Solicitado", $"{FormatearFecha(s.CreateDate)} {FormatearHora(s.CreateTime)}".Trim()),
                ("Solicitante", Html(s.Solicitante ?? $"Usuario {s.SolicitanteID}"))
            };

            if (!string.IsNullOrWhiteSpace(s.CardName) || !string.IsNullOrWhiteSpace(s.CardCode))
                filas.Add(("Socio de negocio", Html($"{s.CardCode} - {s.CardName}".Trim(' ', '-'))));

            // Los traslados casi siempre traen DocTotal = 0: se muestran almacenes en lugar de monto
            if (s.ObjType == AprobacionesSapRepository.ObjTypeTraslado)
            {
                AgregarAlmacenes(filas, s.AlmacenOrigen, s.AlmacenOrigenNombre, s.AlmacenDestino, s.AlmacenDestinoNombre);
            }
            else if (s.DocTotal.HasValue)
            {
                filas.Add(("Monto", Html($"{s.DocTotal.Value.ToString("N2", CultureInfo.InvariantCulture)} {s.DocCur}".Trim())));
            }

            filas.Add(("Etapa", Html(s.Etapa ?? $"Etapa {s.CurrStep}")));

            if (!string.IsNullOrWhiteSpace(s.Remarks))
                filas.Add(("Comentarios", HtmlMultilinea(s.Remarks)));

            var pie = new StringBuilder();
            pie.Append(Parrafo(opt.InstruccionesAprobacion));
            string? url = UrlWebClient(s, opt);
            if (url != null)
                pie.Append($"<p>También puedes hacerlo desde el <a href=\"{WebUtility.HtmlEncode(url)}\">Web Client</a>.</p>");

            string cuerpo = Documento(
                "Solicitud de aprobación pendiente",
                $"<p>{Saludo(s.AprobadorNombre ?? s.Aprobador)}</p><p>Tienes una solicitud de aprobación pendiente en SAP Business One:</p>",
                filas,
                pie.ToString());

            return (asunto, cuerpo);
        }

        public static (string Asunto, string Cuerpo) Resultado(EstadoSolicitudSap e, AlertasAprobacionOptions opt)
        {
            string resultado = TextoResultado(e.Status);
            string asunto = $"Solicitud {resultado.ToLowerInvariant()}: {e.TipoDoc} #{e.Folio}";

            var filas = new List<(string, string)>
            {
                ("Tipo de documento", Html(e.TipoDoc)),
                ("Folio", e.Folio.ToString()),
                ("Resultado", $"<b>{Html(resultado)}</b>")
            };

            if (e.Status != "C" && !string.IsNullOrWhiteSpace(e.UltimoDecisor))
                filas.Add(("Decidido por", Html(e.UltimoDecisor)));

            if (!string.IsNullOrWhiteSpace(e.ComentarioDecision))
                filas.Add(("Comentario", HtmlMultilinea(e.ComentarioDecision)));

            if (e.ObjType == AprobacionesSapRepository.ObjTypeTraslado)
                AgregarAlmacenes(filas, e.AlmacenOrigen, e.AlmacenOrigenNombre, e.AlmacenDestino, e.AlmacenDestinoNombre);

            filas.Add(("Última etapa", Html(e.Etapa ?? $"Etapa {e.CurrStep}")));

            // Solo 'Y': en 'P' y 'A' el documento ya se generó
            string pie = e.Status == "Y" ? Parrafo(opt.NotaAprobada) : "";

            string cuerpo = Documento(
                $"Solicitud {resultado.ToLowerInvariant()}",
                $"<p>{Saludo(e.Solicitante)}</p><p>Tu solicitud de aprobación en SAP Business One cambió de estado:</p>",
                filas,
                pie);

            return (asunto, cuerpo);
        }

        public static string TextoResultado(string status) => status switch
        {
            "Y" => "Aprobada",
            "N" => "Rechazada",
            "C" => "Cancelada",
            "P" => "Aprobada (documento generado)",
            "A" => "Aprobada (generada por autorizador)",
            _ => $"Estado {status}"
        };

        private static string Documento(string titulo, string intro, List<(string Etiqueta, string Valor)> filas, string pie)
        {
            var sb = new StringBuilder();
            sb.Append("<div style=\"font-family:Arial,Helvetica,sans-serif;font-size:14px;color:#222;max-width:640px\">");
            sb.Append($"<h2 style=\"color:#1a4f8b;margin-bottom:8px\">{Html(titulo)}</h2>");
            sb.Append(intro);
            sb.Append("<table style=\"border-collapse:collapse;width:100%\">");
            foreach (var (etiqueta, valor) in filas)
            {
                sb.Append("<tr>");
                sb.Append($"<td style=\"padding:6px 10px;border:1px solid #ddd;background:#f5f7fa;font-weight:bold;width:170px;vertical-align:top\">{Html(etiqueta)}</td>");
                sb.Append($"<td style=\"padding:6px 10px;border:1px solid #ddd\">{valor}</td>");
                sb.Append("</tr>");
            }
            sb.Append("</table>");
            sb.Append(pie);
            sb.Append("<p style=\"color:#888;font-size:12px\">Este es un aviso automático; no respondas a este correo.</p>");
            sb.Append("</div>");
            return sb.ToString();
        }

        // Sin plantilla para el tipo = sin enlace. Solo se sustituye {DraftEntry}; el resto de la URL
        // no se toca (el HtmlEncode del href solo escapa '&' a nivel HTML, el cliente lo revierte)
        private static string? UrlWebClient(SolicitudPendienteSap s, AlertasAprobacionOptions opt)
        {
            if (!opt.UrlWebClientPorTipo.TryGetValue(s.ObjType, out var plantilla) || string.IsNullOrWhiteSpace(plantilla))
                return null;

            return plantilla.Trim().Replace("{DraftEntry}", s.Folio.ToString(CultureInfo.InvariantCulture));
        }

        private static string Saludo(string? nombre) =>
            string.IsNullOrWhiteSpace(nombre) ? "Hola," : $"Hola {Html(nombre)},";

        // Texto configurable en appsettings: se escapa como el resto; vacío = no se muestra
        private static string Parrafo(string? texto) =>
            string.IsNullOrWhiteSpace(texto) ? "" : $"<p>{HtmlMultilinea(texto.Trim())}</p>";

        private static void AgregarAlmacenes(List<(string, string)> filas, string? origen, string? origenNombre, string? destino, string? destinoNombre)
        {
            if (origen != null || destino != null)
                filas.Add(("Almacenes", $"{Almacen(origen, origenNombre)} &rarr; {Almacen(destino, destinoNombre)}"));
        }

        private static string Almacen(string? codigo, string? nombre) =>
            codigo == null ? "<i>N/D</i>" : Html(nombre == null ? codigo : $"{codigo} - {nombre}");

        private static string FormatearFecha(DateTime? fecha) => fecha?.ToString("dd/MM/yyyy") ?? "";

        // OWDD.CreateTime viene como entero HHmm (p. ej. 944 = 09:44)
        private static string FormatearHora(int? hhmm) =>
            hhmm.HasValue ? $"{hhmm.Value / 100:00}:{hhmm.Value % 100:00}" : "";

        private static string Html(string? texto) => WebUtility.HtmlEncode(texto?.Trim() ?? "");

        // Se escapa primero y luego se convierten los saltos, para que el <br> no quede escapado
        private static string HtmlMultilinea(string texto) =>
            Html(texto).Replace("\r\n", "<br>").Replace("\n", "<br>").Replace("\r", "<br>");
    }
}
