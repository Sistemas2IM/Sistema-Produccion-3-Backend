using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Turnos;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class turnosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public turnosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/turnos
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<TurnosDto>>> Getturnos()
        {
            var turnos = await _context.turnos.ToListAsync();
            var turnosDto = _mapper.Map<TurnosDto>(turnos);

            return Ok(turnosDto);
        }

        // GET: api/turnos/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TurnosDto>> Getturnos(int id)
        {
            var turnos = await _context.turnos.FindAsync(id);
            var turnosDto = _mapper.Map<TurnosDto>(turnos);

            if (turnosDto == null)
            {
                return NotFound($"No se encontro el turno con el ID: {id}");
            }

            return Ok(turnosDto);
        }

        // PUT: api/turnos/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putturnos(int id, UpdateTurnosDto updateTurnos)
        {
            var turnos = await _context.turnos.FindAsync(id);

            if (turnos == null)
            {
                return NotFound($"No se encontro el turno con el ID: {id}");
            }

            _mapper.Map(updateTurnos, turnos);
            _context.Entry(turnos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!turnosExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTurnos);
        }

        // POST: api/turnos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<turnos>> Postturnos(AddTurnosDto addTurnos)
        {
            var turnos = _mapper.Map<turnos>(addTurnos);
            _context.turnos.Add(turnos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getturnos", new { id = turnos.id }, turnos);
        }
      
        private bool turnosExists(int id)
        {
            return _context.turnos.Any(e => e.id == id);
        }
    }
}
