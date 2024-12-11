using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Motoristas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class motoristaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public motoristaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/motorista
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MotoristaDto>>> Getmotorista()
        {
            var motorista = await _context.motorista.ToListAsync();
            var motoristaDto = _mapper.Map<List<MotoristaDto>>(motorista);

            return Ok(motoristaDto);
        }

        // GET: api/motorista/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MotoristaDto>> Getmotorista(int id)
        {
            var motorista = await _context.motorista.FindAsync(id);
            var motoristaDto = _mapper.Map<MotoristaDto>(motorista);

            if (motoristaDto == null)
            {
                return NotFound("No se encontro el motorista con el ID: " + id);
            }

            return Ok(motoristaDto);
        }

        // PUT: api/motorista/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putmotorista(int id, UpdateMotoristaDto updateMotorista)
        {
            var motorista = await _context.motorista.FindAsync(id);

            if (motorista == null)
            {
                return NotFound("No se encontro el motorista con el Id: " + id);
            }

            _mapper.Map(updateMotorista, motorista);
            _context.Entry(motorista).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!motoristaExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else 
                { 
                    throw;
                }  
            }
            return Ok(updateMotorista);
        }

        // POST: api/motorista
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<motorista>> Postmotorista(AddMotoristaDto addMotorista)
        {
            var motorista = _mapper.Map<motorista>(addMotorista);
            _context.motorista.Add(motorista);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmotorista", new { id = motorista.idMotorista }, motorista);
        }       

        private bool motoristaExists(int id)
        {
            return _context.motorista.Any(e => e.idMotorista == id);
        }
    }
}
