using System.Text.RegularExpressions;

namespace Sistema_Produccion_3_Backend.Services
{
    public static class PinHasher
    {
        // Hash usado para gastar el mismo tiempo cuando el usuario no existe o no tiene PIN,
        // así la respuesta no revela cuál de las validaciones falló
        private static readonly string HashFicticio = BCrypt.Net.BCrypt.HashPassword("0000");

        public static bool EsPinValido(string? pin)
        {
            return pin != null && Regex.IsMatch(pin, @"^\d{4,6}$");
        }

        public static string HashPin(string pin)
        {
            return BCrypt.Net.BCrypt.HashPassword(pin);
        }

        public static bool VerificarPin(string? pin, string? pinHash)
        {
            return VerificarHash(pin, pinHash);
        }

        // Mismo mecanismo que AuthService (BCrypt.Verify), con hash ficticio si falta el valor o el hash
        public static bool VerificarHash(string? valor, string? hash)
        {
            if (string.IsNullOrEmpty(valor) || string.IsNullOrEmpty(hash))
            {
                BCrypt.Net.BCrypt.Verify(valor ?? string.Empty, HashFicticio);
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(valor, hash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}
