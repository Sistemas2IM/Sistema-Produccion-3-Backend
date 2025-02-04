using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.Auxiliares;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.ReporteOperador.Auxiliares
{
    [Route("api/[controller]")]
    [ApiController]
    public class auxiliaresController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public auxiliaresController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/auxiliares
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<AuxiliaresDto>>> Getauxiliares()
        {
            var auxiliar = await _context.auxiliares.ToListAsync();
            var auxiliarDto = _mapper.Map<List<AuxiliaresDto>>(auxiliar);

            return Ok(auxiliarDto);
        }

        // GET: api/auxiliares/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<AuxiliaresDto>> Getauxiliares(int id)
        {
            var auxiliar = await _context.auxiliares.FindAsync(id);
            var auxiliarDto = _mapper.Map<AuxiliaresDto>(auxiliar);

            if (auxiliarDto == null)
            {
                return NotFound($"No se encontro el Auxiliar con el id: {id}");
            }

            return Ok(auxiliarDto);
        }

        // PUT: api/auxiliares/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putauxiliares(int id, UpdateAuxiliaresDto updateUuxiliares)
        {
            var auxiliar = await _context.auxiliares.FindAsync(id);

            if (auxiliar == null)
            {
                return NotFound($"No se encontro el Auxiliar con el id: {id}");
            }

            _mapper.Map(updateUuxiliares, auxiliar);
            _context.Entry(auxiliar).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!auxiliaresExists(id))
                {
                    return BadRequest($"El Id {id} no coincide con el registro");
                }
                else 
                { 
                    throw; 
                
                }
            }
            return Ok(updateUuxiliares);
        }

        // POST: api/auxiliares
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<auxiliares>> Postauxiliares(AddAuxiliaresDto addAuxiliares)
        {
            var auxiliar = _mapper.Map<auxiliares>(addAuxiliares);
            _context.auxiliares.Add(auxiliar);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getauxiliares", new { id = auxiliar.id }, auxiliar);
        }
   
        private bool auxiliaresExists(int id)
        {
            return _context.auxiliares.Any(e => e.id == id);
        }
    }
}
