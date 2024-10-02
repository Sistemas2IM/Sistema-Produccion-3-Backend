using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class TipoCierreDto
    {
        public int idTipoCierre { get; set; }

        public string nombreTipoCierre { get; set; }

    }
}
