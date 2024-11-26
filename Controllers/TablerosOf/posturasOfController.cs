using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection.Metadata.Ecma335;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class posturasOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public posturasOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/posturasOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<PosturasOfDto>>> GetposturasOf()
        {
            var postura = await _context.posturasOf
                .Include(p => p.idTableroNavigation)
                .ToListAsync();

            var posturaDto = _mapper.Map<List<PosturasOfDto>>(postura);

            return Ok(posturaDto);
        }     

        // GET: api/posturasOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PosturasOfDto>> GetposturasOf(int id)
        {
            var postura = await _context.posturasOf
                .Include(p => p.idTableroNavigation)
                .FirstOrDefaultAsync(u => u.idPostura == id);

            var posturaDto = _mapper.Map<PosturasOfDto>(postura);

            if (posturaDto == null)
            {
                return NotFound("No se encontro la Postura con el ID: " + id);
            }

            return Ok(posturaDto);
        }

        // PUT: api/posturasOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutposturasOf(int id, UpdatePosturasOfDto updatePosturasOf)
        {
            var postura = await _context.posturasOf.FindAsync(id);

            if (postura == null)
            {
                return NotFound("No se encontro la Postura con el ID: " + id);
            }

            _mapper.Map(updatePosturasOf, postura);
            _context.Entry(postura).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!posturasOfExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updatePosturasOf);
        }

        // POST: api/posturasOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<posturasOf>> PostposturasOf(AddPosturasOfDto addPosturasOf)
        {
            var postura = _mapper.Map<posturasOf>(addPosturasOf);
            _context.posturasOf.Add(postura);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetposturasOf", new { id = postura.idPostura }, postura);
        }       

        private bool posturasOfExists(int id)
        {
            return _context.posturasOf.Any(e => e.idPostura == id);
        }
    }
}
