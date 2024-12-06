using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetallePegado.TipoPega;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetallePegado.TipoPega
{
    [Route("api/[controller]")]
    [ApiController]
    public class tipoPegaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public tipoPegaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/tipoPega
        [HttpGet("get/DetallePegado/{id}")]
        public async Task<ActionResult<IEnumerable<TipoPegaDto>>> GettipoPegaPegado(int id)
        {
            var tipoPega = await _context.tipoPega
                .Where(u => u.idDetallePegado == id)
                .ToListAsync();

            var tipoPegaDto = _mapper.Map<List<TipoPegaDto>>(tipoPega);

            return Ok(tipoPegaDto);
        }

        // GET: api/tipoPega/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<tipoPega>> GettipoPega(int id)
        {
            var tipoPega = await _context.tipoPega
                .FindAsync(id);

            var tipoPegaDto = _mapper.Map<TipoPegaDto>(tipoPega);

            if (tipoPegaDto == null)
            {
                return NotFound($"No se encontro el detalle de tipo de pega con el ID {id}");
            }

            return Ok(tipoPegaDto);
        }

        // PUT: api/tipoPega/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PuttipoPega(int id, UpdateTipoPegaDto updateTipoPega)
        {
            var tipoPega = await _context.tipoPega.FindAsync(id);

            if (tipoPega == null)
            {
                return NotFound($"No se encontro el detalle de tipo de pega con el ID {id}");
            }

            _mapper.Map(updateTipoPega, tipoPega);
            _context.Entry(tipoPega).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!tipoPegaExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateTipoPega);
        }

        // POST: api/tipoPega
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<tipoPega>> PosttipoPega(AddTipoPegaDto addTipoPega)
        {
            var tipoPega = _mapper.Map<tipoPega>(addTipoPega);
            _context.tipoPega.Add(tipoPega);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GettipoPega", new { id = tipoPega.idTipoPega }, tipoPega);
        }      

        private bool tipoPegaExists(int id)
        {
            return _context.tipoPega.Any(e => e.idTipoPega == id);
        }
    }
}
