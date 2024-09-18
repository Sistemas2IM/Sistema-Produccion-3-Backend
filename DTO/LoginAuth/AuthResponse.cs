namespace Sistema_Produccion_3_Backend.DTO.LoginAuth
{
    public class AuthResponse
    {
        public string token { get; set; }
        public string refreshToken { get; set; }
        public bool result { get; set; }
        public string message { get; set; }
    }
}
