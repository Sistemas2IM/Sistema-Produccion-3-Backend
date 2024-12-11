using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.CamposPersonalizados
{
    public class CamposPersonalizadosDto
    {
        public int idCampo { get; set; }

        public string? nombreCampo { get; set; }

        public string? tipo { get; set; }

        public string? valoresPosibles { get; set; }
    }
}
