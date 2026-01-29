namespace Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ConfiguracionProceso
{
    public class ConfiguracionProcesoDto
    {
        public int idConfig { get; set; }

        public int? idProceso { get; set; }

        public int? tipoEntrada { get; set; }

        public int? tipoSalida { get; set; }

        public int? tipoComponente { get; set; }

        public bool? esEnsamblaje { get; set; }
    }
}
