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
    public class vehiculoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public vehiculoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/vehiculo
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<VehiculoDto>>> Getvehiculo()
        {
            var vehiculo = await _context.vehiculo.ToListAsync();
            var vehiculoDto = _mapper.Map<List<VehiculoDto>>(vehiculo);

            return Ok(vehiculoDto);
        }

        // GET: api/vehiculo/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<VehiculoDto>> Getvehiculo(int id)
        {
            var vehiculo = await _context.vehiculo.FindAsync(id);
            var vehiculoDto = _mapper.Map<VehiculoDto>(vehiculo);

            if (vehiculoDto == null)
            {
                return NotFound("No se encontro el vehiculo con el ID: " + id);
            }

            return Ok(vehiculoDto);
        }

        // PUT: api/vehiculo/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putvehiculo(int id, UpdateVehiculoDto updateVehiculo)
        {
            var vehiculo = await _context.vehiculo.FindAsync(id);

            if (vehiculo == null)
            {
                return NotFound("No se encontro el vehiculo con el ID: " + id);
            }

            _mapper.Map(updateVehiculo, vehiculo);
            _context.Entry(vehiculo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!vehiculoExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateVehiculo);
        }

        // POST: api/vehiculo
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<vehiculo>> Postvehiculo(AddVehiculoDto addVehiculo)
        {
            var vehiculo = _mapper.Map<vehiculo>(addVehiculo);
            _context.vehiculo.Add(vehiculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getvehiculo", new { id = vehiculo.idVehiculo }, vehiculo);
        }
       
        private bool vehiculoExists(int id)
        {
            return _context.vehiculo.Any(e => e.idVehiculo == id);
        }
    }
}
