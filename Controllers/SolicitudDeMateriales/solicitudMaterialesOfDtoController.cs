using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class solicitudMaterialesOfDtoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public solicitudMaterialesOfDtoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<solicitudMaterialesOfDtoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesOfDto>>> GetSolicitud()
        {
            var solicitudMaterialesOf = await _context.solicitudMaterialesOf
                .Include(of => of.oFNavigation)
                .ToListAsync();
            var solicitudMaterialesOfDto = _mapper.Map<List<solicitudMaterialesOfDto>>(solicitudMaterialesOf);

            return Ok(solicitudMaterialesOfDto);
        }

        // GET api/<solicitudMaterialesOfDtoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<IEnumerable<solicitudMaterialesOfDto>>> GetSolicitudId(int id)
        {
            var solicitudMaterialesOf = await _context.solicitudMaterialesOf
                .Include(of => of.oFNavigation)
                .FirstOrDefaultAsync(e => e.idSolicitud == id);

            var solicitudMaterialesOfDto = _mapper.Map<solicitudMaterialesOfDto>(solicitudMaterialesOf);

            if (solicitudMaterialesOfDto == null)
            {
                return NotFound($"No se encontro el registro con el id: {id}");
            }

            return Ok(solicitudMaterialesOfDto);
        }

        // POST api/<solicitudMaterialesOfDtoController>
        [HttpPost("post")]
        public async Task<ActionResult<solicitudMaterialesOfDto>> PostSolicitud(AddSolicitudMaterialesOfDto solicitudMaterialesOfDto)
        {
            var solicitudMaterialesOf = _mapper.Map<solicitudMaterialesOf>(solicitudMaterialesOfDto);
            _context.solicitudMaterialesOf.Add(solicitudMaterialesOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSolicitud", new { id = solicitudMaterialesOf.idSolicitud }, solicitudMaterialesOf);
        }

        // PUT api/<solicitudMaterialesOfDtoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutetiquetaOf(int id, UpdateSolicitudMaterialesOfDto solicitudMaterialesOfDto)
        {
            var solicitudMaterialesOf = await _context.solicitudMaterialesOf.FindAsync(id);
            if (solicitudMaterialesOf == null)
            {
                return NotFound($"No se encontro la etiqueta con el id {id}");
            }

            _mapper.Map(solicitudMaterialesOfDto, solicitudMaterialesOf);
            _context.Entry(solicitudMaterialesOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!solicitudMaterialesOfExist(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(solicitudMaterialesOfDto);
        }

        private bool solicitudMaterialesOfExist(int id)
        {
            return _context.solicitudMaterialesOf.Any(e => e.idSolicitud == id);
        }

    }
}
