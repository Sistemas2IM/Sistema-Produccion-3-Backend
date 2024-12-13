using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.FamiliaMaquina;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class familliaDeMaquinaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public familliaDeMaquinaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/familliaDeMaquina
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<FamilliaDeMaquinaDto>>> GetfamilliaDeMaquina()
        {
            var familiaMaquina = await _context.familliaDeMaquina.ToListAsync();
            var familiaMaquinaDto = _mapper.Map<List<FamilliaDeMaquinaDto>>(familiaMaquina);

            return Ok(familiaMaquinaDto);
        }

        // GET: api/familliaDeMaquina
        [HttpGet("get/maquinas")]
        public async Task<ActionResult<IEnumerable<FamilliaDeMaquinaDto>>> GetfamilliaDeMaquinaLista()
        {
            var familiaMaquina = await _context.familliaDeMaquina
                .Include(u => u.maquinas)
                .ToListAsync();
            var familiaMaquinaDto = _mapper.Map<List<FamilliaDeMaquinaDto>>(familiaMaquina);

            return Ok(familiaMaquinaDto);
        }

        // GET: api/familliaDeMaquina/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<FamilliaDeMaquinaDto>> GetfamilliaDeMaquina(int id)
        {
            var familiaMaquina = await _context.familliaDeMaquina.FindAsync(id);
            var familiaMaquinaDto = _mapper.Map<FamilliaDeMaquinaDto>(familiaMaquina);

            if (familiaMaquinaDto == null)
            {
                return NotFound($"No se encontro la familia de maquina con el ID: {id}");
            }

            return Ok(familiaMaquinaDto);
        }

        //// PUT: api/familliaDeMaquina/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutfamilliaDeMaquina(int id, familliaDeMaquina familliaDeMaquina)
        //{
        //    if (id != familliaDeMaquina.idFamilia)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(familliaDeMaquina).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!familliaDeMaquinaExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        //// POST: api/familliaDeMaquina
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost]
        //public async Task<ActionResult<familliaDeMaquina>> PostfamilliaDeMaquina(familliaDeMaquina familliaDeMaquina)
        //{
        //    _context.familliaDeMaquina.Add(familliaDeMaquina);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("GetfamilliaDeMaquina", new { id = familliaDeMaquina.idFamilia }, familliaDeMaquina);
        //}
       
        private bool familliaDeMaquinaExists(int id)
        {
            return _context.familliaDeMaquina.Any(e => e.idFamilia == id);
        }
    }
}
