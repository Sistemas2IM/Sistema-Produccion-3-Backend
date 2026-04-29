using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales.Batch;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class solicitudMaterialesController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly base_nuevaContextProcedures _contextSP;

        public solicitudMaterialesController(base_nuevaContext context, IMapper mapper, base_nuevaContextProcedures contextSP)
        {
            _context = context;
            _mapper = mapper;
            _contextSP=contextSP;
        }


        // GET: api/<solicitudMaterialesController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesDto>>> GetSolicitud()
        {
            var solicitudMateriales = await _context.solicitudMateriales
                .OrderBy(p => p.posicion)
                .Include(s => s.solicitudMaterialesOf)
                    .ThenInclude(so => so.oFNavigation)
                .Include(ma => ma.idMaquinaNavigation)
                .Include(s => s.etiquetaSolicitud)
                    .ThenInclude(se => se.idEtiquetaNavigation)
                .Where(s => s.archivado == false)
                .ToListAsync();

            var solicitudMaterialesDto = _mapper.Map<List<solicitudMaterialesDto>>(solicitudMateriales);

            return Ok(solicitudMaterialesDto);
        }

        // GET api/<solicitudMaterialesController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesDto>>> GetSolicitudId(int id)
        {
            var solicitudMateriales = await _context.solicitudMateriales
                .OrderBy(p => p.posicion)
                .Include(s => s.solicitudMaterialesOf)
                    .ThenInclude(so => so.oFNavigation)
                .Include(ma => ma.idMaquinaNavigation)
                .Include(s => s.etiquetaSolicitud)
                    .ThenInclude(se => se.idEtiquetaNavigation)
                .Where(s => s.archivado == false)
                .FirstOrDefaultAsync(s => s.idSolicitud == id);

            var solicitudMaterialesDto = _mapper.Map<solicitudMaterialesDto>(solicitudMateriales);

            if(solicitudMaterialesDto == null)
            {
                return NotFound($"No se encontro el registro con el id: {id}");
            }

            return Ok(solicitudMaterialesDto);
        }

        // GET api/<solicitudMaterialesController>/5
        [HttpGet("get/idSap/{idSap}")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesDto>>> GetSolicitudIdSap(int idSap)
        {
            var solicitudMateriales = await _context.solicitudMateriales
                .Where(s => s.idSap == idSap)
                .OrderBy(p => p.posicion)
                .Include(s => s.solicitudMaterialesOf)
                    .ThenInclude(so => so.oFNavigation)
                .Include(ma => ma.idMaquinaNavigation)
                .Include(s => s.etiquetaSolicitud)
                    .ThenInclude(se => se.idEtiquetaNavigation)
                .Where(s => s.archivado == false)
                .FirstOrDefaultAsync();

            var solicitudMaterialesDto = _mapper.Map<solicitudMaterialesDto>(solicitudMateriales);

            if (solicitudMaterialesDto == null)
            {
                return NotFound($"No se encontro el registro con el id: {idSap}");
            }

            return Ok(solicitudMaterialesDto);
        }

        [HttpGet("get/solicitudes/filtros")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesDto>>> GetSolicitudesFiltros(
        [FromQuery] int? idSolicitud = null,      // Filtro exacto por ID de solicitud
        [FromQuery] int? of = null,               // Filtro por OF (busca en la tabla relación)
        [FromQuery] DateTime? fechaInicio = null, // Rango fecha: Inicio
        [FromQuery] DateTime? fechaFin = null,    // Rango fecha: Fin
        [FromQuery] int? tipoOperacion = null,
        [FromQuery] string? estado = "",
        [FromQuery] string? materialDesc = "")
        {
            // 1. Consulta base con Include
            // Es importante incluir la relación si luego el AutoMapper necesita datos de ahí
            var query = _context.solicitudMateriales
                .Include(s => s.solicitudMaterialesOf)
                    .ThenInclude(so => so.oFNavigation)
                .Include(ma => ma.idMaquinaNavigation)
                .Include(s => s.etiquetaSolicitud)
                    .ThenInclude(se => se.idEtiquetaNavigation)
                .Where(s => s.archivado == false)
                .AsQueryable();

            // 2. Filtro exacto para IdSolicitud
            if (idSolicitud.HasValue)
            {
                query = query.Where(s => s.idSolicitud == idSolicitud.Value);
            }

            // 3. Filtro por OF (A TRAVÉS DE LA TABLA RELACIÓN)
            // "Traeme las solicitudes donde CUALQUIERA (Any) de sus registros en 'solicitudMaterialesOf' tenga este idOf"
            if (of.HasValue)
            {
                query = query.Where(s => s.solicitudMaterialesOf
                    .Any(rel => rel.oF == of.Value));
            }

            if (tipoOperacion.HasValue)
            {
                query = query.Where(s => s.tipoOperacion == tipoOperacion.Value);
            }

            if (estado != "")
            {
                query = query.Where(s => s.estado == estado);
            }

            if (materialDesc != "")
            {
                query = query.Where(s => s.materialDescripcion == materialDesc);
            }

            // 4. Filtro por Rango de Fechas
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                // Ajuste opcional: forzar fechaFin al final del día (23:59:59) para no perder datos
                var finDelDia = fechaFin.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.fechaSolicitud >= fechaInicio.Value && s.fechaSolicitud <= finDelDia);
            }
            else if (fechaInicio.HasValue)
            {
                query = query.Where(s => s.fechaSolicitud >= fechaInicio.Value);
            }
            else if (fechaFin.HasValue)
            {
                var finDelDia = fechaFin.Value.Date.AddDays(1).AddTicks(-1);
                query = query.Where(s => s.fechaSolicitud <= finDelDia);
            }

            // 5. Ordenamiento (Opcional, pero recomendado: lo más nuevo primero)
            query = query.OrderByDescending(s => s.fechaSolicitud);

            // 6. Ejecutar y Mapear
            var solicitudes = await query.ToListAsync();

            // Si usas AutoMapper, asegúrate que tenga configurado cómo mapear la lista de OFs
            var resultDto = _mapper.Map<List<solicitudMaterialesDto>>(solicitudes);

            return Ok(resultDto);
        }

        [HttpGet("get/catalogo")]
        public async Task<ActionResult<Dictionary<string, List<string>>>> GetCatalogoSolicitudes()
        {
            var catalogos = new Dictionary<string, List<string>>();

            // Obtener estados únicos
            var materialDesc = await _context.solicitudMateriales
                .Select(s => s.materialDescripcion)
                .Distinct()
                .ToListAsync();

            catalogos.Add("materialDescripcion", materialDesc);

            return Ok(catalogos);
        }

        [HttpGet("ResumenSolicitud/{idSolicitud}")]
        public async Task<ActionResult<List<ResumenSolicitudResult>>> GetResumenSolicitud(string idSolicitud)
        {
            // Validación de entrada
            if (string.IsNullOrWhiteSpace(idSolicitud))
            {
                return BadRequest("El id de la solicitud es obligatorio.");
            }

            // Llamada al contexto
            var resultados = await _contextSP.ResumenSolicitudAsync(idSolicitud);

            // Validación de resultados vacíos
            if (resultados == null || resultados.Count == 0)
            {
                return NotFound($"No se encontraron datos para la solicitud: {idSolicitud}");
            }

            // Retorno exitoso
            return Ok(resultados);
        }

        // POST api/<solicitudMaterialesController>
        [HttpPost("post")]
        public async Task<ActionResult<solicitudMaterialesDto>> PostSolicitud(AddSolicitudMaterialesDto solicitudMaterialesDto)
        {
            var solicitudMateriales = _mapper.Map<solicitudMateriales>(solicitudMaterialesDto);
            _context.solicitudMateriales.Add(solicitudMateriales);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitud", new { id = solicitudMateriales.idSolicitud }, solicitudMateriales);
        }

        // PUT api/<solicitudMaterialesController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutetiquetaOf(int id, UpdateSolicitudMaterialesDto solicitudMaterialesDto)
        {
            var solicitudMateriales = await _context.solicitudMateriales.FindAsync(id);
            if (solicitudMateriales == null)
            {
                return NotFound($"No se encontro la etiqueta con el id {id}");
            }

            _mapper.Map(solicitudMaterialesDto, solicitudMateriales);
            _context.Entry(solicitudMateriales).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!solicitudMaterialesExist(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(solicitudMaterialesDto);
        }

        [HttpPut("put/batch/posiciones")]
        public async Task<IActionResult> BatchUpdateSM([FromBody] BatchUpdatePosicionSMDto batchUpdateDto)
        {
            if (batchUpdateDto.SolicitudesMateriales == null || !batchUpdateDto.SolicitudesMateriales.Any())
            {
                return BadRequest("La lista de solicitudes de materiales está vacía.");
            }

            var ids = batchUpdateDto.SolicitudesMateriales.Select(s => s.idSolicitud).ToList();

            var solicitudes = await _context.solicitudMateriales.Where(s => ids.Contains(s.idSolicitud)).ToListAsync();

            if (!solicitudes.Any())
            {
                return NotFound("No se encontraron las SM con los id proporcionados :V");
            }

            foreach (var solicitudDto in batchUpdateDto.SolicitudesMateriales)
            {
                var solicitud = solicitudes.FirstOrDefault(s => s.idSolicitud == solicitudDto.idSolicitud);
                if (solicitud != null)
                {
                    if(solicitudDto.idSolicitud > 0)
                    {
                        solicitud.posicion = solicitudDto.posicion;
                    }

                    _context.Entry(solicitud).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar las SM");
            }

            return Ok("Posiciones actualizadas correctamente.");
        }

        private bool solicitudMaterialesExist(int id) 
        {
            return _context.solicitudMateriales.Any(e => e.idSolicitud == id);
        }

    }
}
