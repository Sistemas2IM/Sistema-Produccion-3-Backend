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
    public class articuloOvController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public articuloOvController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/articuloOv
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ArticuloDto>>> GetarticuloOv()
        {
            var articuloOv = await _context.articuloOv.ToListAsync();
            var articuloOvDto = _mapper.Map<List<ArticuloDto>>(articuloOv);

            return Ok(articuloOvDto);
        }

        // GET: api/articuloOv/5
        [HttpGet("get/id/{id}")]
        public async Task<ActionResult<ArticuloDto>> GetarticuloOv(int id)
        {
            var articuloOv = await _context.articuloOv.FindAsync(id);
            var articuloOvDto = _mapper.Map<ArticuloDto>(articuloOv);

            if (articuloOvDto == null)
            {
                return NotFound("No se encontro el articulo con el id: " + id);
            }

            return Ok(articuloOvDto);
        }

        // PUT: api/articuloOv/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> PutarticuloOv(int id, articuloOv articuloOv)
        {
            if (id != articuloOv.idArticulo)
            {
                return BadRequest();
            }

            _context.Entry(articuloOv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!articuloOvExists(id))
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

        // POST: api/articuloOv
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<articuloOv>> PostarticuloOv(AddArticuloDto addArticuloOv)
        {
            var articuloOv = _mapper.Map<articuloOv>(addArticuloOv);

            _context.articuloOv.Add(articuloOv);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetarticuloOv", new { id = articuloOv.idArticulo }, articuloOv);
        }

        // DELETE: api/articuloOv/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> DeletearticuloOv(int id)
        {
            var articuloOv = await _context.articuloOv.FindAsync(id);
            if (articuloOv == null)
            {
                return NotFound();
            }

            _context.articuloOv.Remove(articuloOv);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool articuloOvExists(int id)
        {
            return _context.articuloOv.Any(e => e.idArticulo == id);
        }
    }
}
