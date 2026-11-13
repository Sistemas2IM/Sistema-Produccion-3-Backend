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
    public record TestCorreoRequest(string? Destino, string? Tipo);

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
                IConfiguration configuration,
                IOptionsMonitor<AlertasAprobacionOptions> opciones,
                CancellationToken ct) =>
            {
                if (string.IsNullOrWhiteSpace(req.Destino) || !MailAddress.TryCreate(req.Destino, out _))
                    return Results.BadRequest(new { error = "destino debe ser un correo válido." });

                string tipo = req.Tipo?.Trim().ToLowerInvariant() ?? "";
                var opt = opciones.CurrentValue;

                // Borradores reales de ODRF/DRF1, uno por tipo con enlace al Web Client:
                // traslado 16621 (1 línea), factura 16685 (4 líneas), pedido 16681, OC 16677
                int? draftEntry = tipo switch
                {
                    "traslado" => 16621,
                    "factura" or "factura-max2" => 16685,
                    "pedido" => 16681,
                    "orden" => 16677,
                    _ => null
                };

                (string Asunto, string Cuerpo) correoPrueba;
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
                        var optPrueba = tipo == "factura-max2" ? ConMaxLineas(opt, 2) : opt;
                        correoPrueba = PlantillasAprobacion.Pendiente(s, optPrueba);
                    }
                    else
                    {
                        switch (tipo)
                        {
                            case "resultado": correoPrueba = PlantillasAprobacion.Resultado(ResultadoAprobado(), opt); break;
                            case "rechazada": correoPrueba = PlantillasAprobacion.Resultado(ResultadoRechazado(), opt); break;
                            default: return Results.BadRequest(new { error = "tipo debe ser traslado, factura, factura-max2, pedido, orden, resultado o rechazada." });
                        }
                    }
                }
                catch (Exception ex)
                {
                    return Results.Json(new { enviado = false, error = ex.Message, detalle = ex.ToString() }, statusCode: StatusCodes.Status500InternalServerError);
                }

                try
                {
                    await correo.EnviarAsync(req.Destino, $"[PRUEBA] {correoPrueba.Asunto}", correoPrueba.Cuerpo, ct);
                    // href tal como quedó en el HTML (con &amp;), para comprobar que %252C no se recodificó
                    var href = Regex.Match(correoPrueba.Cuerpo, "href=\"([^\"]*)\"");
                    return Results.Ok(new { enviado = true, destino = req.Destino, tipo, asunto = correoPrueba.Asunto, enlace = href.Success ? href.Groups[1].Value : null });
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

        private static AlertasAprobacionOptions ConMaxLineas(AlertasAprobacionOptions o, int max) => new()
        {
            UrlWebClientPorTipo = o.UrlWebClientPorTipo,
            InstruccionesAprobacion = o.InstruccionesAprobacion,
            NotaAprobada = o.NotaAprobada,
            TextoEnlaceWebClient = o.TextoEnlaceWebClient,
            MaxLineasDetalle = max
        };

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
