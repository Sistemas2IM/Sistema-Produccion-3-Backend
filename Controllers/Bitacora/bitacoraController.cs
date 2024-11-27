using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Bitacora;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Bitacora
{
    [Route("api/[controller]")]
    [ApiController]
    public class bitacoraController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public bitacoraController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/bitacora
        [HttpGet]
        public async Task<ActionResult<IEnumerable<BitacoraDto>>> Getbitacora()
        {
            var bitacora = await _context.bitacora.ToListAsync();
            var bitacoraDto = _mapper.Map<List<BitacoraDto>>(bitacora);

            return Ok(bitacoraDto);
        }

        // GET: api/bitacora/5
        [HttpGet("get{id}")]
        public async Task<ActionResult<BitacoraDto>> Getbitacora(int id)
        {
            var bitacora = await _context.bitacora.FindAsync(id);
            var bitacoraDto = _mapper.Map<BitacoraDto>(bitacora);

            if (bitacoraDto == null)
            {
                return NotFound("No se encontro el registro de Bitacora con el ID: " + id);
            }

            return Ok(bitacoraDto);
        }

        // PUT: api/bitacora/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putbitacora(int id, UpdateBitacoraDto updateBitacora)
        {
            var bitacora = await _context.bitacora.FindAsync(id);

            if (bitacora == null)
            {
                return NotFound("No se encontro el registro de bitacora con el ID: " + id);
            }

            _mapper.Map(updateBitacora, bitacora);
            _context.Entry(bitacora).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!bitacoraExists(id))
                {
                    return BadRequest($"El ID {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateBitacora);
        }

        // POST: api/bitacora
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<bitacora>> Postbitacora(AddBitacoraDto addBitacora)
        {
            var bitacora = _mapper.Map<bitacora>(addBitacora);
            _context.bitacora.Add(bitacora);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getbitacora", new { id = bitacora.idBitacora }, bitacora);
        }      

        private bool bitacoraExists(int id)
        {
            return _context.bitacora.Any(e => e.idBitacora == id);
        }
    }
}
