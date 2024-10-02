using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class MaterialDto
    {
        public int idMaterial { get; set; }

        public string nombreMaterial { get; set; }
    }
}
