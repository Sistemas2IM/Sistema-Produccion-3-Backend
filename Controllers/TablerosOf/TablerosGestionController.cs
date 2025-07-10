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
                .Include(u => u.idMaquinaNavigation)
                .ThenInclude(m => m.idFamiliaNavigation)
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
                .Where(f => f.idTableroNavigation.idArea == idArea && f.archivada == false && f.cancelada == false)
                .Include(u => u.detalleReporte)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(u => u.detalleReporte)
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

        // ===============================================================================

        [HttpGet("get/tableros/linea/{linea}")]
        public async Task<ActionResult<IEnumerable<TablerosOfDto>>> GetTablerosLineaNegocio(string linea)
        {
            var tableros = await _context.tablerosOf
                .Include(t => t.idAreaNavigation)
                .Include(t => t.idMaquinaNavigation)
                    .ThenInclude(m => m.idFamiliaNavigation)
                .Include(t => t.posturasOf)
                    .ThenInclude(p => p.procesoOf)
                        .ThenInclude(proc => proc.oFNavigation)
                .Where(t => t.posturasOf.Any(p =>
                    p.procesoOf.Any(proc =>
                        proc.oFNavigation != null &&
                        proc.oFNavigation.lineaDeNegocio == linea)))
                .ToListAsync();

            var tablerosDto = _mapper.Map<List<TablerosOfDto>>(tableros);
            return Ok(tablerosDto);
        }


        // GET: api/procesoOf por Area
        [HttpGet("get/procesosOf/linea{linea}")]
        public async Task<ActionResult<IEnumerable<ProcesoOfVistaTableroDto>>> GetprocesoOfLineaNegocio(string linea)
        {
            var procesoOf = await _context.procesoOf
                .OrderBy(p => p.posicion)
                .Where(f => f.oFNavigation.lineaDeNegocio == linea && f.archivada == false && f.cancelada == false)
                .Include(u => u.detalleReporte)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(u => u.detalleReporte)
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

        // ==============================================================================

        [HttpGet("get/procesosOfGestion/filtros")]
        public async Task<ActionResult<IEnumerable<ProcesoOfVistaTableroDto>>> GetProcesoOfAreaFiltro(
        [FromQuery] int? idArea = null,
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null,
        [FromQuery] string? cliente = null,
        [FromQuery] string? ejecutivo = null,
        [FromQuery] string? articulo = null,
        [FromQuery] int? oF = null,
        [FromQuery] int? oV = null,
        [FromQuery] string? lineaNegocio = null,
        [FromQuery] string? idsEtiquetas = null,
        [FromQuery] int? idProceso = null,
        [FromQuery] int? tablero = null,
        [FromQuery] bool mostrarArchivados = false, // Parámetro opcional para la postura
        [FromQuery] string? vendedor = null) // Nuevo parámetro para el vendedor
        {
            try
            {
                // Validar el usuario si se proporciona el parámetro vendedor
                if (!string.IsNullOrEmpty(vendedor))
                {
                    var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.nombres + " " + u.apellidos == vendedor);
                    if (usuario == null)
                    {
                        return NotFound("Usuario no encontrado");
                    }
                }

                // Consulta base
                var query = _context.procesoOf
                    .OrderBy(p => p.posicion)
                    .Include(u => u.detalleOperacionProceso)
                    .ThenInclude(o => o.idOperacionNavigation)
                    .Include(m => m.tarjetaCampo)
                    .Include(s => s.tarjetaEtiqueta)
                    .ThenInclude(e => e.idEtiquetaNavigation)
                    .Include(f => f.oFNavigation)
                    .Include(l => l.idPosturaNavigation)
                    .Include(v => v.idMaterialNavigation)
                    .Include(a => a.asignacion)
                    .ThenInclude(u => u.userNavigation)
                    .AsQueryable();

                // Aplicar filtros condicionales
                if (idArea.HasValue)
                {
                    query = query.Where(p => p.idTableroNavigation.idArea == idArea.Value);
                }

                if (fechaInicio.HasValue && fechaFin.HasValue)
                {
                    query = query.Where(p => p.fechaVencimiento >= fechaInicio.Value && p.fechaVencimiento <= fechaFin.Value);
                }
                else if (fechaInicio.HasValue)
                {
                    query = query.Where(p => p.fechaVencimiento >= fechaInicio.Value);
                }
                else if (fechaFin.HasValue)
                {
                    query = query.Where(p => p.fechaVencimiento <= fechaFin.Value);
                }

                if (!string.IsNullOrEmpty(cliente))
                {
                    query = query.Where(p => p.oFNavigation.clienteOf.Contains(cliente));
                }

                if (!string.IsNullOrEmpty(ejecutivo))
                {
                    query = query.Where(p => p.oFNavigation.vendedorOf.Contains(ejecutivo));
                }
                
                if (!string.IsNullOrEmpty(articulo))
                {
                    query = query.Where(p => p.oFNavigation.productoOf.Contains(articulo));
                }

                if (oF.HasValue)
                {
                    query = query.Where(p => p.oF == oF.Value);
                }

                if (oV.HasValue)
                {
                    query = query.Where(p => p.oV == oV.Value);
                }

                if (!string.IsNullOrEmpty(lineaNegocio))
                {
                    query = query.Where(p => p.oFNavigation.lineaDeNegocio.Contains(lineaNegocio));
                }

                if (!string.IsNullOrEmpty(idsEtiquetas))
                {
                    var idsEtiquetasLista = idsEtiquetas.Split(',')
                        .Select(id => int.Parse(id))
                        .ToList();

                    query = query.Where(p => p.tarjetaEtiqueta
                        .Any(etiquetaOf => idsEtiquetasLista.Contains((int)etiquetaOf.idTarjetaEtiqueta)));
                }

                if (idProceso.HasValue)
                {
                    query = query.Where(p => p.idProceso == idProceso.Value);
                }

                if (tablero.HasValue)
                {
                    query = query.Where(p => p.idTablero == tablero.Value);
                }
                // ✅ Aplicar el filtro solo si NO se quieren mostrar los archivados
                if (!mostrarArchivados)
                    query = query.Where(p => p.archivada == false);

                // Aplicar filtros por usuario si se proporciona el parámetro vendedor
                if (!string.IsNullOrEmpty(vendedor))
                {
                    switch (vendedor)
                    {
                        // Vendedores normales (solo ven sus propias órdenes)
                        case "Claudia Ruano":
                        case "Jenny Gálvez":
                        case "Juan Mónico":
                        case "Hugo Campos":
                        case "Javier Toledo":
                        case "Xiomara Cruz":
                            query = query.Where(p => p.oFNavigation.vendedorOf == vendedor);
                            break;

                        // Encargados con acceso completo
                        case "Eliseo Menjívar":
                        case "Fátima García":
                            // Ven todo excepto oficina y freelance
                            query = query.Where(p => p.oFNavigation.vendedorOf != "Oficina");
                            break;

                        // Gerente de ventas con acceso completo
                        case "Manuel Díaz":
                        case "Root Admin":
                        case "Gabriela Menendez":
                            // No aplica filtro, ve todo
                            break;

                        // Encargados con acceso restringido por línea de negocio
                        case "Floridalma Alfaro":
                            query = query.Where(p => p.oFNavigation.lineaDeNegocio == "FLEXO" &&
                                                  p.oFNavigation.vendedorOf != "Oficina");
                            break;

                        // Cotizadores con diferentes niveles de acceso
                        case "Ingrid Guevara":
                        case "Katya":
                        case "Elba Deleon":
                            // Oficina y freelance
                            query = query.Where(p => p.oFNavigation.vendedorOf != "Oficina");
                            break;

                        case "Diana Munguia":
                            // Oficina, Engracia y Margarita
                            query = query.Where(p => p.oFNavigation.vendedorOf == "Oficina" ||
                                                  p.oFNavigation.vendedorOf == "Engracia Díaz" ||
                                                  p.oFNavigation.vendedorOf == "Margarita Díaz");
                            break;

                        case "Wendy Del Cid":
                            // Todos con línea de negocio promocionales
                            query = query.Where(p => p.oFNavigation.lineaDeNegocio == "PROMO");
                            break;

                        default:
                            return BadRequest("Usuario no tiene permisos configurados");
                    }
                }

                // Ejecutar la consulta y mapear a DTO
                var procesoOf = await query.ToListAsync();
                var procesoOfDto = _mapper.Map<List<ProcesoOfVistaTableroDto>>(procesoOf);

                return Ok(procesoOfDto);
            }
            catch (Exception ex)
            {
                return StatusCode(500, $"Error interno del servidor: {ex.Message}");
            }
        }

        // GET: api/tarjetaOf VENDEDORES
        /*[HttpGet("get/vendedor{vendedor}")]
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
        }*/

        // GET: api/tarjetaOf/filtradas/{vendedor}
        [HttpGet("get/vendedor{vendedor}")]
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

                    // Gerente de ventas con acceso completo
                    case "Manuel Díaz":
                    case "Root Admin":
                    case "Gabriela Menendez":
                        // No aplica filtro, ve todo
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
                        query = query.Where(t => t.vendedorOf != "Oficina" /*||
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

        [HttpGet("get/tarjetasOf/Vendedor/Filtros")]
        public async Task<ActionResult<IEnumerable<TarjetaOfDto>>> GetTarjetasOfVendedorFiltro(
        [FromQuery] string vendedor,             // Vendedor (requerido para aplicar reglas)
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null,
        [FromQuery] string cliente = null,
        [FromQuery] string ejecutivo = null,
        [FromQuery] string articulo = null,
        [FromQuery] int? of = null,
        [FromQuery] int? ov = null,
        [FromQuery] string? lineaNegocio = null,
        [FromQuery] bool mostrarArchivados = false,   // Parámetro opcional para la postura
        [FromQuery] string? idsEtiquetas = null)
        {
            try
            {
                var usuario = await _context.usuario.FirstOrDefaultAsync(u => (u.nombres + " " + u.apellidos) == vendedor);
                if (usuario == null)
                {
                    return NotFound("Usuario no encontrado");
                }

                var query = _context.tarjetaOf
                    .Include(u => u.idEstadoOfNavigation)
                    .Include(r => r.etiquetaOf)
                    .ThenInclude(o => o.idEtiquetaNavigation)
                    .AsQueryable();

                // === Reglas por tipo de vendedor ===
                switch (vendedor)
                {
                    case "Claudia Ruano":
                    case "Jenny Gálvez":
                    case "Juan Mónico":
                    case "Hugo Campos":
                    case "Javier Toledo":
                    case "Xiomara Cruz":
                        query = query.Where(t => t.vendedorOf == vendedor);
                        break;

                    case "Eliseo Menjívar":
                    case "Fátima García":
                        query = query.Where(t => t.vendedorOf != "Oficina" && t.vendedorOf != "freelance");
                        break;

                    case "Floridalma Alfaro":
                        query = query.Where(t => t.lineaDeNegocio == "FLEXO" && t.vendedorOf != "Oficina" && t.vendedorOf != "freelance");
                        break;

                    case "Ingrid Guevara":
                    case "Katya":
                    case "Elba Deleon":
                        query = query.Where(t => t.vendedorOf == "Oficina" || t.vendedorOf == "freelance");
                        break;

                    case "Diana Munguia":
                        query = query.Where(t => t.vendedorOf == "Oficina" || t.vendedorOf == "Engracia Díaz" || t.vendedorOf == "Margarita Díaz");
                        break;

                    case "Wendy Del Cid":
                        query = query.Where(t => t.lineaDeNegocio == "PROMO");
                        break;

                    default:
                        return BadRequest("Usuario no tiene permisos configurados");
                }

                // === Aplicar filtros adicionales de búsqueda ===

                if (fechaInicio.HasValue && fechaFin.HasValue)
                    query = query.Where(p => p.fechaVencimiento >= fechaInicio && p.fechaVencimiento <= fechaFin);
                else if (fechaInicio.HasValue)
                    query = query.Where(p => p.fechaVencimiento >= fechaInicio);
                else if (fechaFin.HasValue)
                    query = query.Where(p => p.fechaVencimiento <= fechaFin);

                if (!string.IsNullOrEmpty(cliente))
                    query = query.Where(p => p.clienteOf.Contains(cliente));

                if (!string.IsNullOrEmpty(ejecutivo))
                    query = query.Where(p => p.vendedorOf.Contains(ejecutivo));

                if (!string.IsNullOrEmpty(articulo))
                    query = query.Where(p => p.productoOf.Contains(articulo));

                if (!string.IsNullOrEmpty(lineaNegocio))
                    query = query.Where(p => p.lineaDeNegocio.Contains(lineaNegocio));

                if (of.HasValue)
                    query = query.Where(p => p.oF == of.Value);

                if (ov.HasValue)
                    query = query.Where(p => p.oV == ov.Value);

                if (!string.IsNullOrEmpty(idsEtiquetas))
                {
                    var idsEtiquetasLista = idsEtiquetas.Split(',').Select(id => int.Parse(id)).ToList();
                    query = query.Where(p => p.etiquetaOf.Any(e => idsEtiquetasLista.Contains((int)e.idEtiqueta)));
                }
                // ✅ Aplicar el filtro solo si NO se quieren mostrar los archivados
                if (!mostrarArchivados)
                    query = query.Where(p => p.archivada == false);

                // === Obtener resultado y retornar ===
                var tarjetas = await query.OrderBy(p => p.posicion).ToListAsync();
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
