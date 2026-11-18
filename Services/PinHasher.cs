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
            if (string.IsNullOrEmpty(pin) || string.IsNullOrEmpty(pinHash))
            {
                BCrypt.Net.BCrypt.Verify(pin ?? string.Empty, HashFicticio);
                return false;
            }

            try
            {
                return BCrypt.Net.BCrypt.Verify(pin, pinHash);
            }
            catch (BCrypt.Net.SaltParseException)
            {
                return false;
            }
        }
    }
}
