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
                .Where(f => f.idTableroNavigation.idArea == idArea)
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

        [HttpGet("get/procesosOfGestion/filtros")]
        public async Task<ActionResult<IEnumerable<ProcesoOfVistaTableroDto>>> GetProcesoOfAreaFiltro(
        [FromQuery] int? idArea = null,          // Parámetro obligatorio para el área
        [FromQuery] DateTime? fechaInicio = null,   // Parámetro opcional para la fecha de inicio del rango
        [FromQuery] DateTime? fechaFin = null,     // Parámetro opcional para la fecha de fin del rango
        [FromQuery] string? cliente = null,         // Parámetro opcional para el cliente
        [FromQuery] string? ejecutivo = null,
        [FromQuery] string? articulo = null,     // Parámetro opcional para el artículo
        [FromQuery] int? oF = null,               // Parámetro opcional para el oF
        [FromQuery] int? oV = null,               // Parámetro opcional para el oV
        [FromQuery] string? lineaNegocio = null, // Parámetro opcional para la línea de negocio
        [FromQuery] string? idsEtiquetas = null, // Parámetro opcional para las etiquetas
        [FromQuery] int? idProceso = null,        // Parámetro opcional para la postura
        [FromQuery] int? tablero = null)      // Parámetro opcional para el ejecutivo
        {
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
                // Filtrar por área
                query = query.Where(p => p.idTableroNavigation.idArea == idArea.Value);
            }
            if (fechaInicio.HasValue && fechaFin.HasValue)
            {
                // Filtrar por rango de fechas (fechaVencimiento entre fechaInicio y fechaFin)
                query = query.Where(p => p.fechaVencimiento >= fechaInicio.Value && p.fechaVencimiento <= fechaFin.Value);
            }
            else if (fechaInicio.HasValue)
            {
                // Si solo se proporciona fechaInicio, filtrar desde esa fecha en adelante
                query = query.Where(p => p.fechaVencimiento >= fechaInicio.Value);
            }
            else if (fechaFin.HasValue)
            {
                // Si solo se proporciona fechaFin, filtrar hasta esa fecha
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
            // Filtro por IDs de etiquetas
            if (!string.IsNullOrEmpty(idsEtiquetas))
            {
                // Convertir la cadena de IDs separados por comas en una lista de enteros
                var idsEtiquetasLista = idsEtiquetas.Split(',')
                    .Select(id => int.Parse(id))
                    .ToList();

                // Filtrar tarjetas que tengan al menos una de las etiquetas especificadas
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

            // Ejecutar la consulta y mapear a DTO
            var procesoOf = await query.ToListAsync();
            var procesoOfDto = _mapper.Map<List<ProcesoOfVistaTableroDto>>(procesoOf);

            return Ok(procesoOfDto);
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
