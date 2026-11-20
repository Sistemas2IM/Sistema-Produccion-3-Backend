using Sap.Data.Hana;
using System.Data;

namespace Sistema_Produccion_3_Backend.Services.SAP.HANA
{
    // Una fila por aprobador de la etapa actual de una solicitud pendiente (consulta 1 de la spec)
    public class SolicitudPendienteSap
    {
        public int WddCode { get; set; }
        public int CurrStep { get; set; }
        public string? Etapa { get; set; }
        public int AprobadorID { get; set; }
        public string? Aprobador { get; set; }
        public string? AprobadorNombre { get; set; }
        public string? AprobadorEmail { get; set; }
        public string ObjType { get; set; } = "";
        public int Folio { get; set; }
        public DateTime? DocDate { get; set; }
        public string? CardCode { get; set; }
        public string? CardName { get; set; }
        public decimal? DocTotal { get; set; }
        public string? DocCur { get; set; }
        public int SolicitanteID { get; set; }
        public string? Solicitante { get; set; }
        public string? Remarks { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? CreateTime { get; set; } // HHmm
        public string? AlmacenOrigen { get; set; }
        public string? AlmacenOrigenNombre { get; set; }
        public string? AlmacenDestino { get; set; }
        public string? AlmacenDestinoNombre { get; set; }
        public string? SerieNombre { get; set; }
        public int? DocNum { get; set; }
        // Líneas del borrador (DRF1); null = no se pudieron consultar (el correo sale sin detalle)
        public List<LineaBorradorSap>? Lineas { get; set; }

        public string TipoDoc => AprobacionesSapRepository.TipoDocumento(ObjType);
    }

    // Línea de un borrador (DRF1)
    public class LineaBorradorSap
    {
        public int DocEntry { get; set; }
        public int LineNum { get; set; }
        public string? ItemCode { get; set; }
        public string? Descripcion { get; set; }
        public decimal Cantidad { get; set; }
        public string? UnidadMedida { get; set; }
        public string? AlmacenOrigen { get; set; }
        public string? AlmacenDestino { get; set; }
        public decimal Precio { get; set; }
        public string? Moneda { get; set; }
        public decimal DescuentoPorc { get; set; }
        public decimal TotalLinea { get; set; }
    }

    // Estado actual de una solicitud ya avisada (consulta 2 de la spec)
    public class EstadoSolicitudSap
    {
        public int WddCode { get; set; }
        public string Status { get; set; } = "";
        public int CurrStep { get; set; }
        public string? Etapa { get; set; }
        public string ObjType { get; set; } = "";
        public int Folio { get; set; }
        public int SolicitanteID { get; set; }
        public string? Solicitante { get; set; }
        public string? SolicitanteEmail { get; set; }
        public string? UltimoDecisor { get; set; }
        public string? ComentarioDecision { get; set; }
        public string? AlmacenOrigen { get; set; }
        public string? AlmacenOrigenNombre { get; set; }
        public string? AlmacenDestino { get; set; }
        public string? AlmacenDestinoNombre { get; set; }

        public string TipoDoc => AprobacionesSapRepository.TipoDocumento(ObjType);
    }

    // Lectura de solicitudes de aprobación (OWDD/WDD1) vía ADO.NET de HANA.
    // No usa la DI API: no consume licencia ni comparte el OCompany estático de HANAConnection.
    public class AprobacionesSapRepository
    {
        public const string ObjTypeTraslado = "1250000001";

        // Límite de parámetros por consulta al pasar listas de WddCode
        private const int TamanoLote = 500;

        private const string ConsultaPendientes = @"
            SELECT
                T0.""WddCode"", T0.""CurrStep"", S.""Name"" AS ""Etapa"",
                T1.""UserID"" AS ""AprobadorID"", UA.""USER_CODE"" AS ""Aprobador"",
                UA.""U_NAME"" AS ""AprobadorNombre"", UA.""E_Mail"" AS ""AprobadorEmail"",
                T0.""ObjType"",
                T0.""DraftEntry"" AS ""Folio"",
                D.""DocDate"", D.""CardCode"", D.""CardName"", D.""DocTotal"", D.""DocCur"",
                T0.""OwnerID"" AS ""SolicitanteID"", UO.""U_NAME"" AS ""Solicitante"",
                T0.""Remarks"", T0.""CreateDate"", T0.""CreateTime"",
                D.""Filler"" AS ""AlmacenOrigen"", WO.""WhsName"" AS ""AlmacenOrigenNombre"",
                D.""ToWhsCode"" AS ""AlmacenDestino"", WD.""WhsName"" AS ""AlmacenDestinoNombre"",
                N.""SeriesName"", D.""DocNum""
            FROM ""OWDD"" T0
            INNER JOIN ""WDD1"" T1
                    ON T1.""WddCode"" = T0.""WddCode""
                   AND T1.""StepCode"" = T0.""CurrStep""
                   AND T1.""Status"" = 'W'
            LEFT JOIN ""OUSR"" UA ON UA.""USERID"" = T1.""UserID""
            LEFT JOIN ""OUSR"" UO ON UO.""USERID"" = T0.""OwnerID""
            LEFT JOIN ""OWST"" S  ON S.""WstCode"" = T0.""CurrStep""
            LEFT JOIN ""ODRF"" D  ON D.""DocEntry"" = T0.""DraftEntry""
            LEFT JOIN ""OWHS"" WO ON WO.""WhsCode"" = D.""Filler""
            LEFT JOIN ""OWHS"" WD ON WD.""WhsCode"" = D.""ToWhsCode""
            LEFT JOIN ""NNM1"" N  ON N.""Series"" = D.""Series""
            WHERE T0.""Status"" = 'W'
            ORDER BY T0.""WddCode"", T1.""SortId""";

        private const string ConsultaLineas = @"
            SELECT L.""DocEntry"", L.""LineNum"", L.""ItemCode"", L.""Dscription"",
                   L.""Quantity"", L.""unitMsr"", L.""FromWhsCod"", L.""WhsCode"",
                   L.""Price"", L.""Currency"", L.""DiscPrcnt"", L.""LineTotal""
            FROM ""DRF1"" L
            WHERE L.""DocEntry"" IN ({0})
            ORDER BY L.""DocEntry"", L.""LineNum""";

        // HANA no permite TOP/ORDER BY en subconsultas correlacionadas: el último decisor sale de ROW_NUMBER()
        private const string ConsultaEstados = @"
            SELECT
                T0.""WddCode"", T0.""Status"", T0.""CurrStep"", S.""Name"" AS ""Etapa"",
                T0.""ObjType"", T0.""DraftEntry"" AS ""Folio"",
                T0.""OwnerID"" AS ""SolicitanteID"", UO.""U_NAME"" AS ""Solicitante"",
                UO.""E_Mail"" AS ""SolicitanteEmail"",
                DEC.""UltimoDecisor"", DEC.""ComentarioDecision"",
                D.""Filler"" AS ""AlmacenOrigen"", WO.""WhsName"" AS ""AlmacenOrigenNombre"",
                D.""ToWhsCode"" AS ""AlmacenDestino"", WD.""WhsName"" AS ""AlmacenDestinoNombre""
            FROM ""OWDD"" T0
            LEFT JOIN ""OWST"" S  ON S.""WstCode"" = T0.""CurrStep""
            LEFT JOIN ""OUSR"" UO ON UO.""USERID"" = T0.""OwnerID""
            LEFT JOIN ""ODRF"" D  ON D.""DocEntry"" = T0.""DraftEntry""
            LEFT JOIN ""OWHS"" WO ON WO.""WhsCode"" = D.""Filler""
            LEFT JOIN ""OWHS"" WD ON WD.""WhsCode"" = D.""ToWhsCode""
            LEFT JOIN (
                SELECT W2.""WddCode"", U2.""U_NAME"" AS ""UltimoDecisor"", W2.""Remarks"" AS ""ComentarioDecision"",
                       ROW_NUMBER() OVER (PARTITION BY W2.""WddCode""
                                          ORDER BY W2.""UpdateDate"" DESC, W2.""UpdateTime"" DESC) AS ""Rn""
                FROM ""WDD1"" W2
                LEFT JOIN ""OUSR"" U2 ON U2.""USERID"" = W2.""UserID""
                WHERE W2.""Status"" IN ('Y','N')
                  AND W2.""WddCode"" IN ({0})
            ) DEC ON DEC.""WddCode"" = T0.""WddCode"" AND DEC.""Rn"" = 1
            WHERE T0.""WddCode"" IN ({0})";

        private readonly string _connectionString;
        private readonly ILogger<AprobacionesSapRepository> _logger;

        public AprobacionesSapRepository(IConfiguration configuration, ILogger<AprobacionesSapRepository> logger)
        {
            _connectionString = configuration.GetConnectionString("SapHanaConnection")
                ?? throw new InvalidOperationException("Falta ConnectionStrings:SapHanaConnection.");
            _logger = logger;
        }

        public async Task<List<SolicitudPendienteSap>> ObtenerPendientesAsync(CancellationToken ct)
        {
            var resultado = new List<SolicitudPendienteSap>();

            using var cn = new HanaConnection(_connectionString);
            await cn.OpenAsync(ct);
            using var cmd = new HanaCommand(ConsultaPendientes, cn);
            using var r = await cmd.ExecuteReaderAsync(ct);

            while (await r.ReadAsync(ct))
            {
                resultado.Add(new SolicitudPendienteSap
                {
                    WddCode = Convert.ToInt32(r["WddCode"]),
                    CurrStep = Convert.ToInt32(r["CurrStep"]),
                    Etapa = Texto(r, "Etapa"),
                    AprobadorID = Convert.ToInt32(r["AprobadorID"]),
                    Aprobador = Texto(r, "Aprobador"),
                    AprobadorNombre = Texto(r, "AprobadorNombre"),
                    AprobadorEmail = Texto(r, "AprobadorEmail"),
                    ObjType = Texto(r, "ObjType") ?? "",
                    Folio = Convert.ToInt32(r["Folio"]),
                    DocDate = Fecha(r, "DocDate"),
                    CardCode = Texto(r, "CardCode"),
                    CardName = Texto(r, "CardName"),
                    DocTotal = r["DocTotal"] is DBNull ? null : Convert.ToDecimal(r["DocTotal"]),
                    DocCur = Texto(r, "DocCur"),
                    SolicitanteID = Convert.ToInt32(r["SolicitanteID"]),
                    Solicitante = Texto(r, "Solicitante"),
                    Remarks = Texto(r, "Remarks"),
                    CreateDate = Fecha(r, "CreateDate"),
                    CreateTime = r["CreateTime"] is DBNull ? null : Convert.ToInt32(r["CreateTime"]),
                    AlmacenOrigen = Texto(r, "AlmacenOrigen"),
                    AlmacenOrigenNombre = Texto(r, "AlmacenOrigenNombre"),
                    AlmacenDestino = Texto(r, "AlmacenDestino"),
                    AlmacenDestinoNombre = Texto(r, "AlmacenDestinoNombre"),
                    SerieNombre = Texto(r, "SeriesName"),
                    DocNum = r["DocNum"] is DBNull ? null : Convert.ToInt32(r["DocNum"])
                });
            }

            await CargarLineasAsync(resultado, ct);
            return resultado;
        }

        // El detalle es opcional: si la consulta falla, Lineas queda en null y el aviso sale sin la sección
        public async Task CargarLineasAsync(IReadOnlyCollection<SolicitudPendienteSap> solicitudes, CancellationToken ct)
        {
            if (solicitudes.Count == 0) return;

            try
            {
                var porBorrador = (await ObtenerLineasAsync(solicitudes.Select(s => s.Folio).ToList(), ct))
                    .GroupBy(l => l.DocEntry)
                    .ToDictionary(g => g.Key, g => g.ToList());

                foreach (var s in solicitudes)
                    s.Lineas = porBorrador.TryGetValue(s.Folio, out var lineas) ? lineas : new List<LineaBorradorSap>();
            }
            catch (Exception ex) when (ex is not OperationCanceledException)
            {
                _logger.LogError(ex, "❌ No se pudieron consultar las líneas de los borradores (DRF1); los avisos se envían sin detalle.");
            }
        }

        public async Task<List<LineaBorradorSap>> ObtenerLineasAsync(IReadOnlyCollection<int> draftEntries, CancellationToken ct)
        {
            var resultado = new List<LineaBorradorSap>();
            if (draftEntries.Count == 0) return resultado;

            using var cn = new HanaConnection(_connectionString);
            await cn.OpenAsync(ct);

            // Una consulta por ciclo (por lote de TamanoLote borradores)
            foreach (var lote in draftEntries.Distinct().Chunk(TamanoLote))
            {
                string marcadores = string.Join(",", lote.Select(_ => "?"));
                using var cmd = new HanaCommand(string.Format(ConsultaLineas, marcadores), cn);
                foreach (var entry in lote)
                    cmd.Parameters.Add(new HanaParameter { HanaDbType = HanaDbType.Integer, Value = entry });

                using var r = await cmd.ExecuteReaderAsync(ct);
                while (await r.ReadAsync(ct))
                {
                    resultado.Add(new LineaBorradorSap
                    {
                        DocEntry = Convert.ToInt32(r["DocEntry"]),
                        LineNum = Convert.ToInt32(r["LineNum"]),
                        ItemCode = Texto(r, "ItemCode"),
                        Descripcion = Texto(r, "Dscription"),
                        Cantidad = Decimal(r, "Quantity"),
                        UnidadMedida = Texto(r, "unitMsr"),
                        AlmacenOrigen = Texto(r, "FromWhsCod"),
                        AlmacenDestino = Texto(r, "WhsCode"),
                        Precio = Decimal(r, "Price"),
                        Moneda = Texto(r, "Currency"),
                        DescuentoPorc = Decimal(r, "DiscPrcnt"),
                        TotalLinea = Decimal(r, "LineTotal")
                    });
                }
            }

            return resultado;
        }

        public async Task<List<EstadoSolicitudSap>> ObtenerEstadosAsync(IReadOnlyCollection<int> wddCodes, CancellationToken ct)
        {
            var resultado = new List<EstadoSolicitudSap>();
            if (wddCodes.Count == 0) return resultado;

            using var cn = new HanaConnection(_connectionString);
            await cn.OpenAsync(ct);

            foreach (var lote in wddCodes.Distinct().Chunk(TamanoLote))
            {
                // La lista aparece dos veces en la consulta (subconsulta y WHERE): los parámetros ? son posicionales
                string marcadores = string.Join(",", lote.Select(_ => "?"));
                using var cmd = new HanaCommand(string.Format(ConsultaEstados, marcadores), cn);
                for (int i = 0; i < 2; i++)
                    foreach (var wdd in lote)
                        cmd.Parameters.Add(new HanaParameter { HanaDbType = HanaDbType.Integer, Value = wdd });

                using var r = await cmd.ExecuteReaderAsync(ct);
                while (await r.ReadAsync(ct))
                {
                    resultado.Add(new EstadoSolicitudSap
                    {
                        WddCode = Convert.ToInt32(r["WddCode"]),
                        Status = Texto(r, "Status") ?? "",
                        CurrStep = Convert.ToInt32(r["CurrStep"]),
                        Etapa = Texto(r, "Etapa"),
                        ObjType = Texto(r, "ObjType") ?? "",
                        Folio = Convert.ToInt32(r["Folio"]),
                        SolicitanteID = Convert.ToInt32(r["SolicitanteID"]),
                        Solicitante = Texto(r, "Solicitante"),
                        SolicitanteEmail = Texto(r, "SolicitanteEmail"),
                        UltimoDecisor = Texto(r, "UltimoDecisor"),
                        ComentarioDecision = Texto(r, "ComentarioDecision"),
                        AlmacenOrigen = Texto(r, "AlmacenOrigen"),
                        AlmacenOrigenNombre = Texto(r, "AlmacenOrigenNombre"),
                        AlmacenDestino = Texto(r, "AlmacenDestino"),
                        AlmacenDestinoNombre = Texto(r, "AlmacenDestinoNombre")
                    });
                }
            }

            return resultado;
        }

        public static string TipoDocumento(string objType) => objType switch
        {
            "22" => "Orden de compra",
            "17" => "Pedido de cliente",
            "13" => "Factura de clientes",
            ObjTypeTraslado => "Solicitud de traslado",
            "59" => "Entrada de mercancías",
            "60" => "Salida de mercancías",
            _ => $"Documento tipo {objType}"
        };

        private static string? Texto(IDataRecord r, string columna)
        {
            var valor = r[columna];
            if (valor is DBNull) return null;
            var texto = valor.ToString()?.Trim();
            return string.IsNullOrEmpty(texto) ? null : texto;
        }

        private static decimal Decimal(IDataRecord r, string columna) =>
            r[columna] is DBNull ? 0m : Convert.ToDecimal(r[columna]);

        private static DateTime? Fecha(IDataRecord r, string columna) =>
            r[columna] is DBNull ? null : Convert.ToDateTime(r[columna]);
    }
}
