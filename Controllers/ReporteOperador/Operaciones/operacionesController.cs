using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.ReporteOperador.Operaciones
{
    [Route("api/[controller]")]
    [ApiController]
    public class operacionesController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public operacionesController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/operaciones
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<OperacionesDto>>> Getoperaciones()
        {
            var operacion = await _context.operaciones.ToListAsync();
            var operacionDto = _mapper.Map<List<OperacionesDto>>(operacion);

            return Ok(operacionDto);
        }

        // GET: api/operaciones/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<OperacionesDto>> Getoperaciones(int id)
        {
            var operaciones = await _context.operaciones.FindAsync(id);
            var operacionDto = _mapper.Map<OperacionesDto>(operaciones);

            if (operaciones == null)
            {
                return NotFound("No se encontro el registro con el ID: " + id);
            }

            return Ok(operacionDto);
        }

        // PUT: api/operaciones/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putoperaciones(int id, UpdateOperacionesDto updateOperaciones)
        {
            var operacion = await _context.operaciones.FindAsync(id);

            if (operacion == null)
            {
                return NotFound("No se encontro el registro con el ID: " + id);
            }

            _mapper.Map(updateOperaciones, operacion);
            _context.Entry(operacion).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!operacionesExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }

            }
            return Ok(updateOperaciones);
        }

        // POST: api/operaciones
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<operaciones>> Postoperaciones(AddOperacionesDto addOperaciones)
        {
            var operacion = _mapper.Map<operaciones>(addOperaciones);
            _context.operaciones.Add(operacion);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getoperaciones", new { id = operacion.idOperacion }, operacion);
        }       

        private bool operacionesExists(int id)
        {
            return _context.operaciones.Any(e => e.idOperacion == id);
        }
    }
}
