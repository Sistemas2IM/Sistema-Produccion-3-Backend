using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificado;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class detalleCertificadoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public detalleCertificadoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/detalleCertificado
        [HttpGet("get/certificado/{idCertificado}")]
        public async Task<ActionResult<IEnumerable<DetalleCertificadoDto>>> GetdetalleCertificado(int idCertificado)
        {
            var detalleCertificado = await _context.detalleCertificado
                .Where(d => d.idCertificado == idCertificado)
                .Include(u => u.idCaracteristaNavigation)
                .ToArrayAsync();

            if (detalleCertificado == null || detalleCertificado.Length == 0)
            {
                return NotFound("No se encontraron los detalles del certificado con id: " + idCertificado);
            }

            var detalleCertificadoDto = _mapper.Map<List<DetalleCertificadoDto>>(detalleCertificado);

            return Ok(detalleCertificadoDto);
        }

        // GET: api/detalleCertificado/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<DetalleCertificadoDto>> GetdetalleCertificadoId(int id)
        {
            var detalleCertificado = await _context.detalleCertificado
                .Include(u => u.idCaracteristaNavigation)
                .FirstOrDefaultAsync(u => u.idDetalleCertificado == id);

            var detalleCertificadoDto = _mapper.Map<DetalleCertificadoDto>(detalleCertificado);

            if (detalleCertificadoDto == null)
            {
                return NotFound("No se encontro el detalle con el id: " + id);
            }

            return Ok(detalleCertificadoDto);
        }

        // PUT: api/detalleCertificado/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put{id}")]
        public async Task<IActionResult> PutdetalleCertificado(int id, UpdateDetalleCertificadoDto updateDetalleCertificado)
        {
            var detalleCertificado = await _context.detalleCertificado.FindAsync(id);

            if (detalleCertificado == null)
            {
                return BadRequest("No se encontro el detalle con el id: " + id);
            }

            _mapper.Map(updateDetalleCertificado, detalleCertificado);
            _context.Entry(detalleCertificado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!detalleCertificadoExists(id))
                {
                    return NotFound("No se encontro el detalle con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateDetalleCertificado);
        }

        // POST: api/detalleCertificado
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<detalleCertificado>> PostdetalleCertificado(int idCertificado, AddDetalleCertificadoDto addDetalleCertificado)
        {
            var certificado = await _context.certificadoDeCalidad.FindAsync(idCertificado);

            if (certificado == null)
            {
                return BadRequest("No se encontro el Certificado con el id: " + idCertificado);
            }

            var detalleCertficado = _mapper.Map<detalleCertificado>(addDetalleCertificado);
            detalleCertficado.idCertificado = idCertificado;

            _context.detalleCertificado.Add(detalleCertficado);
            await _context.SaveChangesAsync();
            
            return CreatedAtAction("GetdetalleCertificado", new { id = detalleCertficado.idDetalleCertificado });
        }


        private bool detalleCertificadoExists(int id)
        {
            return _context.detalleCertificado.Any(e => e.idDetalleCertificado == id);
        }
    }
}
