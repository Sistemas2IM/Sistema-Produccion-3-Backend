using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaCorteConversion;
using Sistema_Produccion_3_Backend.Models;

[Route("api/[controller]")]
[ApiController]
public class infoMaquinaCorteConversionController : ControllerBase
{
    private readonly base_nuevaContext _context;
    private readonly IMapper _mapper;
    public infoMaquinaCorteConversionController(base_nuevaContext context, IMapper mapper)
    {
        _context = context;
        _mapper = mapper;
    }

    // GET: api/infoMaquinaCorteConversion
    [HttpGet("get")]
    public async Task<ActionResult<IEnumerable<InfoMaquinaCorteConversionDto>>> GetinfoMaquinaCorteConversion()
    {
        var infomaquinacorteconversion = await _context.infoMaquinaCorteConversion.ToListAsync();
        var infomaquinacorteconversionDto = _mapper.Map<List<InfoMaquinaCorteConversionDto>>(infomaquinacorteconversion);

        return Ok(infomaquinacorteconversionDto);
    }

    // GET: api/infoMaquinaCorteConversion/5
    [HttpGet("get/{idmaquina}")]
    public async Task<ActionResult<InfoMaquinaCorteConversionDto>> GetinfoMaquinaCorteConversion(int idmaquina)
    {
        var infomaquinacorteconversion = await _context.infoMaquinaCorteConversion.FindAsync(idmaquina);
        var infomaquinacorteconversionDto = _mapper.Map<InfoMaquinaCorteConversionDto>(infomaquinacorteconversion);

        if (infomaquinacorteconversionDto == null)
        {
            return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
        }

        return Ok(infomaquinacorteconversionDto);
    }

    // PUT: api/infoMaquinaCorteConversion/5
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPut("put/{idmaquina}")]
    public async Task<IActionResult> PutinfoMaquinaCorteConversion(int idmaquina, InfoMaquinaCorteConversionDto infomaquinacorteconversionDto)
    {
        var infoMaquina = await _context.infoMaquinaCorteConversion.FindAsync(idmaquina);

        if (infoMaquina == null)
        {
            return NotFound($"No se encontro la información de la máquina con el id {idmaquina}");
        }

        _mapper.Map(infomaquinacorteconversionDto, infoMaquina);
        _context.Entry(infoMaquina).State = EntityState.Modified;

        try
        {
            await _context.SaveChangesAsync();
        }
        catch (DbUpdateConcurrencyException)
        {
            if (!infoMaquinaCorteConversionExists(idmaquina))
            {
                return NotFound();
            }
            else
            {
                throw;
            }
        }

        return Ok(infomaquinacorteconversionDto);
    }

    // POST: api/infoMaquinaCorteConversion
    // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
    [HttpPost("post")]
    public async Task<ActionResult<InfoMaquinaCorteConversionDto>> PostinfoMaquinaCorteConversion(InfoMaquinaCorteConversionDto infomaquinacorteconversionDto)
    {
        var infomaquinacorteconversion = _mapper.Map<infoMaquinaCorteConversion>(infomaquinacorteconversionDto);
        _context.infoMaquinaCorteConversion.Add(infomaquinacorteconversion);
        await _context.SaveChangesAsync();

        return CreatedAtAction("GetinfoMaquinaCorteConversion", new { idmaquina = infomaquinacorteconversion.idMaquina }, infomaquinacorteconversionDto);
    }

    private bool infoMaquinaCorteConversionExists(int? idmaquina)
    {
        return _context.infoMaquinaCorteConversion.Any(e => e.idMaquina == idmaquina);
    }
}
