using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class valeBobinaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public valeBobinaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<valeBobinaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ValeBobinaDto>>> GetValeBobina()
        {
            var valeBobinas = await _context.valeBobina
                .OrderByDescending(vb => vb.idVale)
                .Include(vb => vb.idMaterialNavigation)
                .Include(vb => vb.estadoNavigation)
                .ToListAsync();

            var valeBobinasDto = _mapper.Map<List<ValeBobinaDto>>(valeBobinas);

            return Ok(valeBobinasDto);
        }


        // GET api/<valeBobinaController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ValeBobinaDto>> GetValeBobina(int id)
        { 
            var valeBobina = await _context.valeBobina
                .OrderByDescending(vb => vb.idVale)
                .Include(vb => vb.idMaterialNavigation)
                .Include(vb => vb.estadoNavigation)
                .FirstOrDefaultAsync(vb => vb.idVale == id);

            var valeBobinaDto = _mapper.Map<ValeBobinaDto>(valeBobina);

            if (valeBobina == null)
            {
                return NotFound();
            }

            return Ok(valeBobinaDto);
        }

        // GET: buscador de campos de vale + idSolicitudMaterial
        [HttpGet("buscar/{termino}")]
        public async Task<ActionResult<IEnumerable<ValeBobinaDto>>> BuscarValeBobinaGlobal(string termino)
        {
            termino = termino.Trim();

            var terminosBusqueda = termino.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                          .Where(t => t.Length > 1)
                                          .ToList();

            if (!terminosBusqueda.Any())
            {
                terminosBusqueda.Add(termino);
            }

            var queryValesBobina = _context.valeBobina
                .AsNoTracking()
                .Include(vb => vb.idMaterialNavigation)
                .Include(vb => vb.estadoNavigation)
                .AsQueryable();

            foreach (var t in terminosBusqueda)
            {
                var terminoActual = t.ToLower();
                bool esNumero = int.TryParse(terminoActual, out int numActual);

                queryValesBobina = queryValesBobina.Where(u =>
                    (esNumero && u.idVale == numActual) ||
                    u.idVale.ToString().Contains(terminoActual) ||
                    (u.loteBobinaSAP != null && u.loteBobinaSAP.ToLower().Contains(terminoActual)) ||
                    (u.idMaterial != null && u.idMaterial.ToLower().Contains(terminoActual)) ||
                    (u.idMaterialNavigation != null && u.idMaterialNavigation.nombreMaterial != null && u.idMaterialNavigation.nombreMaterial.ToLower().Contains(terminoActual)) ||
                    (u.idMaterialNavigation != null && u.idMaterialNavigation.marca != null && u.idMaterialNavigation.marca.ToLower().Contains(terminoActual)) ||
                    (u.descripcionBobina != null && u.descripcionBobina.ToLower().Contains(terminoActual)) ||
                    (u.proveedorBobina != null && u.proveedorBobina.ToLower().Contains(terminoActual)) ||

                    (esNumero && _context.detalleReporte.Any(dr =>
                        dr.codBobina == u.loteBobinaSAP &&
                        _context.procesoOf.Any(p =>
                            p.idProceso == dr.idProceso &&
                            p.idSolicitudMateriales == numActual
                        )
                    ))
                );
            }

            var valeBobinas = await queryValesBobina
                .OrderByDescending(vb => vb.idVale)
                .Take(50) 
                .ToListAsync();

            if (!valeBobinas.Any())
            {
                return NotFound($"No se encontraron resultados para el término: {termino}");
            }

            var valeBobinasDto = _mapper.Map<List<ValeBobinaDto>>(valeBobinas);

            return Ok(valeBobinasDto);
        }

        [HttpGet("get/existencia/{loteBobina}")]
        public async Task<ActionResult<string>> GetExistenciaValeBobina(string loteBobina)
        {
            var valeBobina = await _context.valeBobina
                .FirstOrDefaultAsync(vb => vb.loteBobinaSAP == loteBobina);
            if (valeBobina == null)
            {
                return "false"; // No se encontraron registros
            }
            return "true"; // Se encontraron registros
        }

        // POST api/<valeBobinaController>
        //[HttpPost("post")]
        //public async Task<ActionResult<valeBobina>> PostValeBobina(AddValeBobinaDto addValeBobinaDto)
        //{
        //    var valeBobina = _mapper.Map<valeBobina>(addValeBobinaDto);
        //    _context.valeBobina.Add(valeBobina);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetValeBobina", new { id = valeBobina.idVale }, valeBobina);
        //}

        // POST api/<valeBobinaController>
        [HttpPost("post")]
        public async Task<ActionResult<valeBobina>> PostValeBobina(AddValeBobinaDto addValeBobinaDto)
        {
            // 1. Verificamos si el material ya existe en la base de datos
            var materialExiste = await _context.material.FindAsync(addValeBobinaDto.idMaterial);

            // 2. Si no existe, lo creamos "al vuelo"
            if (materialExiste == null)
            {
                var nuevoMaterial = new material
                {
                    idMaterial = addValeBobinaDto.idMaterial,
                    nombreMaterial = addValeBobinaDto.descripcionBobina
                };

                _context.material.Add(nuevoMaterial);

                // Guardamos el material primero para que la llave foránea se registre
                await _context.SaveChangesAsync();
            }

            // 3. Continuamos con el flujo normal de guardar el vale
            var valeBobina = _mapper.Map<valeBobina>(addValeBobinaDto);
            _context.valeBobina.Add(valeBobina);

            await _context.SaveChangesAsync();

            return CreatedAtAction("GetValeBobina", new { id = valeBobina.idVale }, valeBobina);
        }

        // PUT api/<valeBobinaController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutValeBobina(int id, UpdateValeBobinaDto updateValeBobinaDto)
        {
            var valeBobina = await _context.valeBobina.FindAsync(id);
            if (valeBobina == null)
            {
                return NotFound();
            }

            _mapper.Map(updateValeBobinaDto, valeBobina);
            _context.Entry(valeBobina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!valeBobinaExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        private bool valeBobinaExists(int id)
        {
            return _context.valeBobina.Any(e => e.idVale == id);
        }
    }
}
