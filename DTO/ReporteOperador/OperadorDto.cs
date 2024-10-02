using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.ReporteOperador
{
    public class OperadorDto
    {
        public string idOperador { get; set; }

        public string user { get; set; }

        public string nombreOperador { get; set; }

        public virtual usuario userNavigation { get; set; }
    }
}
