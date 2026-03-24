using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos;
using AutoMapper;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnicaProcesos
{
    [Route("api/[controller]")]
    [ApiController]
    public class FichaTecnicaProcesosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public FichaTecnicaProcesosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<FichaTecnicaProcesosController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FichaTecnicaProcesosDto>>> GetFichaTecnicaProceso()
        {
            var fichaTecnicaProceso = await _context.fichaTecnicaProcesos
                .Include(f => f.detalleFichaProcesos)
                .Include(f => f.formulacionTinta)
                .ThenInclude(ft => ft.especificacionTintas)
                .Include(o => o.oFNavigation)
                .Include(u => u.operadorNavigation)
                .Include(t => t.formuladorTintaNavigation)
                .Include(s => s.secuenciaColor)
                .Include(r => r.registroLamparas)
                .ThenInclude(v => v.idVariableNavigation)
                .Include(e => e.estadoNavigation)
                .ToListAsync();

            var fichaTecnicaProcesoDto = _mapper.Map<List<FichaTecnicaProcesosDto>>(fichaTecnicaProceso);

            return Ok(fichaTecnicaProcesoDto);
        }

        // GET api/<FichaTecnicaProcesosController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FichaTecnicaProcesosDto>> GetFichaTecnicaProceso(int id)
        {
            var fichaTecnicaProceso = await _context.fichaTecnicaProcesos
                .Include(f => f.detalleFichaProcesos)
                .Include(f => f.formulacionTinta)
                .ThenInclude(ft => ft.especificacionTintas)
                .Include(o => o.oFNavigation)
                .Include(u => u.operadorNavigation)
                .Include(t => t.formuladorTintaNavigation)
                .Include(s => s.secuenciaColor)
                .Include(r => r.registroLamparas)
                .ThenInclude(v => v.idVariableNavigation)
                .Include(e => e.estadoNavigation)
                .FirstOrDefaultAsync(f => f.idFichaProceso == id);

            if (fichaTecnicaProceso == null)
            {
                return NotFound();
            }
            var fichaTecnicaProcesoDto = _mapper.Map<FichaTecnicaProcesosDto>(fichaTecnicaProceso);

            return Ok(fichaTecnicaProcesoDto);
        }

        // GET api/<FichaTecnicaProcesosController>/5
        [HttpGet("get/of/{of}/articulo/{codArticulo}")]
        public async Task<ActionResult<FichaTecnicaProcesosDto>> GetFichaTecnicaProcesoOfArticulo(int of, string codArticulo)
        {
            var fichaTecnicaProceso = await _context.fichaTecnicaProcesos
                .Include(f => f.detalleFichaProcesos)
                .Include(f => f.formulacionTinta)
                .ThenInclude(ft => ft.especificacionTintas)
                .Include(o => o.oFNavigation)
                .Include(u => u.operadorNavigation)
                .Include(t => t.formuladorTintaNavigation)
                .Include(s => s.secuenciaColor)
                .Include(r => r.registroLamparas)
                .ThenInclude(v => v.idVariableNavigation)
                .Include(e => e.estadoNavigation)
                .FirstOrDefaultAsync(f => f.oFNavigation.oF == of && f.oFNavigation.codArticulo == codArticulo);

            if (fichaTecnicaProceso == null)
            {
                return NotFound();
            }
            var fichaTecnicaProcesoDto = _mapper.Map<FichaTecnicaProcesosDto>(fichaTecnicaProceso);

            return Ok(fichaTecnicaProcesoDto);
        }

        // POST api/<FichaTecnicaProcesosController>
        [HttpPost("post")]
        public async Task<ActionResult<fichaTecnicaProcesos>> PostFichaTecnicaProcesos(AddFichaTecnicaProcesosDto addFichaTecnicaProcesosDto)
        {
            var fichaTecnicaProceso = _mapper.Map<fichaTecnicaProcesos>(addFichaTecnicaProcesosDto);

            _context.fichaTecnicaProcesos.Add(fichaTecnicaProceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetFichaTecnicaProceso", new { id = fichaTecnicaProceso.idFichaProceso }, fichaTecnicaProceso);
        }

        // PUT api/<FichaTecnicaProcesosController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutFichaTecnicaProcesos(int id, UpdateFichaTecnicaProcesosDto updateFichaTecnicaProcesosDto)
        {
            var fichaTecnicaProceso = await _context.fichaTecnicaProcesos.FindAsync(id);

            if (fichaTecnicaProceso == null)
            {
                return NotFound();
            }

            _mapper.Map(updateFichaTecnicaProcesosDto, fichaTecnicaProceso);
            _context.Entry(fichaTecnicaProceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!FichaTecnicaProcesoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateFichaTecnicaProcesosDto);
        }

        private bool FichaTecnicaProcesoExists(int id)
        {
            return _context.fichaTecnicaProcesos.Any(e => e.idFichaProceso == id);
        }
    }
}
