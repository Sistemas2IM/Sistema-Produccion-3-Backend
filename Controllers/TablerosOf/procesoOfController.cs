using System.Text.Json;
using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.BusquedaProcesos;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Acabado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Barnizado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Impresión;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Pegadora;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Preprensa;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Troquelado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.UpdateSAP;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class procesoOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public procesoOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/procesoOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOf()
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .ToListAsync();

            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesoOf);

            return Ok(procesoOfDto);
        }

        [HttpGet("get/filtros")]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOffiltros(
        [FromQuery] DateTime? fechaInicio = null,   // Parámetro opcional para la fecha de inicio del rango
        [FromQuery] DateTime? fechaFin = null,     // Parámetro opcional para la fecha de fin del rango
        [FromQuery] string cliente = null,         // Parámetro opcional para el cliente
        [FromQuery] string ejecutivo = null,
        [FromQuery] int? tablero = null)      // Parámetro opcional para el ejecutivo

        {
            // Consulta base
            var query = _context.procesoOf
                .OrderBy(p => p.posicion)
                .Include(u => u.oFNavigation)
                .Include(l => l.idPosturaNavigation)
                .Include(s => s.tarjetaEtiqueta)
                .ThenInclude(e => e.idEtiquetaNavigation)
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
            if (!string.IsNullOrEmpty(cliente))
            {
                query = query.Where(p => p.oFNavigation.clienteOf.Contains(cliente));
            }
            if (!string.IsNullOrEmpty(ejecutivo))
            {
                query = query.Where(p => p.oFNavigation.vendedorOf.Contains(ejecutivo));
            }
            if (tablero.HasValue)
            {
                query = query.Where(p => p.idTablero == tablero.Value);
            }

            // Ejecutar la consulta y mapear a DTO
            var procesoOf = await query.ToListAsync();
            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesoOf);

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProcesoOfDto>> GetprocesoOf(int id)
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .FirstOrDefaultAsync(u => u.idProceso == id);

            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesoOf);

            if (procesoOfDto == null)
            {
                return NotFound("No se encontro el proceso de la Of con el id: " + id);
            }

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/5
        [HttpGet("get/oF/{of}")]
        public async Task<ActionResult<ProcesoOfDto>> GetprocesoOfTarjeta(int of)
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .FirstOrDefaultAsync(u => u.oF == of);

            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesoOf);

            if (procesoOfDto == null)
            {
                return NotFound("No se encontro el proceso de la Of con el numero de of: " + of);
            }

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/5 LISTA
        [HttpGet("get/lista/oF/{of}")]
        public async Task<ActionResult<ListaProcesoOfDto>> GetprocesoOfTarjetaLista(int of)
        {
            var procesoOf = await _context.procesoOf
                .Where(o => o.oF == of)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .ThenInclude(v => v.idAreaNavigation)
                .Include(c => c.idTableroNavigation)
                .ThenInclude(m => m.idMaquinaNavigation)
                .Include(f => f.oFNavigation)
                .ToListAsync();

            var procesoOfDto = _mapper.Map<List<ListaProcesoOfDto>>(procesoOf);

            if (procesoOfDto == null)
            {
                return NotFound("No se contraron los procesos de la Of con el numero de of: " + of);
            }

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf
        [HttpGet("get/tablero/{id}")]
        public async Task<ActionResult<IEnumerable<ProcesoOfVistaTableroDto>>> GetprocesoOfTablero(int id)
        {
            var procesoOf = await _context.procesoOf
                .OrderBy(p => p.posicion)
                .Where(t => t.idTablero == id)
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .ThenInclude(e => e.idEtiquetaNavigation)
                .Include(f => f.oFNavigation)
                .Include(l => l.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .ToListAsync();

            var procesoOfDto = _mapper.Map<List<ProcesoOfVistaTableroDto>>(procesoOf);

            return Ok(procesoOfDto);
        }

        // CONTROLADOR DINAMICO PARA OBTENER LOS DETALLES POR MAQUINA

        [HttpGet("get/procesoOfMaquina/{id}")]
        public async Task<ActionResult<ProcesoOfMaquinas>> GetProcesoOf(int id)
        {
            // Cargar datos generales del proceso
            var proceso = await _context.procesoOf
                .Include(p => p.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .FirstOrDefaultAsync(u => u.idProceso == id);
            if (proceso == null) return NotFound();

            // Mapear el modelo general al DTO principal
            var dto = _mapper.Map<ProcesoOfMaquinas>(proceso);

            // Cargar detalles específicos según el tipo de máquina
            switch (proceso.tipoMaquinaSAP)
            {
                case "impresion":
                    var detalleImpresora = await _context.procesoImpresora
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoImpresoraDto>(detalleImpresora);
                    break;

                case "troquel":
                    var detalleTroqueladora = await _context.procesoTroqueladora
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoTroqueladoraDto>(detalleTroqueladora);
                    break;

                case "barniz":
                    var detalleBarnizadora = await _context.procesoBarniz
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoBarnizDto>(detalleBarnizadora);
                    break;

                case "pegadora":
                    var detallePegadora = await _context.procesoPegadora
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoPegadoraDto>(detallePegadora);
                    break;

                case "acabado":
                    var detalleAcabado = await _context.procesoAcabado
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoAcabadoDto>(detalleAcabado);
                    break;

                case "preprensa":
                    var detallePreprensa = await _context.procesoPreprensa
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoPreprensaDto>(detallePreprensa);
                    break;

                default:
                    dto.DetalleProceso = null;
                    break;
            }

            return Ok(dto);
        }

        // PUT: api/procesoOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutprocesoOf(int id, UpdateProcesoOfDto updateProcesoOf)
        {
            var procesoOf = await _context.procesoOf.FindAsync(id);

            if (procesoOf == null)
            {
                return NotFound("No se encontro el proceso de la of con el id: " + id);
            }

            _mapper.Map(updateProcesoOf, procesoOf);
            _context.Entry(procesoOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!procesoOfExists(id))
                {
                    return NotFound("No se encontro el proceso de la of con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateProcesoOf);
        }

        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateProcesos([FromBody] BatchUpdateProcesoOfDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.ProcesosOf == null || !batchUpdateDto.ProcesosOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.ProcesosOf.Select(t => t.idProceso).ToList();

            // Obtener todas los procesos of relacionadas
            var procesos = await _context.procesoOf.Where(t => ids.Contains(t.idProceso)).ToListAsync();

            if (!procesos.Any())
            {
                return NotFound("No se encontraron procesos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.ProcesosOf)
            {
                var proceso = procesos.FirstOrDefault(t => t.idProceso == dto.idProceso);
                if (proceso != null)
                {
                    if (dto.idProceso > 0)
                    {
                        proceso.posicion = dto.posicion;
                    }

                    _context.Entry(proceso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los procesos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        [HttpPut("put/BatchUpdateSAP")]
        public async Task<IActionResult> BatchUpdateProcesosSAP([FromBody] ListSAPUpdateProcesoOf batchUpdateDtoSAP)
        {
            if (batchUpdateDtoSAP == null || batchUpdateDtoSAP.SAPProcesoOf == null || !batchUpdateDtoSAP.SAPProcesoOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDtoSAP.SAPProcesoOf.Select(t => t.idProceso).Distinct().ToList();

            // Obtener procesos relacionados
            var procesos = await _context.procesoOf.Where(t => ids.Contains(t.idProceso)).ToListAsync();

            if (!procesos.Any())
            {
                return NotFound("No se encontraron procesos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDtoSAP.SAPProcesoOf)
            {

                var proceso = procesos.FirstOrDefault(t => t.idProceso == dto.idProceso);
                if (proceso != null)
                {
                    if (dto.posicion.HasValue)
                    {
                        proceso.posicion = dto.posicion.Value;
                    }
                    _context.Entry(proceso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los Procesos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        [HttpPut("put/procesoOfMaquina/{idProceso}")]
        public async Task<IActionResult> UpdateProcesoOf(int idProceso, [FromBody] UpProcesoOfMaquinas dto)
        {
            // Buscar el proceso general existente
            var procesoExistente = await _context.procesoOf.FindAsync(idProceso);
            if (procesoExistente == null)
            {
                return NotFound("El proceso con el ID proporcionado no existe.");
            }

            // Actualizar los campos del proceso general
            _mapper.Map(dto, procesoExistente);
            _context.procesoOf.Update(procesoExistente);

            // Verificar si se necesita actualizar el detalle según el tipo de máquina
            if (!string.IsNullOrEmpty(dto.tipoMaquinaSAP) && dto.DetalleProceso != null)
            {
                switch (dto.tipoMaquinaSAP.ToLower())
                {
                    case "preprensa":
                        await ActualizarDetalleMaquina<procesoPreprensa, UpProcesoPreprensaDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "impresion":
                        await ActualizarDetalleMaquina<procesoImpresora, UpProcesoImpresoraDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "troquel":
                        await ActualizarDetalleMaquina<procesoTroqueladora, UpProcesoTroqueladoDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "pegadora":
                        await ActualizarDetalleMaquina<procesoPegadora, UpProcesoPegadoraDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "acabado":
                        await ActualizarDetalleMaquina<procesoAcabado, UpProcesoAcabadoDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "barniz":
                        await ActualizarDetalleMaquina<procesoBarniz, UpProcesoBarnizDto>(idProceso, dto.DetalleProceso);
                        break;

                    default:
                        return BadRequest("Tipo de máquina no soportado.");
                }
            }

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Proceso Of actualizado exitosamente.");
        }




        // POST: api/procesoOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<procesoOf>> PostprocesoOf(AddProcesoOfDto addProcesoOf)
        {
            var procesoOf = _mapper.Map<procesoOf>(addProcesoOf);
            _context.procesoOf.Add(procesoOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetprocesoOf", new { id = procesoOf.idProceso }, procesoOf);
        }

        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddProcesoOf([FromBody] BatchAddProcesoOf batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchProcesoDto == null || !batchAddDto.addBatchProcesoDto.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            // Mapear los DTOs a las entidades
            var procesos = batchAddDto.addBatchProcesoDto.Select(dto => _mapper.Map<procesoOf>(dto)).ToList();

            // Agregar los procesos a la base de datos
            await _context.procesoOf.AddRangeAsync(procesos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar los procesos: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Procesos agregados exitosamente.",
                ProcesosAgregados = procesos
            });
        }

        // POST PARA DETALLES POR MAQUINA

        [HttpPost("post/procesoOfMaquina")]
        public async Task<ActionResult> CreateProcesoOf(AddProcesoOfMaquinas dto)
        {
            // Validar que el DTO contenga los datos necesarios
            if (dto.DetalleProceso == null || string.IsNullOrEmpty(dto.tipoMaquinaSAP))
            {
                return BadRequest("El detalle del proceso o el tipo de máquina son inválidos.");
            }

            // Mapear el DTO al modelo general
            var proceso = _mapper.Map<procesoOf>(dto);

            // Guardar los datos generales en la tabla `procesoOf`
            _context.procesoOf.Add(proceso);
            await _context.SaveChangesAsync();

            // Insertar los detalles específicos según el tipo de máquina
            switch (dto.tipoMaquinaSAP.ToLower()) // Convertir a minúsculas por seguridad
            {
                case "preprensa":
                    if (dto.DetalleProceso is JsonElement jsonPreprensa)
                    {
                        var detallePreprensa = JsonSerializer.Deserialize<procesoPreprensa>(jsonPreprensa.GetRawText());
                        if (detallePreprensa != null)
                        {
                            detallePreprensa.idProceso = proceso.idProceso;
                            _context.procesoPreprensa.Add(detallePreprensa);
                        }
                    }
                    break;

                case "impresion":
                    if (dto.DetalleProceso is JsonElement jsonImpresion)
                    {
                        var detalleImpresora = JsonSerializer.Deserialize<procesoImpresora>(jsonImpresion.GetRawText());
                        if (detalleImpresora != null)
                        {
                            detalleImpresora.idProceso = proceso.idProceso;
                            _context.procesoImpresora.Add(detalleImpresora);
                        }
                    }
                    break;

                case "troquel":
                    if (dto.DetalleProceso is JsonElement jsonTroquel)
                    {
                        var detalleTroqueladora = JsonSerializer.Deserialize<procesoTroqueladora>(jsonTroquel.GetRawText());
                        if (detalleTroqueladora != null)
                        {
                            detalleTroqueladora.idProceso = proceso.idProceso;
                            _context.procesoTroqueladora.Add(detalleTroqueladora);
                        }
                    }
                    break;

                case "pegadora":
                    if (dto.DetalleProceso is JsonElement jsonPegadora)
                    {
                        var detallePegadora = JsonSerializer.Deserialize<procesoPegadora>(jsonPegadora.GetRawText());
                        if (detallePegadora != null)
                        {
                            detallePegadora.idProceso = proceso.idProceso;
                            _context.procesoPegadora.Add(detallePegadora);
                        }
                    }
                    break;

                case "acabado":
                    if (dto.DetalleProceso is JsonElement jsonAcabado)
                    {
                        var detalleAcabado = JsonSerializer.Deserialize<procesoAcabado>(jsonAcabado.GetRawText());
                        if (detalleAcabado != null)
                        {
                            detalleAcabado.idProceso = proceso.idProceso;
                            _context.procesoAcabado.Add(detalleAcabado);
                        }
                    }
                    break;

                case "barniz":
                    if (dto.DetalleProceso is JsonElement jsonBarniz)
                    {
                        var detalleBarniz = JsonSerializer.Deserialize<procesoBarniz>(jsonBarniz.GetRawText());
                        if (detalleBarniz != null)
                        {
                            detalleBarniz.idProceso = proceso.idProceso;
                            _context.procesoBarniz.Add(detalleBarniz);
                        }
                    }
                    break;

                default:
                    return BadRequest("Tipo de máquina no soportado.");
            }

            await _context.SaveChangesAsync();
            return Ok("Proceso Of creado exitosamente.");
        }



        private bool procesoOfExists(int id)
        {
            return _context.procesoOf.Any(e => e.idProceso == id);
        }

        private async Task ActualizarDetalleMaquina<TEntity, TDto>(int idProceso, object detalleProceso)
    where TEntity : class
    where TDto : class
        {
            // Buscar el detalle existente en la base de datos
            var detalleExistente = await _context.Set<TEntity>().FirstOrDefaultAsync(e => EF.Property<int>(e, "idProceso") == idProceso);

            if (detalleExistente == null)
            {
                // Si no existe, se crea un nuevo detalle
                var nuevoDetalle = JsonSerializer.Deserialize<TDto>(detalleProceso.ToString());
                if (nuevoDetalle != null)
                {
                    var entidad = _mapper.Map<TEntity>(nuevoDetalle);
                    _context.Set<TEntity>().Add(entidad);
                }
            }
            else
            {
                // Si existe, se actualiza el detalle existente
                var detalleDto = JsonSerializer.Deserialize<TDto>(detalleProceso.ToString());
                if (detalleDto != null)
                {
                    _mapper.Map(detalleDto, detalleExistente);
                    _context.Set<TEntity>().Update(detalleExistente);
                }
            }
        }

    }
}
