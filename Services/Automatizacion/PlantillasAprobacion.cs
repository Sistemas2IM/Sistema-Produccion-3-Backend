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
                ("Folio", s.Folio.ToString())
            };

            // Serie + número del borrador tal como se ve en SAP (sin serie: solo el número)
            if (s.DocNum.HasValue)
                filas.Add(("Número en SAP (borrador)", Html($"{s.SerieNombre} {s.DocNum.Value}".Trim())));

            filas.Add(("Fecha del documento", FormatearFecha(s.DocDate ?? s.CreateDate)));
            filas.Add(("Solicitado", $"{FormatearFecha(s.CreateDate)} {FormatearHora(s.CreateTime)}".Trim()));
            filas.Add(("Solicitante", Html(s.Solicitante ?? $"Usuario {s.SolicitanteID}")));

            if (!string.IsNullOrWhiteSpace(s.CardName) || !string.IsNullOrWhiteSpace(s.CardCode))
                filas.Add(("Socio de negocio", Html($"{s.CardCode} - {s.CardName}".Trim(' ', '-'))));

            // Los traslados casi siempre traen DocTotal = 0: se muestran almacenes en lugar de monto
            if (s.ObjType == AprobacionesSapRepository.ObjTypeTraslado)
            {
                AgregarAlmacenes(filas, s.AlmacenOrigen, s.AlmacenOrigenNombre, s.AlmacenDestino, s.AlmacenDestinoNombre);
            }
            else if (s.DocTotal.HasValue)
            {
                filas.Add(("Total (con IVA)", Html($"{Total(s.DocTotal.Value)} {s.DocCur}".Trim())));
            }

            filas.Add(("Etapa", Html(s.Etapa ?? $"Etapa {s.CurrStep}")));

            if (!string.IsNullOrWhiteSpace(s.Remarks))
                filas.Add(("Comentarios", HtmlMultilinea(s.Remarks)));

            var pie = new StringBuilder();
            pie.Append(Detalle(s, opt.MaxLineasDetalle));
            pie.Append(Parrafo(opt.InstruccionesAprobacion));
            // El Web Client solo sirve para revisar el borrador; la aprobación se hace desde la alerta en B1
            string? url = UrlWebClient(s, opt);
            if (url != null && !string.IsNullOrWhiteSpace(opt.TextoEnlaceWebClient))
                pie.Append($"<p>{TextoConEnlace(opt.TextoEnlaceWebClient.Trim(), url)}</p>");

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

        // "Web Client" (sin distinguir mayúsculas) pasa a ser el enlace; si no aparece, todo el texto lo es.
        // Los tramos se escapan sin Trim para conservar los espacios alrededor del enlace
        private static string TextoConEnlace(string texto, string url)
        {
            const string marcador = "Web Client";
            string Enlace(string t) => $"<a href=\"{WebUtility.HtmlEncode(url)}\">{WebUtility.HtmlEncode(t)}</a>";

            int i = texto.IndexOf(marcador, StringComparison.OrdinalIgnoreCase);
            if (i < 0) return Enlace(texto);

            return WebUtility.HtmlEncode(texto[..i])
                + Enlace(texto.Substring(i, marcador.Length))
                + WebUtility.HtmlEncode(texto[(i + marcador.Length)..]);
        }

        // Sección "Detalle" con las líneas del borrador. Sin líneas (o si la consulta falló) no se muestra
        private static string Detalle(SolicitudPendienteSap s, int maxLineas)
        {
            if (s.Lineas == null || s.Lineas.Count == 0) return "";

            var mostradas = s.Lineas.Take(Math.Max(1, maxLineas)).ToList();
            int restantes = s.Lineas.Count - mostradas.Count;
            bool traslado = s.ObjType == AprobacionesSapRepository.ObjTypeTraslado;

            var columnas = new List<(string Titulo, bool Numero, bool Ajustar, Func<LineaBorradorSap, string> Valor)>
            {
                ("Código", false, false, l => Html(l.ItemCode)),
                ("Descripción", false, true, l => Html(l.Descripcion)),
                ("Cantidad", true, false, l => FormatearCantidad(l.Cantidad)),
                ("UM", false, false, l => Html(l.UnidadMedida))
            };

            // Los traslados traen precios en 0: se muestran los almacenes de cada línea
            if (traslado)
            {
                columnas.Add(("De → A", false, false, l => $"{Html(l.AlmacenOrigen)} &rarr; {Html(l.AlmacenDestino)}"));
            }
            else
            {
                // Una sola moneda en las líneas mostradas: va en el encabezado; si hay varias, en cada celda
                var monedas = mostradas.Select(l => Moneda(l, s)).Distinct().ToList();
                bool monedaUnica = monedas.Count == 1;
                string sufijo = monedaUnica && monedas[0] != "" ? $" ({monedas[0]})" : "";
                string Monto(LineaBorradorSap l, string valor) =>
                    Html(monedaUnica ? valor : $"{valor} {Moneda(l, s)}");

                columnas.Add(($"Precio unitario{sufijo}", true, false, l => Monto(l, FormatearPrecio(l.Precio))));
                if (mostradas.Any(l => l.DescuentoPorc > 0))
                    columnas.Add(("% Desc.", true, false, l => FormatearPorcentaje(l.DescuentoPorc)));
                columnas.Add(($"Total línea{sufijo}", true, false, l => Monto(l, Total(l.TotalLinea))));
            }

            const string celda = "padding:5px 6px;border:1px solid #ddd;vertical-align:top;";
            var sb = new StringBuilder();
            sb.Append("<h3 style=\"color:#1a4f8b;font-size:15px;margin:18px 0 6px\">Detalle</h3>");
            sb.Append("<table style=\"border-collapse:collapse;width:100%;font-size:13px\">");

            sb.Append("<tr>");
            foreach (var c in columnas)
                sb.Append($"<th style=\"{celda}background:#f5f7fa;text-align:{(c.Numero ? "right" : "left")};{(c.Ajustar ? "" : "white-space:nowrap;")}\">{Html(c.Titulo)}</th>");
            sb.Append("</tr>");

            foreach (var l in mostradas)
            {
                sb.Append("<tr>");
                foreach (var c in columnas)
                {
                    // La descripción puede ser larga: hace salto de línea en lugar de cortarse
                    string estilo = c.Ajustar
                        ? "word-wrap:break-word;overflow-wrap:break-word;"
                        : "white-space:nowrap;";
                    sb.Append($"<td style=\"{celda}{estilo}{(c.Numero ? "text-align:right;" : "")}\">{c.Valor(l)}</td>");
                }
                sb.Append("</tr>");
            }

            if (restantes > 0)
            {
                string texto = restantes == 1 ? "… y 1 línea más." : $"… y {restantes} líneas más.";
                sb.Append($"<tr><td colspan=\"{columnas.Count}\" style=\"{celda}color:#666;font-style:italic\">{Html($"{texto} Revisa el documento completo en SAP.")}</td></tr>");
            }

            sb.Append("</table>");

            // Suma de todas las líneas, no solo de las mostradas
            if (!traslado)
            {
                string moneda = s.DocCur ?? s.Lineas.Select(l => l.Moneda).FirstOrDefault(m => m != null) ?? "";
                sb.Append($"<p style=\"margin:6px 0 0;text-align:right\"><b>Subtotal sin IVA:</b> {Html($"{Total(s.Lineas.Sum(l => l.TotalLinea))} {moneda}")}</p>");
            }

            return sb.ToString();
        }

        // Currency de la línea; si viene vacía, la moneda del documento
        private static string Moneda(LineaBorradorSap l, SolicitudPendienteSap s) => l.Moneda ?? s.DocCur ?? "";

        private static string FormatearCantidad(decimal v) => v.ToString("0.###", CultureInfo.InvariantCulture);

        // Mínimo 2 y máximo 5 decimales: 0.30, 0.245, 0.07105
        private static string FormatearPrecio(decimal v) => v.ToString("0.00###", CultureInfo.InvariantCulture);

        private static string FormatearPorcentaje(decimal v) => v.ToString("0.##", CultureInfo.InvariantCulture) + "%";

        private static string Total(decimal v) => v.ToString("N2", CultureInfo.InvariantCulture);

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
