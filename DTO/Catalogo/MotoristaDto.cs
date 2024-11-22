using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo
{
    public class MotoristaDto
    {
        public int idMotorista { get; set; }

        public string? nombres { get; set; }

        public string? apellidos { get; set; }

    }
}
