using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleTroquelado.TipoPleca;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleTroquelado.TipoPleca
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoPlecaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoPlecaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tipoPleca
        [HttpGet("get/DetalleTroquelado/{id}")]
        public async Task<ActionResult<IEnumerable<TipoPlecaDto>>> GettipoPlecaTroqueladora(int id)
        {
            var tipoPleca = await _context.tipoPleca
                .Where(u => u.idDetalleTroquelado == id)
                .ToListAsync();

            var tipoPlecaDto = _mapper.Map<List<TipoPlecaDto>>(tipoPleca);

            return Ok(tipoPlecaDto);
        }

        // GET: api/tipoPleca/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TipoPlecaDto>> GettipoPleca(int id)
        {
            var tipoPleca = await _context.tipoPleca
                .FindAsync(id);

            var tipoPlecaDto = _mapper.Map<TipoPlecaDto>(tipoPleca);

            if (tipoPlecaDto == null)
            {
                return NotFound($"No se encontro el tipo de pleca con el ID: {id}");
            }

            return Ok(tipoPlecaDto);
        }

        // PUT: api/tipoPleca/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttipoPleca(int id, UpdateTipoPlecaDto updateTipoPleca)
        {
            var tipoPleca = await _context.tipoPleca.FindAsync(id);

            if (tipoPleca == null)
            {
                return NotFound($"No se encontro el tipo de pleca con el ID: {id}");
            }

            _mapper.Map(updateTipoPleca, tipoPleca);
            _context.Entry(tipoPleca).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tipoPlecaExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateTipoPleca);
        }

        // POST: api/tipoPleca
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tipoPleca>> PosttipoPleca(AddTipoPlecaDto addTipoPleca)
        {
            var tipoPleca =  _mapper.Map<tipoPleca>(addTipoPleca);
            _context.tipoPleca.Add(tipoPleca);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettipoPleca", new { id = tipoPleca.idTipoPleca }, tipoPleca);
        }

        private bool tipoPlecaExists(int id)
        {
            return _context.tipoPleca.Any(e => e.idTipoPleca == id);
        }
    }
}
