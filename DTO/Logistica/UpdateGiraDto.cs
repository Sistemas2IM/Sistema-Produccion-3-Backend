using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.Logistica
{
    public class UpdateGiraDto
    {
        public int? idMotorista { get; set; }

        public int? idVehiculo { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaGira { get; set; }

        public DateTime? horaDeSaida { get; set; }

        public bool? abierto { get; set; }
    }
}
