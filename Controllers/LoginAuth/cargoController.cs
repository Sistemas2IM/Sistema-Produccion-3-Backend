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
    public class cargoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public cargoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/cargo
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CargoDto>>> Getcargo()
        {
            var cargo = await _context.cargo.ToListAsync();
            var cargoDto = _mapper.Map<List<CargoDto>>(cargo);

            return Ok(cargoDto);
        }

        // GET: api/cargo/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CargoDto>> Getcargo(int id)
        {
            var cargo = await _context.cargo.FindAsync(id);
            var cargoDto = _mapper.Map<CargoDto>(cargo);

            if (cargoDto == null)
            {
                return NotFound("no se encontro el cargo con id: " + id);
            }

            return Ok(cargoDto);
        }

        // GET: api/cargo/usuarios
        [HttpGet("get/usuarios")]
        public async Task<ActionResult<IEnumerable<CargoDto>>> GetcargoUsuarios()
        {
            var cargo = await _context.cargo
                .Include(t => t.usuario)
                .ToListAsync();
            var cargoDto = _mapper.Map<List<CargoDto>>(cargo);

            return Ok(cargoDto);
        }

        // PUT: api/cargo/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putcargo(int id, UpdateCargoDto updateCargo)
        {
            var cargo = await _context.cargo.FindAsync(id);

            if (cargo == null)
            {
                return NotFound("No se encontro el Cargo con el ID: " + id);
            }

            _mapper.Map(updateCargo, cargo);
            _context.Entry(cargo).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!cargoExists(id))
                {
                    return BadRequest($"ID = {id} no coindide con el registro");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateCargo);
        }

        // POST: api/cargo
        [HttpPost]
        public async Task<ActionResult<cargo>> Postcargo(AddCargoDto addCargo)
        {
            var cargo = _mapper.Map<cargo>(addCargo);
            _context.cargo.Add(cargo);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getcargo", new { id = cargo.idCargo }, cargo);
        }
       
        private bool cargoExists(int id)
        {
            return _context.cargo.Any(e => e.idCargo == id);
        }
    }
}
