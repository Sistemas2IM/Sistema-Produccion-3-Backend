using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class asignacionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public asignacionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/asignacion
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<AsignarOfDto>>> Getasignacion()
        {
            // Cargar las relaciones de disenador y tarjetaOf con Include
            var asignarOf = await _context.asignacion
                .Include(a => a.idDisenadorNavigation) // Cargar la tabla relacionada con disenador
                .Include(a => a.idTarjetaOfNavigation)  // Cargar la tabla relacionada con tarjetaOf
                .ToListAsync();

            // Mapear a DTO
            var asignarOfDto = _mapper.Map<List<AsignarOfDto>>(asignarOf);

            return Ok(asignarOfDto);
        }


        // GET: api/asignacion/5
        [HttpGet("get/id/")]
        public async Task<ActionResult<AsignarOfDto>> Getasignacion(int id)
        {
            var asignacion = await _context.asignacion.FindAsync(id);
            var asignarOfDto = _mapper.Map<AsignarOfDto>(asignacion);

            if (asignarOfDto == null)
            {
                return NotFound("No se encontro el registro con el id: " + id);
            }

            return Ok(asignarOfDto);
        }

        // PUT: api/asignacion/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/")]
        public async Task<IActionResult> Putasignacion(int id, AsignarOfDto asignarOfDto)
        {
            var asignarOf = await _context.asignacion.FindAsync(id);

            if (asignarOf == null)
            {
                return NotFound("No se encontro el registro con el id: " + id);
            }

            _mapper.Map(asignarOfDto, asignarOf);
            _context.Entry(asignarOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!asignacionExists(id))
                {
                    return NotFound("No se encontro el registro con el id: " + id);
                }
                else
                {
                    throw;
                }         
            }
            return Ok(asignarOfDto);
        }

        // POST: api/asignacion
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<asignacion>> Postasignacion(AsignarOfDto asignarOfDto)
        {
            var asignarOf = _mapper.Map<asignacion>(asignarOfDto);

            _context.asignacion.Add(asignarOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetAsignacion", new { id = asignarOf.idAsignacion}, asignarOf);
        }

        private bool asignacionExists(int id)
        {
            return _context.asignacion.Any(e => e.idAsignacion == id);
        }
    }
}
