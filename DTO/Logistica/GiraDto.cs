using Sistema_Produccion_3_Backend.DTO.Catalogo;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.Logistica
{
    public class GiraDto
    {
        public int idGira { get; set; }

        public int? idMotorista { get; set; }

        public int? idVehiculo { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public DateTime? fechaGira { get; set; }

        public DateTime? horaDeSaida { get; set; }

        public bool? abierto { get; set; }

        public List<DetalleGiraDto>? detalleGiraDto { get; set; }

        public VehiculoDto vehiculoDto { get; set; }

        public MotoristaDto motoristaDto { get; set; }
    }
}
