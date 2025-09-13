using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.LotePliego
{
    public class lotePliegoDto
    {
        public int idLote { get; set; }

        public int? idSolicitud { get; set; }

        public int? largo { get; set; }

        public int? ancho { get; set; }

        public int? cantidadPliegos { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public string? creadoPor { get; set; }

        public string? idMaterialSAP { get; set; }
    }
}
