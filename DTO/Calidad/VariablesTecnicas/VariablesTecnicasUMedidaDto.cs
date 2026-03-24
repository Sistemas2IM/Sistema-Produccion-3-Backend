using Sistema_Produccion_3_Backend.DTO.Calidad.VariableUnidadMedida;

namespace Sistema_Produccion_3_Backend.DTO.Calidad.VariablesTecnicas
{
    public class VariablesTecnicasUMedidaDto
    {
        public int? idVariable { get; set; }

        public int? idSeccion { get; set; }

        public string? nombre { get; set; }

        public string? etiqueta { get; set; }

        public string? tipoDato { get; set; }

        public bool? obligatorio { get; set; }

        public int? ordenVisual { get; set; }

        public string? valoresPosibles { get; set; }

        public string? valoresMaximo { get; set; }

        public string? valoresMinimo { get; set; }

        public string? tolerancia { get; set; }

        public bool? activo { get; set; }

        public bool? fichaTecnica { get; set; }

        public bool? fichaCliente { get; set; }

        public bool? fichaProceso { get; set; }

        public bool? certificadoCalidad { get; set; }

        public DateTime? fechaCreacion { get; set; }

        public int? tipoProceso { get; set; }

        public string? tooltip { get; set; }

        public List<VariableUnidadMedidaDto>? variableUnidadMedidas { get; set; }
    }
}
