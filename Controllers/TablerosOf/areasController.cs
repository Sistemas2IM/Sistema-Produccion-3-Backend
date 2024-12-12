using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.LoginAuth;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.Tableros.Areas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class areasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public areasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<AreasDto>>> Getareas()
        {
            var area = await _context.areas.ToListAsync();

            var areaDto = _mapper.Map<List<AreasDto>>(area);

            return Ok(areaDto);
        }

        // GET: api/areas 
        [HttpGet("get/tableros")] // <--------------- Con tableros
        public async Task<ActionResult<IEnumerable<AreasTablerosDto>>> GetareasTableros()
        {
            var area = await _context.areas
                .Include(u => u.tablerosOf)
                .ToListAsync();

            var areaDto = _mapper.Map<List<AreasTablerosDto>>(area);

            return Ok(areaDto);
        }

        [HttpGet("get/usuarios")] // <-------------- Con usuarios
        public async Task<ActionResult<IEnumerable<AreasUsuariosDto>>> GetareasUsuarios()
        {
            var area = await _context.areas
                .Include(u => u.usuario)
                .ToListAsync();

            var areaDto = _mapper.Map<List<AreasUsuariosDto>>(area);

            return Ok(areaDto);
        }

        // GET: api/areas/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<AreasDto>> Getareas(int id)
        {
           var area = await _context.areas.FindAsync(id);

            var areaDto = _mapper.Map<AreasDto>(area);

            if (areaDto == null)
            {
                return NotFound("No se encontro el Area con ID: " + id);
            }
            return Ok(areaDto);
        }

        // PUT: api/areas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putareas(int id, UpdateAreasDto updateAreas)
        {
            var area = await _context.areas.FindAsync(id);

            if (area == null)
            {
                return NotFound("No se encontro el Area con el ID: " + id);
            }

            _mapper.Map(updateAreas, area);
            _context.Entry(area).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!areasExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateAreas);
        }

        // POST: api/areas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<areas>> Postareas(AddAreasDto addAreas)
        {
            var area = _mapper.Map<areas>(addAreas);
            _context.areas.Add(area);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getareas", new { id = area.idArea }, area);
        }       

        private bool areasExists(int id)
        {
            return _context.areas.Any(e => e.idArea == id);
        }
    }
}
