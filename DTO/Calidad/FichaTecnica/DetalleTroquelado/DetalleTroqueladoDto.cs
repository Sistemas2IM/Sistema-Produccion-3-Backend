using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleTroquelado.TipoAcabado;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica.DetalleTroquelado
{
    public class DetalleTroqueladoDto
    {
        public int idDetalleTroquelado { get; set; }

        public int? idFichaTecnica { get; set; }

        public string anchoSinArmar { get; set; }

        public string largoSinArmar { get; set; }

        public string anchoArmada { get; set; }

        public string largoArmada { get; set; }

        public string altoArmada { get; set; }

        public decimal? velocidadMaquina { get; set; }

        public string maquina { get; set; }

        public string operario { get; set; }

        public string presionMaquina { get; set; }

        public string codigoTroquel { get; set; }

        public string tipoMatriz { get; set; }

        public int? repeticionesPorTroquel { get; set; }

        public string? observaciones { get; set; }

        public List<TipoAcabadoDto>? tipoAcabadoDto { get; set; }
    }
}
