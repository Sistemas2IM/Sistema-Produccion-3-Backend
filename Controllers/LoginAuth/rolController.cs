using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class rolController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public rolController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/rol
        [HttpGet("rol")]
        public async Task<ActionResult<IEnumerable<RolDto>>> Getrol()
        {
            var rol = await _context.rol.ToListAsync();
            var rolDto = _mapper.Map<List<RolDto>>(rol);

            return Ok(rolDto);
        }

        // GET: api/rol/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<RolDto>> Getrol(int id)
        {
            var rol = await _context.rol.FindAsync(id);
            var rolDto = _mapper.Map<RolDto>(rol);

            if (rolDto == null)
            {
                return NotFound("No se encontro el rol con el id: " + id);
            }

            return Ok(rolDto);
        }

        // PUT: api/rol/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPut("{id}")]
        public async Task<IActionResult> Putrol(int id, rol rol)
        {
            if (id != rol.idRol)
            {
                return BadRequest();
            }

            _context.Entry(rol).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!rolExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }*/

        // POST: api/rol
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        /*[HttpPost]
        public async Task<ActionResult<rol>> Postrol(rol rol)
        {
            _context.rol.Add(rol);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getrol", new { id = rol.idRol }, rol);
        }*/

        private bool rolExists(int id)
        {
            return _context.rol.Any(e => e.idRol == id);
        }
    }
}
