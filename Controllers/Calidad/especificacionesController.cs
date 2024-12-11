using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class especificacionesController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public especificacionesController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/especificaciones
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EspecificacionesCerDto>>> Getespecificaciones()
        {
            var especificacionesCer = await _context.especificaciones
                .ToListAsync();

            var especificacionesCerDto = _mapper.Map<List<EspecificacionesCerDto>>(especificacionesCer);

            return Ok(especificacionesCerDto);
        }
        
        // GET: api/especificaciones/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EspecificacionesCerDto>> Getespecificaciones(int id)
        {
            var especificacionesCer = await _context.especificaciones
                .FindAsync(id);

            var especificacionesCerDto = _mapper.Map<EspecificacionesCerDto>(especificacionesCer);

            if (especificacionesCerDto == null)
            {
                return Ok($"No se encontro la especificacion con el ID: {id}");
            }

            return Ok(especificacionesCerDto);
        }

        // PUT: api/especificaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putespecificaciones(int id, UpdateEspecificacionesCerDto updateEspecificaciones)
        {
            var especificacionesCer = await _context.especificaciones.FindAsync(id);

            if (especificacionesCer == null)
            {
                return NotFound($"No se encontro la especificacion con el ID: {id}");
            }

            _mapper.Map(updateEspecificaciones, especificacionesCer);
            _context.Entry(especificacionesCer).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!especificacionesExists(id))
                {
                    return NotFound($"No se encontro la especificacion con el ID: {id}");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateEspecificaciones);
        }

        // POST: api/especificaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<especificaciones>> Postespecificaciones(AddEspecificacionesCerDto addEspecificaciones)
        {
            var especificacionesCer = _mapper.Map<especificaciones>(addEspecificaciones);
            _context.especificaciones.Add(especificacionesCer);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getespecificaciones", new { id = especificacionesCer.idCaracterista }, especificacionesCer);
        }

        private bool especificacionesExists(int id)
        {
            return _context.especificaciones.Any(e => e.idCaracterista == id);
        }
    }
}
