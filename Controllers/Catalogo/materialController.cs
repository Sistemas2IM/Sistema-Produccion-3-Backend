using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.MaterialOf;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class materialController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public materialController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<materialController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MaterialDto>>> GetMaterial()
        {
            var material = await _context.material.ToListAsync();
            var materialDto = _mapper.Map<List<MaterialDto>>(material);

            return Ok(materialDto);
        }

        // GET api/<materialController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MaterialDto>> GetMaterial(string id)
        {
            var material = await _context.material.FindAsync(id);
            var materialDto = _mapper.Map<MaterialDto>(material);

            if (materialDto == null)
            {
                return NotFound($"No se encontro el material con el id {id}");
            }

            return Ok(materialDto);
        }


        // PUT api/<materialController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutMaterial(string id,UpdateMaterialOfDto updateMaterialOf)
        {
            var material = await _context.material.FindAsync(id);

            if (material == null)
            {
                return NotFound($"No se encontro el material con el ID {id}");
            }

            _mapper.Map(updateMaterialOf, material);
            _context.Entry(material).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!materialExists(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }
            return Ok(updateMaterialOf);
        }

        // POST api/<materialController>
        [HttpPost("post")]
        public async Task<ActionResult<maquinas>> Postmaterial(AddMaterialOfDto addMaterialOf)
        {
            var material = _mapper.Map<material>(addMaterialOf);
            _context.material.Add(material);
            await _context.SaveChangesAsync();

            return CreatedAtAction("Getmaquinas", new { id = material.idMaterial }, material);
        }


        private bool materialExists(string id)
        {
            return _context.material.Any(e => e.idMaterial == id);
        }
    }
}
