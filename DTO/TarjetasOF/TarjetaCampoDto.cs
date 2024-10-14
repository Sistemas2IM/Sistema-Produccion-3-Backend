using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Sistema_Produccion_3_Backend.DTO.TarjetasOF
{
    public class TarjetaCampoDto
    {
        public int idCamposTarjeta { get; set; }

        public int? idProceso { get; set; }

        public int? idCampo { get; set; }

        public string? valorTexto { get; set; }

        public decimal? valorNumero { get; set; }

        public DateTime? valorFecha { get; set; }

        public string? valorLista { get; set; }

        public bool? valorCasilla { get; set; }

        public CamposPersonalizadosDto? camposPersonalizadosDto { get; set; }
    }
}
