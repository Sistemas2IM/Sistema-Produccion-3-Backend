using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleTroquelado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleTroquelado
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleTroqueladoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleTroqueladoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleTroquelado
        [HttpGet("get/IdFicha/{id}")]
        public async Task<ActionResult<IEnumerable<DetalleTroqueladoDto>>> GetdetalleTroqueladoFicha(int id)
        {
            var detalleTroquelado = await _context.detalleTroquelado
                .Where(u => u.idFichaTecnica == id)
                .Include(i => i.tipoAcabado)
                .Include(o => o.tipoPleca)
                .ToListAsync();

            var detalleTroqueladoDto = _mapper.Map<List<DetalleTroqueladoDto>>(detalleTroquelado);

            return Ok(detalleTroqueladoDto);
        }

        // GET: api/detalleTroquelado/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleTroqueladoDto>> GetdetalleTroquelado(int id)
        {
            var detalleTroquelado = await _context.detalleTroquelado
                .Include(i => i.tipoAcabado)
                .Include(o => o.tipoPleca)
                .FirstOrDefaultAsync(u => u.idDetalleTroquelado == id);

            var detalleTroqueladoDto = _mapper.Map<DetalleTroqueladoDto>(detalleTroquelado);

            if (detalleTroqueladoDto == null)
            {
                return NotFound($"No se encontro el Detalle de troquelado con el ID: {id}");
            }

            return Ok(detalleTroqueladoDto);
        }

        // PUT: api/detalleTroquelado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleTroquelado(int id, UpdateDetalleTroqueladoDto updateDetalleTroquelado)
        {
            var detalleTroquelado = await _context.detalleTroquelado.FindAsync(id);

            if (detalleTroquelado == null)
            {
                return NotFound($"No se encontro el Detalle de troquelado con el ID: {id}");
            }

            _mapper.Map(updateDetalleTroquelado, detalleTroquelado);
            _context.Entry(detalleTroquelado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleTroqueladoExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateDetalleTroquelado);
        }

        // POST: api/detalleTroquelado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<detalleTroquelado>> PostdetalleTroquelado(AddDetalleTroqueladoDto addDetalleTroquelado)
        {
            var detalleTroquelado = _mapper.Map<detalleTroquelado>(addDetalleTroquelado);
            _context.detalleTroquelado.Add(detalleTroquelado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleTroquelado", new { id = detalleTroquelado.idDetalleTroquelado }, detalleTroquelado);
        }


        private bool detalleTroqueladoExists(int id)
        {
            return _context.detalleTroquelado.Any(e => e.idDetalleTroquelado == id);
        }
    }
}
