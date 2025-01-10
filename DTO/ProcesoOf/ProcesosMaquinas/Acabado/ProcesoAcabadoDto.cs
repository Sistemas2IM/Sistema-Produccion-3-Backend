using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Acabado
{
    public class ProcesoAcabadoDto
    {
        public int idProcesoAcabado { get; set; }

        public int? idProcesoOf { get; set; }

        public string? cantidadMrequerido { get; set; }

        public string? cantidadDemasia { get; set; }

        public TimeOnly? tiempoArreglo { get; set; }

        public TimeOnly? tiempoCorrida { get; set; }
    }
}
