using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.LoginAuth.SesionOperador;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class sesionOperadorController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public sesionOperadorController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/sesionOperador
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<SesionOperadorDto>>> GetsesionOperador()
        {
            var sesionOp = await _context.sesionOperador.ToListAsync();
            var sesionOpDto = _mapper.Map<List<SesionOperadorDto>>(sesionOp);

            return Ok(sesionOpDto);
        }

        // GET: api/sesionOperador/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SesionOperadorDto>> GetsesionOperador(int id)
        {
           var sesionOp = await _context.sesionOperador.FindAsync(id);
            var sesionOpDto = _mapper.Map<SesionOperadorDto>(sesionOp);

            if (sesionOpDto == null)
            {
                return NotFound($"No se encontro la sesion con id: {id}");
            }

            return Ok(sesionOpDto);
        }

        // PUT: api/sesionOperador/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutsesionOperador(int id, UpdateSesionOperadorDto updateSesionOperador)
        {
            var sesionOp = await _context.sesionOperador.FindAsync(id);

            if (sesionOp == null)
            {
                return NotFound($"No se encontro la sesion con id: {id}");
            }

            _mapper.Map(updateSesionOperador, sesionOp);
            _context.Entry(sesionOp).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!sesionOperadorExists(id))
                {
                    return BadRequest($"El id: {id} ya existe papu idk");
                }
                else 
                { 
                    throw; 
                }
            }
            return Ok(updateSesionOperador);
        }

        // POST: api/sesionOperador
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<sesionOperador>> PostsesionOperador(AddSesionOperadorDto addSesionOperador)
        {
            var sesionOp = _mapper.Map<sesionOperador>(addSesionOperador);
            _context.sesionOperador.Add(sesionOp);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetsesionOperador", new { id = sesionOp.id }, sesionOp);
        }       

        private bool sesionOperadorExists(int id)
        {
            return _context.sesionOperador.Any(e => e.id == id);
        }
    }
}
