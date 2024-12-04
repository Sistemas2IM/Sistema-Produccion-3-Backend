using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleImpresion.Filtros;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.FichaTecnica.DetalleImpresion.Filtros
{
    [Route("api/[controller]")]
    [ApiController]
    public class filtrosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public filtrosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/filtros
        [HttpGet("get/DetalleImpresion/{id}")]
        public async Task<ActionResult<IEnumerable<FiltrosDto>>> GetfiltrosImpresion(int id)
        {
            var filtro = await _context.filtros
                .Where(u => u.idDetalleImpresion == id)
                .ToListAsync();

            var filtroDto = _mapper.Map<List<FiltrosDto>>(filtro);

            return Ok(filtroDto);
        }

        // GET: api/filtros/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FiltrosDto>> Getfiltros(int id)
        {
            var filtro = await _context.filtros
                .FirstOrDefaultAsync(u => u.idFiltro == id);

            var filtroDto = _mapper.Map<FiltrosDto>(filtro);

            if (filtroDto == null)
            {
                return Ok($"No se encontro el registro con el ID: {id}");
            }
            return Ok(filtroDto);
        }

        // PUT: api/filtros/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putfiltros(int id, UpdateFiltrosDto updateFiltros)
        {
            var filtros = await _context.filtros.FindAsync(id);

            if (filtros == null)
            {
                return NotFound($"No se encontro el filtro con ID: {id}");
            }

            _mapper.Map(updateFiltros, filtros);
            _context.Entry(filtros).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!filtrosExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con ningun registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateFiltros);
        }

        // POST: api/filtros
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<filtros>> Postfiltros(AddFiltrosDto addFiltros)
        {
            var filtros = _mapper.Map<filtros>(addFiltros);
            _context.filtros.Add(filtros);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getfiltros", new { id = filtros.idFiltro }, filtros);
        }

        private bool filtrosExists(int id)
        {
            return _context.filtros.Any(e => e.idFiltro == id);
        }
    }
}
