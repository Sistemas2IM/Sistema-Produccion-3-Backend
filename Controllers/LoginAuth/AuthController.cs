using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.LoginAuth;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services;
using System.IdentityModel.Tokens.Jwt;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IAuthService _authService;
        private readonly base_nuevaContext _context;

        public AuthController(IAuthService authService, base_nuevaContext context)
        {
            _authService = authService;
            _context = context;
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto login)
        {
            var authResult = await _authService.AuthResponse(login);

            if (authResult == null)
            {
                return NotFound(new { message = "Usuario no encontrado" });
            }
            if (!authResult.result)
            {
                return Unauthorized(authResult);
            }

            return Ok(authResult);
        }

        [HttpPost("refreshtoken")]
        public async Task<IActionResult> RefreshToken([FromBody] TokenDto token)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenExpiradoSupuestamente = tokenHandler.ReadJwtToken(token.Token);

            if (tokenExpiradoSupuestamente.ValidTo > DateTime.UtcNow)
                return BadRequest(new AuthResponse { result = false, message = "Token no ha expirado" });

            string idUsuario = tokenExpiradoSupuestamente.Claims.First(x =>
                x.Type == JwtRegisteredClaimNames.Name).Value.ToString();

            var autorizacionResponse = await _authService.RefreshTokenResponse(token, idUsuario);

            if (autorizacionResponse.result)
                return Ok(autorizacionResponse);
            else
                return BadRequest(autorizacionResponse);
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterDto register)
        {
            var registerResult = await _authService.RegisterUser(register);

            if (!registerResult.result)
            {
                return BadRequest(registerResult);
            }

            return Ok(registerResult);
        }

        [HttpDelete("revoke")]
        public async Task<IActionResult> Revoke([FromBody] string id)
        {
            var usuario = await _context.refreshToken.Where(f => f.user == id).ToListAsync();
            if (usuario == null)
            {
                return NotFound("Usuario no encontrado");
            }

            _context.refreshToken.RemoveRange(usuario);
            await _context.SaveChangesAsync();

            return Ok(new { message = "Log Out" });
        }
    }
}
