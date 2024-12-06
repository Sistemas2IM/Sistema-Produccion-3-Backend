using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetallePegado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetallePegado
{
    [Route("api/[controller]")]
    [ApiController]
    public class detallePegadoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detallePegadoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detallePegado
        [HttpGet("get/IdFicha/{id}")]
        public async Task<ActionResult<IEnumerable<DetallePegadoDto>>> GetdetallePegadoFicha(int id)
        {
            var detallePegado = await _context.detallePegado
                .Where(i => i.idFichaTecnica == id)
                .Include(u => u.tipoPega)
                .Include(o => o.tipoPegado)
                .ToListAsync();

            var detallePegadoDto = _mapper.Map<List<DetallePegadoDto>>(detallePegado);

            return Ok(detallePegadoDto);
        }

        // GET: api/detallePegado/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetallePegadoDto>> GetdetallePegado(int id)
        {
            var detallePegado = await _context.detallePegado
                .Include(u => u.tipoPega)
                .Include(o => o.tipoPegado)
                .FirstOrDefaultAsync(u => u.idDetallePegado == id);

            var detallePegadoDto = _mapper.Map<DetallePegadoDto>(detallePegado);

            if (detallePegadoDto == null)
            {
                return NotFound($"No se encontro el Detalle de Pegado con el ID: {id}");
            }
            return Ok(detallePegadoDto);
        }

        // PUT: api/detallePegado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetallePegado(int id, UpdateDetallePegadoDto updateDetallePegado)
        {
            var detallePegado = await _context.detallePegado.FindAsync(id);

            if (detallePegado == null)
            {
                return NotFound($"No se encontro el Detalle de Pegado con el Id: {id}");
            }

            _mapper.Map(updateDetallePegado, detallePegado);
            _context.Entry(detallePegado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detallePegadoExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateDetallePegado);
        }

        // POST: api/detallePegado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detallePegado>> PostdetallePegado(AddDetallePegadoDto addDetallePegado)
        {
            var detallePegado = _mapper.Map<detallePegado>(addDetallePegado);
            _context.detallePegado.Add(detallePegado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetallePegado", new { id = detallePegado.idDetallePegado }, detallePegado);
        }       

        private bool detallePegadoExists(int id)
        {
            return _context.detallePegado.Any(e => e.idDetallePegado == id);
        }
    }
}
