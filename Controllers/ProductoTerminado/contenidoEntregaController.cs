using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.ProductoTerminado
{
    [Route("api/[controller]")]
    [ApiController]
    public class contenidoEntregaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public contenidoEntregaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/contenidoEntrega
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ContenidoEntregaDto>>> GetcontenidoEntrega(int idEntregaPt)
        {
            var contenidoEntrega = await _context.contenidoEntrega
                .Where(d => d.idEntregaPt == idEntregaPt)
                .ToArrayAsync();

            if (contenidoEntrega == null || contenidoEntrega.Length == 0) 
            {
                return NotFound("No se encontraron contenidos relacionados con el reporte con id: " + idEntregaPt);
            }

            var contenidoEntregaDto = _mapper.Map<List<ContenidoEntregaDto>>(contenidoEntrega);

            return Ok(contenidoEntregaDto);
        }

        // GET: api/contenidoEntrega/5
        [HttpGet("get/id/")]
        public async Task<ActionResult<ContenidoEntregaDto>> GetcontenidoEntregaId(int id)
        {
            var contenidoEntrega = await _context.contenidoEntrega.FindAsync(id);

            var contenidoEntregaDto = _mapper.Map<ContenidoEntregaDto>(contenidoEntrega);

            if (contenidoEntregaDto == null)
            {
                return NotFound("No se encuentra el contenido con el id: " + id);
            }

            return Ok(contenidoEntregaDto);
        }

        // PUT: api/contenidoEntrega/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/id/")]
        public async Task<IActionResult> PutcontenidoEntrega(int id, UpdateContenidoEntregaDto updateContenidoEntrega)
        {
            var contenidoEntrega = await _context.contenidoEntrega.FindAsync(id);

            if (contenidoEntrega == null)
            {
                return NotFound("No se encontro el contenido con el id: " + id);
            }

            _mapper.Map(updateContenidoEntrega, contenidoEntrega);
            _context.Entry(contenidoEntrega).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!contenidoEntregaExists(id))
                {
                    return NotFound("No se encontro el contenido con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateContenidoEntrega);
        }

        // POST: api/contenidoEntrega
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<contenidoEntrega>> PostcontenidoEntrega(AddContenidoEntregaDto addContenidoEntrega, int idEntregaPt)
        {
            var entregaPt = await _context.contenidoEntrega.FindAsync(idEntregaPt);

            if (entregaPt == null)
            {
                return BadRequest("Id de Entrega no valido: " + idEntregaPt);
            }

            var contenidoEntrega = _mapper.Map<contenidoEntrega>(addContenidoEntrega);
            contenidoEntrega.idEntregaPt = idEntregaPt;

            _context.contenidoEntrega.Add(contenidoEntrega);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetcontenidoEntrega", new { id = contenidoEntrega.idContenidoEntrega }, contenidoEntrega);
        }

        private bool contenidoEntregaExists(int id)
        {
            return _context.contenidoEntrega.Any(e => e.idContenidoEntrega == id);
        }
    }
}
