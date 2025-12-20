using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales;
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

        public solicitudMaterialesController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }


        // GET: api/<solicitudMaterialesController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesDto>>> GetSolicitud()
        {
            var solicitudMateriales = await _context.solicitudMateriales
                .Include(s => s.solicitudMaterialesOf)
                    .ThenInclude(so => so.oFNavigation)
                .ToListAsync();

            var solicitudMaterialesDto = _mapper.Map<List<solicitudMaterialesDto>>(solicitudMateriales);

            return Ok(solicitudMaterialesDto);
        }

        // GET api/<solicitudMaterialesController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesDto>>> GetSolicitudId(int id)
        {
            var solicitudMateriales = await _context.solicitudMateriales
                .Include(s => s.solicitudMaterialesOf)
                    .ThenInclude(so => so.oFNavigation)
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
                .Include(s => s.solicitudMaterialesOf)
                    .ThenInclude(so => so.oFNavigation)
                .FirstOrDefaultAsync();

            var solicitudMaterialesDto = _mapper.Map<solicitudMaterialesDto>(solicitudMateriales);

            if (solicitudMaterialesDto == null)
            {
                return NotFound($"No se encontro el registro con el id: {idSap}");
            }

            return Ok(solicitudMaterialesDto);
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

        private bool solicitudMaterialesExist(int id) 
        {
            return _context.solicitudMateriales.Any(e => e.idSolicitud == id);
        }

    }
}
