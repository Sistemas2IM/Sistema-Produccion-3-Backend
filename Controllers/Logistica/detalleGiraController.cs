using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Logistica.DetalleGira;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Logistica
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleGiraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleGiraController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleGira
        [HttpGet("get/gira/{idGira}")]
        public async Task<ActionResult<IEnumerable<DetalleGiraDto>>> GetdetalleGira(int idGira)
        {
            var detalleGira = await _context.detalleGira
                .Where(d => d.idGira == idGira)
                .Include(u => u.idGiraNavigation)
                .ToArrayAsync();

            if (detalleGira == null || detalleGira.Length == 0)
            {
                return NotFound($"No se encontro el ID del reporte padre de Gira con ID: {idGira}");
            }

            var detalleGiraDto = _mapper.Map<List<DetalleGiraDto>>(detalleGira);

            return Ok(detalleGiraDto);
        }

        // GET: api/detalleGira/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleGiraDto>> GetdetalleGiraId(int id)
        {
            var detalleGira = await _context.detalleGira
                .Include(u => u.idGiraNavigation)
                .FirstOrDefaultAsync(u => u.idDetalleGira == id);


            if (detalleGira == null)
            {
                return NotFound($"No se encontro el registro detalle de Gira con el ID: {id}");
            }

            var detalleGiraDto = _mapper.Map<DetalleGiraDto>(detalleGira);

            return Ok(detalleGiraDto);
        }

        // PUT: api/detalleGira/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutdetalleGira(int id, UpdateDetalleGiraDto updateDetalleGira)
        {
           var detalleGira = await _context.detalleGira.FindAsync(id);

            if (detalleGira == null)
            {
                return NotFound("No se encontro el detalle de Gira con el ID: " + id);
            }

            _mapper.Map(updateDetalleGira, detalleGira);
            _context.Entry(detalleGira).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleGiraExists(id))
                {
                    return BadRequest($"ID: {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateDetalleGira);
        }

        // POST: api/detalleGira
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detalleGira>> PostdetalleGira(AddDetalleGiraDto addDetalleGira)
        {
            var detalleGira = _mapper.Map<detalleGira>(addDetalleGira);

            _context.detalleGira.Add(detalleGira);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetdetalleGira", new { id = detalleGira.idDetalleGira }, detalleGira);
        }
      

        private bool detalleGiraExists(int id)
        {
            return _context.detalleGira.Any(e => e.idDetalleGira == id);
        }
    }
}
