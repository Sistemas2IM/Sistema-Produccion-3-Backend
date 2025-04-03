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

        // GET: api/tarjetaOf/filtradas/{vendedor}
        [HttpGet("get/vendedor/dinamico/{vendedor}")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GetTarjetasFiltradas(string vendedor)
        {
            try
            {
                // Primero determinamos el tipo de usuario
                var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.nombres + " " + u.apellidos == vendedor);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                IQueryable<tarjetaOf> query = _context.tarjetaOf
                    .Include(u => u.idEstadoOfNavigation)
                    .Include(r => r.etiquetaOf)
                    .ThenInclude(o => o.idEtiquetaNavigation);

                // Filtrado según tipo de usuario
                switch (vendedor)
                {
                    // Vendedores normales (solo ven sus propias órdenes en proceso)
                    case "Claudia Ruano":
                    case "Jenny Gálvez":
                    case "Juan Mónico":
                    case "Hugo Campos":
                    case "Javier Toledo":
                    case "Xiomara Cruz":
                        query = query.Where(t => t.vendedorOf == vendedor /*&&
                                               t.idEstadoOfNavigation.nombreEstado == "En proceso"*/)
                                   .OrderBy(p => p.posicion);
                        break;

                    // Encargados con acceso completo
                    case "Eliseo Menjívar":
                    case "Fátima García":
                        // Ven todo excepto oficina y freelance
                        query = query.Where(t => t.vendedorOf != "Oficina" /*&&
                                               t.vendedorOf != "freelance"*/)
                                   .OrderBy(p => p.posicion);
                        break;

                    // Encargados con acceso restringido por línea de negocio
                    case "Floridalma Alfaro":
                        query = query.Where(t => t.lineaDeNegocio == "FLEXO" &&
                                               t.vendedorOf != "Oficina" /*&&
                                               t.vendedorOf != "freelance"*/)
                                   .OrderBy(p => p.posicion);
                        break;

                    // Cotizadores con diferentes niveles de acceso
                    case "Ingrid Guevara":
                    case "Katya":
                    case "Elba Deleon":
                        // Oficina y freelance
                        query = query.Where(t => t.vendedorOf == "Oficina" /*||
                                               t.vendedorOf == "freelance"*/)
                                   .OrderBy(p => p.posicion);
                        break;

                    case "Diana Munguia":
                        // Oficina, Engracia y Margarita
                        query = query.Where(t => t.vendedorOf == "Oficina" ||
                                               t.vendedorOf == "Engracia Díaz" ||
                                               t.vendedorOf == "Margarita Díaz")
                                   .OrderBy(p => p.posicion);
                        break;

                    case "Wendy Del Cid":
                        // Todos con línea de negocio promocionales
                        query = query.Where(t => t.lineaDeNegocio == "PROMO")
                                   .OrderBy(p => p.posicion);
                        break;

                    default:
                        return BadRequest("Usuario no tiene permisos configurados");
                }

                var tarjetas = await query.ToListAsync();
                var tarjetasDto = _mapper.Map<List<TarjetaOfDto>>(tarjetas);

                return Ok(tarjetasDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }
    }
}
