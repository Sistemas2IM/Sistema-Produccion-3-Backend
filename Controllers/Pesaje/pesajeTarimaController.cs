using System.Data;
using System.Security.Claims;
using AutoMapper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Pesaje;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Pesaje
{
    [Route("api/pesaje")]
    [ApiController]
    [Authorize]
    public class pesajeTarimaController : ControllerBase
    {
        private const string EstadoPesada = "Pesada";

        private static readonly string[] TiposUnidad = { "Tarima", "Bobina", "Paquete" };
        private static readonly string[] EstadosLotePesable = { "Pendiente", "Transferencia Parcial" };

        private const string EstadoEntregada = "Entregada";
        private const string EstadoLoteTransferido = "Transferido";

        // Valores con los que se crean hoy las transferencias de material de un lote hacia impresión
        private const string TipoTransferenciaMaterial = "Material";
        private const string EstadoTransferenciaPendiente = "Pendiente";
        private const string UnidadPliegos = "Pliegos";
        private const int AreaOrigenLote = 5;

        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public pesajeTarimaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // POST api/pesaje/lotes/5/tarimas
        [HttpPost("lotes/{idLote}/tarimas")]
        public async Task<ActionResult<PesajeTarimaRegistradaDto>> PostTarima(int idLote, AddPesajeTarimaDto dto)
        {
            var usuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                return Unauthorized(new { mensaje = "No se pudo identificar al usuario del token" });
            }

            var tipoUnidad = TiposUnidad.FirstOrDefault(t => string.Equals(t, dto.tipoUnidad?.Trim(), StringComparison.OrdinalIgnoreCase));
            if (tipoUnidad == null)
            {
                return BadRequest(new { mensaje = "tipoUnidad debe ser Tarima, Bobina o Paquete" });
            }
            if (dto.pesoBruto <= 0)
            {
                return BadRequest(new { mensaje = "pesoBruto debe ser mayor a 0" });
            }
            if (dto.tara < 0)
            {
                return BadRequest(new { mensaje = "tara no puede ser negativa" });
            }
            if (Math.Round(dto.pesoBruto - dto.tara, 2) != Math.Round(dto.pesoNeto, 2))
            {
                return BadRequest(new { mensaje = "pesoNeto debe ser igual a pesoBruto - tara" });
            }
            if (dto.pesoNeto <= 0)
            {
                return BadRequest(new { mensaje = "pesoNeto debe ser mayor a 0" });
            }

            if (dto.idTipoTara.HasValue)
            {
                var taraActiva = await _context.catalogoTara
                    .AnyAsync(t => t.idTipoTara == dto.idTipoTara.Value && t.activo);
                if (!taraActiva)
                {
                    return BadRequest(new { mensaje = $"El tipo de tara {dto.idTipoTara} no existe o está inactivo" });
                }
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            // Bloquea la fila del lote hasta el commit: dos tablets pesando el mismo lote se serializan aquí,
            // así el correlativo calculado abajo no se repite.
            var lote = await _context.lotePliego
                .FromSqlInterpolated($"SELECT * FROM lotePliego WITH (UPDLOCK, HOLDLOCK) WHERE idLote = {idLote}")
                .AsNoTracking()
                .FirstOrDefaultAsync();

            if (lote == null)
            {
                return NotFound(new { mensaje = $"No se encontró el lote con el id {idLote}" });
            }
            if (!EstadosLotePesable.Any(e => string.Equals(e, lote.estado?.Trim(), StringComparison.OrdinalIgnoreCase)))
            {
                return Conflict(new { mensaje = $"El lote está en estado '{lote.estado}', solo se puede pesar en Pendiente o Transferencia Parcial" });
            }

            var tieneFinalizadas = await _context.pesajeTarima
                .AnyAsync(p => p.idLote == idLote && !p.cancelado && (p.idTransferencia != null || p.estado != EstadoPesada));
            if (tieneFinalizadas)
            {
                return Conflict(new { mensaje = "El lote ya tiene tarimas finalizadas" });
            }

            // Incluye canceladas para que un correlativo/viñeta nunca se reutilice
            var correlativo = (await _context.pesajeTarima
                .Where(p => p.idLote == idLote)
                .MaxAsync(p => (int?)p.correlativo) ?? 0) + 1;

            var tarima = new pesajeTarima
            {
                idLote = idLote,
                correlativo = correlativo,
                tipoUnidad = tipoUnidad,
                pesoBruto = dto.pesoBruto,
                tara = dto.tara,
                pesoNeto = dto.pesoNeto,
                idTipoTara = dto.idTipoTara,
                manual = dto.manual,
                gramaje = dto.gramaje,
                conteoEstimado = dto.conteoEstimado,
                codigoVineta = $"PZ-{idLote}-{correlativo:D2}",
                reimpresiones = 0,
                pesadoPor = usuario,
                fechaPesaje = DateTime.Now,
                idBascula = string.IsNullOrWhiteSpace(dto.idBascula) ? null : dto.idBascula.Trim(),
                estado = EstadoPesada,
                cancelado = false
            };

            _context.pesajeTarima.Add(tarima);

            try
            {
                await _context.SaveChangesAsync();
                await transaction.CommitAsync();
            }
            catch (DbUpdateException ex) when (ex.InnerException is SqlException { Number: 2601 or 2627 })
            {
                // Respaldo de los índices únicos (idLote, correlativo) y codigoVineta
                return Conflict(new { mensaje = "Otra báscula registró una tarima al mismo tiempo, intente de nuevo" });
            }

            return Ok(new PesajeTarimaRegistradaDto
            {
                idPesaje = tarima.idPesaje,
                correlativo = tarima.correlativo,
                codigoVineta = tarima.codigoVineta,
                mensaje = "Tarima registrada"
            });
        }

        // POST api/pesaje/lotes/5/finalizar
        [HttpPost("lotes/{idLote}/finalizar")]
        public async Task<ActionResult<PesajeFinalizadoDto>> FinalizarLote(int idLote, [FromBody(EmptyBodyBehavior = EmptyBodyBehavior.Allow)] FinalizarPesajeLoteDto? dto)
        {
            var usuario = User.FindFirstValue(ClaimTypes.NameIdentifier);
            if (string.IsNullOrWhiteSpace(usuario))
            {
                return Unauthorized(new { mensaje = "No se pudo identificar al usuario del token" });
            }

            if (dto?.idMotivoFinalizacion is int idMotivo
                && !await _context.motivoConciliacion.AnyAsync(m => m.idMotivo == idMotivo))
            {
                return BadRequest(new { mensaje = $"El motivo de finalización {idMotivo} no existe" });
            }
            if (dto?.comentarioFinalizacion?.Trim().Length > 500)
            {
                return BadRequest(new { mensaje = "comentarioFinalizacion no puede exceder 500 caracteres" });
            }

            await using var transaction = await _context.Database.BeginTransactionAsync(IsolationLevel.ReadCommitted);

            // Mismo bloqueo que al registrar tarimas: no se puede pesar mientras se finaliza el lote
            var lote = await _context.lotePliego
                .FromSqlInterpolated($"SELECT * FROM lotePliego WITH (UPDLOCK, HOLDLOCK) WHERE idLote = {idLote}")
                .Include(l => l.idSolicitudNavigation)
                .FirstOrDefaultAsync();

            if (lote == null)
            {
                return NotFound(new { mensaje = $"No se encontró el lote con el id {idLote}" });
            }

            var tarimas = await _context.pesajeTarima
                .Where(p => p.idLote == idLote && !p.cancelado)
                .ToListAsync();

            if (tarimas.Any(p => p.idTransferencia != null || p.estado != EstadoPesada))
            {
                return Conflict(new { mensaje = "El lote ya tiene tarimas finalizadas" });
            }
            if (tarimas.Count == 0)
            {
                return Conflict(new { mensaje = "El lote no tiene tarimas pesadas" });
            }

            var of = await PesajeLoteHelper.ObtenerOfAsync(_context, lote);
            if (of == null)
            {
                return Conflict(new { mensaje = "No se pudo determinar la OF del lote" });
            }

            var procesoImpresion = await _context.procesoOf
                .Where(p => p.oF == of && p.tipoMaquinaSAP == PesajeLoteHelper.TipoMaquinaImpresion)
                .OrderBy(p => p.secuencia ?? int.MaxValue)
                .ThenBy(p => p.idProceso)
                .Select(p => new { p.idProceso, p.idTableroNavigation.idArea })
                .FirstOrDefaultAsync();

            if (procesoImpresion == null)
            {
                return Conflict(new { mensaje = $"La OF {of} no tiene proceso de impresión" });
            }

            // Misma creación que POST api/transferenciaProceso/post (AddTransferenciaProcesoDto + AutoMapper)
            var transferencia = _mapper.Map<transferenciaProceso>(new AddTransferenciaProcesoDto
            {
                idLote = lote.idLote,
                tipo = TipoTransferenciaMaterial,
                estado = EstadoTransferenciaPendiente,
                unidadDeMedida = UnidadPliegos,
                fechaEnvio = DateTime.Now,
                enviadoPor = usuario,
                cantidadEnviada = lote.cantidadPendiente ?? lote.cantidadPliegos,
                idDestino = procesoImpresion.idProceso,
                areaOrigen = AreaOrigenLote,
                areaDestino = procesoImpresion.idArea,
                oFDestino = of,
                idSolicitudOrigen = lote.idSolicitud,
                idMaterialSAP = lote.idMaterialSAP,
                materialDescripcion = lote.idSolicitudNavigation?.materialDescripcion
            });
            transferencia.idMotivoFinalizacion = dto?.idMotivoFinalizacion;
            transferencia.comentarioFinalizacion = string.IsNullOrWhiteSpace(dto?.comentarioFinalizacion)
                ? null
                : dto.comentarioFinalizacion.Trim();

            _context.transferenciaProceso.Add(transferencia);

            lote.peso = tarimas.Sum(p => p.pesoNeto);
            lote.estado = EstadoLoteTransferido;
            lote.cantidadPendiente = 0;

            foreach (var tarima in tarimas)
            {
                tarima.estado = EstadoEntregada;
                tarima.idTransferenciaNavigation = transferencia;
            }

            await _context.SaveChangesAsync();
            await transaction.CommitAsync();

            return Ok(new PesajeFinalizadoDto
            {
                idTransferencia = transferencia.idTransferencia,
                mensaje = "Pesaje finalizado"
            });
        }

        // DELETE api/pesaje/tarimas/5
        [HttpDelete("tarimas/{idPesaje}")]
        public async Task<IActionResult> CancelarTarima(int idPesaje)
        {
            // Update condicional: si la tarima se transfiere entre la validación y el guardado, no se cancela
            var filas = await _context.pesajeTarima
                .Where(p => p.idPesaje == idPesaje && !p.cancelado && p.estado == EstadoPesada && p.idTransferencia == null)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.cancelado, true));

            if (filas > 0)
            {
                return NoContent();
            }

            var tarima = await _context.pesajeTarima.AsNoTracking().FirstOrDefaultAsync(p => p.idPesaje == idPesaje);
            if (tarima == null)
            {
                return NotFound(new { mensaje = $"No se encontró la tarima con el id {idPesaje}" });
            }
            if (tarima.cancelado)
            {
                return Conflict(new { mensaje = "La tarima ya está cancelada" });
            }

            return Conflict(new { mensaje = "La tarima ya fue transferida o el lote se finalizó, no se puede cancelar" });
        }

        // POST api/pesaje/tarimas/5/reimprimir
        [HttpPost("tarimas/{idPesaje}/reimprimir")]
        public async Task<ActionResult<VinetaTarimaDto>> ReimprimirTarima(int idPesaje)
        {
            var filas = await _context.pesajeTarima
                .Where(p => p.idPesaje == idPesaje && !p.cancelado)
                .ExecuteUpdateAsync(s => s.SetProperty(p => p.reimpresiones, p => p.reimpresiones + 1));

            if (filas == 0)
            {
                var existe = await _context.pesajeTarima.AnyAsync(p => p.idPesaje == idPesaje);
                return existe
                    ? Conflict(new { mensaje = "La tarima está cancelada, no se puede reimprimir" })
                    : NotFound(new { mensaje = $"No se encontró la tarima con el id {idPesaje}" });
            }

            var tarima = await _context.pesajeTarima
                .AsNoTracking()
                .Include(p => p.idLoteNavigation)
                    .ThenInclude(l => l.idSolicitudNavigation)
                .FirstAsync(p => p.idPesaje == idPesaje);
            var lote = tarima.idLoteNavigation;

            var of = await PesajeLoteHelper.ObtenerOfAsync(_context, lote);
            var cliente = of == null
                ? null
                : await _context.tarjetaOf
                    .Where(t => t.oF == of)
                    .Select(t => t.clienteOf)
                    .FirstOrDefaultAsync();

            var material = lote.idSolicitudNavigation?.materialDescripcion;
            if (string.IsNullOrWhiteSpace(material) && !string.IsNullOrWhiteSpace(lote.idMaterialSAP))
            {
                material = await _context.material
                    .Where(m => m.idMaterial == lote.idMaterialSAP)
                    .Select(m => m.nombreMaterial)
                    .FirstOrDefaultAsync();
            }

            return Ok(new VinetaTarimaDto
            {
                codigoVineta = tarima.codigoVineta,
                codLote = lote.codLote,
                of = of?.ToString(),
                cliente = cliente,
                material = material ?? lote.idMaterialSAP,
                pesoNeto = tarima.pesoNeto,
                conteoEstimado = tarima.conteoEstimado,
                manual = tarima.manual,
                fechaPesaje = tarima.fechaPesaje,
                pesadoPor = tarima.pesadoPor,
                reimpresiones = tarima.reimpresiones
            });
        }
    }
}
