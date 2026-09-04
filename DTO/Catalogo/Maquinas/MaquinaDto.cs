using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.CatalogoTipo;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaMaquina;

namespace Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas
{
    public class MaquinaDto
    {
        public int idMaquina { get; set; }

        public int? idFamilia { get; set; }

        public string? nombreMaquina { get; set; }

        public string? nombreCorto { get; set; }

        public bool? necesitaAuxiliar { get; set; }

        public string? nombreAlterno { get; set; }

        public string? descripcion { get; set; }

        public string? textoAyuda { get; set; }

        public int? velocidadObjetivo { get; set; }

        public int? velocidadMaxima { get; set; }

        public int? velocidadTeorica { get; set; }

        public int? idUnidad { get; set; }

        public decimal? tamanoMaxLargoPulg { get; set; }

        public decimal? tamanoMaxAnchoPulg { get; set; }

        public decimal? tamanoMinLargoPulg { get; set; }

        public decimal? tamanoMinAnchoPulg { get; set; }

        public string? metodoImpresion { get; set; }

        public int? cantidadColoresProcesar { get; set; }

        public string? automatizacionControl { get; set; }

        public decimal? anchoImpresion { get; set; }

        public string? marcaMaquina { get; set; }

        public string? modeloMaquina { get; set; }

        public string? serieMaquina { get; set; }

        public int? anioFabricacion { get; set; }

        public int? anioInstalacion { get; set; }

        public string? paisOrigen { get; set; }

        public string? estado { get; set; }

        public string? ubicacionFisica { get; set; }

        public string? tipoAlimentacion { get; set; }

        public string? tipoSalida { get; set; }

        public decimal? gramajeMinimo { get; set; }

        public decimal? gramajeMaximo { get; set; }

        public decimal? espesorMinimo { get; set; }

        public decimal? espesorMaximo { get; set; }

        public decimal? resolucionMaxima { get; set; }

        public string? tipoTinta { get; set; }

        public string? tipoSecado { get; set; }

        public bool? registroAutomatico { get; set; }

        public bool? cambioAutomaticoPlanchas { get; set; }

        public decimal? oeeObjetivo { get; set; }

        public decimal? eficienciaObjetivo { get; set; }

        public decimal? tiempoPreparacionEstandar { get; set; }

        public decimal? tiempoCambioTrabajo { get; set; }

        public decimal? tiempoLavado { get; set; }

        public decimal? tiempoCambioPlancha { get; set; }

        public decimal? desperdicioPromedio { get; set; }

        public decimal? produccionDiariaEstimada { get; set; }

        public string? proveedor { get; set; }

        public string? responsableTecnico { get; set; }

        public string? procesoNativo { get; set; }

        public string? familiaNombre { get; set; }

        // Area desde familia maquina
        public int? idArea { get; set; }

        public string? areaNombre { get; set; }

        public object? infoMaquina { get; set; }

        public List<listaMaquinaCatalogoDto>? listaMaquinaCatalogoDto { get; set; }

        // USO TIPICO
        public List<CatalogoUsoTipicoDto> UsosTipicos { get; set; } = new List<CatalogoUsoTipicoDto>();
        public List<CatalogoTipoPapelDto> TiposPapel { get; set; } = new List<CatalogoTipoPapelDto>();
        public List<CatalogoTipoAcabadoDto> TiposAcabado { get; set; } = new List<CatalogoTipoAcabadoDto>();
    }
}
