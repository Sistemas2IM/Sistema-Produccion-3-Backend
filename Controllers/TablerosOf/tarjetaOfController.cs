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
                .AsNoTracking() // 🚀 Libera la memoria de EF Core (clave para endpoints GET)
                .AsSplitQuery() // 🚀 Evita la "explosión cartesiana" al traer las listas de etiquetaOf y ffeTiempos
                .Where(t => t.archivada == false)
                // 🚀 Ordenamiento directo: EF Core traducirá esto a un CASE en SQL sin instanciar objetos anónimos
                .OrderBy(t => t.idEstadoOf == 1 ? 0 : 1)
                .ThenBy(t => t.idEstadoOf == 1 ? -t.oF : t.posicion)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                    .ThenInclude(o => o.idEtiquetaNavigation)
                .Include(f => f.ffeTiemposOfGlobal)
                .Include(se => se.secuenciadoPorNavigation)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetasOrdenadas);

            return Ok(tarjetaOfDto);
        }

        // GET: api/tarjetaOf/consolidadas-ov
        [HttpGet("get/ov-consolidadas")]
        public async Task<ActionResult<object>> GetTarjetasConsolidadasPorOv()
        {
            // 1. Ejecutamos la consulta optimizada
            var tarjetasOrdenadas = await _context.tarjetaOf
                .AsNoTracking()
                .AsSplitQuery()
                .Where(t =>
                    t.archivada == false &&
                    t.oV != null && // 🚀 Filtramos los nulos desde la base de datos
                    t.oV != 0       // 🚀 Filtramos los ceros desde la base de datos
                )
                .OrderBy(t => t.idEstadoOf == 1 ? 0 : 1)
                .ThenBy(t => t.idEstadoOf == 1 ? -t.oF : t.posicion)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                    .ThenInclude(o => o.idEtiquetaNavigation)
                .Include(f => f.ffeTiemposOfGlobal)
                .Include(se => se.secuenciadoPorNavigation)
                .ToListAsync();

            // 2. Mapeamos a tu DTO
            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetasOrdenadas);

            // 3. 🚀 CONSOLIDACIÓN POR oV SIMPLIFICADA
            var consolidadasOV = tarjetaOfDto
                // Como ya filtramos los nulos arriba, agrupamos directamente por el oV
                .GroupBy(t => t.oV.ToString())
                .Select(g => new
                {
                    ov = g.Key,
                    cantidadTarjetas = g.Count(),
                    tarjetas = g.ToList()
                })
                // Ordenamos limpiamente de mayor a menor oV
                .OrderByDescending(g => g.ov)
                .ToList();

            return Ok(consolidadasOV);
        }

        // GET: api/tarjetaOf/consolidadas-ov filtro por ov
        [HttpGet("get/ov-consolidadas/{ov}")]
        public async Task<ActionResult<object>> GetTarjetasConsolidadasPorOv(string ov)
        {
            // 1. Ejecutamos la consulta optimizada
            var tarjetasOrdenadas = await _context.tarjetaOf
                .AsNoTracking()
                .AsSplitQuery()
                .Where(t =>
                    t.archivada == false &&
                    t.oV != null && // 🚀 Filtramos los nulos desde la base de datos
                    t.oV != 0       // 🚀 Filtramos los ceros desde la base de datos
                    && t.oV.ToString() == ov // Filtramos por el oV recibido como parámetro
                )
                .OrderBy(t => t.idEstadoOf == 1 ? 0 : 1)
                .ThenBy(t => t.idEstadoOf == 1 ? -t.oF : t.posicion)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                    .ThenInclude(o => o.idEtiquetaNavigation)
                .Include(f => f.ffeTiemposOfGlobal)
                .Include(se => se.secuenciadoPorNavigation)
                .ToListAsync();

            // 2. Mapeamos a tu DTO
            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetasOrdenadas);

            // 3. 🚀 CONSOLIDACIÓN POR oV SIMPLIFICADA
            var consolidadasOV = tarjetaOfDto
                // Como ya filtramos los nulos arriba, agrupamos directamente por el oV
                .GroupBy(t => t.oV.ToString())
                .Select(g => new
                {
                    ov = g.Key,
                    cantidadTarjetas = g.Count(),
                    tarjetas = g.ToList()
                })
                // Ordenamos limpiamente de mayor a menor oV
                .OrderByDescending(g => g.ov)
                .ToList();

            return Ok(consolidadasOV);
        }

        // GET por user, si es el usuario "emenjivar" debe mostrar solo las del area con id 19, si no, mostrar todo
        // Ejemplo URL: /api/tarjetaOf/get/por-usuario/emenjivar
        [HttpGet("get/por-usuario/{username}")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOfPorUsuario(string username)
        {
            // 1. Limpiamos el nombre del usuario recibido por parámetro
            string usuarioActual = username?.Trim().ToLower() ?? "";
            bool esEmenjivar = usuarioActual == "emenjivar";

            // 2. Ejecutamos la consulta
            var tarjetasOrdenadas = await _context.tarjetaOf
                .AsNoTracking()
                .AsSplitQuery()
                .Where(t =>
                    t.archivada == false &&
                    (!esEmenjivar || t.procesoOf.Any(p => p.idTableroNavigation.idArea == 19))
                )
                // OrderBy siempre debe ir después del Where
                .OrderBy(t => t.idEstadoOf == 1 ? 0 : 1)
                .ThenBy(t => t.idEstadoOf == 1 ? -t.oF : t.posicion)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                    .ThenInclude(o => o.idEtiquetaNavigation)
                .Include(f => f.ffeTiemposOfGlobal)
                .Include(se => se.secuenciadoPorNavigation)
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
        [FromQuery] bool mostrarArchivados = false,
        [FromQuery] bool reproceso = false,
        [FromQuery] string? serieOf = null)
        {
            // Consulta base
            var query = _context.tarjetaOf
                .Include(r => r.etiquetaOf)
                .ThenInclude(o => o.idEtiquetaNavigation)
                .Include(e => e.idEstadoOfNavigation)
                .Include(f => f.ffeTiemposOfGlobal)
                .Include(se => se.secuenciadoPorNavigation)
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

            // Campo para filtrar por reproceso true/false
            if (reproceso)
            {
                query = query.Where(p => p.reproceso == true);
            }

            // Filtro "like" para serieOf
            if (!string.IsNullOrEmpty(serieOf))
            {
                query = query.Where(p => p.seriesOf.Contains(serieOf));
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
                    .ThenInclude(o => o.idEtiquetaNavigation)
                .Include(f => f.ffeTiemposOfGlobal)
                .Include(se => se.secuenciadoPorNavigation)
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
            const string urlWebhookClaudia = "https://chat.googleapis.com/v1/spaces/sYGT9CAAAAE/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=J66jBKxAB0SxqKV7igGtV5g3qLAhQhuPZBzXuY-tK1M";
            const string urlWebhookJavier = "https://chat.googleapis.com/v1/spaces/9dQbXUAAAAE/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=NErWICVNwIEyHaWSbK8ffpFu0-5f03sTV87Yus9jdoA";
            const string urlGeneral = "https://chat.googleapis.com/v1/spaces/AAQAWq4gutM/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=MS6Q3qwuymWfjsILDQA0p4OrRgg3I69CQkejymsLGxU";

            // 1. Cargamos la entidad...
            var tarjetaOf = await _context.tarjetaOf
                .Include(t => t.idEstadoOfNavigation)
                .Include(l => l.logCambiosOf)
                    .ThenInclude(lc => lc.usuario)
                .FirstOrDefaultAsync(t => t.oF == id);

            if (tarjetaOf == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            // 2. Capturamos el estado ANTES
            int idEstadoAnterior = (int)tarjetaOf.idEstadoOf;

            // 3. Verificamos si el DTO trae un nuevo estado
            bool idEstadoVinoEnDto = updateTarjetaOf.idEstadoOf.HasValue;

            // 4. Mapeamos
            _mapper.Map(updateTarjetaOf, tarjetaOf);

            // 5. ¡CORRECCIÓN CRÍTICA!
            if (!idEstadoVinoEnDto)
            {
                tarjetaOf.idEstadoOf = idEstadoAnterior;
            }

            _context.Entry(tarjetaOf).State = EntityState.Modified;

            // 6. Guardamos los cambios en la Base de Datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                // ... tu lógica de concurrencia ...
            }

            // === INICIO DE LA INTEGRACIÓN CONDICISqlException: The UPDATE statement conflicted with the FOREIGN KEY SAME TABLE constraint "FK_REPROCESA_OF". The conflict occurred in database "NEXO_DB", table "dbo.tarjetaOf", column 'oF'.ONAL ===

            // 7. Verificamos el cambio de estado Y el vendedor
            bool estadoCambio = idEstadoVinoEnDto && idEstadoAnterior != updateTarjetaOf.idEstadoOf.Value;

            string? urlWebhookSeleccionada = null;
            if (tarjetaOf.vendedorOf?.Equals("Claudia Ruano", StringComparison.OrdinalIgnoreCase) == true)
            {
                urlWebhookSeleccionada = urlWebhookClaudia;
            }
            else if (tarjetaOf.vendedorOf?.Equals("Javier Toledo", StringComparison.OrdinalIgnoreCase) == true)
            {
                urlWebhookSeleccionada = urlWebhookJavier;
            }

            // El IF ahora comprueba si el estado cambió Y si encontramos una URL (para el ejecutivo)
            if (estadoCambio && !string.IsNullOrEmpty(urlWebhookSeleccionada))
            {
                _logger.LogInformation($"El estado de la OF {id} cambió para {tarjetaOf.vendedorOf}. Enviando notificación a ejecutivo y general.");

                try
                {
                    // 8. Obtenemos el nombre del NUEVO estado
                    await _context.Entry(tarjetaOf).Reference(t => t.idEstadoOfNavigation).LoadAsync();
                    string nombreEstado = tarjetaOf.idEstadoOfNavigation?.nombreEstado ?? $"ID: {updateTarjetaOf.idEstadoOf.Value}";

                    // 9. (MODIFICADO) GENERAR CLAVES DE CACHÉ BASADAS EN LA 'OV'
                    //    Esto agrupará todas las OFs de la misma venta en un solo hilo.

                    // Usamos tarjetaOf.oV (convirtiéndolo a string)
                    string idAgrupador = tarjetaOf.oV.ToString();

                    // Clave para el ejecutivo (única por OV y Vendedor)
                    string cacheKeyExec = $"GoogleChatThread_OV_{idAgrupador}_{tarjetaOf.vendedorOf}";

                    // Clave para el general (única por OV)
                    string cacheKeyGeneral = $"GoogleChatThread_OV_{idAgrupador}_General";

                    // Buscamos los hilos existentes
                    _memoryCache.TryGetValue(cacheKeyExec, out string? currentThreadIdExec);
                    _memoryCache.TryGetValue(cacheKeyGeneral, out string? currentThreadIdGeneral);

                    object finalPayload;

                    // ⬇️ (CORREGIDO) Usamos la variable 'currentThreadIdExec' ⬇️
                    if (string.IsNullOrEmpty(currentThreadIdExec))
                     {
                        // 10a. CASO 1: HILO NUEVO -> ENVIAR TARJETA DETALLADA

                        // --- Obtenemos los datos adicionales para la tarjeta ---

                        // 8a. Obtenemos el nombre del estado ANTERIOR
                        // ⚠️ Asume que tu DbSet se llama 'estadosOf' y la PK es 'IdEstado'
                        string nombreEstadoAnterior = (await _context.estadosOf.FindAsync(idEstadoAnterior))?.nombreEstado ?? "N/A";

                        var ultimoLog = tarjetaOf.logCambiosOf
                                         .OrderByDescending(log => log.fecha_hora) // ⚠️ Asume que 'fecha' es el campo
                                         .FirstOrDefault();

                        string usuarioCambio = ultimoLog?.usuario?.nombres + " " + ultimoLog?.usuario?.apellidos ?? "Sistema";
                        // Código corregido
                        string fechaCambio = ultimoLog?.fecha_hora?.ToString("g") ?? DateTime.Now.ToString("g");

                        // 8c. Formateamos otros campos (directamente desde 'tarjetaOf')
                        string fechaEntrega = tarjetaOf.fechaVencimiento?.ToString("dd-MMM") ?? "N/A";
                        string cantidad = $"{tarjetaOf.cantidadOf} {tarjetaOf.unidadMedida}"; // ⚠️ Asume 'unidadMedida'
                        string producto = $"{tarjetaOf.codArticulo} - {tarjetaOf.productoOf}"; // ⚠️ Asume 'codArticulo'
                        string lineaNegocio = tarjetaOf.lineaDeNegocio ?? "N/A";
                        string fecheActualizacion = DateTime.Now.ToString("g");
                        string serieOf = tarjetaOf.seriesOf ?? "N/A";
                        string urlNexo = "http://nexo.it:8080/main/inicio"; // ⚠️ ¡REEMPLAZA ESTA URL POR LA REAL!

                        // ⬇️ PASO NUEVO (8d) ⬇️
                        // Creamos el texto simple que SÍ será indexado por la búsqueda.
                        // Incluimos todos los datos clave que alguien podría buscar.
                        // ⬇️ (PASO 8d - MODIFICADO) ⬇️
                        // Creamos el texto simple que SÍ será indexado por la búsqueda.
                        var sb = new System.Text.StringBuilder();
                        sb.AppendLine($"*OV:* {tarjetaOf.oV}");
                        sb.AppendLine($"*OF:* {tarjetaOf.oF}");
                        sb.AppendLine($"*Cliente:* {tarjetaOf.clienteOf}");
                        sb.AppendLine($"*Producto:* {producto}");
                        string textoDeBusqueda = sb.ToString();

                        // --- Construimos el payload de la tarjeta ---
                        finalPayload = new
                        {
                            text = textoDeBusqueda,
                            cardsV2 = new[] {
                        new {
                            cardId = $"nexo-of-{tarjetaOf.oF}",
                            card = new {
                                header = new {
                                    title = $"🟢 {serieOf} {tarjetaOf.oF} - OV: {tarjetaOf.oV})",
                                    subtitle = $"Linea de negocio: {lineaNegocio}"
                                },
                                sections = new object[] {
                                    // Sección 1: Info del Cambio
                                    new {
                                        widgets = new object[] {
                                            new {
                                                decoratedText = new {
                                                    topLabel = "🔃 CAMBIO DE ESTADO EN CONTROL PEDIDOS",
                                                    // ⬇️ (MODIFICADO) Añadimos el usuario del log
                                                    text = $"{nombreEstadoAnterior} → <b>{nombreEstado}</b> · {usuarioCambio} · {fecheActualizacion}",
                                                    wrapText = true
                                                }
                                            },
                                            new { divider = new {} }
                                        }
                                    },
                                    // Sección 2: Columnas (Proceso, Estado, Entrega)
                                    new {
                                        widgets = new object[] {
                                            new {
                                                columns = new {
                                                    columnItems = new object[] {
                                                        new { widgets = new object[] { new { decoratedText = new { topLabel = "ESTADO", text = nombreEstado } } } },
                                                        new { widgets = new object[] { new { decoratedText = new { topLabel = "ENTREGA", text = fechaEntrega } } } }
                                                    }
                                                }
                                            }
                                        }
                                    },
                                    // Sección 3: Cliente y Producto
                                    new {
                                        widgets = new object[] {
                                            new {
                                                decoratedText = new {
                                                    topLabel = "👤 CLIENTE",
                                                    text = tarjetaOf.clienteOf,
                                                    wrapText = true
                                                }
                                            },
                                            new {
                                                decoratedText = new {
                                                    topLabel = "📦 PRODUCTO",
                                                    text = producto,
                                                    wrapText = true
                                                }
                                            }
                                        }
                                    },
                                    // Sección 4: Cantidad, ejecutivo y Botones
                                    new {
                                        widgets = new object[] {
                                            new {
                                                columns = new {
                                                    columnItems = new object[] {
                                                        new { widgets = new object[] { new { decoratedText = new { topLabel = "#️⃣ CANTIDAD", text = cantidad } } } },
                                                        new { widgets = new object[] { new { decoratedText = new { topLabel = "👤 EJECUTIVO", text = tarjetaOf.vendedorOf } } } }
                                                    }
                                                }
                                            },
                                            new {
                                                buttonList = new {
                                                    buttons = new[] {
                                                        new {
                                                            text = "Ver orden",
                                                            onClick = new { openLink = new { url = urlNexo } }
                                                        }
                                                    }
                                                }
                                            }
                                        }
                                    }
                                }, // Fin de 'sections'
                                fixedFooter = new {
                                    primaryButton = new {
                                        text = "Abrir tarjeta en NEXO",
                                        onClick = new {
                                            openLink = new { url = urlNexo }
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
                        var messageBuilder = new System.Text.StringBuilder();
                        messageBuilder.AppendLine($"🔄 *CAMBIO DE ESTADO EN CONTROL PEDIDOS*");
                        messageBuilder.AppendLine($"La Tarjeta/OF *ID: {id}* cambió al estado: *{nombreEstado}*.");

                        finalPayload = new
                        {
                            text = messageBuilder.ToString(),                         
                        };
                    }

                    // 11. ⬇️ (MODIFICADO) ENVIAR A AMBOS WEBHOOKS EN PARALELO

                    // --- Tarea 1: Enviar al ejecutivo ---
                    //Task<string?> execTask = SendToGoogleChat(
                    //    finalPayload,
                    //    currentThreadIdExec,
                    //    urlWebhookSeleccionada // 👈 URL de Claudia o Javier
                    //);

                    // --- Tarea 2: Enviar al general ---
                    Task<string?> generalTask = SendToGoogleChat(
                        finalPayload, // 👈 Usamos el mismo payload
                        currentThreadIdGeneral,
                        urlGeneral // 👈 URL General
                    );

                    // --- Ejecutar ambas tareas en paralelo ---
                    await Task.WhenAll(/*execTask,*/ generalTask);

                    // 12. GUARDAR LOS IDs DE HILO DE AMBAS TAREAS (Con expiración larga para OVs)

                    // Aumentamos a 30 días porque las OVs duran más
                    var cacheEntryOptions = new MemoryCacheEntryOptions()
                        .SetSlidingExpiration(TimeSpan.FromDays(30));

                    //string? newThreadIdExec = execTask.Result;
                    //if (!string.IsNullOrEmpty(newThreadIdExec))
                    //{
                    //    _memoryCache.Set(cacheKeyExec, newThreadIdExec, cacheEntryOptions);
                    //}

                    string? newThreadIdGeneral = generalTask.Result;
                    if (!string.IsNullOrEmpty(newThreadIdGeneral))
                    {
                        _memoryCache.Set(cacheKeyGeneral, newThreadIdGeneral, cacheEntryOptions);
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
        /// Envía un payload (Tarjeta o Texto) a una URL de Webhook específica.
        /// </summary>
        /// <param name="messagePayload">El objeto completo a serializar (ej. { text: "..." } o { cardsV2: [...] }).</param>
        /// <param name="currentThreadId">El ID del hilo actual (si existe).</param>
        /// <param name="webhookUrl">La URL específica del webhook al que se debe enviar.</param>
        /// <returns>El ID del hilo si se creó uno nuevo; null si solo se respondió.</returns>
        private async Task<string?> SendToGoogleChat(object messagePayload, string? currentThreadId, string webhookUrl)
        {
            // ⚠️ ¡Importante! 'webhookUrl' AHORA VIENE COMO PARÁMETRO.
            // Ya no se definen URLs aquí adentro.

            var client = _httpClientFactory.CreateClient();
            string postUrl = webhookUrl; // 👈 Usamos la URL del parámetro

            // Si ya estamos en un hilo, añadimos el parámetro de respuesta
            if (!string.IsNullOrEmpty(currentThreadId))
            {
                // 👈 Usamos la URL del parámetro
                postUrl = $"{webhookUrl}&messageReplyOption=REPLY_MESSAGE_FALLBACK_TO_NEW_THREAD";
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

                // 3. SI CREAMOS UN HILO NUEVO...
                if (string.IsNullOrEmpty(currentThreadId))
                {
                    var responseBody = await response.Content.ReadAsStringAsync();
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
