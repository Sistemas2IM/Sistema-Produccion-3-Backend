using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Microsoft.Extensions.Caching.Memory;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.BusquedaProcesos;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.BusquedaTarjetas;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.Reportes;
using Sistema_Produccion_3_Backend.Models;
using System.Linq;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class tarjetaOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly IHttpClientFactory _httpClientFactory;
        private readonly ILogger<tarjetaOfController> _logger;
        private readonly IMemoryCache _memoryCache;

        // Replace the constructor signature to fix CS1552
        public tarjetaOfController(base_nuevaContext context, IMapper mapper, IHttpClientFactory httpClientFactory, IMemoryCache memoryCache, ILogger<tarjetaOfController> logger)
        {
            _context = context;
            _mapper = mapper;
            _httpClientFactory=httpClientFactory;
            _logger=logger;
            _memoryCache = memoryCache;
        }

        // GET: api/tarjetaOf
        /*[HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOf()
        {
            var tarjetaOf = await _context.tarjetaOf
                .OrderBy(p => p.posicion)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .ThenInclude(o => o.idEtiquetaNavigation)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
        }*/

        // GET: api/tarjetaOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOf()
        {
            var tarjetasOrdenadas = await _context.tarjetaOf
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .ThenInclude(o => o.idEtiquetaNavigation)
                .Where(t => t.archivada == false) // Incluye solo los registros donde archivada es false (excluye null y true)
                .OrderBy(t => t.idEstadoOf == 1 ? 0 : 1)  // Primero las de estado 1
                .Select(t => new
                {
                    Tarjeta = t,
                    Orden = t.idEstadoOf == 1 ? -t.oF : t.posicion  // -OF para descendente
                })
                .OrderBy(x => x.Tarjeta.idEstadoOf == 1 ? 0 : 1)
                .ThenBy(x => x.Orden)
                .Select(x => x.Tarjeta)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetasOrdenadas);

            return Ok(tarjetaOfDto);
        }

        [HttpGet("get/filtros")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOffiltros(
        [FromQuery] DateTime? fechaInicio = null,   // Parámetro opcional para la fecha de inicio del rango
        [FromQuery] DateTime? fechaFin = null,     // Parámetro opcional para la fecha de fin del rango
        [FromQuery] string cliente = null,         // Parámetro opcional para el cliente
        [FromQuery] string ejecutivo = null,       // Parámetro opcional para el ejecutivo
        [FromQuery] string articulo = null,        // Parámetro opcional para el artículo
        [FromQuery] int? of = null,                // Parámetro opcional para el número de OF
        [FromQuery] int? ov = null,                // Parámetro opcional para el número de OV
        [FromQuery] string? lineaNegocio = null,    // Parámetro opcional para la línea de negocio
        [FromQuery] string? idsEtiquetas = null,
        [FromQuery] bool mostrarArchivados = false)
        {
            // Consulta base
            var query = _context.tarjetaOf
                .Include(r => r.etiquetaOf)
                .ThenInclude(o => o.idEtiquetaNavigation)
                .Include(e => e.idEstadoOfNavigation)
                .AsQueryable();

            // Aplicar filtros condicionales
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                // Filtrar por rango de fechas (fechaVencimiento entre fechaInicio y fechaFin)
                query = query.Where(p => p.fechaVencimiento >= fechaInicio.Value && p.fechaVencimiento <= fechaFin.Value);
            }
            else if (fechaInicio.HasValue)
            {
                // Si solo se proporciona fechaInicio, filtrar desde esa fecha en adelante
                query = query.Where(p => p.fechaVencimiento >= fechaInicio.Value);
            }
            else if (fechaFin.HasValue)
            {
                // Si solo se proporciona fechaFin, filtrar hasta esa fecha
                query = query.Where(p => p.fechaVencimiento <= fechaFin.Value);
            }

            // Filtro "like" para cliente
            if (!string.IsNullOrEmpty(cliente))
            {
                query = query.Where(p => p.clienteOf.Contains(cliente));
            }

            // Filtro "like" para ejecutivo
            if (!string.IsNullOrEmpty(ejecutivo))
            {
                query = query.Where(p => p.vendedorOf.Contains(ejecutivo));
            }

            // Filtro "like" para artículo
            if (!string.IsNullOrEmpty(articulo))
            {
                query = query.Where(p => p.productoOf.Contains(articulo));
            }

            // Filtro "like" para línea de negocio
            if (!string.IsNullOrEmpty(lineaNegocio))
            {
                query = query.Where(p => p.lineaDeNegocio.Contains(lineaNegocio));
            }

            // Filtro exacto para OF
            if (of.HasValue)
            {
                query = query.Where(p => p.oF == of.Value);
            }

            // Filtro exacto para OV
            if (ov.HasValue)
            {
                query = query.Where(p => p.oV == ov.Value);
            }

            // Filtro por IDs de etiquetas
            if (!string.IsNullOrEmpty(idsEtiquetas))
            {
                // Convertir la cadena de IDs separados por comas en una lista de enteros
                var idsEtiquetasLista = idsEtiquetas.Split(',')
                    .Select(id => int.Parse(id))
                    .ToList();

                // Filtrar tarjetas que tengan al menos una de las etiquetas especificadas
                query = query.Where(p => p.etiquetaOf
                    .Any(etiquetaOf => idsEtiquetasLista.Contains((int)etiquetaOf.idEtiqueta)));
            }

            // ✅ Aplicar el filtro solo si NO se quieren mostrar los archivados
            if (!mostrarArchivados)
                query = query.Where(p => p.archivada == false);

            // Ejecutar la consulta y mapear a DTO
            var tarjetaOf = await query
                .ToListAsync();
            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
        }

        [HttpGet("get/sugerencias")]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GetSugerencias(
        [FromQuery] string? cliente = null,   // Parámetro opcional para el cliente
        [FromQuery] string? ejecutivo = null, // Parámetro opcional para el ejecutivo
        [FromQuery] string? articulo = null)  // Parámetro opcional para el artículo
        {
            // Diccionario para almacenar las sugerencias
            var sugerencias = new Dictionary<string, List<string>>();

            // Consulta base
            var query = _context.tarjetaOf.AsQueryable();

            // Sugerencias para cliente
            if (!string.IsNullOrEmpty(cliente))
            {
                var clientesSugeridos = await query
                    .Where(p => p.clienteOf.Contains(cliente))
                    .Select(p => p.clienteOf)
                    .Distinct()
                    .Take(10) // Limitar el número de sugerencias
                    .ToListAsync();

                sugerencias.Add("clientes", clientesSugeridos);
            }

            // Sugerencias para ejecutivo
            if (!string.IsNullOrEmpty(ejecutivo))
            {
                var ejecutivosSugeridos = await query
                    .Where(p => p.vendedorOf.Contains(ejecutivo))
                    .Select(p => p.vendedorOf)
                    .Distinct()
                    .Take(10) // Limitar el número de sugerencias
                    .ToListAsync();

                sugerencias.Add("ejecutivos", ejecutivosSugeridos);
            }

            // Sugerencias para artículo
            if (!string.IsNullOrEmpty(articulo))
            {
                var articulosSugeridos = await query
                    .Where(p => p.productoOf.Contains(articulo))
                    .Select(p => p.productoOf)
                    .Distinct()
                    .Take(10) // Limitar el número de sugerencias
                    .ToListAsync();

                sugerencias.Add("articulos", articulosSugeridos);
            }

            // Devolver las sugerencias
            return Ok(sugerencias);
        }

        [HttpGet("get/catalogo")]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GettarjetaOfCatalogo()
        {
            // Diccionario para almacenar los catálogos
            var catalogos = new Dictionary<string, List<string>>();

            // Obtener clientes únicos
            var clientes = await _context.tarjetaOf
                .Select(p => p.clienteOf)
                .Distinct()
                .OrderBy(c => c) // Ordenar alfabéticamente
                .ToListAsync();

            catalogos.Add("clientes", clientes);

            // Obtener ejecutivos únicos
            var ejecutivos = await _context.tarjetaOf
                .Select(p => p.vendedorOf)
                .Distinct()
                .OrderBy(e => e) // Ordenar alfabéticamente
                .ToListAsync();

            catalogos.Add("ejecutivos", ejecutivos);

            // Obtener artículos únicos
            var articulos = await _context.tarjetaOf
                .Select(p => p.productoOf)
                .Distinct()
                .OrderBy(a => a) // Ordenar alfabéticamente
                .ToListAsync();

            catalogos.Add("articulos", articulos);

            // Devolver los catálogos
            return Ok(catalogos);
        }

        // GET: api/tarjetaOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TarjetaOfDto>> GettarjetaOf(int id)
        {
            var tarjetaOf = await _context.tarjetaOf
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .FirstOrDefaultAsync(u => u.oF == id);
            var tarjetaOfDto = _mapper.Map<TarjetaOfDto>(tarjetaOf);
            
            if (tarjetaOfDto == null)
            {
                return Ok("");
            }

            return Ok(tarjetaOfDto);
        }

        // PUT: api/tarjetaOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("put/{id}")]
        //public async Task<IActionResult> PuttarjetaOf(int id, UpdateTarjetaOfDto updateTarjetaOf)
        //{
        //    var tarjetaOf = await _context.tarjetaOf.FindAsync(id);

        //    if (tarjetaOf == null)
        //    {
        //        return NotFound("No se encontro la tarjeta con el id: " + id);
        //    }

        //    _mapper.Map(updateTarjetaOf, tarjetaOf);
        //    _context.Entry(tarjetaOf).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!tarjetaOfExists(id))
        //        {
        //            return NotFound("No se encontro la tarjeta con el id: " + id);
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return Ok(updateTarjetaOf);
        //}

        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttarjetaOf(int id, UpdateTarjetaOfDto updateTarjetaOf)
        {
            var tarjetaOf = await _context.tarjetaOf.FirstOrDefaultAsync(t => t.oF == id);

            if (tarjetaOf == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            // 1. Capturamos el estado ANTES de cualquier modificación
            int idEstadoAnterior = (int)tarjetaOf.idEstadoOf;

            // 2. Verificamos si el DTO realmente trae un nuevo estado
            //    (Asumiendo que '0' no es un ID de estado válido y significa "no proporcionado")
            bool idEstadoVinoEnDto = updateTarjetaOf.idEstadoOf != 0;

            // 3. Mapeamos los nuevos valores del DTO a la entidad
            _mapper.Map(updateTarjetaOf, tarjetaOf);

            // 4. ¡CORRECCIÓN CRÍTICA!
            // Si el estado no venía en el DTO (es 0), revertimos el cambio
            if (!idEstadoVinoEnDto)
            {
                tarjetaOf.idEstadoOf = idEstadoAnterior;
            }

            _context.Entry(tarjetaOf).State = EntityState.Modified;

            // 5. Guardamos los cambios en la Base de Datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // ... tu lógica de concurrencia ...
                if (!tarjetaOfExists(id))
                {
                    return NotFound("No se encontro la tarjeta con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            // === INICIO DE LA INTEGRACIÓN CONDICIONAL (MODIFICADO) ===

            // 6. Verificamos si el estado se proporcionó Y si es diferente al anterior
            if (idEstadoVinoEnDto && idEstadoAnterior != updateTarjetaOf.idEstadoOf)
            {
                _logger.LogInformation($"El estado de la OF {id} cambió de {idEstadoAnterior} a {updateTarjetaOf.idEstadoOf}. Enviando notificación.");

                try
                {
                    // 7. Cargamos la información del NUEVO estado
                    await _context.Entry(tarjetaOf).Reference(t => t.idEstadoOfNavigation).LoadAsync();

                    // 8. Obtenemos el nombre del estado (¡Ajusta 'nombreEstado'!)
                    string nombreEstado = tarjetaOf.idEstadoOfNavigation?.nombreEstado ?? $"ID: {updateTarjetaOf.idEstadoOf}";

                    // 9. (MODIFICADO) BUSCAR EL ID DEL HILO EN CACHÉ
                    string cacheKey = $"GoogleChatThread_OF_{id}";
                    _memoryCache.TryGetValue(cacheKey, out string? currentThreadId);

                    // 10. (NUEVA LÓGICA) CONSTRUIR EL PAYLOAD ADECUADO
                    object finalPayload;

                    if (string.IsNullOrEmpty(currentThreadId))
                    {
                        // 10a. CASO 1: HILO NUEVO -> ENVIAR TARJETA
                        // (Construimos la tarjeta que ya tenías)
                        finalPayload = new
                        {
                            cardsV2 = new[] {
                        new {
                            cardId = "estadoOfCard",
                            card = new {
                                header = new {
                                    title = "🔄 Cambio de Estado",
                                    subtitle = $"Tarjeta/OF ID: {id}",
                                    imageUrl = "https://i.imgur.com/v8R2yK4.png"
                                },
                                sections = new object[] { // <-- Usando object[]
                                    new { // Sección de texto
                                        widgets = new object[] {
                                            new {
                                                decoratedText = new {
                                                    topLabel = "Nuevo Estado",
                                                    text = nombreEstado,
                                                    startIcon = new { knownIcon = "BOOKMARK" }
                                                }
                                            }
                                        }
                                    },
                                    new { // Sección de botón
                                        widgets = new object[] {
                                            new {
                                                buttonList = new {
                                                    buttons = new[] {
                                                        new {
                                                            text = "Ir al Tablero de OF",
                                                            onClick = new {
                                                                openLink = new {
                                                                    url = "http://nexo.it:8080/main/inicio"
                                                                }
                                                            }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }
                            }
                        }
                    }
                        };
                    }
                    else
                    {
                        // 10b. CASO 2: HILO EXISTENTE -> ENVIAR TEXTO

                        // Construimos el mensaje de texto simple
                        var messageBuilder = new System.Text.StringBuilder();
                        messageBuilder.AppendLine($"🔄 *Actualización de Estado*");
                        messageBuilder.AppendLine($"La Tarjeta/OF *ID: {id}* cambió al estado: *{nombreEstado}*.");

                        // Construimos el payload de texto CON el hilo
                        finalPayload = new
                        {
                            text = messageBuilder.ToString(),
                            thread = new { name = currentThreadId }
                        };
                    }

                    // 11. ENVIAR AL WEBHOOK
                    // (La función SendToGoogleChat no cambia, felizmente acepta cualquier 'object')
                    string? newThreadId = await SendToGoogleChat(
                        finalPayload,
                        currentThreadId // Lo pasamos para saber si debemos capturar la respuesta
                    );

                    // 12. GUARDAR EL NUEVO ID DE HILO EN CACHÉ
                    if (!string.IsNullOrEmpty(newThreadId))
                    {
                        var cacheEntryOptions = new MemoryCacheEntryOptions()
                            .SetSlidingExpiration(TimeSpan.FromDays(7));
                        _memoryCache.Set(cacheKey, newThreadId, cacheEntryOptions);
                    }
                }
                catch (Exception chatEx)
                {
                    _logger.LogError(chatEx, "Error al preparar o enviar la notificación de CAMBIO DE ESTADO a Google Chat.");
                }
            }
            // === FIN DE LA INTEGRACIÓN ===

            return Ok(updateTarjetaOf);
        }

        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateTarjetas([FromBody] BatchUpdatePosicionTarjetaOfDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.Tarjetas == null || !batchUpdateDto.Tarjetas.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.Tarjetas.Select(t => t.oF).ToList();

            // Obtener todas las tarjetas relacionadas
            var tarjetas = await _context.tarjetaOf.Where(t => ids.Contains(t.oF)).ToListAsync();

            if (!tarjetas.Any())
            {
                return NotFound("No se encontraron tarjetas para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.Tarjetas)
            {
                var tarjeta = tarjetas.FirstOrDefault(t => t.oF == dto.oF);
                if (tarjeta != null)
                {
                    // Actualizar la posición si es proporcionada
                    if (dto.posicion.HasValue)
                    {
                        tarjeta.posicion = dto.posicion.Value;
                    }

                    _context.Entry(tarjeta).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar las tarjetas.");
            }

            return Ok("Actualización realizada correctamente.");
        }


        // POST: api/tarjetaOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tarjetaOf>> PosttarjetaOf(AddTarjetaOfDto addTarjetaOf)
        {
            var tarjetaOf = _mapper.Map<tarjetaOf>(addTarjetaOf);

            _context.tarjetaOf.Add(tarjetaOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarjetaOf", new { id = tarjetaOf.oF }, tarjetaOf);
        }


        // DASHBOARD =========================================================================

        /*// GET: api/tarjetaOf/lineaNegocio
        [HttpGet("get/lineaNegocio")]
        public async Task<ActionResult<IEnumerable<object>>> GetTarjetaOfCountByLineaNegocio()
        {
            var tarjetaOf = await _context.tarjetaOf.ToListAsync();

            // Agrupar las tarjetas por línea de negocio y contar cada grupo
            var tarjetaOfGrouped = tarjetaOf.GroupBy(t => t.lineaDeNegocio)
                                            .Select(group => new {
                                                LineaNegocio = group.Key,
                                                CantidadTarjetas = group.Count()
                                            });

            return Ok(tarjetaOfGrouped);
        }

        // GET: api/tarjetaOf/vencidas
        [HttpGet("get/vencidas")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOfVencidas()
        {
            var tarjetaOf = await _context.tarjetaOf
                .Where(d => d.fechaVencimiento < DateTime.Today)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
        }

        // GET: api/tarjetaOf/cerradasHoy
        [HttpGet("get/cerradasHoy")]
        public async Task<ActionResult<int>> GetTarjetaOfCerradasHoy()
        {
            var today = DateTime.Now.Date;

            // Contar las tarjetas que se cerraron hoy y tienen el estado cerrado (idEstadoOf == 4)
            var cantidadTarjetasCerradasHoy = await _context.tarjetaOf
                .Where(d => d.finalizacion.HasValue && d.finalizacion.Value.Date == today && d.idEstadoOf == 4)
                .CountAsync();

            return Ok(new { CantidadTarjetasCerradasHoy = cantidadTarjetasCerradasHoy });
        }*/


        // - REPORTES - =======================================================================================

        // GET: api/tarjetaOf
        [HttpGet("get/PM")]
        public async Task<ActionResult<IEnumerable<ReportePMTarjetaOf>>> GettarjetaOfPM()
        {
            var tarjetas = await _context.tarjetaOf
                .OrderBy(p => p.vendedorOf)
                .ToListAsync();

            var tarjetasDto = _mapper.Map<List<ReportePMTarjetaOf>>(tarjetas);

            // Calcular días en planta y días a la entrega
            foreach (var tarjeta in tarjetasDto)
            {
                if (tarjeta.fechaCreacion.HasValue)
                {
                    tarjeta.diasEnPlanta = (DateTime.Now - tarjeta.fechaCreacion.Value).Days;
                }

                if (tarjeta.fechaVencimiento.HasValue)
                {
                    tarjeta.diasALaEntrega = (tarjeta.fechaVencimiento.Value - DateTime.Now).Days;
                }
            }

            return Ok(tarjetasDto);
        }


        private bool tarjetaOfExists(int id)
        {
            return _context.tarjetaOf.Any(e => e.oF == id);
        }

        /// <summary>
        /// Envía un payload (Tarjeta o Texto) a Google Chat, manejando hilos.
        /// </summary>
        /// <param name="messagePayload">El objeto completo a serializar (ej. { text: "..." } o { cardsV2: [...] }).</param>
        /// <param name="currentThreadId">El ID del hilo actual (si existe). Se usa para saber si capturar la respuesta.</param>
        /// <returns>El ID del hilo si se creó uno nuevo; null si solo se respondió.</returns>
        private async Task<string?> SendToGoogleChat(object messagePayload, string? currentThreadId)
        {
            // ❗️ IMPORTANTE: Mueve esta URL a tu appsettings.json
            var baseUrl = "https://chat.googleapis.com/v1/spaces/AAQAWq4gutM/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=OWw9jgq6-DXEFzMCkl3HI4tZxjN1clp9_p-6xPfdRjM";

            var client = _httpClientFactory.CreateClient();
            string postUrl = baseUrl;

            // Si ya estamos en un hilo, añadimos el parámetro de respuesta
            if (!string.IsNullOrEmpty(currentThreadId))
            {
                postUrl = $"{baseUrl}&messageReplyOption=REPLY_MESSAGE_FALLBACK_TO_NEW_THREAD";
            }

            try
            {
                // Serializamos el payload que nos pasó el controlador
                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(messagePayload);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                // Enviar la petición
                var response = await client.PostAsync(postUrl, content);

                if (!response.IsSuccessStatusCode)
                {
                    var errorBody = await response.Content.ReadAsStringAsync();
                    _logger.LogError($"Error de Google Chat API: {response.StatusCode} - {errorBody}");
                    return null;
                }

                // 3. SI CREAMOS UN HILO NUEVO (porque currentThreadId era null), 
                //    CAPTURAMOS Y DEVOLVEMOS EL ID
                if (string.IsNullOrEmpty(currentThreadId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
                    // Usamos las clases auxiliares para leer la respuesta
                    var chatResponse = System.Text.Json.JsonSerializer.Deserialize<GoogleChatResponse>(responseBody);

                    var newThreadId = chatResponse?.Thread?.Name;

                    if (newThreadId != null)
                    {
                        _logger.LogInformation($"Nuevo hilo de Google Chat creado: {newThreadId}");
                        return newThreadId; // 👈 Devolvemos el ID
                    }
                }

                // Si estábamos en un hilo, no devolvemos nada.
                _logger.LogInformation($"Respuesta enviada al hilo: {currentThreadId}");
                return null;
            }
            catch (Exception ex)
            {
                // Solo registrar el error, no fallar la solicitud principal
                _logger.LogError(ex, "Error al enviar notificación a Google Chat.");
                return null;
            }
        }

        /// <summary>
        /// Clase auxiliar para deserializar la respuesta de Google Chat
        /// </summary>
        private class GoogleChatResponse
        {
            // Mapea la propiedad "thread" del JSON
            [System.Text.Json.Serialization.JsonPropertyName("thread")]
            public GoogleChatThread? Thread { get; set; }
        }

        private class GoogleChatThread
        {
            // Mapea la propiedad "name" del JSON
            [System.Text.Json.Serialization.JsonPropertyName("name")]
            public string? Name { get; set; }
        }
    }
}
