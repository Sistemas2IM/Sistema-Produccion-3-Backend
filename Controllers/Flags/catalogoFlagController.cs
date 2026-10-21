using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Sistema_Produccion_3_Backend.DTO.Flags.CatalogoFlag;
using Sistema_Produccion_3_Backend.Models;
using Microsoft.EntityFrameworkCore;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Flags
{
    [Route("api/[controller]")]
    [ApiController]
    public class catalogoFlagController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public catalogoFlagController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // 🚀 1. FLAGS MANUALES CONFIGURADAS POR TABLERO DE PROCESOS
        // GET: api/catalogoFlag/tablero/{idTablero}
        [HttpGet("tablero/{idTablero}")]
        public async Task<ActionResult<IEnumerable<CatalogoFlagDto>>> GetFlagsPorTablero(int idTablero)
        {
            var flags = await _context.tableroFlag
                .AsNoTracking()
                .Where(tf => tf.idTablero == idTablero
                          && tf.idFlagNavigation.activo == true // Cambiar a == 1 si en tu entidad es byte/int
                          && tf.idFlagNavigation.modoAsignacion == "MANUAL")
                .OrderBy(tf => tf.orden)
                .Select(tf => tf.idFlagNavigation)
                .ToListAsync();

            return Ok(_mapper.Map<List<CatalogoFlagDto>>(flags));
        }

        // 🚀 2. FLAGS MANUALES CONFIGURADAS POR TIPO DE DOCUMENTO (Ej: OF en Control de Pedidos)
        // GET: api/catalogoFlag/tipoDocumento/{tipoDocumento}
        [HttpGet("tipoDocumento/{tipoDocumento}")]
        public async Task<ActionResult<IEnumerable<CatalogoFlagDto>>> GetFlagsPorTipoDocumento(int tipoDocumento)
        {
            var flags = await _context.flagTipoDocumento
                .AsNoTracking()
                .Where(ftd => ftd.tipoDocumento == tipoDocumento
                           && ftd.idFlagNavigation.activo == true // Cambiar a == 1 si en tu entidad es byte/int
                           && ftd.idFlagNavigation.modoAsignacion == "MANUAL")
                .OrderBy(ftd => ftd.orden)
                .Select(ftd => ftd.idFlagNavigation)
                .ToListAsync();

            return Ok(_mapper.Map<List<CatalogoFlagDto>>(flags));
        }
    }
}
