using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.ApiKey;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class tarjetaOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
                   
        public tarjetaOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tarjetaOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOf()
        {
            var tarjetaOf = await _context.tarjetaOf.ToListAsync();
            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
        }

        // GET: api/tarjetaOf/5
        [HttpGet("get/id/")]
        public async Task<ActionResult<TarjetaOfDto>> GettarjetaOf(int id)
        {
            var tarjetaOf = await _context.tarjetaOf.FindAsync(id);
            var tarjetaOfDto = _mapper.Map<TarjetaOfDto>(tarjetaOf);
            
            if (tarjetaOfDto == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            return Ok(tarjetaOfDto);
        }

        // PUT: api/tarjetaOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/")]
        public async Task<IActionResult> PuttarjetaOf(int id, UpdateTarjetaOfDto updateTarjetaOf)
        {
            var tarjetaOf = await _context.tarjetaOf.FindAsync(id);

            if (tarjetaOf == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            _mapper.Map(updateTarjetaOf, tarjetaOf);
            _context.Entry(tarjetaOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tarjetaOfExists(id))
                {
                    return NotFound("No se encontro la tarjeta con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTarjetaOf);
        }

        // POST: api/tarjetaOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tarjetaOf>> PosttarjetaOf(AddTarjetaOfDto addTarjetaOf)
        {
            var tarjetaOf = _mapper.Map<tarjetaOf>(addTarjetaOf);

            _context.tarjetaOf.Add(tarjetaOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetTarjetaOf", new { id = tarjetaOf.idTarjetaOf }, tarjetaOf);
        }

        // DELETE: api/tarjetaOf/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeletetarjetaOf(int id)
        {
            var tarjetaOf = await _context.tarjetaOf.FindAsync(id);
            if (tarjetaOf == null)
            {
                return NotFound();
            }

            _context.tarjetaOf.Remove(tarjetaOf);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool tarjetaOfExists(int id)
        {
            return _context.tarjetaOf.Any(e => e.idTarjetaOf == id);
        }
    }
}
