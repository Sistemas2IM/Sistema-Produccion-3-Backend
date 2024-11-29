namespace Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnica
{
    public class FichaTecnicaDto
    {
        public int idFichaTecnica { get; set; }

        public int? oF { get; set; }

        public int? idTipoFicha { get; set; }

        public string? TipoFicha { get; set; }

        public string? cliente { get; set; }

        public string? codigoProducto { get; set; }

        public string? producto { get; set; }

        public string? tipoDeProducto { get; set; }

        public DateTime? fechaFicha { get; set; }

        public string? tipoMaterial { get; set; }

        public string? toleranciaMaterial { get; set; }

        public string? marcarMaterial { get; set; }

        public string? calibreMaterial { get; set; }

        public string? proveedorMaterial { get; set; }

        public string? encargadoCalidad { get; set; }

        public DateTime? fechaCalidad { get; set; }

        public int? tipoObjeto { get; set; }

        public string? creadoPor { get; set; }

        public int? version { get; set; }
       
    }
}
