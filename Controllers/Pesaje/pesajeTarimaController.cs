using System.Data;
using System.Security.Claims;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Pesaje;
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

        private readonly base_nuevaContext _context;

        public pesajeTarimaController(base_nuevaContext context)
        {
            _context = context;
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

            var vineta = await _context.pesajeTarima
                .AsNoTracking()
                .Where(p => p.idPesaje == idPesaje)
                .Select(p => new
                {
                    p.codigoVineta,
                    p.idLoteNavigation.codLote,
                    p.idLoteNavigation.idMaterialSAP,
                    of = p.idLoteNavigation.procesoOrigenNavigation.oF,
                    cliente = p.idLoteNavigation.procesoOrigenNavigation.oFNavigation.clienteOf,
                    materialSolicitud = p.idLoteNavigation.idSolicitudNavigation.materialDescripcion,
                    p.pesoNeto,
                    p.conteoEstimado,
                    p.manual,
                    p.fechaPesaje,
                    p.pesadoPor,
                    p.reimpresiones
                })
                .FirstAsync();

            var material = vineta.materialSolicitud;
            if (string.IsNullOrWhiteSpace(material) && !string.IsNullOrWhiteSpace(vineta.idMaterialSAP))
            {
                material = await _context.material
                    .Where(m => m.idMaterial == vineta.idMaterialSAP)
                    .Select(m => m.nombreMaterial)
                    .FirstOrDefaultAsync();
            }

            return Ok(new VinetaTarimaDto
            {
                codigoVineta = vineta.codigoVineta,
                codLote = vineta.codLote,
                of = vineta.of?.ToString(),
                cliente = vineta.cliente,
                material = material ?? vineta.idMaterialSAP,
                pesoNeto = vineta.pesoNeto,
                conteoEstimado = vineta.conteoEstimado,
                manual = vineta.manual,
                fechaPesaje = vineta.fechaPesaje,
                pesadoPor = vineta.pesadoPor,
                reimpresiones = vineta.reimpresiones
            });
        }
    }
}
