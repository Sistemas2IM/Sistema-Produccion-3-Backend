using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnicaCliente
{
    [Route("api/[controller]")]
    [ApiController]
    public class fichaTecnicaClienteController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public fichaTecnicaClienteController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<fichaTecnicaClienteController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FichaTecnicaClienteDto>>> GetFichaCliente()
        {
            var fichaTecnicaCliente = await _context.fichaTecnicaCliente
                .Include(f => f.detalleFichaClientes)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(f => f.detalleFichaClientes)
                .ThenInclude(d => d.idUnidadNavigation)
                .Include(f => f.oFNavigation)
                .Where(f => f.archivado == false)
                .ToListAsync();

            var fichaTecnicaClienteDto = _mapper.Map<List<FichaTecnicaClienteDto>>(fichaTecnicaCliente);

            return Ok(fichaTecnicaClienteDto);
        }

        // GET api/<fichaTecnicaClienteController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FichaTecnicaClienteDto>> GetFichaCliente(int id)
        {
            var fichaTecnicaCliente = await _context.fichaTecnicaCliente
                .Include(c => c.detalleFichaClientes)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(f => f.detalleFichaClientes)
                .ThenInclude(d => d.idUnidadNavigation)
                .Include(f => f.oFNavigation)
                .Where(f => f.archivado == false)
                .FirstOrDefaultAsync(u => u.idFichaCliente == id);

            if (fichaTecnicaCliente == null)
            {
                return NotFound();
            }

            var fichaTecnicaClienteDto = _mapper.Map<FichaTecnicaClienteDto>(fichaTecnicaCliente);
            return Ok(fichaTecnicaClienteDto);
        }

        // GET api/<fichaTecnicaClienteController>/5
        [HttpGet("get/of/{of}")]
        public async Task<ActionResult<IEnumerable<FichaTecnicaClienteDto>>> GetFichaClienteOf(int of)
        {
            var fichaTecnicaCliente = await _context.fichaTecnicaCliente            
                .Include(f => f.detalleFichaClientes)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(f => f.oFNavigation)
                .Where(f => f.oF == of || f.archivado == false)
                .ToListAsync();

            var fichaTecnicaClienteDto = _mapper.Map<List<FichaTecnicaClienteDto>>(fichaTecnicaCliente);

            return Ok(fichaTecnicaClienteDto);
        }

        [HttpGet("get/lineaNegocio/{linea}")]
        public async Task<ActionResult<IEnumerable<FichaTecnicaClienteDto>>> GetFichaClienteLineaNegocio(bool linea)
        {
            // 1. Preparamos la consulta base (sin el ToListAsync todavía)
            var query = _context.fichaTecnicaCliente
                .Include(f => f.detalleFichaClientes)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(f => f.detalleFichaClientes)
                .ThenInclude(d => d.idUnidadNavigation)
                .Include(f => f.oFNavigation)
                .Where(f => f.archivado == false);

            // 2. Aplicamos la lógica del booleano sobre la query
            // NOTA: Reemplaza 'NombreLineaNegocio' por el nombre real de la propiedad en tu tabla OF
            if (linea)
            {
                // Si es TRUE: Trae SOLO las que son FLEXO
                query = query.Where(f => f.oFNavigation.lineaDeNegocio == "FLEXO");
            }
            else
            {
                // Si es FALSE: Trae todas MENOS las que son FLEXO
                query = query.Where(f => f.oFNavigation.lineaDeNegocio != "FLEXO");
            }

            // 3. Ejecutamos la consulta en la Base de Datos
            var fichaTecnicaCliente = await query.ToListAsync();

            // 4. Mapeo y retorno
            var fichaTecnicaClienteDto = _mapper.Map<List<FichaTecnicaClienteDto>>(fichaTecnicaCliente);

            return Ok(fichaTecnicaClienteDto);
        }

        // POST api/<fichaTecnicaClienteController>
        [HttpPost("post")]
        public async Task<ActionResult<fichaTecnicaCliente>> PostFichaCliente(AddFichaTecnicaClienteDto fichaTecnicaClienteDto)
        {
            var fichaTecnicaCliente = _mapper.Map<fichaTecnicaCliente>(fichaTecnicaClienteDto);

            _context.fichaTecnicaCliente.Add(fichaTecnicaCliente);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFichaCliente", new { id = fichaTecnicaCliente.idFichaCliente }, fichaTecnicaCliente);
        }

        // PUT api/<fichaTecnicaClienteController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFichaCliente(int id, UpdateFichaTecnicaClienteDto updateFichaTecnicaClienteDto)
        {
            var fichaTecnicaCliente = await _context.fichaTecnicaCliente.FindAsync(id);

            if (fichaTecnicaCliente == null)
            {
                return NotFound();
            }

            _mapper.Map(updateFichaTecnicaClienteDto, fichaTecnicaCliente);
            _context.Entry(fichaTecnicaCliente).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!fichaTecnicaClienteExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateFichaTecnicaClienteDto);
        }

        private bool fichaTecnicaClienteExists(int id)
        {
            return _context.fichaTecnicaCliente.Any(e => e.idFichaCliente == id);
        }
    }
}
