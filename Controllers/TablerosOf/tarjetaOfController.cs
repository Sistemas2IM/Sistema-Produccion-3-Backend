using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
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

        public tarjetaOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tarjetaOf
        [HttpGet("get")]
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
        [FromQuery] string? idsEtiquetas = null)    // Parámetro opcional para los IDs de etiquetas (separados por comas)
        {
            // Consulta base
            var query = _context.tarjetaOf
                .Include(r => r.etiquetaOf)
                .ThenInclude(o => o.idEtiquetaNavigation)
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
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttarjetaOf(int id, UpdateTarjetaOfDto updateTarjetaOf)
        {
            var tarjetaOf = await _context.tarjetaOf.FindAsync(id);

            if (tarjetaOf == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            _mapper.Map(updateTarjetaOf, tarjetaOf);
            _context.Entry(tarjetaOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tarjetaOfExists(id))
                {
                    return NotFound("No se encontro la tarjeta con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

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

        // GET: api/tarjetaOf/lineaNegocio
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
        }


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
    }
}
