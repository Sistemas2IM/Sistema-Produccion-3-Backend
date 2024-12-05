using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleImpresion.FormulacionTintas
{
    [Route("api/[controller]")]
    [ApiController]
    public class formulacionTintasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public formulacionTintasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/formulacionTintas
        [HttpGet("get/DetalleImpresion/{id}")]
        public async Task<ActionResult<IEnumerable<FormulacionTintasDto>>> GetformulacionTintasImpresion(int id)
        {
            var formulacionTinta = await _context.formulacionTintas
                .Where(d => d.idDetalleImpresion == id)
                .Include(u => u.espacioColor)
                .ThenInclude(g => g.generalidadColor)
                .ToListAsync();

            var formulacionTintaDto = _mapper.Map<List<FormulacionTintasDto>>(formulacionTinta);

            return Ok(formulacionTintaDto);
        }

        // GET: api/formulacionTintas/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FormulacionTintasDto>> GetformulacionTintas(int id)
        {
            var formulacionTinta = await _context.formulacionTintas
                .Include(u => u.espacioColor)
                .ThenInclude(g => g.generalidadColor)
                .FirstOrDefaultAsync(i => i.idFormulacionTinta == id);

            var formulacionTintaDto = _mapper.Map<FormulacionTintasDto>(formulacionTinta);

            if (formulacionTintaDto == null)
            {
                return NotFound($"No se encontro el registro de formulacion de tinta con el ID: {id}");
            }
            return Ok(formulacionTintaDto);
        }

        // PUT: api/formulacionTintas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutformulacionTintas(int id, UpdateFormulacionTintasDto updateFormulacionTintas)
        {
            var formulacionTinta = await _context.formulacionTintas.FindAsync(id);

            if (formulacionTinta == null)
            {
                return NotFound($"No se encontro el registro de formulacion de tinta con el ID: {id}");
            }

            _mapper.Map(updateFormulacionTintas, formulacionTinta);
            _context.Entry(formulacionTinta).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!formulacionTintasExists(id))
                {
                    return BadRequest($"El ID {1} no coincide con ningun registro");
                }
                else
                {
                    throw;

                }              
            }
            return Ok(updateFormulacionTintas);

        }

        // POST: api/formulacionTintas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<formulacionTintas>> PostformulacionTintas(AddFormulacionTintasDto addFormulacionTintas)
        {
            var formulacionTinta = _mapper.Map<formulacionTintas>(addFormulacionTintas);
            _context.formulacionTintas.Add(formulacionTinta);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetformulacionTintas", new { id = formulacionTinta.idFormulacionTinta }, formulacionTinta);
        }    

        private bool formulacionTintasExists(int id)
        {
            return _context.formulacionTintas.Any(e => e.idFormulacionTinta == id);
        }
    }
}
