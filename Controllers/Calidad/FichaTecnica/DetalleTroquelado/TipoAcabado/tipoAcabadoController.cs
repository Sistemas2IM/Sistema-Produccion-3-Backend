using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleTroquelado.TipoAcabado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleTroquelado.TipoAcabado
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoAcabadoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoAcabadoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tipoAcabado
        [HttpGet("get/DetalleTroquelado/{id}")]
        public async Task<ActionResult<IEnumerable<TipoAcabadoDto>>> GettipoAcabadoTroqueladora(int id)
        {
            var tipoAcabado = await _context.tipoAcabado
                .Where(u => u.idDetalleTroquelado == id)
                .ToListAsync();

             var tipoAcabadoDto = _mapper.Map<List<TipoAcabadoDto>>(tipoAcabado);

            return Ok(tipoAcabadoDto);
        }

        // GET: api/tipoAcabado/5
        [HttpGet("{id}")]
        public async Task<ActionResult<tipoAcabado>> GettipoAcabado(int id)
        {
            var tipoAcabado = await _context.tipoAcabado.FindAsync(id);

            if (tipoAcabado == null)
            {
                return NotFound();
            }

            return tipoAcabado;
        }

        // PUT: api/tipoAcabado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PuttipoAcabado(int id, tipoAcabado tipoAcabado)
        {
            if (id != tipoAcabado.idTipoAcabado)
            {
                return BadRequest();
            }

            _context.Entry(tipoAcabado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tipoAcabadoExists(id))
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

        // POST: api/tipoAcabado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<tipoAcabado>> PosttipoAcabado(tipoAcabado tipoAcabado)
        {
            _context.tipoAcabado.Add(tipoAcabado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettipoAcabado", new { id = tipoAcabado.idTipoAcabado }, tipoAcabado);
        }

        // DELETE: api/tipoAcabado/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeletetipoAcabado(int id)
        {
            var tipoAcabado = await _context.tipoAcabado.FindAsync(id);
            if (tipoAcabado == null)
            {
                return NotFound();
            }

            _context.tipoAcabado.Remove(tipoAcabado);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool tipoAcabadoExists(int id)
        {
            return _context.tipoAcabado.Any(e => e.idTipoAcabado == id);
        }
    }
}
