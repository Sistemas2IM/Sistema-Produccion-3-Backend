using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class maquinasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public maquinasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/maquinas
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MaquinaDto>>> Getmaquinas()
        {
            var maquina = await _context.maquinas.ToListAsync();
            var maquinaDto = _mapper.Map<List<MaquinaDto>>(maquina);

            return Ok(maquinaDto);
        }

        // GET: api/maquinas/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MaquinaDto>> Getmaquinas(int id)
        {
            var maquinas = await _context.maquinas.FindAsync(id);
            var maquinaDto = _mapper.Map<MaquinaDto>(maquinas);

            if (maquinaDto == null)
            {
                return NotFound("No se encontro la maquina con ID: " + id);
            }

            return Ok(maquinaDto);
        }

        // PUT: api/maquinas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putmaquinas(int id, UpdateMaquinaDto updateMaquinas)
        {
            var maquina = await _context.maquinas.FindAsync(id);

            if (maquina == null)
            {
                return NotFound("No se encontro la maquina con el ID: " + id);
            }

            _mapper.Map(updateMaquinas, maquina);
            _context.Entry(maquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!maquinasExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateMaquinas);
        }

        // POST: api/maquinas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<maquinas>> Postmaquinas(AddMaquinaDto addMaquinas)
        {
            var maquina = _mapper.Map<maquinas>(addMaquinas);
            _context.maquinas.Add(maquina);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmaquinas", new { id = maquina.idMaquina }, maquina);
        }

        private bool maquinasExists(int id)
        {
            return _context.maquinas.Any(e => e.idMaquina == id);
        }
    }
}
