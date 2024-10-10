using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class EstadoOfDto
    {
        public int idEstadoOf { get; set; }

        public string? nombreEstado { get; set; }

        public int? secuencia { get; set; }
    }
}
