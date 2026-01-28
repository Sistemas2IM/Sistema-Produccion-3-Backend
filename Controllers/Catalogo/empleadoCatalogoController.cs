using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Empleados;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class empleadoCatalogoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
       private readonly IMapper _mapper;

        public empleadoCatalogoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<empleadoCatalogoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<EmpleadoCatalogoDto>>> GetEmpleado()
        {
            var empleado = await _context.empleadoCatalogo.ToListAsync();
            var empleadoDto = _mapper.Map<List<EmpleadoCatalogoDto>>(empleado);

            return Ok(empleadoDto);
        }

        // GET api/<empleadoCatalogoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<EmpleadoCatalogoDto>> GetEmpleado(int id)
        {
            var empleado = await _context.empleadoCatalogo.FindAsync(id);
            var empleadoDto = _mapper.Map<EmpleadoCatalogoDto>(empleado);

            if (empleadoDto == null)
            {
                return NotFound($"No se encontro el empleado con el id {id}");
            }

            return Ok(empleadoDto);
        }

        // POST api/<empleadoCatalogoController>
        [HttpPost("post")]
        public async Task<ActionResult<AddEmpleadoCatalogoDto>> PostEmpleado(AddEmpleadoCatalogoDto addEmpleadoCatalogoDto)
        {
            var empleado = _mapper.Map<empleadoCatalogo>(addEmpleadoCatalogoDto);
            _context.empleadoCatalogo.Add(empleado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetEmpleado", new { id = empleado.idEmpleado }, empleado);
        }

        // PUT api/<empleadoCatalogoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutEmpleado(int id, UpdateEmpleadoCatalogoDto updateEmpleadoCatalogoDto )
        {
            var empleado = await _context.empleadoCatalogo.FindAsync(id);

            if (empleado == null)
            {
                return NotFound($"No se encontro el material con el ID {id}");
            }

            _mapper.Map(updateEmpleadoCatalogoDto, empleado);
            _context.Entry(empleado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!EmpleadoExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateEmpleadoCatalogoDto);
        }

        private bool EmpleadoExists(int id)
        {
            return _context.empleadoCatalogo.Any(e => e.idEmpleado == id);
        }
    }
}
