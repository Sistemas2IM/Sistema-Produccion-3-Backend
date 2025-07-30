using AutoMapper;
using Google.Apis.Auth.OAuth2;
using Google.Apis.Drive.v3;
using Google.Apis.Services;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.AnexosNEXO;
using Sistema_Produccion_3_Backend.Models;
using static Sistema_Produccion_3_Backend.Controllers.AnexosSAP.AnexosOF;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.AnexosNEXO
{
    [Route("api/[controller]")]
    [ApiController]
    public class anexoNEXOController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly IWebHostEnvironment _env;

        public anexoNEXOController(base_nuevaContext context, IMapper mapper, IWebHostEnvironment env)
        {
            _context = context;
            _mapper = mapper;
            _env=env;
        }

        [HttpPost("subir")]
        public async Task<IActionResult> SubirAnexo([FromForm] AddAnexoNEXODto request)
        {
            if (request.archivo == null || request.archivo.Length == 0)
                return BadRequest("Archivo no válido");

            try
            {
                var nombreArchivo = Path.GetFileName(request.archivo.FileName);
                var fechaSubida = DateTime.Now;

                var rutaBase = @"\\192.168.2.160\sap\nexo";
                var rutaDestinoCarpeta = Path.Combine(rutaBase, request.TipoEntidad, request.EntidadId.ToString());

                Directory.CreateDirectory(rutaDestinoCarpeta);

                var rutaArchivo = Path.Combine(rutaDestinoCarpeta, nombreArchivo);

                using (var stream = new FileStream(rutaArchivo, FileMode.Create))
                {
                    await request.archivo.CopyToAsync(stream);
                }

                // Guardar en base de datos
                var anexo = new anexos_NEXO
                {
                    TipoEntidad = request.TipoEntidad,
                    EntidadId = request.EntidadId,
                    NombreArchivo = nombreArchivo,
                    RutaArchivo = rutaArchivo,
                    FechaSubida = fechaSubida,
                    SubidoPor = request.SubidoPor,
                    Descripcion = request.Descripcion
                };

                _context.anexos_NEXO.Add(anexo);
                await _context.SaveChangesAsync();

                return Ok(new { mensaje = "Archivo subido correctamente", anexoId = anexo.Id });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al subir archivo: {ex.Message}");
            }
        }

        [HttpGet("listar")]
        public async Task<IActionResult> ObtenerAnexos([FromQuery] string tipoEntidad, [FromQuery] int entidadId)
        {
            try
            {
                var anexos = await _context.anexos_NEXO
                    .Where(a => a.TipoEntidad == tipoEntidad && a.EntidadId == entidadId)
                    .Select(a => new
                    {
                        a.Id,
                        a.NombreArchivo,
                        a.RutaArchivo,
                        a.FechaSubida,
                        a.SubidoPor,
                        a.Descripcion
                    })
                    .ToListAsync();

                if (anexos == null || anexos.Count == 0)
                {
                    return NotFound("No se encontraron anexos para esta entidad.");
                }

                return Ok(anexos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener anexos: {ex.Message}");
            }
        }

        [HttpGet("get/files")]
        public async Task<IActionResult> GetAnexosFiles([FromQuery] string tipoEntidad, [FromQuery] int entidadId)
        {
            try
            {
                var anexos = await _context.anexos_NEXO
                    .Where(a => a.TipoEntidad == tipoEntidad && a.EntidadId == entidadId)
                    .ToListAsync();

                if (anexos == null || anexos.Count == 0)
                    return NotFound("No se encontraron anexos para esta entidad.");

                var archivosAdjuntos = new List<AnexoResponse>();

                foreach (var anexo in anexos)
                {
                    try
                    {
                        using (var fileStream = new FileStream(anexo.RutaArchivo, FileMode.Open, FileAccess.Read))
                        {
                            byte[] fileBytes = new byte[fileStream.Length];
                            await fileStream.ReadExactlyAsync(fileBytes);
                            string contentType = GetContentType(anexo.RutaArchivo);

                            archivosAdjuntos.Add(new AnexoResponse
                            {
                                FileBytes = fileBytes,
                                ContentType = contentType,
                                FileName = anexo.NombreArchivo,
                                FileDate = anexo.FechaSubida.ToString("yyyy-MM-dd HH:mm")
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al leer archivo {anexo.RutaArchivo}: {ex.Message}");
                    }
                }

                if (archivosAdjuntos.Count == 0)
                    return NotFound("No se pudieron cargar los archivos anexos.");

                return Ok(archivosAdjuntos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener archivos: {ex.Message}");
            }
        }

        // =====================================================================================================

        [HttpPost("subir-drive")]
        public async Task<IActionResult> SubirAnexoDrive([FromForm] AddAnexoNEXODto request)
        {
            if (request.archivo == null || request.archivo.Length == 0)
                return BadRequest("Archivo no válido");

            try
            {
                var nombreArchivo = Path.GetFileName(request.archivo.FileName);
                var fechaSubida = DateTime.Now;

                var driveService = GetDriveService();

                // Paso 1: Asegurar la ruta completa en Drive
                var rootFolderId = "11ktAUrkx9G-VHNrDl0P7W5bo0GrxezDt"; // <-- ID fijo de la carpeta "Anexos" ya creada en Drive
                var tipoFolderId = await GetOrCreateFolderAsync(driveService, request.TipoEntidad, rootFolderId);
                var entidadFolderId = await GetOrCreateFolderAsync(driveService, request.EntidadId.ToString(), tipoFolderId);

                // Paso 2: Subir el archivo a la carpeta final
                var fileMetadata = new Google.Apis.Drive.v3.Data.File
                {
                    Name = nombreArchivo,
                    Parents = new List<string> { entidadFolderId }
                };

                using var stream = request.archivo.OpenReadStream();
                var uploadRequest = driveService.Files.Create(fileMetadata, stream, request.archivo.ContentType);
                uploadRequest.Fields = "id, webViewLink";
                uploadRequest.SupportsAllDrives = true;
                var uploadResult = await uploadRequest.UploadAsync();

                if (uploadResult.Status != Google.Apis.Upload.UploadStatus.Completed)
                    return StatusCode(500, "Error al subir archivo a Google Drive");

                var file = uploadRequest.ResponseBody;

                // Paso 3: Guardar en BD
                var anexo = new anexos_NEXO
                {
                    TipoEntidad = request.TipoEntidad,
                    EntidadId = request.EntidadId,
                    NombreArchivo = nombreArchivo,
                    RutaArchivo = file.Id,
                    FechaSubida = fechaSubida,
                    SubidoPor = request.SubidoPor,
                    Descripcion = request.Descripcion
                };

                _context.anexos_NEXO.Add(anexo);
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    mensaje = "Archivo subido correctamente a Google Drive",
                    driveFileId = file.Id,
                    link = file.WebViewLink
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error: {ex.Message}");
            }
        }

        private async Task<string> GetOrCreateFolderAsync(DriveService service, string folderName, string parentId)
        {
            var listRequest = service.Files.List();
            listRequest.Q = $"mimeType='application/vnd.google-apps.folder' and trashed=false and name='{folderName}' and '{parentId}' in parents";
            listRequest.Fields = "files(id, name)";
            listRequest.SupportsAllDrives = true;
            listRequest.IncludeItemsFromAllDrives = true;

            var folders = await listRequest.ExecuteAsync();
            var folder = folders.Files.FirstOrDefault();

            if (folder != null)
                return folder.Id;

            var fileMetadata = new Google.Apis.Drive.v3.Data.File
            {
                Name = folderName,
                MimeType = "application/vnd.google-apps.folder",
                Parents = new List<string> { parentId }
            };

            var createRequest = service.Files.Create(fileMetadata);
            createRequest.Fields = "id";
            createRequest.SupportsAllDrives = true;

            var createdFolder = await createRequest.ExecuteAsync();
            return createdFolder.Id;
        }

        [HttpGet("get/filesDrive")]
        public async Task<IActionResult> GetAnexosFilesDrive([FromQuery] string tipoEntidad, [FromQuery] int entidadId)
        {
            try
            {
                var anexos = await _context.anexos_NEXO
                    .Where(a => a.TipoEntidad == tipoEntidad && a.EntidadId == entidadId)
                    .ToListAsync();

                if (anexos == null || anexos.Count == 0)
                    return NotFound("No se encontraron anexos para esta entidad.");

                var archivosAdjuntos = new List<AnexoResponse>();
                var driveService = GetDriveService();

                foreach (var anexo in anexos)
                {
                    try
                    {
                        var request = driveService.Files.Get(anexo.RutaArchivo);
                        request.SupportsAllDrives = true;

                        // Obtener metadatos (para mimeType y nombre real del archivo)
                        var fileMetadata = await request.ExecuteAsync();

                        using (var stream = new MemoryStream())
                        {
                            // Descargar contenido
                            await request.DownloadAsync(stream);
                            stream.Position = 0;

                            archivosAdjuntos.Add(new AnexoResponse
                            {
                                FileBytes = stream.ToArray(),
                                ContentType = fileMetadata.MimeType,
                                FileName = fileMetadata.Name,
                                FileDate = anexo.FechaSubida.ToString("yyyy-MM-dd HH:mm")
                            });
                        }
                    }
                    catch (Exception ex)
                    {
                        Console.WriteLine($"Error al obtener archivo {anexo.RutaArchivo}: {ex.Message}");
                    }
                }

                if (archivosAdjuntos.Count == 0)
                    return NotFound("No se pudieron cargar los archivos anexos.");

                return Ok(archivosAdjuntos);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error al obtener archivos: {ex.Message}");
            }
        }



        private DriveService GetDriveService()
        {
            var credentialPath = Path.Combine(_env.ContentRootPath, "Services", "DriveAuth", "anexosnexo-48439f7b5586.json");
            GoogleCredential credential;

            using (var stream = new FileStream(credentialPath, FileMode.Open, FileAccess.Read))
            {
                credential = GoogleCredential.FromStream(stream).CreateScoped(DriveService.ScopeConstants.Drive);
            }

            return new DriveService(new BaseClientService.Initializer
            {
                HttpClientInitializer = credential,
                ApplicationName = "AnexosNEXO"
            });
        }


        // ====================================================================================================

        private string GetContentType(string path)
        {
            var provider = new Microsoft.AspNetCore.StaticFiles.FileExtensionContentTypeProvider();
            if (!provider.TryGetContentType(path, out var contentType))
            {
                contentType = "application/octet-stream";
            }
            return contentType;
        }



    }
}
