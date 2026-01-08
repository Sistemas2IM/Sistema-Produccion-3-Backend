using Microsoft.AspNetCore.Mvc;
using SAPbobsCOM;
using System.Collections.Generic;
using System.Threading.Tasks;
using System;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;

namespace TuNamespace.Controllers // Asegúrate de ajustar el namespace
{
    [Route("api/[controller]")]
    [ApiController]
    public class LoteInfoController : ControllerBase
    {
        // GET: api/loteinfo/get/LOTE12345
        [HttpGet("get/{distNumber}")]
        public IActionResult GetInfoPorLote(string distNumber)
        {
            try
            {
                // 1. Conectar a HANA (Usando tu clase helper existente)
                HANAConnection.sapConn();
                int retVal = HANAConnection.RetVal;
                SAPbobsCOM.Company oCompany = HANAConnection.OCompany;

                if (retVal != 0)
                {
                    return StatusCode(500, "Error al conectar con SAP HANA: " + oCompany.GetLastErrorDescription());
                }

                // 2. Limpieza básica para evitar inyección SQL simple 
                // (Si distNumber contiene una comilla simple, podría romper la query)
                string safeDistNumber = distNumber.Replace("'", "''");

                // 3. Consulta SQL adaptada para C#
                // Nota: Las comillas dobles de las columnas se escapan poniéndolas dobles ("")
                string query = $@"
               SELECT DISTINCT
                    T1.""DistNumber"",
                    T1.""ItemCode"",

                    COALESCE(T3.""CardName"", T2.""U_Marca"") AS ""Proveedor"",
                    T2.""U_Marca"",
                    T2.""ItemName"",
                    T2.""U_Ancho"",
                    CASE 
                            WHEN T2.""U_SubFamilia"" IN ('02003', '02005', '02006', '02007', '09003') 
                                THEN 'C' || TO_VARCHAR(TO_INT(T2.""U_Calibre""))
                            WHEN T2.""U_SubFamilia"" IN ('02001', '09002', '09005') 
                                THEN 'B' || TO_VARCHAR(TO_INT(T2.""U_Calibre""))
                            ELSE TO_VARCHAR(TO_INT(T2.""U_Calibre""))
                    END AS ""Calibre"",
                    T2.""U_Gramaje""

                FROM OBTN T1
                INNER JOIN OITM T2 ON T1.""ItemCode"" = T2.""ItemCode""

                LEFT JOIN IBT1 T_LINK ON T1.""ItemCode"" = T_LINK.""ItemCode"" 
                                      AND T1.""DistNumber"" = T_LINK.""BatchNum"" 
                                      AND T_LINK.""BaseType"" = 20

                LEFT JOIN OPDN T3 ON T_LINK.""BaseEntry"" = T3.""DocEntry""
                WHERE T1.""DistNumber"" = '{safeDistNumber}'";

                // 4. Ejecutar la consulta
                Recordset recordSet = null;
                try
                {
                    recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                    recordSet.DoQuery(query);
                }
                catch (Exception ex)
                {
                    return StatusCode(500, $"Error al ejecutar query en SAP: {ex.Message}");
                }

                // 5. Verificar resultados
                if (recordSet.RecordCount == 0)
                {
                    return NotFound($"No se encontró información para el lote: {distNumber}");
                }

                // 6. Mapear resultados a la lista
                var listaResultados = new List<LoteInfoResponse>();

                while (!recordSet.EoF)
                {
                    var item = new LoteInfoResponse
                    {
                        DistNumber = recordSet.Fields.Item("DistNumber").Value.ToString(),
                        ItemCode = recordSet.Fields.Item("ItemCode").Value.ToString(),
                        // Usamos ?.ToString() por si algún campo viene nulo de SAP
                        Proveedor = recordSet.Fields.Item("Proveedor").Value?.ToString() ?? "",
                        Marca = recordSet.Fields.Item("U_Marca").Value?.ToString() ?? "",
                        ItemName = recordSet.Fields.Item("ItemName").Value?.ToString() ?? "",
                        Ancho = recordSet.Fields.Item("U_Ancho").Value?.ToString() ?? "0",
                        Calibre = recordSet.Fields.Item("Calibre").Value?.ToString() ?? "",
                        Gramaje = recordSet.Fields.Item("U_Gramaje").Value?.ToString() ?? "0"
                    };

                    listaResultados.Add(item);
                    recordSet.MoveNext();
                }

                // Liberar el objeto COM de memoria (Buena práctica en SAP DI API)
                System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                recordSet = null;

                return Ok(listaResultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        [HttpGet("get/lotes-almacen/{itemCode}")]
        public IActionResult GetLotesPorAlmacen(string itemCode)
        {
            try
            {
                HANAConnection.sapConn();
                int retVal = HANAConnection.RetVal;
                SAPbobsCOM.Company oCompany = HANAConnection.OCompany;

                if (retVal != 0) return StatusCode(500, "Error SAP");

                string safeItemCode = itemCode.Replace("'", "''");

                string query = $@"
        SELECT 
            T0.""DistNumber"" AS ""codLote"", 
            T1.""WhsCode"" AS ""codAlmacen"",
            T3.""ItemCode"" AS ""codMaterial"",
            T3.""ItemName"" AS ""descMaterial"",
            T3.""U_Gramaje"" AS ""gramaje"",
            T2.""Quantity"" AS ""pesoInicial"",
            T1.""Quantity"" AS ""pesoActualLote"",
            T4.""OnHand""   AS ""stockTotalItem""
        FROM OBTN T0
        INNER JOIN OBTQ T1 ON T0.""ItemCode"" = T1.""ItemCode"" AND T0.""SysNumber"" = T1.""SysNumber""
        INNER JOIN ITL1 T2 ON T0.""ItemCode"" = T2.""ItemCode"" AND T0.""SysNumber"" = T2.""SysNumber""
        INNER JOIN OITM T3 ON T0.""ItemCode"" = T3.""ItemCode""
        INNER JOIN OITW T4 ON T0.""ItemCode"" = T4.""ItemCode"" AND T1.""WhsCode"" = T4.""WhsCode""
        WHERE T1.""WhsCode"" = 'IM01' 
          AND T0.""ItemCode"" = '{safeItemCode}' 
          AND T1.""Quantity"" > 0
          AND T2.""LogEntry"" = (
              SELECT MIN(X.""LogEntry"") 
              FROM ITL1 X 
              WHERE X.""ItemCode"" = T0.""ItemCode"" 
                AND X.""SysNumber"" = T0.""SysNumber""
         )";

                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSet.DoQuery(query);

                if (recordSet.RecordCount == 0) return NotFound($"Sin datos para: {itemCode}");

                var listaResultados = new List<LoteAlmacenResponse>();

                while (!recordSet.EoF)
                {
                    // Función auxiliar local para formatear números
                    string Fmt(object val) => Convert.ToDouble(val).ToString("0.##");

                    listaResultados.Add(new LoteAlmacenResponse
                    {
                        CodLote = recordSet.Fields.Item("codLote").Value.ToString(),
                        CodAlmacen = recordSet.Fields.Item("codAlmacen").Value.ToString(),
                        CodMaterial = recordSet.Fields.Item("codMaterial").Value.ToString(),
                        DescMaterial = recordSet.Fields.Item("descMaterial").Value.ToString(),
                        Gramaje = recordSet.Fields.Item("gramaje").Value?.ToString() ?? "",

                        PesoInicial = Fmt(recordSet.Fields.Item("pesoInicial").Value),
                        PesoActualLote = Fmt(recordSet.Fields.Item("pesoActualLote").Value),
                        StockTotalItem = Fmt(recordSet.Fields.Item("stockTotalItem").Value)
                    });

                    recordSet.MoveNext();
                }

                System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);
                return Ok(listaResultados);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno: {ex.Message}");
            }
        }


        // Clases de respuesta

        public class LoteInfoResponse
        {
            public string? DistNumber { get; set; }
            public string? ItemCode { get; set; }
            public string? Proveedor { get; set; }
            public string? Marca { get; set; }
            public string? ItemName { get; set; }
            public string? Ancho { get; set; }
            public string? Calibre { get; set; }
            public string? Gramaje { get; set; }
        }

        public class LoteAlmacenResponse
        {
            public string CodLote { get; set; }
            public string CodAlmacen { get; set; }
            public string CodMaterial { get; set; }
            public string DescMaterial { get; set; }
            public string Gramaje { get; set; }
            public string PesoInicial { get; set; }
            public string PesoActualLote { get; set; } // Nuevo
            public string StockTotalItem { get; set; } // Nuevo
        }
    }
}