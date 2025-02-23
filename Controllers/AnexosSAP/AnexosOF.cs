using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SAPbobsCOM;
using Sistema_Produccion_3_Backend.DTO.Bitacora;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Sistema_Produccion_3_Backend.Controllers.AnexosSAP
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnexosOF : ControllerBase
    {
        // GET: api/anexosof/get/5
        [HttpGet("get/{of}")]
        public async Task<ActionResult<string>> GetAnexo(int of)
        {
            try
            {
                // Conexion a HANA -------------------------------------
                HANAConnection.sapConn();
                int retVal = HANAConnection.RetVal;
                SAPbobsCOM.Company oCompany = HANAConnection.OCompany;

                if (retVal != 0)
                {
                    return StatusCode(500, "Error al conectar con SAP HANA.");
                }

                // Crear la consulta SQL con el parámetro OF
                string query = @"
                    SELECT 
                        T1.""trgtPath"" || '\' || T1.""FileName"" || '.' || T1.""FileExt"" AS ""AnexoOf""
                    FROM OWOR T0 
                    LEFT JOIN ATC1 T1 ON T0.""AtcEntry"" = T1.""AbsEntry""
                    WHERE T0.""DocNum"" = ?";

                // Ejecutar la consulta
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSet.DoQuery(query.Replace("?", of.ToString()));

                // Verificar si hay resultados
                if (recordSet.RecordCount == 0)
                {
                    return NotFound("No se encontraron anexos para la OF especificada.");
                }

                // Obtener el resultado
                recordSet.MoveFirst();
                string anexoOf = recordSet.Fields.Item("AnexoOf").Value.ToString();

                // Liberar recursos
                _ = System.Runtime.InteropServices.Marshal.ReleaseComObject(recordSet);

                // Devolver el resultado como texto plano
                return Content(anexoOf, "text/plain");
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
