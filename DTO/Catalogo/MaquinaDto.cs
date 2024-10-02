using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo
{
    public class MaquinaDto
    {
        public int idMaquina { get; set; }

        public int? idFamilia { get; set; }

        public string nombreMaquina { get; set; }

        public int? familiaId { get; set; }

        public virtual ICollection<entregasProductoTerminado> entregasProductoTerminado { get; set; } = new List<entregasProductoTerminado>();

        public virtual familliaDeMaquina idFamiliaNavigation { get; set; }

        public virtual ICollection<operaciones> operaciones { get; set; } = new List<operaciones>();

        public virtual ICollection<tablerosOf> tablerosOf { get; set; } = new List<tablerosOf>();
    }
}
