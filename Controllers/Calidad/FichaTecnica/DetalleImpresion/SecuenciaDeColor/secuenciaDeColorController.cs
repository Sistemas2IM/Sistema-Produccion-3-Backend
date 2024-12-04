using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.SecuenciaDeColor;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleImpresion.SecuenciaDeColor
{
    [Route("api/[controller]")]
    [ApiController]
    public class secuenciaDeColorController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public secuenciaDeColorController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/secuenciaDeColor
        [HttpGet("get/DetalleImpresion/{id}")]
        public async Task<ActionResult<IEnumerable<SecuenciaDeColorDto>>> GetsecuenciaDeColorImpresion(int id)
        {
            var secuenciaColor = await _context.secuenciaDeColor
                .Where(u => u.idDetalleImpresion == id)
                .ToListAsync();

            var secuenciaColorDto = _mapper.Map<List<SecuenciaDeColorDto>>(secuenciaColor);

            return Ok(secuenciaColorDto);
        }

        // GET: api/secuenciaDeColor/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SecuenciaDeColorDto>> GetsecuenciaDeColor(int id)
        {
            var secuenciaColor = await _context.secuenciaDeColor
                .FirstOrDefaultAsync(u => u.idSecuencia == id);

            var secuenciaColorDto = _mapper.Map<SecuenciaDeColorDto>(secuenciaColor);

            if (secuenciaColorDto == null)
            {
                return NotFound($"No se encontro la secuencia de color con el ID: {id}");
            }

            return Ok(secuenciaColorDto);
        }

        // PUT: api/secuenciaDeColor/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutsecuenciaDeColor(int id, UpdateSecuenciaDeColorDto updateSecuenciaDeColor)
        {
            var secuenciaColor = await _context.secuenciaDeColor.FindAsync(id);

            if (secuenciaColor == null) 
            {
                return NotFound($"No se encontro la secuencia de color con el ID: {id}");
            }

            _mapper.Map(updateSecuenciaDeColor, secuenciaColor);
            _context.Entry(secuenciaColor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!secuenciaDeColorExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateSecuenciaDeColor);
        }

        // POST: api/secuenciaDeColor
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<secuenciaDeColor>> PostsecuenciaDeColor(AddSecuenciaDeColorDto addSecuenciaDeColor)
        {
            var secuenciaDeColor = _mapper.Map<secuenciaDeColor>(addSecuenciaDeColor);
            _context.secuenciaDeColor.Add(secuenciaDeColor);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetsecuenciaDeColor", new { id = secuenciaDeColor.idSecuencia }, secuenciaDeColor);
        }       

        private bool secuenciaDeColorExists(int id)
        {
            return _context.secuenciaDeColor.Any(e => e.idSecuencia == id);
        }
    }
}
