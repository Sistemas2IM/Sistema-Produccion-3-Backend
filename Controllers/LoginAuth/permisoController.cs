using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class permisoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public permisoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/permiso
        [HttpGet]
        public async Task<ActionResult<IEnumerable<permiso>>> Getpermiso()
        {

            return await _context.permiso.ToListAsync();
        }

        // GET: api/permiso/5
        [HttpGet("{id}")]
        public async Task<ActionResult<permiso>> Getpermiso(int id)
        {
            var permiso = await _context.permiso.FindAsync(id);

            if (permiso == null)
            {
                return NotFound();
            }

            return permiso;
        }

        // PUT: api/permiso/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> Putpermiso(int id, permiso permiso)
        {
            if (id != permiso.idPermiso)
            {
                return BadRequest();
            }

            _context.Entry(permiso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!permisoExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/permiso
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<permiso>> Postpermiso(permiso permiso)
        {
            _context.permiso.Add(permiso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getpermiso", new { id = permiso.idPermiso }, permiso);
        }

        // DELETE: api/permiso/5
        /*[HttpDelete("{id}")]
        public async Task<IActionResult> Deletepermiso(int id)
        {
            var permiso = await _context.permiso.FindAsync(id);
            if (permiso == null)
            {
                return NotFound();
            }

            _context.permiso.Remove(permiso);
            await _context.SaveChangesAsync();

            return NoContent();
        }*/

        private bool permisoExists(int id)
        {
            return _context.permiso.Any(e => e.idPermiso == id);
        }
    }
}
