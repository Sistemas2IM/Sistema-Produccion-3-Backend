using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.StaticFiles;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using SAPbobsCOM;
using Sistema_Produccion_3_Backend.DTO.Bitacora;
using Sistema_Produccion_3_Backend.Services.SAP.HANA;
using static System.Runtime.InteropServices.JavaScript.JSType;
using JsonSerializer = System.Text.Json.JsonSerializer;


namespace Sistema_Produccion_3_Backend.Controllers.AnexosSAP
{
    [Route("api/[controller]")]
    [ApiController]
    public class AnexosOF : ControllerBase
    {
        // GET: api/anexosof/get/5
        [HttpGet("get/files/{of}/{nombreRol}")]
        //[HttpGet("get/files/{of}")]
        public async Task<IActionResult> GetAnexosFiles(int of, string nombreRol)
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
                string query = $@"
                SELECT 
                    T1.""trgtPath"" || '\' || T1.""FileName"" || '.' || T1.""FileExt"" AS ""AnexoOf"",
                    T1.""FileName"" AS ""NombreArchivo"",
                    T1.""Date"" AS ""FechaAnexo"",
                    T1.""U_permisoAnexo"" AS ""PermisoAnexo""
                FROM OWOR T0 
                LEFT JOIN ATC1 T1 ON T0.""AtcEntry"" = T1.""AbsEntry""
                WHERE T0.""DocNum"" = '{of}'";

                // Ejecutar la consulta
                var recordSet = (Recordset)oCompany.GetBusinessObject(BoObjectTypes.BoRecordset);
                recordSet.DoQuery(query);

                // Verificar si hay resultados
                if (recordSet.RecordCount == 0)
                {
                    return NotFound($"No se encontraron anexos para la OF: {of}");
                }

                // Crear una lista para almacenar los archivos
                var archivosAdjuntos = new List<AnexoResponse>();

                while (!recordSet.EoF)
                {
                    string filePath = recordSet.Fields.Item("AnexoOf").Value.ToString();
                    string fileName = recordSet.Fields.Item("NombreArchivo").Value.ToString();
                    string fileDate = recordSet.Fields.Item("FechaAnexo").Value.ToString();
                    string permisoAnexo = recordSet.Fields.Item("PermisoAnexo").Value.ToString();

                    // Validar si el rol del usuario tiene permiso para ver el anexo
                    if (TienePermiso(nombreRol, permisoAnexo))
                    {
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
                                archivosAdjuntos.Add(new AnexoResponse
                                {
                                    FileBytes = fileBytes,
                                    ContentType = contentType,
                                    FileName = fileName,
                                    FileDate = fileDate
                                });
                            }
                        }
                        catch (Exception ex)
                        {
                            Console.WriteLine($"Error al leer el archivo {filePath}: {ex.Message}");
                        }
                    }
                    else
                    {
                        Console.WriteLine($"El usuario con rol {nombreRol} no tiene permiso para ver el archivo {fileName}.");
                    }

                    recordSet.MoveNext();
                }

                if (archivosAdjuntos.Count == 0)
                {
                    return NotFound("No se pudieron cargar los archivos anexos o no tiene permisos para verlos.");
                }

                // Retornar la lista de archivos
                return Ok(archivosAdjuntos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // Método para validar si el rol tiene permiso
        private bool TienePermiso(string nombreRol, string permisoAnexo)
        {
            // Si el permiso es "ninguno", nadie tiene acceso
            if (permisoAnexo == "ninguno")
            {
                return false;
            }

            //permisoAnexo = "Administrador";

            // Mapear los roles a los grupos de permisos
            var gruposPermisos = new Dictionary<string, List<string>>
    {
        { "comercial", new List<string> { "Ventas", "AsistenteVenta", "Cotizaciones", "JefeVentas", "Planificación", "Administrador" } },
        { "producción", new List<string> { "Operador", "Planificación", "Diseñador", "Calidad", "Administrador" } },
        { "calidad", new List<string> { "Calidad", "Administrador" } },
        { "financiero", new List<string> { "Gerencia", "Digitador", "Administrador" } }
    };

            // Verificar si el rol del usuario está en el grupo de permisos
            if (gruposPermisos.ContainsKey(permisoAnexo))
            {
                return gruposPermisos[permisoAnexo].Contains(nombreRol);
            }

            return false;
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

        public class AnexoResponse
        {
            public byte[]? FileBytes { get; set; }
            public string? ContentType { get; set; }
            public string? FileName { get; set; }
            public string? FileDate { get; set; }
        }
    }
}
