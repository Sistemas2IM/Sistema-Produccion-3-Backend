using System.Globalization;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Pesaje;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Pesaje
{
    [Route("api/pesaje")]
    [ApiController]
    [Authorize]
    public class pesajeConsultasController : ControllerBase
    {
        private const int MaxUsuariosRecientes = 6;

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public pesajeConsultasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET api/pesaje/usuarios-recientes?idBascula=BP-01
        // Sin token: se usa en la pantalla de bloqueo para elegir al usuario antes de pedir el PIN
        [AllowAnonymous]
        [HttpGet("usuarios-recientes")]
        public async Task<ActionResult<IEnumerable<UsuarioRecienteDto>>> GetUsuariosRecientes([FromQuery] string? idBascula)
        {
            var pesajes = _context.pesajeTarima.AsNoTracking();
            if (!string.IsNullOrWhiteSpace(idBascula))
            {
                var bascula = idBascula.Trim();
                pesajes = pesajes.Where(p => p.idBascula == bascula);
            }

            var usuarios = await pesajes
                .GroupBy(p => p.pesadoPor)
                .Select(g => new { user = g.Key, ultimoPesaje = g.Max(p => p.fechaPesaje) })
                .Join(_context.usuario.Where(u => u.status == true),
                    p => p.user,
                    u => u.user,
                    (p, u) => new { p.ultimoPesaje, u.user, u.nombres, u.apellidos })
                .OrderByDescending(x => x.ultimoPesaje)
                .Take(MaxUsuariosRecientes)
                .ToListAsync();

            return Ok(usuarios.Select(u => new UsuarioRecienteDto
            {
                user = u.user,
                nombre = $"{u.nombres} {u.apellidos}".Trim()
            }));
        }

        // GET api/pesaje/taras
        [HttpGet("taras")]
        public async Task<ActionResult<IEnumerable<CatalogoTaraDto>>> GetTaras()
        {
            var taras = await _context.catalogoTara
                .AsNoTracking()
                .Where(t => t.activo)
                .OrderBy(t => t.nombre)
                .Select(t => new CatalogoTaraDto
                {
                    idTipoTara = t.idTipoTara,
                    nombre = t.nombre,
                    pesoPromedio = t.pesoPromedio
                })
                .ToListAsync();

            return Ok(taras);
        }

        // GET api/pesaje/lotes/5
        [HttpGet("lotes/{idLote}")]
        public async Task<ActionResult<PesajeLoteDto>> GetLote(int idLote)
        {
            var lote = await _context.lotePliego
                .AsNoTracking()
                .Include(l => l.unidadMedidaNavigation)
                .Include(l => l.idSolicitudNavigation)
                    .ThenInclude(s => s.idMaquinaNavigation)
                .Include(l => l.pesajeTarima.Where(p => !p.cancelado).OrderBy(p => p.correlativo))
                .AsSplitQuery()
                .FirstOrDefaultAsync(l => l.idLote == idLote);

            if (lote == null)
            {
                return NotFound(new { mensaje = $"No se encontró el lote con el id {idLote}" });
            }

            var dto = _mapper.Map<PesajeLoteDto>(lote);

            var of = await PesajeLoteHelper.ObtenerOfAsync(_context, lote);
            var cliente = of == null
                ? null
                : await _context.tarjetaOf
                    .Where(t => t.oF == of)
                    .Select(t => t.clienteOf)
                    .FirstOrDefaultAsync();

            var material = string.IsNullOrWhiteSpace(lote.idMaterialSAP)
                ? null
                : await _context.material
                    .AsNoTracking()
                    .Where(m => m.idMaterial == lote.idMaterialSAP)
                    .Select(m => new { m.nombreMaterial, m.gramaje })
                    .FirstOrDefaultAsync();

            dto.of = of?.ToString();
            dto.cliente = cliente;
            dto.materialDescripcion = lote.idSolicitudNavigation?.materialDescripcion ?? material?.nombreMaterial;
            dto.gramaje = decimal.TryParse(material?.gramaje, NumberStyles.Number, CultureInfo.InvariantCulture, out var gramaje)
                ? gramaje
                : null;
            dto.maquinaDestino = lote.idSolicitudNavigation?.idMaquinaNavigation?.nombreMaquina;
            dto.tarimas = _mapper.Map<List<PesajeTarimaDto>>(lote.pesajeTarima);

            return Ok(dto);
        }
    }
}
