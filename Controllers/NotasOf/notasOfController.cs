using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.NotasOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.NotasOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class notasOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public notasOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/notasOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<NotasDto>>> GetnotasOf()
        {
            var notas = await _context.notasOf.ToListAsync();
            var notasDto = _mapper.Map<List<NotasDto>>(notas);

            return Ok(notasDto);
        }

        // GET: api/notasOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<NotasDto>> GetnotasOf(int id)
        {
            var notasOf = await _context.notasOf.FindAsync(id);
            var notasDto = _mapper.Map<NotasDto>(notasOf);

            if (notasDto == null)
            {
                return NotFound($"No se encontro la nota con el id {id}");
            }

            return Ok(notasDto);
        }

        // PUT: api/notasOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutnotasOf(int id, UpdateNotasDto updateNotasOf)
        {
            var notas = await _context.notasOf.FindAsync(id);

            if (notas == null)
            {
                return NotFound($"No se encontro la nota con el id {id}");
            }

            _mapper.Map(updateNotasOf, notas);
            _context.Entry(notas).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!notasOfExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateNotasOf);
        }

        // POST: api/notasOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<notasOf>> PostnotasOf(AddNotasDto addNotasOf)
        {
            var notas = _mapper.Map<notasOf>(addNotasOf);
            _context.notasOf.Add(notas);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetnotasOf", new { id = notas.idComentario }, notas);
        }
      
        private bool notasOfExists(int id)
        {
            return _context.notasOf.Any(e => e.idComentario == id);
        }
    }
}
