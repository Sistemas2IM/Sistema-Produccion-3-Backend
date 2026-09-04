using System.Text.Json.Serialization;
using Sistema_Produccion_3_Backend.Services;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Rol;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Diseño
{
    public class UsuarioDiseñoCargaDto
    {
        public string? user { get; set; }

        public int? idRol { get; set; }

        public int? idCargo { get; set; }

        public int? idArea { get; set; }

        public bool? status { get; set; }

        public string? nombres { get; set; }

        public string? apellidos { get; set; }

        public string? email { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaDeCreacion { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ultimaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        public int? codEmpleado { get; set; }

        public string? cargo { get; set; }

        // calculados
        public int cantidadProcesos { get; set; }
        public decimal horasTotales { get; set; }
    }
}
