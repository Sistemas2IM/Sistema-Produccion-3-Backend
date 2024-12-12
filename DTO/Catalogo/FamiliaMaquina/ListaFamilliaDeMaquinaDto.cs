using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.FamiliaMaquina
{
    public class ListaFamilliaDeMaquinaDto
    {
        public int idFamilia { get; set; }

        public string nombreFamilia { get; set; }

        public List<MaquinaDto> maquinas { get; set; }
    }
}
