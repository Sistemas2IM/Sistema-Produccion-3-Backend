using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.DetalleSecado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleImpresion.DetalleSecado
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleSecadoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleSecadoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleSecado
        [HttpGet("get/DetalleImpresion/{id}")]
        public async Task<ActionResult<IEnumerable<DetalleSecadoDto>>> GetdetalleSecadoImpresion(int id)
        {
            var detalleSecado = await _context.detalleSecado
                .Where(u => u.idDetalleImpresion == id)
                .ToListAsync();

            var detalleSecadoDto = _mapper.Map<List<DetalleSecadoDto>>(detalleSecado);

            return Ok(detalleSecadoDto);
        }

        // GET: api/detalleSecado/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleSecadoDto>> GetdetalleSecado(int id)
        {
            var detalleSecado = await _context.detalleSecado
                .FirstOrDefaultAsync(u => u.idSecadoMaquina == id);

            var detalleSecadoDto = _mapper.Map<DetalleSecadoDto>(detalleSecado);

            if (detalleSecadoDto == null)
            {
                return NotFound($"No se encontro el detalle de secado con el ID: {id}");
            }

            return Ok(detalleSecadoDto);
        }

        // PUT: api/detalleSecado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleSecado(int id, UpdateDetalleSecado updateDetalleSecado)
        {
            var detalleSecado = await _context.detalleSecado.FindAsync(id);

            if (detalleSecado == null)
            {
                return NotFound($"No se encontro el detalle de secado con el ID: {id}");
            }

            _mapper.Map(updateDetalleSecado, detalleSecado);
            _context.Entry(detalleSecado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleSecadoExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateDetalleSecado);
        }

        // POST: api/detalleSecado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detalleSecado>> PostdetalleSecado(AddDetalleSecado addDetalleSecado)
        {
            var detalleSecado = _mapper.Map<detalleSecado>(addDetalleSecado);
            _context.detalleSecado.Add(detalleSecado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleSecado", new { id = detalleSecado.idSecadoMaquina }, detalleSecado);
        }     

        private bool detalleSecadoExists(int id)
        {
            return _context.detalleSecado.Any(e => e.idSecadoMaquina == id);
        }
    }
}
