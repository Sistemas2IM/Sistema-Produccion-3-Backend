using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.SolicitudDeMateriales
{
    [Route("api/[controller]")]
    [ApiController]
    public class transferenciaProcesoController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public transferenciaProcesoController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<transferenciaProcesoController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<transferenciaProcesoDto>>> GetetiquetaOf()
        {
            var transferenciaProceso = await _context.transferenciaProceso.ToListAsync();
            var transferenciaProcesoDto = _mapper.Map<List<transferenciaProcesoDto>>(transferenciaProceso);

            return (transferenciaProcesoDto);
        }

        // GET api/<transferenciaProcesoController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<transferenciaProcesoDto>> GetetiquetaOf(int id)
        {
            var transferenciaProceso = await _context.transferenciaProceso.FindAsync(id);
            var transferenciaProcesoDto = _mapper.Map<transferenciaProcesoDto>(transferenciaProceso);

            if (transferenciaProcesoDto == null)
            {
                return NotFound($"No se encontro la etiqueta con id {id}");
            }

            return Ok(transferenciaProcesoDto);
        }

        // POST api/<transferenciaProcesoController>
        [HttpPost("post")]
        public async Task<ActionResult<transferenciaProceso>> PostetiquetaOf(AddTransferenciaProcesoDto addTransferenciaProcesoDto)
        {
            var transferenciaProceso = _mapper.Map<transferenciaProceso>(addTransferenciaProcesoDto);
            _context.transferenciaProceso.Add(transferenciaProceso);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetetiquetaOf", new { id = transferenciaProceso.idTransferencia }, transferenciaProceso);
        }

        // PUT api/<transferenciaProcesoController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutetiquetaOf(int id, UpdateTransferenciaProcesoDto updateTransferenciaProcesoDto)
        {
            var transferenciaProceso = await _context.transferenciaProceso.FindAsync(id);
            if (transferenciaProceso == null)
            {
                return NotFound($"No se encontro la etiqueta con el id {id}");
            }

            _mapper.Map(updateTransferenciaProcesoDto, transferenciaProceso);
            _context.Entry(transferenciaProceso).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!transferenciaProcesoExist(id))
                {
                    return BadRequest();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateTransferenciaProcesoDto);
        }

        private bool transferenciaProcesoExist(int id)
        {
            return _context.transferenciaProceso.Any(e => e.idTransferencia == id);
        }

    }
}
