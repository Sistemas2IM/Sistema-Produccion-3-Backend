using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Logistica;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Logistica
{
    [Route("api/[controller]")]
    [ApiController]
    public class giraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public giraController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/gira
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<GiraDto>>> Getgira()
        {
            var giro = await _context.gira
                .Include(u => u.detalleGira)
                .Include(r => r.idVehiculoNavigation)
                .Include(p => p.idMotoristaNavigation)
                .ToListAsync();

            var giraDto = _mapper.Map<List<GiraDto>>(giro);

            return Ok(giraDto);
        }

        // GET: api/gira/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<GiraDto>> Getgira(int id)
        {
            var gira = await _context.gira
                .Include(u => u.detalleGira)
                .Include(r => r.idVehiculoNavigation)
                .Include(p => p.idMotoristaNavigation)
                .FirstOrDefaultAsync(u => u.idGira == id);

            if (gira == null)
            {
                return NotFound("No se encontro el registro de Gira con el ID: " + id);
            }

            var giraDto = _mapper.Map<GiraDto>(gira);

            return Ok(giraDto);
        }

        // PUT: api/gira/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putgira(int id, UpdateGiraDto updateGira)
        {
            var gira = await _context.gira.FindAsync(id);

            if (gira == null) 
            {
                return BadRequest("No se encontro el registro de Gira con el ID: " + id);
            }

            _mapper.Map(updateGira, gira);
            _context.Entry(gira).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!giraExists(id))
                {
                    return BadRequest($"");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateGira);
        }

        // POST: api/gira
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<gira>> Postgira(AddGiraDto addGira)
        {
            var gira = _mapper.Map<gira>(addGira);

            _context.gira.Add(gira);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getgira", new { id = gira.idGira }, gira);
        }       

        private bool giraExists(int id)
        {
            return _context.gira.Any(e => e.idGira == id);
        }
    }
}
