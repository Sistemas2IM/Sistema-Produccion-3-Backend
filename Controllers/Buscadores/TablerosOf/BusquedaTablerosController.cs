using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Buscadores.DTOGlobales;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Buscadores.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusquedaTablerosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public BusquedaTablerosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/buscarGlobal/{termino}
        [HttpGet("buscarGlobal/{termino}")]
        public async Task<ActionResult<object>> BuscarGlobal(string termino)
        {
            termino = termino.Trim();

            // 1. INTELIGENCIA DE BÚSQUEDA: Separar en palabras
            // Ignoramos palabras de 1 sola letra para no saturar la base de datos (ej. "y", "a", "o")
            var terminosBusqueda = termino.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                          .Where(t => t.Length > 1)
                                          .ToList();

            // Si el usuario buscó literalmente solo una letra (ej. "A"), la usamos como único término
            if (!terminosBusqueda.Any())
            {
                terminosBusqueda.Add(termino);
            }

            // 2. PREPARAR LAS CONSULTAS BASE (Sin ejecutar aún)
            var queryProcesos = _context.procesoOf.AsNoTracking().AsSplitQuery()
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .AsQueryable();

            var queryTarjetas = _context.tarjetaOf.AsNoTracking().AsSplitQuery()
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .AsQueryable();

            var queryTableros = _context.tablerosOf.AsNoTracking().AsQueryable();

            var queryEntregas = _context.entregasProductoTerminado.AsNoTracking()
                .Include(f => f.ofNavigation)
                .AsQueryable();

            var querySolicitudes = _context.solicitudMaterialesOf.AsNoTracking()
                .Include(f => f.oFNavigation)
                .Include(sm => sm.idSolicitudNavigation)
                .AsQueryable();

            // 3. CONSTRUCCIÓN DINÁMICA: Aplicar cada palabra como un filtro obligatorio (AND)
            foreach (var t in terminosBusqueda)
            {
                var terminoActual = t.ToLower();
                bool esNumero = int.TryParse(terminoActual, out int numActual);

                queryProcesos = queryProcesos.Where(u =>
                    (esNumero && u.oF == numActual) ||
                    (esNumero && u.oFNavigation != null && u.oFNavigation.oV == numActual) ||
                    u.oF.ToString().Contains(terminoActual) ||
                    (u.oFNavigation != null && u.oFNavigation.oV.ToString().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.clienteOf != null && u.oFNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.vendedorOf != null && u.oFNavigation.vendedorOf.ToLower().Contains(terminoActual)) || // 🚀 ¡AQUÍ ESTÁ! Ahora sí busca al ejecutivo
                    (u.productoOf != null && u.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.codArticulo != null && u.oFNavigation.codArticulo.ToLower().Contains(terminoActual)) ||
                    (u.idMaquinaSAP != null && u.idMaquinaSAP.ToLower().Contains(terminoActual)));

                queryTarjetas = queryTarjetas.Where(u =>
                    (esNumero && u.oF == numActual) ||
                    (esNumero && u.oV == numActual) ||
                    u.oF.ToString().Contains(terminoActual) ||
                    (u.oV != null && u.oV.ToString().Contains(terminoActual)) ||
                    (u.clienteOf != null && u.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.vendedorOf != null && u.vendedorOf.ToLower().Contains(terminoActual)) || // (Este ya lo tenías, pero le agregamos protección null)
                    (u.productoOf != null && u.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.codArticulo != null && u.codArticulo.ToLower().Contains(terminoActual)));

                queryTableros = queryTableros.Where(u =>
                    (u.nombreTablero != null && u.nombreTablero.ToLower().Contains(terminoActual)) ||
                    (u.idSapMaquina != null && u.idSapMaquina.ToLower().Contains(terminoActual)));

                queryEntregas = queryEntregas.Where(u =>
                    (esNumero && u.idEntregaPt == numActual) ||
                    (esNumero && u.of == numActual) ||
                    u.of.ToString().Contains(terminoActual) ||
                    (u.ofNavigation != null && u.ofNavigation.clienteOf != null && u.ofNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.ofNavigation != null && u.ofNavigation.vendedorOf != null && u.ofNavigation.vendedorOf.ToLower().Contains(terminoActual)) || // 🚀 Vendedor agregado
                    (u.ofNavigation != null && u.ofNavigation.oV != null && u.ofNavigation.oV.ToString().Contains(terminoActual)) ||
                    (u.ofNavigation != null && u.ofNavigation.productoOf != null && u.ofNavigation.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.ofNavigation != null && u.ofNavigation.codArticulo != null && u.ofNavigation.codArticulo.ToLower().Contains(terminoActual)));

                querySolicitudes = querySolicitudes.Where(u =>
                    (esNumero && u.idSolicitud == numActual) ||
                    (esNumero && u.oF == numActual) ||
                    (u.idSolicitudNavigation != null && u.idSolicitudNavigation.materialDescripcion != null && u.idSolicitudNavigation.materialDescripcion.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.productoOf != null && u.oFNavigation.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.clienteOf != null && u.oFNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.vendedorOf != null && u.oFNavigation.vendedorOf.ToLower().Contains(terminoActual)) || // 🚀 Vendedor agregado
                    (u.oFNavigation != null && u.oFNavigation.oV != null && u.oFNavigation.oV.ToString().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.codArticulo != null && u.oFNavigation.codArticulo.ToLower().Contains(terminoActual)));
            }

            // 4. EJECUTAR CONSULTAS CON LÍMITES
            var procesosOf = await queryProcesos.Take(30).ToListAsync();
            var tarjetasOf = await queryTarjetas.Take(30).ToListAsync();
            var tableros = await queryTableros.Take(20).ToListAsync();
            var entregasProductoTerminado = await queryEntregas.Take(30).ToListAsync();
            var solicitudesMateriales = await querySolicitudes.Take(30).ToListAsync();

            // 5. MAPEO Y RESPUESTA
            var procesosOfDto = _mapper.Map<List<SB_ProcesoOfDto>>(procesosOf);
            var tarjetasOfDto = _mapper.Map<List<SB_TarjetaOfDto>>(tarjetasOf);
            var entregasProductoTerminadoDto = _mapper.Map<List<SB_ProductoTerminadoDto>>(entregasProductoTerminado);
            var solicitudesMaterialesDto = _mapper.Map<List<SB_SolicitudMaterialesOfDto>>(solicitudesMateriales);

            var resultado = new
            {
                ProcesosOf = procesosOfDto,
                TarjetasOf = tarjetasOfDto,
                //Tableros = tablerosDto,
                EntregaProductoTerminado = entregasProductoTerminadoDto,
                SolicitudesMateriales = solicitudesMaterialesDto
            };

            if (!procesosOfDto.Any() && !tarjetasOfDto.Any() && !entregasProductoTerminadoDto.Any() && !solicitudesMaterialesDto.Any())
            {
                return NotFound($"No se encontraron resultados para el término de búsqueda: {termino}");
            }

            return Ok(resultado);
        }

    }
}
