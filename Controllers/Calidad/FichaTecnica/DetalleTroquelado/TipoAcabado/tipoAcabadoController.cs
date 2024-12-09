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
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TipoAcabadoDto>> GettipoAcabado(int id)
        {
            var tipoAcabado = await _context.tipoAcabado
                .FindAsync(id);

            var tipoAcabadoDto = _mapper.Map<TipoAcabadoDto>(tipoAcabado);

            if (tipoAcabadoDto == null)
            {
                return NotFound($"No se encontro el tipo de acabado con el ID {id}");
            }

            return Ok(tipoAcabadoDto);
        }

        // PUT: api/tipoAcabado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttipoAcabado(int id, UpdateTipoAcabadoDto updateTipoAcabado)
        {
            var tipoAcabado = await _context.tipoAcabado.FindAsync(id);

            if (tipoAcabado == null)
            {
                return NotFound($"No se encontro el tipo de acabado con el ID: {id}");
            }

            _mapper.Map(updateTipoAcabado, tipoAcabado);
            _context.Entry(tipoAcabado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tipoAcabadoExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateTipoAcabado);
        }

        // POST: api/tipoAcabado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tipoAcabado>> PosttipoAcabado(AddTipoAcabadoDto addTipoAcabado)
        {
            var tipoAcabado = _mapper.Map<tipoAcabado>(addTipoAcabado);
            _context.tipoAcabado.Add(tipoAcabado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettipoAcabado", new { id = tipoAcabado.idTipoAcabado }, tipoAcabado);
        }
      
        private bool tipoAcabadoExists(int id)
        {
            return _context.tipoAcabado.Any(e => e.idTipoAcabado == id);
        }
    }
}
