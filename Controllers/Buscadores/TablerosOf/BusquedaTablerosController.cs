using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
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
            // Convertir el término de búsqueda a minúsculas para hacer la búsqueda insensible a mayúsculas
            termino = termino.ToLower();

            // Buscar en la tabla procesoOf
            var procesosOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .Where(u =>
                    u.oF.ToString().Contains(termino) ||  // Buscar en el número de OF
                    u.oFNavigation.clienteOf.ToLower().Contains(termino) ||  // Buscar en el nombre del material
                    u.productoOf.ToLower().Contains(termino) ||
                    u.oFNavigation.codArticulo.ToLower().Contains(termino) ||
                    u.oFNavigation.oV.ToString().Contains(termino) ||
                    u.idMaquinaSAP.ToLower().Contains(termino))
                .ToListAsync();

            // Buscar en la tabla tarjetaOf
            var tarjetasOf = await _context.tarjetaOf
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .Where(u =>
                    u.oF.ToString().Contains(termino) ||  // Buscar en el número de OF
                    u.clienteOf.ToLower().Contains(termino) ||  // Buscar en el nombre del material
                    u.vendedorOf.ToLower().Contains(termino) ||
                    u.productoOf.ToLower().Contains(termino) ||
                    u.codArticulo.ToLower().Contains(termino) ||
                    u.oV.ToString().Contains(termino))
                .ToListAsync();

            // Tableros
            var tableros = await _context.tablerosOf
                .Where(u =>
                    u.nombreTablero.ToLower().Contains(termino) ||
                    u.idSapMaquina.ToLower().Contains(termino))          
                .ToListAsync();

            // Mapear los resultados a DTOs
            var procesosOfDto = _mapper.Map<List<ProcesoOfDto>>(procesosOf);
            var tarjetasOfDto = _mapper.Map<List<TarjetaOfDto>>(tarjetasOf);
            var tablerosDto = _mapper.Map<List<TablerosOfDto>>(tableros);

            // Crear un objeto anónimo para devolver ambos resultados
            var resultado = new
            {
                ProcesosOf = procesosOfDto,  // Lista de procesos
                TarjetasOf = tarjetasOfDto,   // Lista de tarjetas
                Tableros = tablerosDto
            };

            // Verificar si se encontraron resultados en alguna de las tablas
            if (!procesosOfDto.Any() && !tarjetasOfDto.Any() && !tablerosDto.Any())
            {
                return NotFound("No se encontraron resultados para el término de búsqueda: " + termino);
            }

            return Ok(resultado);
        }

    }
}
