using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Ov
{
    [Route("api/[controller]")]
    [ApiController]
    public class oVController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public oVController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/oV
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<OVDto>>> GetoV()
        {
            var oV = await _context.oV.ToListAsync();
            var oVDto = _mapper.Map<List<OVDto>>(oV);

            return Ok(oVDto);
        }

        // GET: api/oV/5
        [HttpGet("get/id/{id}")]
        public async Task<ActionResult<OVDto>> GetoV(int id)
        {
            var oV = await _context.oV.FindAsync(id);
            var oVDto = _mapper.Map<OVDto>(oV);

            if (oVDto == null)
            {
                return NotFound("No se encontro la tarjeta con el id: " + id);
            }

            return Ok(oVDto);
        }

        // PUT: api/oV/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutoV(int id, oV oV)
        {
            if (id != oV.idOv)
            {
                return BadRequest();
            }

            _context.Entry(oV).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!oVExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/oV
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<oV>> PostoV(AddOVDto addOv)
        {
            var oV = _mapper.Map<oV>(addOv);
            _context.oV.Add(oV);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetoV", new { id = oV.idOv }, oV);
        }

        // DELETE: api/oV/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeleteoV(int id)
        {
            var oV = await _context.oV.FindAsync(id);
            if (oV == null)
            {
                return NotFound();
            }

            _context.oV.Remove(oV);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool oVExists(int id)
        {
            return _context.oV.Any(e => e.idOv == id);
        }
    }
}
