using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaBarnizadora
{
    public class InfoMaquinaBarnizadoraDto
    {
        public int idMaquina { get; set; }

        public string? tipoBarniz { get; set; }

        public bool? uv { get; set; }

        public bool? acuoso { get; set; }

        public int? numLamparas { get; set; }

        public decimal? potenciaUV { get; set; }
    }
}
