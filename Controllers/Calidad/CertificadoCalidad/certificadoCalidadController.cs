using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.CertificadoCalidad
{
    [Route("api/[controller]")]
    [ApiController]
    public class certificadoCalidadController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public certificadoCalidadController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<certificadoCalidadController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<CertificadoCalidadDto>>> GetCertificado()
        {
            var certificado = await _context.certificadoCalidad
                .OrderByDescending(c => c.idCertificadoCalidad)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idUnidadNavigation)
                .Include(c => c.oFNavigation)
                .Where(c => c.archivado == false)
                .ToListAsync();

            var certificadoDto = _mapper.Map<List<CertificadoCalidadDto>>(certificado);

            return Ok(certificadoDto);

        }

        // GET api/<certificadoCalidadController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<CertificadoCalidadDto>> Get(int id)
        {
            var certificado = await _context.certificadoCalidad
                .OrderByDescending(c => c.idCertificadoCalidad)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idUnidadNavigation)
                .Include(c => c.oFNavigation)
                .Where(c => c.archivado == false)
                .FirstOrDefaultAsync(u => u.idCertificadoCalidad == id);
            if (certificado == null)
            {
                return NotFound();
            }
            var certificadoDto = _mapper.Map<CertificadoCalidadDto>(certificado);
            return Ok(certificadoDto);
        }

        // GET: api/<certificadoCalidadController>
        [HttpGet("get/of/{of}")]
        public async Task<ActionResult<IEnumerable<CertificadoCalidadDto>>> GetCertificadoOf(int of)
        {
            var certificado = await _context.certificadoCalidad
                .OrderByDescending(c => c.idCertificadoCalidad)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idUnidadNavigation)
                .Include(c => c.oFNavigation)
                .Where(c => c.oF == of && c.archivado == false)
                .ToListAsync();

            var certificadoDto = _mapper.Map<List<CertificadoCalidadDto>>(certificado);

            return Ok(certificadoDto);

        }

        // GET: api/<certificadoCalidadController>
        [HttpGet("get/lineaNegocio/{linea}")]
        public async Task<ActionResult<IEnumerable<CertificadoCalidadDto>>> GetCertificadoLinea(bool linea)
        {
            // 1. Construimos la consulta base (Query)
            var query = _context.certificadoCalidad
                .OrderByDescending(c => c.idCertificadoCalidad)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idVariableNavigation)
                .Include(c => c.detalleCertificadoCalidad)
                .ThenInclude(d => d.idUnidadNavigation)
                .Include(c => c.oFNavigation)
                .Where(c => c.archivado == false);

            // 2. Aplicamos el filtro condicional sobre la relación oFNavigation
            // RECUERDA: Cambia 'NombreLineaNegocio' por el nombre real de tu columna en la BD
            if (linea)
            {
                // Si es true: Solo FLEXO
                query = query.Where(c => c.oFNavigation.lineaDeNegocio == "FLEXO");
            }
            else
            {
                // Si es false: Todo lo que NO sea FLEXO
                query = query.Where(c => c.oFNavigation.lineaDeNegocio != "FLEXO");
            }

            // 3. Ejecutamos la consulta (Ahora sí vamos a la BD)
            var certificado = await query.ToListAsync();

            // 4. Mapeamos y retornamos
            var certificadoDto = _mapper.Map<List<CertificadoCalidadDto>>(certificado);

            return Ok(certificadoDto);
        }

        // POST api/<certificadoCalidadController>
        [HttpPost("post")]
        public async Task<ActionResult<certificadoCalidad>> PostCertificadoCalidad(AddCertificadoCalidadDto addCertificadoCalidadDto)
        {
            var certificado = _mapper.Map<certificadoCalidad>(addCertificadoCalidadDto);

            _context.certificadoCalidad.Add(certificado);
            await _context.SaveChangesAsync();

            return CreatedAtAction("get", new { id = certificado.idCertificadoCalidad }, certificado);
        }

        // PUT api/<certificadoCalidadController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutCertificadoCalidad(int id, UpdateCertificadoCalidadDto updateCertificadoCalidadDto)
        {
            var certificado = await _context.certificadoCalidad.FindAsync(id);

            if (certificado == null)
            {
                return NotFound();
            }

            _mapper.Map(updateCertificadoCalidadDto, certificado);
            _context.Entry(certificado).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CertificadoCalidadExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateCertificadoCalidadDto);
        }

        private bool CertificadoCalidadExists(int id)
        {
            return _context.certificadoCalidad.Any(e => e.idCertificadoCalidad == id);
        }
    }
}
