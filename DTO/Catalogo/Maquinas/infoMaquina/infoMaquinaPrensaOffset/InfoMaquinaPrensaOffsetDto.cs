namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPrensaOffset
{
    public class InfoMaquinaPrensaOffsetDto
    {
        public int idMaquina { get; set; }

        public int? numeroCuerpos { get; set; }

        public int? numeroColores { get; set; }

        public bool? torreBarnizadora { get; set; }

        public bool? cambioAutomaticoPlanchas { get; set; }

        public bool? controlDensidad { get; set; }
    }
}
