using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica
{
    [Route("api/[controller]")]
    [ApiController]
    public class fichaTecnicaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public fichaTecnicaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/fichaTecnica
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FichaTecnicaDto>>> GetfichaTecnica()
        {
            var fichaTecnica = await _context.fichaTecnica
                .ToListAsync();

            var fichaTecnicaDto = _mapper.Map<List<FichaTecnicaDto>>(fichaTecnica);

            return Ok(fichaTecnicaDto);
        }

        // GET: api/fichaTecnica/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FichaTecnicaDto>> GetfichaTecnica(int id)
        {
            var fichaTecnica = await _context.fichaTecnica
                .FirstOrDefaultAsync(u => u.idFichaTecnica == id);

            var fichaTecnicaDto = _mapper.Map<FichaTecnicaDto>(fichaTecnica);

            if (fichaTecnicaDto == null)
            {
                return NotFound($"No se encontro la Ficha Tecnica con el ID: {id}");
            }

            return Ok(fichaTecnicaDto);
        }

        // PUT: api/fichaTecnica/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutfichaTecnica(int id, UpdateFichaTecnicaDto updateFichaTecnica)
        {
            var fichaTecnica = await _context.fichaTecnica.FindAsync(id);

            if (fichaTecnica == null)
            {
                return NotFound($"No se encontro la Ficha Tecnica con el ID: {id}");
            }

            _mapper.Map(updateFichaTecnica, fichaTecnica);
            _context.Entry(fichaTecnica).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!fichaTecnicaExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro de Ficha");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateFichaTecnica);
        }

        // POST: api/fichaTecnica
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<fichaTecnica>> PostfichaTecnica(AddFichaTecnicaDto addFichaTecnica)
        {
            var fichaTecnica = _mapper.Map<fichaTecnica>(addFichaTecnica);
            _context.fichaTecnica.Add(fichaTecnica);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetfichaTecnica", new { id = fichaTecnica.idFichaTecnica }, fichaTecnica);
        }

        private bool fichaTecnicaExists(int id)
        {
            return _context.fichaTecnica.Any(e => e.idFichaTecnica == id);
        }
    }
}
