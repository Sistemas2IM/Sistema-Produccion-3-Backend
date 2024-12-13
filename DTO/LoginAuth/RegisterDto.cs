namespace Sistema_Produccion_3_Backend.DTO.LoginAuth
{
    public class RegisterDto
    {
        public int? idRol {  get; set; }
        public int? idCargo { get; set; }
        public int? idArea { get; set; }
        public string User { get; set; }
        public string Password { get; set; }
        public string Nombres { get; set; }
        public string Apellidos { get; set; }
        public string Email { get; set; }
    }
}
