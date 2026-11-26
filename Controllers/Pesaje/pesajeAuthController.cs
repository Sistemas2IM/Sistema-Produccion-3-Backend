using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;
using Sistema_Produccion_3_Backend.DTO.Pesaje;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services;

namespace Sistema_Produccion_3_Backend.Controllers.Pesaje
{
    [Route("api/pesaje/auth")]
    [ApiController]
    public class pesajeAuthController : ControllerBase
    {
        private const int MaxIntentosFallidos = 5;
        private static readonly TimeSpan DuracionBloqueo = TimeSpan.FromMinutes(15);

        private readonly base_nuevaContext _context;
        private readonly IMemoryCache _memoryCache;

        public pesajeAuthController(base_nuevaContext context, IMemoryCache memoryCache)
        {
            _context = context;
            _memoryCache = memoryCache;
        }

        // POST api/pesaje/auth/pin
        // Sin [Authorize]: la pantalla se bloquea por reposo y para entonces el JWT puede haber expirado
        [HttpPost("pin")]
        public async Task<ActionResult<DesbloqueoPinResponseDto>> DesbloquearConPin(DesbloqueoPinDto dto)
        {
            var user = dto.user?.Trim() ?? string.Empty;
            var intentos = ObtenerIntentos(user);

            var segundosBloqueo = SegundosRestantesBloqueo(intentos);
            if (segundosBloqueo > 0)
            {
                return RespuestaBloqueado(segundosBloqueo);
            }

            var usuario = string.IsNullOrEmpty(user)
                ? null
                : await _context.usuario
                    .AsNoTracking()
                    .Where(u => u.user == user)
                    .Select(u => new { u.status, u.pinHash, u.nombres, u.apellidos })
                    .FirstOrDefaultAsync();

            // Se verifica siempre (con hash ficticio si no hay usuario o PIN) para no revelar cuál validación falló
            var pinCorrecto = PinHasher.VerificarPin(dto.pin, usuario?.pinHash);

            if (usuario == null || usuario.status != true || !pinCorrecto)
            {
                lock (intentos)
                {
                    intentos.Fallidos++;
                    if (intentos.Fallidos >= MaxIntentosFallidos)
                    {
                        intentos.BloqueadoHasta = DateTime.UtcNow.Add(DuracionBloqueo);
                        intentos.Fallidos = 0;
                    }
                }

                segundosBloqueo = SegundosRestantesBloqueo(intentos);
                if (segundosBloqueo > 0)
                {
                    return RespuestaBloqueado(segundosBloqueo);
                }

                return Unauthorized(new { desbloqueado = false, mensaje = "Usuario o PIN incorrecto" });
            }

            _memoryCache.Remove(ClaveIntentos(user));

            return Ok(new DesbloqueoPinResponseDto
            {
                desbloqueado = true,
                nombre = $"{usuario.nombres} {usuario.apellidos}".Trim()
            });
        }

        // PUT api/pesaje/auth/pin
        // Se pide la contraseña del usuario para autorizar el cambio; el PIN se guarda solo como hash
        [HttpPut("pin")]
        public async Task<IActionResult> CambiarPin(CambioPinDto dto)
        {
            if (!PinHasher.EsPinValido(dto.pinNuevo))
            {
                return BadRequest(new { mensaje = "El PIN debe tener de 4 a 6 dígitos" });
            }

            var user = dto.user?.Trim() ?? string.Empty;
            var intentos = ObtenerIntentos(user);

            var segundosBloqueo = SegundosRestantesBloqueo(intentos);
            if (segundosBloqueo > 0)
            {
                return RespuestaBloqueado(segundosBloqueo);
            }

            var usuario = string.IsNullOrEmpty(user)
                ? null
                : await _context.usuario.FirstOrDefaultAsync(u => u.user == user);

            var passwordCorrecta = PinHasher.VerificarHash(dto.password, usuario?.password);

            if (usuario == null || usuario.status != true || !passwordCorrecta)
            {
                lock (intentos)
                {
                    intentos.Fallidos++;
                    if (intentos.Fallidos >= MaxIntentosFallidos)
                    {
                        intentos.BloqueadoHasta = DateTime.UtcNow.Add(DuracionBloqueo);
                        intentos.Fallidos = 0;
                    }
                }

                segundosBloqueo = SegundosRestantesBloqueo(intentos);
                if (segundosBloqueo > 0)
                {
                    return RespuestaBloqueado(segundosBloqueo);
                }

                return Unauthorized(new { mensaje = "Usuario o contraseña incorrectos" });
            }

            usuario.pinHash = PinHasher.HashPin(dto.pinNuevo!);
            usuario.pinActualizado = DateTime.Now;
            await _context.SaveChangesAsync();

            _memoryCache.Remove(ClaveIntentos(user));

            return NoContent();
        }

        private IntentosPin ObtenerIntentos(string user)
        {
            return _memoryCache.GetOrCreate(ClaveIntentos(user), entry =>
            {
                entry.SlidingExpiration = DuracionBloqueo;
                return new IntentosPin();
            })!;
        }

        private static string ClaveIntentos(string user) => $"pesaje:pin:intentos:{user.ToLowerInvariant()}";

        private static int SegundosRestantesBloqueo(IntentosPin intentos)
        {
            lock (intentos)
            {
                if (intentos.BloqueadoHasta is not DateTime hasta)
                {
                    return 0;
                }

                var restante = hasta - DateTime.UtcNow;
                if (restante <= TimeSpan.Zero)
                {
                    intentos.BloqueadoHasta = null;
                    return 0;
                }

                return (int)Math.Ceiling(restante.TotalSeconds);
            }
        }

        private ObjectResult RespuestaBloqueado(int segundos)
        {
            Response.Headers.RetryAfter = segundos.ToString();
            var minutos = (int)Math.Ceiling(segundos / 60.0);

            return StatusCode(StatusCodes.Status429TooManyRequests, new
            {
                desbloqueado = false,
                mensaje = $"Demasiados intentos fallidos. Intente de nuevo en {minutos} minuto(s)"
            });
        }

        private class IntentosPin
        {
            public int Fallidos { get; set; }

            public DateTime? BloqueadoHasta { get; set; }
        }
    }
}
