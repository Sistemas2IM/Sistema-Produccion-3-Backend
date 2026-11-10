// TEMPORAL - borrar después de la prueba de correo
using Microsoft.Extensions.Options;
using Sistema_Produccion_3_Backend.Services.Automatizacion;
using Sistema_Produccion_3_Backend.Services.Correo;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;
using System.Net.Mail;

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
                IOptionsMonitor<AlertasAprobacionOptions> opciones,
                CancellationToken ct) =>
            {
                if (string.IsNullOrWhiteSpace(req.Destino) || !MailAddress.TryCreate(req.Destino, out _))
                    return Results.BadRequest(new { error = "destino debe ser un correo válido." });

                string tipo = req.Tipo?.Trim().ToLowerInvariant() ?? "";
                var opt = opciones.CurrentValue;

                (string Asunto, string Cuerpo) correoPrueba;
                switch (tipo)
                {
                    case "traslado": correoPrueba = PlantillasAprobacion.Pendiente(Traslado(), opt); break;
                    case "factura": correoPrueba = PlantillasAprobacion.Pendiente(Factura(), opt); break;
                    case "resultado": correoPrueba = PlantillasAprobacion.Resultado(ResultadoAprobado(), opt); break;
                    case "rechazada": correoPrueba = PlantillasAprobacion.Resultado(ResultadoRechazado(), opt); break;
                    default: return Results.BadRequest(new { error = "tipo debe ser traslado, factura, resultado o rechazada." });
                }

                try
                {
                    await correo.EnviarAsync(req.Destino, $"[PRUEBA] {correoPrueba.Asunto}", correoPrueba.Cuerpo, ct);
                    return Results.Ok(new { enviado = true, destino = req.Destino, tipo, asunto = correoPrueba.Asunto });
                }
                catch (Exception ex)
                {
                    // Mensaje completo (incluye excepciones internas y la respuesta del servidor SMTP)
                    return Results.Json(new { enviado = false, error = ex.Message, detalle = ex.ToString() }, statusCode: StatusCodes.Status500InternalServerError);
                }
            })
            .WithTags("TEMPORAL - Prueba de correo");
        }

        private static SolicitudPendienteSap Traslado() => new()
        {
            WddCode = 99001,
            CurrStep = 3,
            Etapa = "SM - Etapa 1",
            AprobadorID = 74,
            Aprobador = "aprobador.prueba",
            AprobadorNombre = "Aprobador de Prueba",
            ObjType = AprobacionesSapRepository.ObjTypeTraslado,
            Folio = 99001,
            DocDate = DateTime.Today,
            DocTotal = 0,
            DocCur = "USD",
            SolicitanteID = 49,
            Solicitante = "Planificación",
            Remarks = "Traslado de materia prima para OF de prueba",
            CreateDate = DateTime.Today,
            CreateTime = DateTime.Now.Hour * 100 + DateTime.Now.Minute,
            AlmacenOrigen = "IM01",
            AlmacenOrigenNombre = "Materia Prima",
            AlmacenDestino = "IM07",
            AlmacenDestinoNombre = "Producción"
        };

        private static SolicitudPendienteSap Factura() => new()
        {
            WddCode = 99002,
            CurrStep = 5,
            Etapa = "Facturas en Mora",
            AprobadorID = 50,
            Aprobador = "aprobador.prueba",
            AprobadorNombre = "Aprobador de Prueba",
            ObjType = "13",
            Folio = 99002,
            DocDate = DateTime.Today,
            CardCode = "CL00999",
            CardName = "DISTRIBUIDORA DE PRUEBA, S.A. DE C.V.",
            DocTotal = 2096.15m,
            DocCur = "USD",
            SolicitanteID = 61,
            Solicitante = "Vendedor de Prueba",
            Remarks = "Solicitud de facturación (cliente con saldo vencido)",
            CreateDate = DateTime.Today,
            CreateTime = DateTime.Now.Hour * 100 + DateTime.Now.Minute
        };

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
