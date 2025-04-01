using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class TablerosGestionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public TablerosGestionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/procesoOf FLEXOGRAFIA
        [HttpGet("get/procesosOf/flexo")]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOfFlexo()
        {
            var procesoOf = await _context.procesoOf
                .Where(f => f.oFNavigation.lineaDeNegocio == "FLEXO")
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .ThenInclude(e => e.idEtiquetaNavigation)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .ToListAsync();

            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesoOf);

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf FLEXOGRAFIA
        [HttpGet("get/tablero/flexoPrensa")]
        public async Task<ActionResult<IEnumerable<TablerosOfDto>>> GetTableroFlexoPrensa()
        {
            var tableros = await _context.tablerosOf
                .Where(f => f.idArea == 1)
                .Include(u => u.idAreaNavigation)
                .ToListAsync();

            var tablerosDto = _mapper.Map<List<TablerosOfDto>>(tableros);

            return Ok(tablerosDto);
        }

        // GET: api/procesoOf FLEXOGRAFIA
        [HttpGet("get/tablero/flexoProceso")]
        public async Task<ActionResult<IEnumerable<TablerosOfDto>>> GetTableroFlexoProceso()
        {
            var tableros = await _context.tablerosOf
                .Where(f => f.idArea == 11)
                .Include(u => u.idAreaNavigation)
                .ToListAsync();

            var tablerosDto = _mapper.Map<List<TablerosOfDto>>(tableros);

            return Ok(tablerosDto);
        }

        // GET: api/tableros por area
        [HttpGet("get/tableros/area{idArea}")]
        public async Task<ActionResult<IEnumerable<TablerosOfDto>>> GetTablerosArea(int idArea)
        {
            var tableros = await _context.tablerosOf
                .Where(f => f.idArea == idArea)
                .Include(u => u.idAreaNavigation)
                .ToListAsync();
            var tablerosDto = _mapper.Map<List<TablerosOfDto>>(tableros);
            return Ok(tablerosDto);
        }

        // GET: api/procesoOf por Area
        [HttpGet("get/procesosOf/area{idArea}")]
        public async Task<ActionResult<IEnumerable<ProcesoOfVistaTableroDto>>> GetprocesoOfArea(int idArea)
        {
            var procesoOf = await _context.procesoOf
                .OrderBy(p => p.posicion)
                .Where(f => f.idTableroNavigation.idArea == idArea)
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(m => m.maquinaNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .ThenInclude(e => e.idEtiquetaNavigation)
                .Include(f => f.oFNavigation)
                .Include(l => l.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(a => a.asignacion)
                .ThenInclude(u => u.userNavigation)
                .ToListAsync();
            var procesoOfDto = _mapper.Map<List<ProcesoOfVistaTableroDto>>(procesoOf);
            return Ok(procesoOfDto);
        }

        // GET: api/tarjetaOf VENDEDORES
        [HttpGet("get/vendedor{vendedor}")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GettarjetaOf(string vendedor)
        {
            var tarjetaOf = await _context.tarjetaOf
                .Where(v => v.vendedorOf == vendedor)
                .OrderBy(p => p.posicion)
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .ThenInclude(o => o.idEtiquetaNavigation)
                .ToListAsync();

            var tarjetaOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetaOf);

            return Ok(tarjetaOfDto);
        }
    }
}
