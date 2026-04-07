using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.CatalogoTipo;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class catalogoMaquinasController : ControllerBase
    {

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public catalogoMaquinasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET CATALOGO TIPO ACABADO
        [HttpGet("get/tipoAcabado")]
        public async Task<ActionResult<IEnumerable<CatalogoTipoAcabadoDto>>> GetCatalogoTipoAcabado()
        {
            var tiposAcabado = await _context.catalogoTipoAcabado.Where(x => x.activo == true).ToListAsync();
            var tiposAcabadoDto = _mapper.Map<List<CatalogoTipoAcabadoDto>>(tiposAcabado);
            return Ok(tiposAcabadoDto);
        }

        // GET CATALOGO TIPO PAPEL
        [HttpGet("get/tipoPapel")]
        public async Task<ActionResult<IEnumerable<CatalogoTipoPapelDto>>> GetCatalogoTipoPapel()
        {
            var tiposPapel = await _context.catalogoTipoPapel.Where(x => x.activo == true).ToListAsync();
            var tiposPapelDto = _mapper.Map<List<CatalogoTipoPapelDto>>(tiposPapel);
            return Ok(tiposPapelDto);
        }

        // GET CATALOGO USO TIPICO
        [HttpGet("get/usoTipico")]
        public async Task<ActionResult<IEnumerable<CatalogoUsoTipicoDto>>> GetCatalogoUsoTipico()
        {
            var usosTipicos = await _context.catalogoUsoTipico.Where(x => x.activo == true).ToListAsync();
            var usosTipicosDto = _mapper.Map<List<CatalogoUsoTipicoDto>>(usosTipicos);
            return Ok(usosTipicosDto);
        }
    }
}
