using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas
{
    public class MaquinaDto
    {
        public int idMaquina { get; set; }

        public int? idFamilia { get; set; }

        public string? nombreMaquina { get; set; }

        public int? familiaId { get; set; }
    }
}
