using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Sistema_Produccion_3_Backend.DTO.LoginAuth;
using Sistema_Produccion_3_Backend.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace Sistema_Produccion_3_Backend.Services
{
    public class AuthService : IAuthService
    {
        private readonly base_nuevaContext _context;
        private readonly IConfiguration _configuration;

        public AuthService(base_nuevaContext context, IConfiguration configuration)
        {
            _context = context;
            _configuration = configuration;
        }

        public async Task<AuthResponse> AuthResponse(LoginDto login)
        {
            var userFound = _context.usuario.FirstOrDefault(x => x.user == login.User);

            if (userFound == null)
            {
                return await Task.FromResult<AuthResponse>(null);
            }
            if (userFound.user == login.User && !BCrypt.Net.BCrypt.Verify(login.Password, userFound.password))
            {
                return new AuthResponse { result = false, message = "Contraseña incorrecta" };
            }

            string token = GenerateToken(userFound.user);
            string refreshToken = GenerateRefreshToken();

            return await SaveTokens(userFound.user, token, refreshToken);
        }

        public async Task<AuthResponse> RefreshTokenResponse(TokenDto refreshToken, string user)
        {
            var refreshTokenEncontrado = _context.refreshToken.FirstOrDefault(x =>
                x.token == refreshToken.Token &&
                x.refreshToken1 == refreshToken.RefreshToken &&
                x.user == refreshToken.User);

            if (refreshTokenEncontrado == null)
                return new AuthResponse { result = false, message = "No existe refresh Token" };

            var refreshTokenCreado = GenerateRefreshToken();
            var tokenCreado = GenerateToken(user);

            return await SaveTokens(user, tokenCreado, refreshTokenCreado);
        }

        public async Task<RegisterResponse> RegisterUser(RegisterDto register)
        {
            var existingUser = _context.usuario.FirstOrDefault(x => x.user == register.User);

            if (existingUser != null)
            {
                return new RegisterResponse { result = false, message = "Este usuario ya existe" };
            }

            var hashedPassword = BCrypt.Net.BCrypt.HashPassword(register.Password);

            var newUser = new usuario
            {
                idRol = register.idRol,
                idCargo = register.idCargo,
                user = register.User,
                nombres = register.Nombres,
                apellidos = register.Apellidos,
                email = register.Email,
                password = hashedPassword,
                fechaDeCreacion = DateTime.Now,
                status = "1",
            };

            await _context.usuario.AddAsync(newUser);
            await _context.SaveChangesAsync();

            string token = GenerateToken(newUser.user.ToString());
            string refreshToken = GenerateRefreshToken();

            return new RegisterResponse { result = true, message = "Usuario Registrado" };
        }

        // -----------------------------------------------------------------------------------------------

        public async Task<RegisterResponse> UpdateUser(UpdateUserDto update)
        {
            // Buscar el usuario existente
            var existingUser = await _context.usuario.FindAsync(update.User);

            if (existingUser == null)
            {
                return new RegisterResponse { result = false, message = "Usuario no encontrado" };
            }

            // Actualizar los campos necesarios
            existingUser.idRol = update.idRol ?? existingUser.idRol;
            existingUser.idCargo = update.idCargo ?? existingUser.idCargo;
            existingUser.nombres = update.Nombres ?? existingUser.nombres;
            existingUser.apellidos = update.Apellidos ?? existingUser.apellidos;
            existingUser.email = update.Email ?? existingUser.email;

            // Si se incluye una nueva contraseña, se actualiza
            if (!string.IsNullOrEmpty(update.Password))
            {
                existingUser.password = BCrypt.Net.BCrypt.HashPassword(update.Password);
            }

            // Actualizar la fecha de modificación (opcional)
            existingUser.ultimaActualizacion = DateTime.Now;

            // Guardar cambios
            _context.usuario.Update(existingUser);
            await _context.SaveChangesAsync();

            // Retornar respuesta
            return new RegisterResponse { result = true, message = "Usuario actualizado con éxito" };
        }


        // ------------------------------------------------------------------------------------------------------

        private string GenerateToken(string idUser)
        {
            var key = _configuration.GetValue<string>("JWTSetting:securitykey");
            var keyBytes = Encoding.ASCII.GetBytes(key);

            var claims = new ClaimsIdentity();
            claims.AddClaim(new Claim(ClaimTypes.NameIdentifier, idUser));

            var credentialsToken = new SigningCredentials
                (
                new SymmetricSecurityKey(keyBytes),
                SecurityAlgorithms.HmacSha256Signature
                );

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = claims,
                Expires = DateTime.UtcNow.AddMinutes(1),
                SigningCredentials = credentialsToken
            };

            var tokenHandler = new JwtSecurityTokenHandler();
            var tokenConfiguration = tokenHandler.CreateToken(tokenDescriptor);

            string token = tokenHandler.WriteToken(tokenConfiguration);

            return token;
        }

        private string GenerateRefreshToken()
        {
            var byteArray = new byte[64];
            var refreshToken = "";

            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(byteArray);
                refreshToken = Convert.ToBase64String(byteArray);
            }
            return refreshToken;
        }

        private async Task<AuthResponse> SaveTokens(string user, string token, string refreshToken)
        {
            var existingToken = await _context.refreshToken.FirstOrDefaultAsync(rt => rt.user == user && rt.token == token);

            if (existingToken != null)
            {
                return new AuthResponse
                {
                    token = existingToken.token,
                    refreshToken = existingToken.refreshToken1,
                    result = true,
                    message = "Ya existe el Token"
                };
            }

            var rfToken = new refreshToken
            {
                user = user,
                token = token,
                refreshToken1 = refreshToken,
            };

            await _context.refreshToken.AddAsync(rfToken);
            await _context.SaveChangesAsync();

            return new AuthResponse { token = token, refreshToken = refreshToken, result = true, message = "Oki" };
        }
    }
}
