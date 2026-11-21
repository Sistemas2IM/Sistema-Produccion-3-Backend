// TEMPORAL - borrar después de la prueba de correo
using Microsoft.Extensions.Options;
using Sap.Data.Hana;
using Sistema_Produccion_3_Backend.Services.Automatizacion;
using Sistema_Produccion_3_Backend.Services.Correo;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;
using System.Data;
using System.Net.Mail;
using System.Text.RegularExpressions;

namespace Sistema_Produccion_3_Backend.Controllers.Test
{
    // TEMPORAL - borrar después de la prueba de correo
    // DraftEntry (opcional) cambia el borrador del tipo elegido, p. ej. una OC con adjuntos con espacios en el nombre
    public record TestCorreoRequest(string? Destino, string? Tipo, int? DraftEntry = null);

    // TEMPORAL - borrar después de la prueba de correo
    // Endpoint mínimo (no controlador) para que solo exista si Program.cs lo mapea en Development
    public static class TestCorreoEndpoint
    {
        public static void MapTestCorreo(this WebApplication app)
        {
            app.MapPost("/api/test/correo", async (
                TestCorreoRequest req,
                ICorreoService correo,
                AprobacionesSapRepository sap,
                AdjuntosAprobacion lectorAdjuntos,
                IConfiguration configuration,
                IOptionsMonitor<AlertasAprobacionOptions> opciones,
                CancellationToken ct) =>
            {
                if (string.IsNullOrWhiteSpace(req.Destino) || !MailAddress.TryCreate(req.Destino, out _))
                    return Results.BadRequest(new { error = "destino debe ser un correo válido." });

                string tipo = req.Tipo?.Trim().ToLowerInvariant() ?? "";
                var opt = opciones.CurrentValue;

                // Borradores reales de ODRF/DRF1, uno por tipo con enlace al Web Client:
                // traslado 16621 (1 línea), factura 16685 (4 líneas), pedido 16681, OC 16677 (2 adjuntos en el NAS)
                int? draftEntry = tipo switch
                {
                    "traslado" => 16621,
                    "factura" or "factura-max2" => 16685,
                    "pedido" => 16681,
                    "orden" or "orden-noexiste" or "orden-nopermitido" or "orden-max0" or "orden-sinadjuntos" or "orden-sinreemplazo" => 16677,
                    _ => null
                };
                if (draftEntry.HasValue && req.DraftEntry.HasValue)
                    draftEntry = req.DraftEntry;

                (string Asunto, string Cuerpo) correoPrueba;
                AdjuntosCorreo? adjuntos = null;
                try
                {
                    if (draftEntry.HasValue)
                    {
                        var s = await BorradorRealAsync(configuration, draftEntry.Value, ct);
                        if (s == null)
                            return Results.NotFound(new { error = $"No existe el borrador {draftEntry} en ODRF." });

                        // Mismo camino que el servicio: si DRF1 falla, el correo sale sin Detalle
                        await sap.CargarLineasAsync(new[] { s }, ct);

                        // Copia: no se modifica la instancia de opciones que usa el servicio
                        var optPrueba = tipo switch
                        {
                            "factura-max2" => Copia(opt, o => o.MaxLineasDetalle = 2),
                            "orden-max0" => Copia(opt, o => o.MaxAdjuntosMB = 0),
                            "orden-sinadjuntos" => Copia(opt, o => o.AdjuntarArchivos = false),
                            "orden-sinreemplazo" => Copia(opt, o => o.ReemplazosRutaAdjuntos = new()),
                            _ => opt
                        };

                        // Mismo camino que el servicio: si ATC1 falla, la sección lo indica y el correo sale igual
                        if (optPrueba.AdjuntarArchivos)
                        {
                            await sap.CargarAdjuntosAsync(new[] { s }, ct);
                            AgregarAdjuntoFicticio(s, tipo, optPrueba);
                            adjuntos = await lectorAdjuntos.PrepararAsync(s, optPrueba, ct);
                        }

                        correoPrueba = PlantillasAprobacion.Pendiente(s, optPrueba, adjuntos);
                    }
                    else
                    {
                        switch (tipo)
                        {
                            case "resultado": correoPrueba = PlantillasAprobacion.Resultado(ResultadoAprobado(), opt); break;
                            case "rechazada": correoPrueba = PlantillasAprobacion.Resultado(ResultadoRechazado(), opt); break;
                            default: return Results.BadRequest(new { error = "tipo debe ser traslado, factura, factura-max2, pedido, orden, orden-noexiste, orden-nopermitido, orden-max0, orden-sinadjuntos, orden-sinreemplazo, resultado o rechazada." });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Results.Json(new { enviado = false, error = ex.Message, detalle = ex.ToString() }, statusCode: StatusCodes.Status500InternalServerError);
                }

                try
                {
                    await correo.EnviarAsync(req.Destino, $"[PRUEBA] {correoPrueba.Asunto}", correoPrueba.Cuerpo, ct, adjuntos?.ParaEnviar);
                    // href tal como quedó en el HTML (con &amp;), para comprobar que %252C no se recodificó
                    var href = Regex.Match(correoPrueba.Cuerpo, "href=\"([^\"]*)\"");
                    return Results.Ok(new
                    {
                        enviado = true,
                        destino = req.Destino,
                        tipo,
                        asunto = correoPrueba.Asunto,
                        enlace = href.Success ? href.Groups[1].Value : null,
                        adjuntosConsultaFallida = adjuntos?.ConsultaFallida,
                        adjuntos = adjuntos?.Archivos.Select(a => new
                        {
                            nombre = a.Nombre,
                            estado = AdjuntosAprobacion.TextoEstado(a.Estado),
                            ruta = a.Ruta,
                            tipoMime = a.Archivo?.TipoMime,
                            kb = a.Archivo == null ? (int?)null : a.Archivo.Contenido.Length / 1024
                        })
                    });
                }
                catch (Exception ex)
                {
                    // Mensaje completo (incluye excepciones internas y la respuesta del servidor SMTP)
                    return Results.Json(new { enviado = false, error = ex.Message, detalle = ex.ToString() }, statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithTags("TEMPORAL - Prueba de correo");
        }

        // Encabezado del borrador como en la consulta 1 (ODRF + NNM1 + OWHS); los datos de la aprobación son ficticios
        private const string ConsultaBorrador = @"
            SELECT D.""DocEntry"", D.""ObjType"", D.""DocDate"", D.""CardCode"", D.""CardName"",
                   D.""DocTotal"", D.""DocCur"", D.""Comments"",
                   D.""Filler"", WO.""WhsName"" AS ""OrigenNombre"",
                   D.""ToWhsCode"", WD.""WhsName"" AS ""DestinoNombre"",
                   N.""SeriesName"", D.""DocNum""
            FROM ""ODRF"" D
            LEFT JOIN ""OWHS"" WO ON WO.""WhsCode"" = D.""Filler""
            LEFT JOIN ""OWHS"" WD ON WD.""WhsCode"" = D.""ToWhsCode""
            LEFT JOIN ""NNM1"" N  ON N.""Series"" = D.""Series""
            WHERE D.""DocEntry"" = ?";

        private static async Task<SolicitudPendienteSap?> BorradorRealAsync(IConfiguration configuration, int draftEntry, CancellationToken ct)
        {
            using var cn = new HanaConnection(configuration.GetConnectionString("SapHanaConnection"));
            await cn.OpenAsync(ct);
            using var cmd = new HanaCommand(ConsultaBorrador, cn);
            cmd.Parameters.Add(new HanaParameter { HanaDbType = HanaDbType.Integer, Value = draftEntry });
            using var r = await cmd.ExecuteReaderAsync(ct);
            if (!await r.ReadAsync(ct)) return null;

            return new SolicitudPendienteSap
            {
                WddCode = 99000 + draftEntry % 1000,
                CurrStep = 3,
                Etapa = "Etapa de prueba",
                AprobadorID = 74,
                Aprobador = "aprobador.prueba",
                AprobadorNombre = "Aprobador de Prueba",
                ObjType = Texto(r, "ObjType") ?? "",
                Folio = draftEntry,
                DocDate = r["DocDate"] is DBNull ? null : Convert.ToDateTime(r["DocDate"]),
                CardCode = Texto(r, "CardCode"),
                CardName = Texto(r, "CardName"),
                DocTotal = r["DocTotal"] is DBNull ? null : Convert.ToDecimal(r["DocTotal"]),
                DocCur = Texto(r, "DocCur"),
                SolicitanteID = 49,
                Solicitante = "Solicitante de Prueba",
                Remarks = Texto(r, "Comments"),
                CreateDate = DateTime.Today,
                CreateTime = DateTime.Now.Hour * 100 + DateTime.Now.Minute,
                AlmacenOrigen = Texto(r, "Filler"),
                AlmacenOrigenNombre = Texto(r, "OrigenNombre"),
                AlmacenDestino = Texto(r, "ToWhsCode"),
                AlmacenDestinoNombre = Texto(r, "DestinoNombre"),
                SerieNombre = Texto(r, "SeriesName"),
                DocNum = r["DocNum"] is DBNull ? null : Convert.ToInt32(r["DocNum"])
            };
        }

        private static AlertasAprobacionOptions Copia(AlertasAprobacionOptions o, Action<AlertasAprobacionOptions> cambio)
        {
            var copia = new AlertasAprobacionOptions
            {
                UrlWebClientPorTipo = o.UrlWebClientPorTipo,
                InstruccionesAprobacion = o.InstruccionesAprobacion,
                NotaAprobada = o.NotaAprobada,
                TextoEnlaceWebClient = o.TextoEnlaceWebClient,
                MaxLineasDetalle = o.MaxLineasDetalle,
                AdjuntarArchivos = o.AdjuntarArchivos,
                MaxAdjuntosMB = o.MaxAdjuntosMB,
                ReemplazosRutaAdjuntos = new List<ReemplazoRuta>(o.ReemplazosRutaAdjuntos),
                RutasAdjuntosPermitidas = new List<string>(o.RutasAdjuntosPermitidas)
            };
            cambio(copia);
            return copia;
        }

        // Adjunto ficticio al final de los reales de ATC1:
        // orden-noexiste = archivo que no existe, con la carpeta tal como la guarda SAP (pasa por el reemplazo);
        // orden-nopermitido = archivo local que sí existe pero está fuera de RutasAdjuntosPermitidas (no debe leerse)
        private static void AgregarAdjuntoFicticio(SolicitudPendienteSap s, string tipo, AlertasAprobacionOptions opt)
        {
            if (s.Adjuntos == null) return;

            var ficticio = tipo switch
            {
                "orden-noexiste" => new AdjuntoBorradorSap
                {
                    Carpeta = @"\\192.168.2.246\anexos",
                    FileName = "PRUEBA_NO_EXISTE",
                    FileExt = "pdf"
                },
                "orden-nopermitido" => new AdjuntoBorradorSap
                {
                    Carpeta = Environment.GetFolderPath(Environment.SpecialFolder.Windows),
                    FileName = "win",
                    FileExt = "ini"
                },
                _ => null
            };
            if (ficticio == null) return;

            ficticio.DocEntry = s.Folio;
            ficticio.Line = s.Adjuntos.Count == 0 ? 0 : s.Adjuntos.Max(a => a.Line) + 1;
            s.Adjuntos.Add(ficticio);
        }

        private static string? Texto(IDataRecord r, string columna)
        {
            var texto = r[columna] is DBNull ? null : r[columna].ToString()?.Trim();
            return string.IsNullOrEmpty(texto) ? null : texto;
        }

        // Traslado aprobado: debe mostrar Almacenes y la nota NotaAprobada
        private static EstadoSolicitudSap ResultadoAprobado() => new()
        {
            WddCode = 99003,
            Status = "Y",
            CurrStep = 4,
            Etapa = "SM - Etapa 2",
            ObjType = AprobacionesSapRepository.ObjTypeTraslado,
            Folio = 99003,
            SolicitanteID = 49,
            Solicitante = "Planificación",
            UltimoDecisor = "Aprobador de Prueba",
            ComentarioDecision = "Aprobado, entregar en el turno de la mañana.",
            AlmacenOrigen = "IM01",
            AlmacenOrigenNombre = "Materia Prima",
            AlmacenDestino = "IM07",
            AlmacenDestinoNombre = "Producción"
        };

        // Traslado rechazado: debe mostrar Almacenes pero NO la nota NotaAprobada
        private static EstadoSolicitudSap ResultadoRechazado() => new()
        {
            WddCode = 99004,
            Status = "N",
            CurrStep = 3,
            Etapa = "SM - Etapa 1",
            ObjType = AprobacionesSapRepository.ObjTypeTraslado,
            Folio = 99004,
            SolicitanteID = 49,
            Solicitante = "Planificación",
            UltimoDecisor = "Aprobador de Prueba",
            ComentarioDecision = "Rechazado: la OF ya tiene el material asignado.",
            AlmacenOrigen = "IM11",
            AlmacenOrigenNombre = "Nejapa-Salitre",
            AlmacenDestino = "IM01",
            AlmacenDestinoNombre = "Materia Prima"
        };
    }
}
