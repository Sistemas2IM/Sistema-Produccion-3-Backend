using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.DetalleBarniz;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleImpresion.DetalleBarniz
{
    [Route("api/[controller]")]
    [ApiController]
    public class potenciaLamparaUvController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public potenciaLamparaUvController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/potenciaLamparaUv
        [HttpGet("get/Barniz/{id}")]
        public async Task<ActionResult<IEnumerable<PotenciaLamparaUvDto>>> GetpotenciaLamparaUvBarniz(int id)
        {
            var potencialLamparaUv = await _context.potenciaLamparaUv
                .Where(u => u.idBarniz == id)
                .ToListAsync();

            var potencialLamparaUvDto = _mapper.Map<List<PotenciaLamparaUvDto>>(potencialLamparaUv);

            return Ok(potencialLamparaUvDto);
        }

        // GET: api/potenciaLamparaUv/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<PotenciaLamparaUvDto>> GetpotenciaLamparaUv(int id)
        {
            var potencialLamparaUv = await _context.potenciaLamparaUv
                .FirstOrDefaultAsync(u => u.idPotencia == id);

            var potencialLamparaUvDto = _mapper.Map<PotenciaLamparaUvDto>(potencialLamparaUv);

            if (potencialLamparaUvDto == null)
            {
                return NotFound($"No se encontro el registro con el ID: {id}");
            }

            return Ok(potencialLamparaUvDto);
        }

        // PUT: api/potenciaLamparaUv/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutpotenciaLamparaUv(int id, UpdatePotenciaLamparaUvDto updatePotenciaLamparaUv)
        {
            var potenciaLamparaUv = await _context.potenciaLamparaUv.FindAsync(id);

            if (potenciaLamparaUv == null)
            {
                return NotFound($"No se encontro el registro con el ID: {id}");
            }

            _mapper.Map(updatePotenciaLamparaUv, potenciaLamparaUv);
            _context.Entry(potenciaLamparaUv).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!potenciaLamparaUvExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updatePotenciaLamparaUv);
        }

        // POST: api/potenciaLamparaUv
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<potenciaLamparaUv>> PostpotenciaLamparaUv(AddDetalleBarnizDto addPotenciaLamparaUv)
        {
            var potenciaLamparaUv = _mapper.Map<potenciaLamparaUv>(addPotenciaLamparaUv);
            _context.potenciaLamparaUv.Add(potenciaLamparaUv);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetpotenciaLamparaUv", new { id = potenciaLamparaUv.idPotencia }, potenciaLamparaUv);
        }

        private bool potenciaLamparaUvExists(int id)
        {
            return _context.potenciaLamparaUv.Any(e => e.idPotencia == id);
        }
    }
}
