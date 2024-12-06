using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetallePegado.TipoPegado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetallePegado.TipoPegado
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoPegadoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoPegadoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tipoPegado
        [HttpGet("get/DetallePegado/{id}")]
        public async Task<ActionResult<IEnumerable<TipoPegadoDto>>> GettipoPegadoPegadora(int id)
        {
            var tipoPegado = await _context.tipoPegado
                .Where(u => u.idDetallePegado == id)
                .ToListAsync();

            var tipoPegadoDto = _mapper.Map<List<TipoPegadoDto>>(tipoPegado);

            return Ok(tipoPegadoDto);
        }

        // GET: api/tipoPegado/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<TipoPegadoDto>> GettipoPegado(int id)
        {
            var tipoPegado = await _context.tipoPegado
                .FindAsync(id);

            var tipoPegadoDto = _mapper.Map<TipoPegadoDto>(tipoPegado);

            if (tipoPegadoDto == null)
            {
                return NotFound($"No se encontro el tipo de pegado con el ID {id}");
            }

            return Ok(tipoPegadoDto);
        }

        // PUT: api/tipoPegado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttipoPegado(int id, UpdateTipoPegadoDto updateTipoPegado)
        {
            var tipoPegado = await _context.tipoPegado.FindAsync(id);

            if (tipoPegado == null)
            {
                return NotFound($"No se encontro el tipo de pegado con el ID: {id}");
            }

            _mapper.Map(updateTipoPegado, tipoPegado);
            _context.Entry(tipoPegado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tipoPegadoExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateTipoPegado);
        }

        // POST: api/tipoPegado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tipoPegado>> PosttipoPegado(AddTipoPegadoDto addTipoPegado)
        {
            var tipoPegado = _mapper.Map<tipoPegado>(addTipoPegado);
            _context.tipoPegado.Add(tipoPegado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettipoPegado", new { id = tipoPegado.idTipoPegado }, tipoPegado);
        }

        private bool tipoPegadoExists(int id)
        {
            return _context.tipoPegado.Any(e => e.idTipoPegado == id);
        }
    }
}
