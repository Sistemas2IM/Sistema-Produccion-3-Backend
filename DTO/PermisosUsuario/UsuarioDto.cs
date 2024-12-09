using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sistema_Produccion_3_Backend.Services;
using System.Text.Json.Serialization;

namespace Sistema_Produccion_3_Backend.DTO.PermisosUsuario
{
    public class UsuarioDto
    {
        public string? user { get; set; }

        public int? idRol { get; set; }

        public int? idCargo { get; set; }

        public int? idArea { get; set; }

        public string? status { get; set; }

        public string? nombres { get; set; }

        public string? apellidos { get; set; }

        public string? email { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? fechaDeCreacion { get; set; }

        [JsonConverter(typeof(JsonDateConverter))]
        public DateTime? ultimaActualizacion { get; set; }

        public string? actualizadoPor { get; set; }

        public RolDto? rol { get; set; }

        public string? cargo { get; set; }

    }
}
