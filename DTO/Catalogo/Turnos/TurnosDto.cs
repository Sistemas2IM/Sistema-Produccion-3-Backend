using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Turnos
{
    public class TurnosDto
    {
        public int id { get; set; }

        public string? turno { get; set; }

        public TimeOnly? horaInicio { get; set; }

        public TimeOnly? horaFinal { get; set; }
    }
}
