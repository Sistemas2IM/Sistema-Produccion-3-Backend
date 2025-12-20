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
    }
}