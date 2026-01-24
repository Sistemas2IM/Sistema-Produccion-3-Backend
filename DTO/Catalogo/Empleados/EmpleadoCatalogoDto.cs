using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Empleados
{
    public class EmpleadoCatalogoDto
    {
        public int idEmpleado { get; set; }

        public string nombres { get; set; }

        public string apellidos { get; set; }
    }
}
