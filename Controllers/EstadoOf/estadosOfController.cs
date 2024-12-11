using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.EstadoOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.EstadoOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class estadosOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public estadosOfController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/estadosOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EstadoOfDto>>> GetestadosOf()
        {
            var estadoof = await _context.estadosOf
                .ToListAsync();

            var estadoOfDto = _mapper.Map<List<EstadoOfDto>>(estadoof);

            return Ok(estadoOfDto);
        }

        // GET: api/estadosOf/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EstadoOfDto>> GetestadosOf(int id)
        {
            var estadosOf = await _context.estadosOf.FindAsync(id);

            var estadoOfDto = _mapper.Map<EstadoOfDto>(estadosOf);

            if (estadoOfDto == null)
            {
                return NotFound($"No se encontro el estado con el ID: {id}");
            }

            return Ok(estadoOfDto);
        }

        // PUT: api/estadosOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutestadosOf(int id, UpdateEstadoOfDto updateEstadosOf)
        {
            var estadoOf = await _context.estadosOf.FindAsync(id);

            if (estadoOf == null)
            {
                return NotFound($"No se encontro el estado con el ID");
            }

            _mapper.Map(updateEstadosOf, estadoOf);
            _context.Entry(estadoOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!estadosOfExists(id))
                {
                    return BadRequest($"El ID {id}, no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateEstadosOf);
        }

        // POST: api/estadosOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<estadosOf>> PostestadosOf(AddEstadoOfDto addEstadosOf)
        {
            var estadoOf = _mapper.Map<estadosOf>(addEstadosOf);
            _context.estadosOf.Add(estadoOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetestadosOf", new { id = estadoOf.idEstadoOf }, estadoOf);
        }

        private bool estadosOfExists(int id)
        {
            return _context.estadosOf.Any(e => e.idEstadoOf == id);
        }
    }
}
