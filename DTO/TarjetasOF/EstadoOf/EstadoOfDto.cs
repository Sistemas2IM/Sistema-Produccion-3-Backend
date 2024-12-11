using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;


namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF.EstadoOf
{
    public class EstadoOfDto
    {
        public int idEstadoOf { get; set; }

        public string? nombreEstado { get; set; }

        public int? secuencia { get; set; }


        [JsonIgnore]
        public List<TarjetaOfDto>? tarjetaOfDtos { get; set; }
    }
}
