using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
            // Opcional: Limpiamos espacios en blanco accidentales
            termino = termino.Trim();

            // Validamos si el término es un número entero exacto para optimizar la búsqueda de OF y OV
            bool esNumero = int.TryParse(termino, out int numeroBuscado);

            // 1. PROCESOS OF
            var procesosOf = await _context.procesoOf
                .AsNoTracking() // Libera la memoria del tracker
                .AsSplitQuery() // Evita explosión cartesiana por los Includes
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .Where(u =>
                    // Si es número, buscamos coincidencia exacta (Usa índices y es 100x más rápido)
                    // Si quieres que busque "12" dentro de "123", mantén u.oF.ToString().Contains(termino)
                    (esNumero && u.oF == numeroBuscado) ||
                    (esNumero && u.oFNavigation.oV == numeroBuscado) ||
                    u.oF.ToString().Contains(termino) ||
                    u.oFNavigation.clienteOf.Contains(termino) ||
                    u.productoOf.Contains(termino) ||
                    u.oFNavigation.codArticulo.Contains(termino) ||
                    u.idMaquinaSAP.Contains(termino))
                .Take(30) // 🚀 EL SALVAVIDAS: Solo traemos los primeros 30 resultados
                .ToListAsync();

            // 2. TARJETAS OF
            var tarjetasOf = await _context.tarjetaOf
                .AsNoTracking()
                .AsSplitQuery()
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .Where(u =>
                    (esNumero && u.oF == numeroBuscado) ||
                    (esNumero && u.oV == numeroBuscado) ||
                    u.oF.ToString().Contains(termino) ||
                    u.clienteOf.Contains(termino) ||
                    u.vendedorOf.Contains(termino) ||
                    u.productoOf.Contains(termino) ||
                    u.codArticulo.Contains(termino))
                .Take(30) // 🚀 Limitamos los resultados
                .ToListAsync();

            // 3. TABLEROS
            var tableros = await _context.tablerosOf
                .AsNoTracking()
                .Where(u =>
                    u.nombreTablero.Contains(termino) ||
                    u.idSapMaquina.Contains(termino))
                .Take(20) // 🚀 Limitamos los resultados
                .ToListAsync();

            // 4. ENTREGA DE PRODUCTO TERMINADO
            var entregasProductoTerminado = await _context.entregasProductoTerminado
                    .AsNoTracking()
                    .Where(u =>
                        (u.idEntregaPt == numeroBuscado) ||
                        (esNumero && u.of == numeroBuscado) ||
                        u.of.ToString().Contains(termino) ||
                        u.ofNavigation.clienteOf.Contains(termino) ||
                        u.ofNavigation.productoOf.Contains(termino) ||
                        u.ofNavigation.codArticulo.Contains(termino))
                    .Take(30)
                    .ToListAsync();

            // 5. SOLICITUD DE MATERIALES
                var solicitudesMateriales = await _context.solicitudMaterialesOf
                        .AsNoTracking()
                        .Where(u =>
                            (u.idSolicitud == numeroBuscado) ||
                            (u.oF == numeroBuscado) ||
                            u.idSolicitudNavigation.materialDescripcion.Contains(termino) ||
                            u.oFNavigation.productoOf.Contains(termino) ||
                            u.oFNavigation.clienteOf.Contains(termino) ||
                            u.oFNavigation.codArticulo.Contains(termino))
                        .Take(30)
                        .ToListAsync();

            // Mapear los resultados a DTOs
            var procesosOfDto = _mapper.Map<List<ProcesoOfDto>>(procesosOf);
            var tarjetasOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetasOf);
            var tablerosDto = _mapper.Map<List<TablerosOfDto>>(tableros);
            var entregasProductoTerminadoDto = _mapper.Map<List<ProductoTerminadoDto>>(entregasProductoTerminado);
            var solicitudesMaterialesDto = _mapper.Map<List<solicitudMaterialesOfDto>>(solicitudesMateriales);

            var resultado = new
            {
                ProcesosOf = procesosOfDto,
                TarjetasOf = tarjetasOfDto,
                Tableros = tablerosDto,
                EntregaProductoTerminado = entregasProductoTerminadoDto
            };

            if (!procesosOfDto.Any() && !tarjetasOfDto.Any() && !tablerosDto.Any())
            {
                return NotFound($"No se encontraron resultados para el término de búsqueda: {termino}");
            }

            return Ok(resultado);
        }

    }
}
