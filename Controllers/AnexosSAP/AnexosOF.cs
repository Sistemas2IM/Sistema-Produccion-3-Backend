using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
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
        [HttpGet("get/files/{of}")]
        public async Task<IActionResult> GetAnexosFiles(int of)
        {
            try
            {
                // Conectar a HANA
                HANAConnection.sapConn();
                int retVal = HANAConnection.RetVal;
                SAPbobsCOM.Company oCompany = HANAConnection.OCompany;

                if (retVal != 0)
                {
                    return StatusCode(500, "Error al conectar con SAP HANA.");
                }

                // Consulta SQL para obtener la ruta de los anexos
                string query = @"
                SELECT 
                    T1.""trgtPath"" || '\' || T1.""FileName"" || '.' || T1.""FileExt"" AS ""AnexoOf"",
                    T1.""FileName"" AS ""NombreArchivo""
                FROM OWOR T0 
                LEFT JOIN ATC1 T1 ON T0.""AtcEntry"" = T1.""AbsEntry""
                WHERE T0.""DocNum"" = ?";

                // Ejecutar la consulta
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSet.DoQuery(query.Replace("?", of.ToString()));

                // Verificar si hay resultados
                if (recordSet.RecordCount == 0)
                {
                    return NotFound($"No se encontraron anexos para la OF: {of}");
                }

                // Crear una lista para almacenar los archivos
                var archivosAdjuntos = new List<FileContentResult>();

                while (!recordSet.EoF)
                {
                    string filePath = recordSet.Fields.Item("AnexoOf").Value.ToString();
                    string fileName = recordSet.Fields.Item("NombreArchivo").Value.ToString();

                    Console.WriteLine($"Ruta obtenida desde SAP HANA: {filePath}");

                    try
                    {
                        // Usar FileStream para manejar permisos explícitos
                        using (var fileStream = new FileStream(filePath, FileMode.Open, FileAccess.Read))
                        {
                            byte[] fileBytes = new byte[fileStream.Length];
                            await fileStream.ReadExactlyAsync(fileBytes);
                            string contentType = GetContentType(filePath);

                            // Agregar el archivo a la lista
                            archivosAdjuntos.Add(File(fileBytes, contentType, fileName));
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al leer el archivo {filePath}: {ex.Message}");
                    }

                    recordSet.MoveNext();
                }

                if (archivosAdjuntos.Count == 0)
                {
                    return NotFound("No se pudieron cargar los archivos anexos.");
                }

                // Retornar la lista de archivos
                return Ok(archivosAdjuntos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Método auxiliar para obtener el Content-Type del archivo
        private string GetContentType(string path)
        {
            var provider = new FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream"; // Tipo genérico
            }
            return contentType;
        }
    }
}
