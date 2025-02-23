using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
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

        // GET: api/buscarGlobal/{of}
        [HttpGet("buscarGlobal/{of}")]
        public async Task<ActionResult<object>> BuscarGlobal(int of)
        {
            // Buscar todos los procesos relacionados con el número de OF
            var procesosOf = await _context.procesoOf
                .Include(u => u.detalleOperacionProceso)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .Where(u => u.oF == of)  // Filtra por el número de OF
                .ToListAsync();  // Obtiene todos los registros

            // Buscar la tarjetaOf relacionada con el número de OF
            var tarjetaOf = await _context.tarjetaOf
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .Where(u => u.oF == of)  // Filtra por el número de OF
                .ToListAsync();

            // Mapear los resultados a DTOs
            var procesosOfDto = _mapper.Map<List<ProcesoOfDto>>(procesosOf);
            var tarjetaOfDto = _mapper.Map<TarjetaOfDto>(tarjetaOf);

            // Crear un objeto anónimo para devolver ambos resultados
            var resultado = new
            {
                ProcesosOf = procesosOfDto,  // Lista de procesos
                TarjetaOf = tarjetaOfDto     // Tarjeta única
            };

            // Verificar si se encontraron resultados en alguna de las tablas
            if (!procesosOfDto.Any() && tarjetaOfDto == null)
            {
                return NotFound("No se encontraron resultados para el número de OF: " + of);
            }

            return Ok(resultado);
        }

    }
}
