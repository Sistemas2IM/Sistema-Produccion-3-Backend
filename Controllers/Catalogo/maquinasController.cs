using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class maquinasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public maquinasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/maquinas
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MaquinaDto>>> Getmaquinas()
        {
            var maquina = await _context.maquinas
                .Include(m => m.idFamiliaNavigation)
                .Include(li => li.listaMaquina)
                .ThenInclude(lo => lo.idListaNavigation)
                .Include(a => a.idFamiliaNavigation.idAreaNavigation)
                .ToListAsync();
            var maquinaDto = _mapper.Map<List<MaquinaDto>>(maquina);

            return Ok(maquinaDto);
        }

        // GET: api/maquinas/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MaquinaDto>> Getmaquinas(int id)
        {
            var maquinas = await _context.maquinas
                .Include(m => m.idFamiliaNavigation)
                .Include(li => li.listaMaquina)
                .ThenInclude(lo => lo.idListaNavigation)
                .Include(a => a.idFamiliaNavigation.idAreaNavigation)
                .FirstOrDefaultAsync(u => u.idMaquina == id);

            var maquinaDto = _mapper.Map<MaquinaDto>(maquinas);

            if (maquinaDto == null)
            {
                return NotFound("No se encontro la maquina con ID: " + id);
            }

            return Ok(maquinaDto);
        }

        // GET: api/maquinas/get/of/5
        [HttpGet("get/of/{numeroOf}")]
        public async Task<ActionResult<List<MaquinaOfDto>>> GetMaquinasPorOf(int numeroOf)
        {
            try
            {
                // 1. Buscamos los IDs de las máquinas únicas asignadas a los procesos de esta OF.
                // Asumiendo que en procesoOf tienes un campo que guarda el número de OF (ej. numeroOf)
                // y una propiedad de navegación hacia tablero (ej. tableroNavigation).
                var idsMaquinas = await _context.procesoOf
                    .Where(p => p.oF == numeroOf) // Ojo: Ajusta 'numeroOF' al nombre real de tu campo
                    .Select(p => p.idTableroNavigation.idMaquina) // Navegamos del proceso al tablero y sacamos el idMaquina
                    .Distinct() // ¡Magia! Esto asegura que no se repita ninguna máquina en la lista
                    .ToListAsync();

                // Si la orden no existe o no tiene máquinas, devolvemos 404
                if (idsMaquinas == null || !idsMaquinas.Any())
                {
                    return NotFound($"No se encontraron máquinas asignadas a los procesos de la OF: {numeroOf}");
                }

                // 2. Traemos la información completa de las máquinas usando los IDs únicos que encontramos
                var maquinas = await _context.maquinas
                    .Include(m => m.idFamiliaNavigation)
                        .ThenInclude(f => f.idAreaNavigation) // Sintaxis correcta para incluir "hijos de los hijos"
                    .Include(m => m.listaMaquina)
                        .ThenInclude(l => l.idListaNavigation)
                    .Where(m => idsMaquinas.Contains(m.idMaquina)) // Filtramos solo por las máquinas de la OF
                    .ToListAsync(); // Usamos ToListAsync para traer toda la colección

                // 3. Mapeamos la lista de Entidades a una Lista de DTOs
                var maquinasDto = _mapper.Map<List<MaquinaOfDto>>(maquinas);

                return Ok(maquinasDto);
            }
            catch (Exception ex)
            {
                // Siempre es bueno atrapar errores inesperados
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al consultar las máquinas: " + ex.Message);
            }
        }

        // PUT: api/maquinas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putmaquinas(int id, UpdateMaquinaDto updateMaquinas)
        {
            var maquina = await _context.maquinas.FindAsync(id);

            if (maquina == null)
            {
                return NotFound("No se encontro la maquina con el ID: " + id);
            }

            _mapper.Map(updateMaquinas, maquina);
            _context.Entry(maquina).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!maquinasExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateMaquinas);
        }

        // POST: api/maquinas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<maquinas>> Postmaquinas(AddMaquinaDto addMaquinas)
        {
            var maquina = _mapper.Map<maquinas>(addMaquinas);
            _context.maquinas.Add(maquina);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmaquinas", new { id = maquina.idMaquina }, maquina);
        }

        private bool maquinasExists(int id)
        {
            return _context.maquinas.Any(e => e.idMaquina == id);
        }
    }
}
