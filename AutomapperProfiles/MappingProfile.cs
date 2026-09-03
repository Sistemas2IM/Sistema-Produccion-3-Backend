using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.AnexosNEXO;
using Sistema_Produccion_3_Backend.DTO.Buscadores.DTOGlobales;
using Sistema_Produccion_3_Backend.DTO.Calidad.AccionSolicitada;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.DetalleAuditoriaProceso;
using Sistema_Produccion_3_Backend.DTO.Calidad.AuditoriaProceso.DetalleAuditoriaProceso.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.BitacoraCaso;
using Sistema_Produccion_3_Backend.DTO.Calidad.CasoAccionSolicitada;
using Sistema_Produccion_3_Backend.DTO.Calidad.CasoCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.CategoriaDefecto;
using Sistema_Produccion_3_Backend.DTO.Calidad.CausaRaizCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.DictamenCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaProcesos.DetalleFichaProcesos.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta;
using Sistema_Produccion_3_Backend.DTO.Calidad.FormulacionTinta.EspecificacionTintas;
using Sistema_Produccion_3_Backend.DTO.Calidad.MedicionAguas;
using Sistema_Produccion_3_Backend.DTO.Calidad.MedicionAguas.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.RegistroLamparas;
using Sistema_Produccion_3_Backend.DTO.Calidad.RegistroLamparas.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.ResolucionCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.SecuenciaColor;
using Sistema_Produccion_3_Backend.DTO.Calidad.SecuenciaColor.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.SeveridadCaso;
using Sistema_Produccion_3_Backend.DTO.Calidad.SubtipoDefecto;
using Sistema_Produccion_3_Backend.DTO.Calidad.TipoCaso;
using Sistema_Produccion_3_Backend.DTO.Calidad.TipoEventoBitacora;
using Sistema_Produccion_3_Backend.DTO.Calidad.TurnoAuditoria;
using Sistema_Produccion_3_Backend.DTO.Calidad.TurnoAuditoria.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.UnidadesMedida;
using Sistema_Produccion_3_Backend.DTO.Calidad.VariablesTecnicas;
using Sistema_Produccion_3_Backend.DTO.Calidad.VariableUnidadMedida;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Empleados;
using Sistema_Produccion_3_Backend.DTO.Catalogo.FamiliaMaquina;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.CatalogoTipo;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaBarnizadora;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaCorteConversion;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaDigital;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaFlexografia;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPegadora;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPrensaOffset;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPreprensa;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaTroqueladora;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Turnos;
using Sistema_Produccion_3_Backend.DTO.CondicionInicial;
using Sistema_Produccion_3_Backend.DTO.CondicionInicial.DetalleCondicionInicial;
using Sistema_Produccion_3_Backend.DTO.CondicionInicial.DetalleCondicionInicial.Batch;
using Sistema_Produccion_3_Backend.DTO.CorridaCombinada;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta.BathcEtiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf.BatchEtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta.BatchTarjetaEtiqueta;
using Sistema_Produccion_3_Backend.DTO.GoogleChat.SoporteNexo;
using Sistema_Produccion_3_Backend.DTO.Horarios.HorariosOperativos;
using Sistema_Produccion_3_Backend.DTO.Horarios.HorariosOperativos.Batch;
using Sistema_Produccion_3_Backend.DTO.Horarios.IndisponibilidadMaquinas;
using Sistema_Produccion_3_Backend.DTO.Horarios.TurnosOperativos;
using Sistema_Produccion_3_Backend.DTO.Horarios.TurnosOperativosArea;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaItem;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaItem.Batch;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaMaquina;
using Sistema_Produccion_3_Backend.DTO.ListaDeOperaciones.ListaMaquina.Batch;
using Sistema_Produccion_3_Backend.DTO.LoginAuth;
using Sistema_Produccion_3_Backend.DTO.LoginAuth.SesionOperador;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoEspecifico;
using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoEspecifico.BatchPermisoEspecifico;
using Sistema_Produccion_3_Backend.DTO.Permisos.PermisoTipo;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Cargo;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Diseño;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo.Permiso;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Operadores;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.PermisoMaquina;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Rol;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.Asignacion;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.BusquedaProcesos;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ConfirmacionPreliminar;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogCambiosProceso;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion.LogProgramacionDetalle;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogProgramacion.LogProgramacionDetalle.Batch;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.MaterialOf;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesoOfSolicitud;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Acabado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.AcabadoFlexo;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Barnizado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Impresión;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.ImpresionFlexo;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.MangaFlexo;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Pegadora;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Preprensa;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.procesosFlexo;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Serigrafia;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.ProcesosMaquinas.Troquelado;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.SolicitudMateriales;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.UpdateMaquina;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.UpdateSAP;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado.DetalleEntrega;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado.ListaEmpaque;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.Auxiliares;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Impresoras;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.EstadoReporte;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.PausaMaquina;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.BobinasAsignadas;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.BobinasAsignadas.Batch;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ComponenteProduccion;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ComponenteProduccion.Batch;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.EtiquetaSolicitud;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.EtiquetaSolicitud.Batch;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.LotePliego;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales.Batch;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales.ProcesosOf;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TipoSemielaborados;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.DecisionConciliacion;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso.Conciliacion.MotivoConciliacion;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina.ValeBobinaCorteEstado;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.Tableros.Areas;
using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.BusquedaTarjetas;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.EstadoOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.HistorialVencimientoOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.logCambiosOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.NotasOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.Reportes;
using Sistema_Produccion_3_Backend.DTO.TiemposEstimados.TiemposOf;
using Sistema_Produccion_3_Backend.DTO.TiemposEstimados.TiemposProceso;
using Sistema_Produccion_3_Backend.DTO.ValidacionArranque;
using Sistema_Produccion_3_Backend.DTO.ValidacionArranque.DetalleValidacionArranque;
using Sistema_Produccion_3_Backend.DTO.ValidacionArranque.DetalleValidacionArranque.Batch;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.AutomapperProfiles
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            // ACA PARA MAPEAR LOS MODELOS->DTO

            // TARJETAS OF - DTO ====================================================================================
            CreateMap<tarjetaOf, TarjetaOfDto>()
                .ForMember(dest => dest.estadonombre, opt => opt.MapFrom(src => src.idEstadoOfNavigation.nombreEstado))
                .ForMember(dest => dest.etiquetaDto, opt => opt.MapFrom(src => src.etiquetaOf))
                .ForMember(dest => dest.inicioEstimado, opt => opt.MapFrom(src => src.ffeTiemposOfGlobal.Inicio_Estimado))
                .ForMember(dest => dest.finEstimado, opt => opt.MapFrom(src => src.ffeTiemposOfGlobal.Fin_Proyectado))
                .ForMember(dest => dest.inicioReal, opt => opt.MapFrom(src => src.ffeTiemposOfGlobal.Inicio_Real))
                .ForMember(dest => dest.finReal, opt => opt.MapFrom(src => src.ffeTiemposOfGlobal.Fin_Real))
                .ForMember(dest => dest.secuenciador, opt => opt.MapFrom(src => src.secuenciadoPorNavigation.nombres + " " + src.secuenciadoPorNavigation.apellidos))
                .ForMember(dest => dest.fechaVencimientoNueva, opt => opt.MapFrom(src => src.historialVencimientoOf.OrderByDescending(h => h.fechaVencimientoNueva).FirstOrDefault().fechaVencimientoNueva))
                .ReverseMap();
            //.ForPath(src => src.idEstadoOfNavigation, opt => opt.Ignore());
            CreateMap<tarjetaOf, TarjetaBusquedaDto>().ReverseMap();

            CreateMap<tarjetaOf, ReportePMTarjetaOf>()
                .ReverseMap();

            CreateMap<tarjetaOf, AddTarjetaOfDto>().ReverseMap();
            CreateMap<UpdateTarjetaOfDto, tarjetaOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            {
                CreateMap<logCambiosOf, logCambiosOfDto>().ReverseMap();
                CreateMap<logCambiosOf, AddlogCambiosOfDto>().ReverseMap();
                CreateMap<UpdatelogCambiosOfDto, logCambiosOf>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            //Estados Of
            CreateMap<estadosOf, EstadoOfDto>()
                .ForMember(dest => dest.tarjetaOfDtos, opt => opt.MapFrom(src => src.tarjetaOf)) // Mapea tarjetasOf a su DTO
                .ReverseMap();
            CreateMap<estadosOf, AddEstadoOfDto>().ReverseMap();
            CreateMap<UpdateEstadoOfDto, estadosOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            {
                CreateMap<etiquetaOf, EtiquetaOfDto>()
                    .ForMember(dest => dest.color, opt => opt.MapFrom(src => src.idEtiquetaNavigation.color))
                    .ForMember(dest => dest.texto, opt => opt.MapFrom(src => src.idEtiquetaNavigation.texto))
                    .ReverseMap();
                CreateMap<etiquetaOf, AddEtiquetaOf>().ReverseMap();
                CreateMap<UpdateEtiquetaOf, etiquetaOf>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<etiquetaOf, AddBatchEtiquetaOf>().ReverseMap();
                CreateMap<UpdateBatchEtiquetaOf, etiquetaOf>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            CreateMap<notasOf, NotasDto>().ReverseMap();
            CreateMap<notasOf, AddNotasDto>().ReverseMap();
            CreateMap<UpdateNotasDto, notasOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateBatchNotasOf, notasOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            // PROCESOS OF ==========================================================================================
            CreateMap<procesoOf, ProcesoOfDto>()
                .ForMember(dest => dest.idArea, opt => opt.MapFrom(src => src.idTableroNavigation.idArea))
                .ForMember(dest => dest.detalleProcesoOf, opt => opt.MapFrom(src => src.detalleReporte))
                .ForMember(dest => dest.tarjetaEtiquetaDto, opt => opt.MapFrom(src => src.tarjetaEtiqueta))
                .ForMember(dest => dest.posturasOfDto, opt => opt.MapFrom(src => src.idPosturaNavigation))
                .ForMember(dest => dest.tablerosOfDto, opt => opt.MapFrom(src => src.idTableroNavigation))
                .ForMember(dest => dest.materialDto, opt => opt.MapFrom(src => src.idMaterialNavigation))
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.codProd, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                .ForMember(dest => dest.nombrePostura, opt => opt.MapFrom(src => src.idPosturaNavigation.nombrePostura))
                .ForMember(dest => dest.lineaNegocio, opt => opt.MapFrom(src => src.oFNavigation.lineaDeNegocio))
                .ForMember(dest => dest.cantRequerida, opt => opt.MapFrom(src => src.oFNavigation.cantidadOf))
                .ForMember(dest => dest.tipoOrden, opt => opt.MapFrom(src => src.oFNavigation.tipoDeOrden))
                .ForMember(dest => dest.unidadMedida, opt => opt.MapFrom(src => src.oFNavigation.unidadMedida))
                .ForMember(dest => dest.fsc, opt => opt.MapFrom(src => src.oFNavigation.fsc))
                .ForMember(dest => dest.asignacionDto, opt => opt.MapFrom(src => src.asignacion))
                .ForMember(dest => dest.serie, opt => opt.MapFrom(src => src.oFNavigation.seriesOf))
                .ForMember(dest => dest.inicioEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Inicio_Estimado))
                .ForMember(dest => dest.finEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Fin_Proyectado))
                .ForMember(dest => dest.fechaVencimiento, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.fechaVencimiento
                            : src.oFNavigation.fechaVencimiento))
                .ForMember(dest => dest.fechaCreacionOf, opt => opt.MapFrom(src => src.oFNavigation.fechaCreacion))
                .ForMember(dest => dest.subordinadas, opt => opt.MapFrom(src =>
                src.corridaCombinadamaestroNavigation
                    .Concat(src.corridaCombinadasubordinadoNavigation != null
                        ? new List<corridaCombinada> { src.corridaCombinadasubordinadoNavigation }
                        : new List<corridaCombinada>())))
                .ForMember(dest => dest.descripcionOf, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.descripcionOf
                            : src.oFNavigation.descipcionOf))
                .ReverseMap();
            CreateMap<procesoOf, ProcesoOfVistaTableroDto>()
                .ForMember(dest => dest.idArea, opt => opt.MapFrom(src => src.idTableroNavigation.idArea))
                .ForMember(dest => dest.detalleProcesoOf, opt => opt.MapFrom(src => src.detalleReporte))
                .ForMember(dest => dest.tarjetaEtiquetaDto, opt => opt.MapFrom(src => src.tarjetaEtiqueta))
                .ForMember(dest => dest.materialDto, opt => opt.MapFrom(src => src.idMaterialNavigation))
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.codProd, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                .ForMember(dest => dest.nombrePostura, opt => opt.MapFrom(src => src.idPosturaNavigation.nombrePostura))
                .ForMember(dest => dest.lineaNegocio, opt => opt.MapFrom(src => src.oFNavigation.lineaDeNegocio))
                .ForMember(dest => dest.cantRequerida, opt => opt.MapFrom(src => src.oFNavigation.cantidadOf))
                .ForMember(dest => dest.tipoOrden, opt => opt.MapFrom(src => src.oFNavigation.tipoDeOrden))
                .ForMember(dest => dest.unidadMedida, opt => opt.MapFrom(src => src.oFNavigation.unidadMedida))
                .ForMember(dest => dest.fsc, opt => opt.MapFrom(src => src.oFNavigation.fsc))
                .ForMember(dest => dest.asignacionDto, opt => opt.MapFrom(src => src.asignacion))
                .ForMember(dest => dest.serie, opt => opt.MapFrom(src => src.oFNavigation.seriesOf))
                .ForMember(dest => dest.inicioEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Inicio_Estimado))
                .ForMember(dest => dest.finEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Fin_Proyectado))
                .ForMember(dest => dest.componentes, opt => opt.MapFrom(src => src.componenteProduccion))
                .ForMember(dest => dest.fechaVencimiento, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.fechaVencimiento
                            : src.oFNavigation.fechaVencimiento))
                .ForMember(dest => dest.fechaCreacionOf, opt => opt.MapFrom(src => src.oFNavigation.fechaCreacion))
                .ForMember(dest => dest.subordinadas, opt => opt.MapFrom(src =>
                src.corridaCombinadamaestroNavigation
                    .Concat(src.corridaCombinadasubordinadoNavigation != null
                        ? new List<corridaCombinada> { src.corridaCombinadasubordinadoNavigation }
                        : new List<corridaCombinada>())))
                .ForMember(dest => dest.descripcionOf, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.descripcionOf
                            : src.oFNavigation.descipcionOf))
                .ReverseMap();

            CreateMap<procesoOf, ProcesoOfTableroListaDto>()
               .ForMember(dest => dest.tarjetaEtiquetaDto, opt => opt.MapFrom(src => src.tarjetaEtiqueta))
               .ForMember(dest => dest.materialDto, opt => opt.MapFrom(src => src.idMaterialNavigation))
               .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
               .ForMember(dest => dest.codProd, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
               .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
               .ForMember(dest => dest.nombrePostura, opt => opt.MapFrom(src => src.idPosturaNavigation.nombrePostura))
               .ForMember(dest => dest.lineaNegocio, opt => opt.MapFrom(src => src.oFNavigation.lineaDeNegocio))
               .ForMember(dest => dest.cantRequerida, opt => opt.MapFrom(src => src.oFNavigation.cantidadOf))
               .ForMember(dest => dest.tipoOrden, opt => opt.MapFrom(src => src.oFNavigation.tipoDeOrden))
               .ForMember(dest => dest.unidadMedida, opt => opt.MapFrom(src => src.oFNavigation.unidadMedida))
               .ForMember(dest => dest.fsc, opt => opt.MapFrom(src => src.oFNavigation.fsc))
               .ForMember(dest => dest.asignacionDto, opt => opt.MapFrom(src => src.asignacion))
               .ForMember(dest => dest.serie, opt => opt.MapFrom(src => src.oFNavigation.seriesOf))
               .ForMember(dest => dest.fechaVencimiento, opt => opt.MapFrom(src =>
                       src.corridaCombinada == true
                           ? src.fechaVencimiento
                           : src.oFNavigation.fechaVencimiento))
               .ForMember(dest => dest.subordinadas, opt => opt.MapFrom(src =>
               src.corridaCombinadamaestroNavigation
                   .Concat(src.corridaCombinadasubordinadoNavigation != null
                       ? new List<corridaCombinada> { src.corridaCombinadasubordinadoNavigation }
                       : new List<corridaCombinada>())))
               .ReverseMap();

            CreateMap<procesoOf, ListaProcesoOfDto>()
                .ForMember(dest => dest.idArea, opt => opt.MapFrom(src => src.idTableroNavigation.idArea))
                .ForMember(dest => dest.PosturasOfDto, opt => opt.MapFrom(src => src.idPosturaNavigation))
                .ForMember(dest => dest.TablerosOfDto, opt => opt.MapFrom(src => src.idTableroNavigation))
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.codProd, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                .ForMember(dest => dest.lineaNegocio, opt => opt.MapFrom(src => src.oFNavigation.lineaDeNegocio))
                .ForMember(dest => dest.cantRequerida, opt => opt.MapFrom(src => src.oFNavigation.cantidadOf))
                .ForMember(dest => dest.tipoOrden, opt => opt.MapFrom(src => src.oFNavigation.tipoDeOrden))
                .ForMember(dest => dest.unidadMedida, opt => opt.MapFrom(src => src.oFNavigation.unidadMedida))
                .ForMember(dest => dest.fsc, opt => opt.MapFrom(src => src.oFNavigation.fsc))
                .ForMember(dest => dest.serie, opt => opt.MapFrom(src => src.oFNavigation.seriesOf))
                .ForMember(dest => dest.tarjetaEtiquetaDto, opt => opt.MapFrom(src => src.tarjetaEtiqueta))
                .ForMember(dest => dest.asignacionDto, opt => opt.MapFrom(src => src.asignacion))
                .ForMember(dest => dest.inicioEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Inicio_Estimado))
                .ForMember(dest => dest.finEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Fin_Proyectado))
                .ForMember(dest => dest.componentes, opt => opt.MapFrom(src => src.componenteProduccion))
                .ForMember(dest => dest.fechaVencimiento, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.fechaVencimiento
                            : src.oFNavigation.fechaVencimiento))
                .ForMember(dest => dest.fechaCreacionOf, opt => opt.MapFrom(src => src.oFNavigation.fechaCreacion))
                .ForMember(dest => dest.secuenciaArea, opt => opt.MapFrom(src => src.idTableroNavigation.idAreaNavigation.secuencia))
                .ForMember(dest => dest.subordinadas, opt => opt.MapFrom(src =>
                src.corridaCombinadamaestroNavigation
                    .Concat(src.corridaCombinadasubordinadoNavigation != null
                        ? new List<corridaCombinada> { src.corridaCombinadasubordinadoNavigation }
                        : new List<corridaCombinada>()))
                )
                .ForMember(dest => dest.descripcionOf, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.descripcionOf
                            : src.oFNavigation.descipcionOf))
                .ReverseMap();
            CreateMap<procesoOf, ProcesosBusquedaDto>()
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                .ForMember(dest => dest.nombrePostura, opt => opt.MapFrom(src => src.idPosturaNavigation.nombrePostura))
                .ReverseMap();
            CreateMap<procesoOf, AddProcesoOfDto>().ReverseMap();
            CreateMap<UpdateProcesoOfDto, procesoOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<AddBatchProcesoDto, procesoOf>().ReverseMap();
            CreateMap<SAPUpdateProcesoOf, procesoOf>().ReverseMap();
            CreateMap<UpdateMaquinaProcesoOf, procesoOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateArchivadaProcesoOf, procesoOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            {
                CreateMap<procesoOf, ProcesoOfMaquinas>()
                    .ForMember(dest => dest.idArea, opt => opt.MapFrom(src => src.idTableroNavigation.idArea))
                    .ForMember(dest => dest.posturasOfDto, opt => opt.MapFrom(src => src.idPosturaNavigation))
                    .ForMember(dest => dest.materialDto, opt => opt.MapFrom(src => src.idMaterialNavigation))
                    .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                    .ForMember(dest => dest.codProd, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                    .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                    .ForMember(dest => dest.lineaNegocio, opt => opt.MapFrom(src => src.oFNavigation.lineaDeNegocio))
                    .ForMember(dest => dest.cantRequerida, opt => opt.MapFrom(src => src.oFNavigation.cantidadOf))
                    .ForMember(dest => dest.tipoOrden, opt => opt.MapFrom(src => src.oFNavigation.tipoDeOrden))
                    .ForMember(dest => dest.unidadMedida, opt => opt.MapFrom(src => src.oFNavigation.unidadMedida))
                    .ForMember(dest => dest.fsc, opt => opt.MapFrom(src => src.oFNavigation.fsc))
                    .ForMember(dest => dest.serie, opt => opt.MapFrom(src => src.oFNavigation.seriesOf))
                    .ForMember(dest => dest.inicioEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Inicio_Estimado))
                    .ForMember(dest => dest.finEstimado, opt => opt.MapFrom(src => src.ffeTiemposProcesosGlobal.Fin_Proyectado))
                    .ForMember(dest => dest.fechaVencimiento, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.fechaVencimiento
                            : src.oFNavigation.fechaVencimiento))
                    .ForMember(dest => dest.fechaCreacionOf, opt => opt.MapFrom(src => src.oFNavigation.fechaCreacion))
                    .ForMember(dest => dest.subordinadas, opt => opt.MapFrom(src =>
                    src.corridaCombinadamaestroNavigation
                        .Concat(src.corridaCombinadasubordinadoNavigation != null
                            ? new List<corridaCombinada> { src.corridaCombinadasubordinadoNavigation }
                            : new List<corridaCombinada>())))
                    .ForMember(dest => dest.descripcionOf, opt => opt.MapFrom(src => src.oFNavigation.descipcionOf))
                    .ReverseMap();

                CreateMap<procesoOf, AddProcesoOfMaquinas>()
                    .ReverseMap();

                CreateMap<UpProcesoOfMaquinas, procesoOf>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<UpdateBatchProcesoOfMaquina, procesoOf>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<procesoOf, ProcesoOfSolicitudDto>()
                .ForMember(dest => dest.idArea, opt => opt.MapFrom(src => src.idTableroNavigation.idArea))
                .ForMember(dest => dest.nombreArea, opt => opt.MapFrom(src => src.idTableroNavigation.idAreaNavigation.nombreArea))
                //.ForMember(dest => )                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                                  
                .ForMember(dest => dest.nombrePostura, opt => opt.MapFrom(src => src.idPosturaNavigation.nombrePostura))
                .ForMember(dest => dest.lineaNegocio, opt => opt.MapFrom(src => src.oFNavigation.lineaDeNegocio))
                .ForMember(dest => dest.cantRequerida, opt => opt.MapFrom(src => src.oFNavigation.cantidadOf))
                .ForMember(dest => dest.tipoOrden, opt => opt.MapFrom(src => src.oFNavigation.tipoDeOrden))
                .ForMember(dest => dest.unidadMedida, opt => opt.MapFrom(src => src.oFNavigation.unidadMedida))
                .ReverseMap();


                CreateMap<procesoAcabado, ProcesoAcabadoDto>().ReverseMap();
                CreateMap<procesoBarniz, ProcesoBarnizDto>().ReverseMap();
                CreateMap<procesoImpresora, ProcesoImpresoraDto>().ReverseMap();
                CreateMap<procesoPegadora, ProcesoPegadoraDto>().ReverseMap();
                CreateMap<procesoPreprensa, ProcesoPreprensaDto>().ReverseMap();
                CreateMap<procesoTroqueladora, ProcesoTroqueladoraDto>().ReverseMap();
                CreateMap<procesoSerigrafia, ProcesoSerigrafiaDto>().ReverseMap();
                CreateMap<procesoImpresoraFlexo, ProcesoImpresoraFlexoDto>().ReverseMap();
                CreateMap<procesoAcabadoFlexo, ProcesoAcabadoFlexoDto>().ReverseMap();
                CreateMap<procesoMangaFlexo, ProcesoMangaFlexoDto>().ReverseMap();
                CreateMap<procesosFlexo, ProcesosFlexoDto>().ReverseMap();

                CreateMap<UpProcesoAcabadoDto, procesoAcabado>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoBarnizDto, procesoBarniz>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoImpresoraDto, procesoImpresora>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoPegadoraDto, procesoPegadora>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoPreprensaDto, procesoPreprensa>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoTroqueladoDto, procesoTroqueladora>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoSerigrafia, procesoSerigrafia>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoImpresoraFlexoDto, procesoImpresoraFlexo>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoAcabadoFlexoDto, procesoAcabadoFlexo>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesoMangaFlexoDto, procesoMangaFlexo>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<UpProcesosFlexoDto, procesosFlexo>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            {
                CreateMap<tarjetaEtiqueta, TarjetaEtiquetaDto>()
                    .ForMember(dest => dest.color, opt => opt.MapFrom(src => src.idEtiquetaNavigation.color))
                    .ForMember(dest => dest.texto, opt => opt.MapFrom(src => src.idEtiquetaNavigation.texto))
                    .ReverseMap();
                CreateMap<tarjetaEtiqueta, AddTarjetaEtiquetaDto>().ReverseMap();
                CreateMap<UpdateTarjetaEtiquetaDto, tarjetaEtiqueta>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<AddBatchTarjetaEtiqueta, tarjetaEtiqueta>().ReverseMap();
                CreateMap<UpdateBatchTarjetaEtiqueta, tarjetaEtiqueta>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<etiqueta, EtiquetaDto>().ReverseMap();
                CreateMap<etiqueta, AddEtiquetaDto>().ReverseMap();
                CreateMap<UpdateEtiquetaDto, etiqueta>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<etiqueta, AddBatchEtiqueta>().ReverseMap();
                CreateMap<UpdateBatchEtiqueta, etiqueta>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }


            {
                CreateMap<logCambiosProceso, LogCambiosProcesoDto>().ReverseMap();
                CreateMap<logCambiosProceso, AddLogCambiosProcesoDto>().ReverseMap();
                CreateMap<UpdateLogCambiosProcesoDto, logCambiosProceso>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            CreateMap<material, MaterialOfDto>().ReverseMap();
            CreateMap<material, AddMaterialOfDto>().ReverseMap();
            CreateMap<UpdateMaterialOfDto, material>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // solicitud material y proceso of
            CreateMap<procesoOf, ProcesoOfSolicitudMaterialDto>()
                .ForMember(dest => dest.nombrePostura, opt => opt.MapFrom(src => src.idPosturaNavigation.nombrePostura))
                .ForMember(dest => dest.solicitudMateriales, opt => opt.MapFrom(src => src.idSolicitudMaterialesNavigation))
                .ForMember(dest => dest.detalleProcesoOf, opt => opt.MapFrom(src => src.detalleReporte))
                .ReverseMap();

            // OV - DTO =============================================================================================
            CreateMap<oV, OVDto>()
                .ForMember(dest => dest.articulo, opt => opt.MapFrom(src => src.articuloOv))
                .ReverseMap();

            CreateMap<oV, AddOVDto>().ReverseMap();

            // Articulo - DTO =======================================================================================
            CreateMap<articuloOv, ArticuloDto>().ReverseMap();
            CreateMap<articuloOv, AddArticuloDto>().ReverseMap();

            // PRODUCTO TERMINADO ====================================================================================
            CreateMap<entregasProductoTerminado, ProductoTerminadoDto>()
                .ForMember(dest => dest.detalleEntrega, opt => opt.MapFrom(src => src.detalleEntrega))
                .ForMember(dest => dest.estadoReporteDto, opt => opt.MapFrom(src => src.idEstadoReporteNavigation))
                .ForMember(dest => dest.maquinaDto, opt => opt.MapFrom(src => src.idMaquinaNavigation))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.ofNavigation.codArticulo))
                .ForMember(dest => dest.articuloOf, opt => opt.MapFrom(src => src.ofNavigation.productoOf))
                .ForMember(dest => dest.cantidadOf, opt => opt.MapFrom(src => src.ofNavigation.cantidadOf))
                .ForMember(dest => dest.nombreCliente, opt => opt.MapFrom(src => src.ofNavigation.clienteOf))
                .ForMember(dest => dest.fsc, opt => opt.MapFrom(src => src.ofNavigation.fsc))
                .ForMember(dest => dest.vendedorOf, opt => opt.MapFrom(src => src.ofNavigation.vendedorOf))
                .ReverseMap();
            CreateMap<entregasProductoTerminado, UltimoProductoTerminadoDto>().ReverseMap(); // Para regresar el ultimo PT + 1
            CreateMap<entregasProductoTerminado, AddProductoTerminadoDto>().ReverseMap();
            CreateMap<UpdateBatchProductoTerminado, entregasProductoTerminado>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateProductoTerminadoDto, entregasProductoTerminado>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleEntrega, DetalleEntregaDto>().ReverseMap();
            CreateMap<detalleEntrega, AddDetalleEntregaDto>().ReverseMap();
            CreateMap<detalleEntrega, AddBatchDetalleDto>().ReverseMap();
            CreateMap<UpdateDetalleEntregaDto, detalleEntrega>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBatchDetalleEntrega, detalleEntrega>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            {
                CreateMap<listasEmpaque, ListasEmpaqueDto>().ReverseMap();
                CreateMap<listasEmpaque, AddListasEmpaqueDto>().ReverseMap();
                CreateMap<UpdateListasEmpaqueDto, listasEmpaque>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            // REPORTES POR OPERADOR ==================================================================================
            CreateMap<reportesDeOperadores, ReporteOperadorDto>()
                .ForMember(dest => dest.estadoReporteDto, opt => opt.MapFrom(src => src.idEstadoReporteNavigation))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.idEstadoReporteNavigation.nombreEstado))
                .ForMember(dest => dest.tipoReporteDto, opt => opt.MapFrom(src => src.idTipoReporteNavigation))
                .ForMember(dest => dest.maquinaDto, opt => opt.MapFrom(src => src.idMaquinaNavigation))
                .ForMember(dest => dest.detalleReporte, opt => opt.MapFrom(src => src.detalleReporte))
                .ForMember(dest => dest.nombreUsuario, opt => opt.MapFrom(src => src.operadorNavigation.nombres + " " + src.operadorNavigation.apellidos))
                .ForMember(dest => dest.nombreAuxiliar, opt => opt.MapFrom(src => src.auxiliarNavigation.nombre))
                .ReverseMap();

            CreateMap<reportesDeOperadores, ReporteOperadorListaDto>()
                .ForMember(dest => dest.estadoReporteDto, opt => opt.MapFrom(src => src.idEstadoReporteNavigation))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.idEstadoReporteNavigation.nombreEstado))
                .ForMember(dest => dest.tipoReporteDto, opt => opt.MapFrom(src => src.idTipoReporteNavigation))
                .ForMember(dest => dest.maquinaDto, opt => opt.MapFrom(src => src.idMaquinaNavigation))
                .ForMember(dest => dest.nombreUsuario, opt => opt.MapFrom(src =>
                    src.operadorNavigation.nombres + " " + src.operadorNavigation.apellidos))
                .ForMember(dest => dest.nombreAuxiliar, opt => opt.MapFrom(src => src.auxiliarNavigation.nombre));

            CreateMap<maquinas, MaquinaReporteDto>()
                .ForMember(dest => dest.nombreFamilia, opt => opt.MapFrom(src => src.idFamiliaNavigation.nombreFamilia));

            CreateMap<reportesDeOperadores, AddReporteOperadorDto>().ReverseMap();
            CreateMap<UpdateReporteOperadorDto, reportesDeOperadores>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<reportesDeOperadores, CountReporteOperadorDto>().ReverseMap();
            CreateMap<detalleReporte, AddBatchDetalleImpresora>().ReverseMap()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBatchReporteOperador, reportesDeOperadores>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            {
                CreateMap<detalleReporte, DetalleReporteDto>()
               .ForMember(dest => dest.operacionesDto, opt => opt.MapFrom(src => src.idOperacionNavigation))
               .ForMember(dest => dest.materialDto, opt => opt.MapFrom(src => src.idMaterialNavigation))
               .ForMember(dest => dest.numOf, opt => opt.MapFrom(src => src.oFNavigation.oF))
               .ForMember(dest => dest.descripcionOf, opt => opt.MapFrom(src => src.oFNavigation.nombreOf))
               .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
               .ForMember(dest => dest.nombreCorto, opt => opt.MapFrom(src => src.maquinaNavigation.nombreCorto))
               .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.maquinaNavigation.nombreMaquina))
               .ForMember(dest => dest.nombreOperacion, opt => opt.MapFrom(src => src.idOperacionNavigation.nombreOperacion))
               .ForMember(dest => dest.prefijo, opt => opt.MapFrom(src => src.idOperacionNavigation.prefijo))
               .ForMember(dest => dest.indicador, opt => opt.MapFrom(src => src.idProcesoNavigation.indicador))
               .ForMember(dest => dest.indicadorProceso, opt => opt.MapFrom(src => src.idProcesoNavigation.indicadorProceso))
               .ForMember(dest => dest.corridaCombinada, opt => opt.MapFrom(src => src.idProcesoNavigation.corridaCombinada))
               .ForMember(dest => dest.reproceso, opt => opt.MapFrom(src => src.idProcesoNavigation.reproceso))
               .ForMember(dest => dest.correlativoCC, opt => opt.MapFrom(src => src.idProcesoNavigation.correlativoCC))
               .ReverseMap();
                CreateMap<detalleReporte, UpdateDetalleReporteDto>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<detalleReporte, AddDetalleReporteDto>().ReverseMap();

                CreateMap<detalleReporte, AddBatchDetalleImpresora>().ReverseMap();
                CreateMap<UpdateBatchDetalleImpresora, detalleReporte>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                {
                    CreateMap<operaciones, OperacionesDto>().ReverseMap();
                    CreateMap<operaciones, AddOperacionesDto>().ReverseMap();
                    CreateMap<UpdateOperacionesDto, operaciones>()
                        .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                }

                CreateMap<auxiliares, AuxiliaresDto>().ReverseMap();
                CreateMap<auxiliares, AddAuxiliaresDto>().ReverseMap();
                CreateMap<UpdateAuxiliaresDto, auxiliares>().ReverseMap();
            }

            CreateMap<estadosReporte, EstadoReporteDto>().ReverseMap();
            CreateMap<tipoReporte, TipoReporteDto>().ReverseMap();
            CreateMap<material, MaterialDto>().ReverseMap();

            // CATALOGOS ==============================================================================================
            CreateMap<maquinas, MaquinaDto>()
                .ForMember(dest => dest.familiaNombre, opt => opt.MapFrom(src => src.idFamiliaNavigation.nombreFamilia))
                .ForMember(dest => dest.listaMaquinaCatalogoDto, opt => opt.MapFrom(src => src.listaMaquina))
                .ForMember(dest => dest.idArea, opt => opt.MapFrom(src => src.idFamiliaNavigation.idAreaNavigation.idArea))
                .ForMember(dest => dest.areaNombre, opt => opt.MapFrom(src => src.idFamiliaNavigation.idAreaNavigation.nombreArea))
                .ForMember(dest => dest.UsosTipicos, opt => opt.MapFrom(src => src.idUsoTipico))
                .ForMember(dest => dest.TiposPapel, opt => opt.MapFrom(src => src.idTipoPapel))
                .ForMember(dest => dest.TiposAcabado, opt => opt.MapFrom(src => src.idTipoAcabado))
                .ReverseMap();
            CreateMap<maquinas, ProcesoMaquinaDto>()
                .ForMember(dest => dest.familiaNombre, opt => opt.MapFrom(src => src.idFamiliaNavigation.nombreFamilia))
                .ReverseMap();
            CreateMap<maquinas, AddMaquinaDto>().ReverseMap();
            CreateMap<UpdateMaquinaDto, maquinas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<maquinas, MaquinaOfDto>()
                .ForMember(dest => dest.familiaNombre, opt => opt.MapFrom(src => src.idFamiliaNavigation.nombreFamilia))
                .ForMember(dest => dest.idArea, opt => opt.MapFrom(src => src.idFamiliaNavigation.idAreaNavigation.idArea))
                .ForMember(dest => dest.areaNombre, opt => opt.MapFrom(src => src.idFamiliaNavigation.idAreaNavigation.nombreArea))
                .ForMember(dest => dest.UsosTipicos, opt => opt.MapFrom(src => src.idUsoTipico))
                .ForMember(dest => dest.TiposPapel, opt => opt.MapFrom(src => src.idTipoPapel))
                .ForMember(dest => dest.TiposAcabado, opt => opt.MapFrom(src => src.idTipoAcabado))
                .ReverseMap();

            // info maquinas
            CreateMap<infoMaquinaBarnizadora, InfoMaquinaBarnizadoraDto>().ReverseMap();

            CreateMap<infoMaquinaCorteConversion, InfoMaquinaCorteConversionDto>().ReverseMap();

            CreateMap<infoMaquinaDigital, InfoMaquinaDigitalDto>().ReverseMap();

            CreateMap<infoMaquinaFlexografia, InfoMaquinaFlexografiaDto>().ReverseMap();

            CreateMap<infoMaquinaPegadora, InfoMaquinaPegadoraDto>().ReverseMap();

            CreateMap<infoMaquinaPrensaOffset, InfoMaquinaPrensaOffsetDto>().ReverseMap();

            CreateMap<infoMaquinaPreprensa, InfoMaquinaPreprensaDto>().ReverseMap();

            CreateMap<infoMaquinaTroqueladora, InfoMaquinaTroqueladoraDto>().ReverseMap();

            // -------------------------------------------------------------------------

            // ==== USO MAQUINAS
            CreateMap<catalogoTipoAcabado, CatalogoTipoAcabadoDto>().ReverseMap();
            CreateMap<catalogoTipoPapel, CatalogoTipoPapelDto>().ReverseMap();
            CreateMap<catalogoUsoTipico, CatalogoUsoTipicoDto>().ReverseMap();

            CreateMap<familliaDeMaquina, FamilliaDeMaquinaDto>().ReverseMap();

            CreateMap<familliaDeMaquina, ListaFamilliaDeMaquinaDto>()
                .ForMember(dest => dest.maquinas, opt => opt.MapFrom(src => src.maquinas))
                .ReverseMap();

            CreateMap<empleadoCatalogo, EmpleadoCatalogoDto>().ReverseMap();
            CreateMap<empleadoCatalogo, AddEmpleadoCatalogoDto>().ReverseMap();
            CreateMap<UpdateEmpleadoCatalogoDto, empleadoCatalogo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


            // PERMISOS / USUARIO ====================================================================================
            CreateMap<usuario, UsuarioDto>()
                .ForMember(dest => dest.rol, opt => opt.MapFrom(src => src.idRolNavigation))
                .ForMember(dest => dest.cargo, opt => opt.MapFrom(src => src.idCargoNavigation.nombreCargo))
                .ForMember(dest => dest.permisosMaquina, opt => opt.MapFrom(src => src.permisoMaquina))
                .ReverseMap();

            CreateMap<usuario, UsuarioDisenoDto>().ReverseMap();

            CreateMap<usuario, UsuarioDiseñoCargaDto>()
                .ForMember(dest => dest.cargo, opt => opt.MapFrom(src => src.idCargoNavigation.nombreCargo))
                .ReverseMap();

            CreateMap<usuario, OperadoresDto>()
                .ForMember(dest => dest.nombreArea, opt => opt.MapFrom(src => src.idAreaNavigation.nombreArea))
                .ForMember(dest => dest.maquinasAsignadas, opt => opt.MapFrom(src => src.permisoMaquina))
                .ReverseMap();

            CreateMap<usuario, UsuarioCortoDto>()
                .ForMember(dest => dest.nombreArea, opt => opt.MapFrom(src => src.idAreaNavigation.nombreArea))
                .ForMember(dest => dest.nombreRol, opt => opt.MapFrom(src => src.idRolNavigation.nombreRol))
                .ReverseMap();

            CreateMap<rol, RolDto>()
                .ForMember(dest => dest.permisos, opt => opt.MapFrom(src => src.permiso))
                .ForMember(dest => dest.permisoEspecificoDto, opt => opt.MapFrom(src => src.permisoEspecifico))
                .ReverseMap();

            CreateMap<rol, AddRolDto>()
                .ForMember(dest => dest.addPermisos, opt => opt.MapFrom(src => src.permiso))
                .ReverseMap();

            CreateMap<UpdateRolDto, rol>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<UpdateBatchRolDto, rol>().ReverseMap();

            CreateMap<permiso, PermisoDto>()
                .ForMember(dest => dest.subModulo, opt => opt.MapFrom(src => src.idSubModuloNavigation))
                .ReverseMap();
            CreateMap<permiso, AddPermisoDto>().ReverseMap();
            CreateMap<UpdatePermisoDto, permiso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBatchPermisosDto, permiso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<subModulo, SubModuloDto>()
                .ForMember(dest => dest.modulo, opt => opt.MapFrom(src => src.idModuloNavigation))
                .ForMember(dest => dest.permisoTipoDto, opt => opt.MapFrom(src => src.permisoTipo))
                .ReverseMap();

            CreateMap<permisoMaquina, PermisoMaquinaDto>()
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.maquinaNavigation.nombreMaquina))
                .ReverseMap();
            CreateMap<permisoMaquina, AddPermisoMaquinaDto>().ReverseMap();
            CreateMap<UpdatePermisoMaquinaDto, permisoMaquina>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBatchPermisoMaquina, permisoMaquina>().ReverseMap();

            CreateMap<permisoMaquina, PermisoMaquinaAsignadaDto>()
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.maquinaNavigation.nombreMaquina))
                .ReverseMap();

            CreateMap<modulo, ModuloDto>()
                //.ForMember(dest => dest.menu, opt => opt.MapFrom(src => src.idMenuNavigation))
                .ReverseMap();

            //CreateMap<menu, MenuDto>().ReverseMap();

            CreateMap<cargo, CargoDto>()
                .ForMember(dest => dest.usuarios, opt => opt.MapFrom(src => src.usuario))
                .ReverseMap();

            // ASIGNACION: PROCESO OF - USUARIO =======================================================================
            CreateMap<asignacion, AsignacionDto>()
                .ForMember(dest => dest.nombreUsuario, opt => opt.MapFrom(src => src.userNavigation.nombres + " " + src.userNavigation.apellidos))
                .ReverseMap();
            CreateMap<asignacion, AddAsignacionDto>().ReverseMap();
            CreateMap<UpdateAsignacionDto, asignacion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //CALIDAD ================================================================================================

            // CERTIFICADO DE CALIDAD =================================================================================
            CreateMap<certificadoCalidad, CertificadoCalidadDto>()
                .ForMember(dest => dest.detallesCertificadoCalidad, opt => opt.MapFrom(src => src.detalleCertificadoCalidad))
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.producto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.nombreElabora, opt => opt.MapFrom(src => src.elaboradoPorNavigation.nombres + " " + src.elaboradoPorNavigation.apellidos))
                .ReverseMap();
            CreateMap<certificadoCalidad, AddCertificadoCalidadDto>().ReverseMap();
            CreateMap<UpdateCertificadoCalidadDto, certificadoCalidad>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            {
                CreateMap<detalleCertificadoCalidad, DetalleCertificadoCalidadDto>()
                    .ForMember(d => d.nombre, opt => opt.MapFrom(s => s.idVariableNavigation.nombre))
                    .ForMember(d => d.etiqueta, opt => opt.MapFrom(s => s.idVariableNavigation.etiqueta))
                    .ForMember(d => d.tipoDato, opt => opt.MapFrom(s => s.idVariableNavigation.tipoDato))
                    .ForMember(d => d.tolerancia, opt => opt.MapFrom(s => s.idVariableNavigation.tolerancia))
                    .ForMember(d => d.simbolo, opt => opt.MapFrom(s => s.idUnidadNavigation.simbolo))
                    .ForMember(d => d.nombreUnidad, opt => opt.MapFrom(s => s.idUnidadNavigation.nombre))
                    .ReverseMap();
                CreateMap<detalleCertificadoCalidad, AddDetalleCertificadoCalidadDto>().ReverseMap();
                CreateMap<UpdateDetalleCertificadoCalidadDto, detalleCertificadoCalidad>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<detalleCertificadoCalidad, AddBatchDetalleCertificadoC>().ReverseMap();
                CreateMap<UpdateBatchDetalleCertificadoC, detalleCertificadoCalidad>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            // FICHA TECNICA DE CALIDAD ==============================================================================
            CreateMap<fichaTecnicaCliente, FichaTecnicaClienteDto>()
                .ForMember(dest => dest.detallesFichaTecnicaCliente, opt => opt.MapFrom(src => src.detalleFichaClientes))
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.producto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.nombreElabora, opt => opt.MapFrom(src => src.elaboradoPorNavigation.nombres + " " + src.elaboradoPorNavigation.apellidos))
                .ReverseMap();
            CreateMap<fichaTecnicaCliente, AddFichaTecnicaClienteDto>().ReverseMap();
            CreateMap<UpdateFichaTecnicaClienteDto, fichaTecnicaCliente>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            {
                CreateMap<detalleFichaClientes, DetalleFichaClientesDto>()
                    .ForMember(d => d.nombre, opt => opt.MapFrom(s => s.idVariableNavigation.nombre))
                    .ForMember(d => d.etiqueta, opt => opt.MapFrom(s => s.idVariableNavigation.etiqueta))
                    .ForMember(d => d.tipoDato, opt => opt.MapFrom(s => s.idVariableNavigation.tipoDato))
                    .ForMember(d => d.tolerancia, opt => opt.MapFrom(s => s.idVariableNavigation.tolerancia))
                    .ForMember(d => d.simbolo, opt => opt.MapFrom(s => s.idUnidadNavigation.simbolo))
                    .ForMember(d => d.nombreUnidad, opt => opt.MapFrom(s => s.idUnidadNavigation.nombre))
                    .ReverseMap();

                CreateMap<detalleFichaClientes, AddDetalleFichaClientesDto>().ReverseMap();
                CreateMap<UpdateDetalleFichaClientesDto, detalleFichaClientes>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<detalleFichaClientes, AddBatchDetalleFichaClientesDto>().ReverseMap();
                CreateMap<UpdateBatchDetalleFichaClientesDto, detalleFichaClientes>();
            }

            // FICHA TECNICA DE PROCESO ==================================================================================
            CreateMap<fichaTecnicaProcesos, FichaTecnicaProcesosDto>()
                .ForMember(dest => dest.detalleFichaProcesos, opt => opt.MapFrom(src => src.detalleFichaProcesos))
                .ForMember(dest => dest.formulacionTinta, opt => opt.MapFrom(src => src.formulacionTinta))
                .ForMember(dest => dest.nombreCliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.nombreProducto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.nombreOperador, opt => opt.MapFrom(src => src.operadorNavigation.nombres + " " + src.operadorNavigation.apellidos))
                .ForMember(dest => dest.nombreFormuladorTinta, opt => opt.MapFrom(src => src.formuladorTintaNavigation.nombres + " " + src.formuladorTintaNavigation.apellidos))
                .ForMember(dest => dest.secuenciaColor, opt => opt.MapFrom(src => src.secuenciaColor))
                .ForMember(dest => dest.registroLamparas, opt => opt.MapFrom(src => src.registroLamparas))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.estadoNavigation.nombreEstado))
                .ForMember(dest => dest.medicionAguas, opt => opt.MapFrom(src => src.medicionAguas))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.maquinaNavigation.nombreMaquina))
                .ReverseMap();
            CreateMap<fichaTecnicaProcesos, AddFichaTecnicaProcesosDto>().ReverseMap();
            CreateMap<UpdateFichaTecnicaProcesosDto, fichaTecnicaProcesos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleFichaProcesos, DetalleFichaProcesosDto>().ReverseMap();
            CreateMap<detalleFichaProcesos, AddDetalleFichaProcesosDto>().ReverseMap();
            CreateMap<UpdateDetalleFichaProcesosDto, detalleFichaProcesos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<detalleFichaProcesos, AddBatchDetalleFichaProcesos>().ReverseMap();
            CreateMap<UpdateBatchDetalleFichaProcesos, detalleFichaProcesos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<medicionAguas, MedicionAguasDto>().ReverseMap();
            CreateMap<medicionAguas, AddMedicionAguasDto>().ReverseMap();
            CreateMap<UpdateMedicionAguasDto, medicionAguas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<medicionAguas, AddBatchMedicionAguasDto>().ReverseMap();
            CreateMap<UpdateBatchMedicionAguasDto, medicionAguas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // =============================================================================================================

            CreateMap<variablesTecnicas, VariablesTecnicasDto>()
                .ForMember(dest => dest.variableUnidadMedidaDto, opt => opt.MapFrom(src => src.variableUnidadMedida))
                .ReverseMap();
            CreateMap<variablesTecnicas, VariablesTecnicasUMedidaDto>()
                .ForMember(dest => dest.variableUnidadMedidas, opt => opt.MapFrom(src => src.variableUnidadMedida))
                .ReverseMap();

            CreateMap<variablesTecnicas, AddVariablesTecnicasDto>().ReverseMap();
            CreateMap<UpdateVariablesTecnicasDto, variablesTecnicas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Variable de medidas / unidades medida
            CreateMap<unidadesMedida, UnidadesMedidaDto>().ReverseMap();
            CreateMap<unidadesMedida, AddUnidadesMedidaDto>().ReverseMap();
            CreateMap<UpdateUnidadesMedidaDto, unidadesMedida>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<variableUnidadMedida, VariableUnidadMedidaDto>()
                .ForMember(dest => dest.nombreUnidad, opt => opt.MapFrom(src => src.idUnidadNavigation.nombre))
                .ForMember(dest => dest.simbolo, opt => opt.MapFrom(src => src.idUnidadNavigation.simbolo))
                .ReverseMap();

            CreateMap<variableUnidadMedida, AddVariableUnidadMedidaDto>().ReverseMap();
            CreateMap<UpdateVariableUnidadMedidaDto, variableUnidadMedida>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // AUDITORIA PROCESO =======================================================================================
            CreateMap<auditoriaProceso, AuditoriaProcesoDto>()
                .ForMember(dest => dest.detalleAuditoriaProceso, opt => opt.MapFrom(src => src.detalleAuditoriaProceso))
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.productoOf, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.maquinaNavigation.nombreMaquina))
                .ForMember(dest => dest.nombreAuditor, opt => opt.MapFrom(src => src.auditorNavigation.nombres + " " + src.auditorNavigation.apellidos))
                .ForMember(dest => dest.nombreOperador, opt => opt.MapFrom(src => src.operadorNavigation.nombres + " " + src.operadorNavigation.apellidos))
                .ForMember(dest => dest.nombreSupervisor, opt => opt.MapFrom(src => src.supervisorNavigation.nombres + " " + src.supervisorNavigation.apellidos))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.estadoNavigation.nombreEstado))
                .ReverseMap();
            CreateMap<auditoriaProceso, AddAuditoriaProcesoDto>().ReverseMap();
            CreateMap<UpdateAuditoriaProcesoDto, auditoriaProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBatchAuditoriaProcesoDto, auditoriaProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleAuditoriaProceso, DetalleAuditoriaProcesoDto>().ReverseMap();
            CreateMap<detalleAuditoriaProceso, AddDetalleAuditoriaProcesoDto>().ReverseMap();
            CreateMap<UpdateDetalleAuditoriaProcesoDto, detalleAuditoriaProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<detalleAuditoriaProceso, AddBatchDetalleAuditoriaProceso>().ReverseMap();
            CreateMap<UpdateBatchDetalleAuditoriaProceso, detalleAuditoriaProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Turno de auditoria
            CreateMap<turnoAuditoria, TurnoAuditoriaDto>()
                .ForMember(dest => dest.nombreAuditor, opt => opt.MapFrom(src => src.auditorNavigation.nombres + " " + src.auditorNavigation.apellidos))
                .ForMember(dest => dest.nombreTurno, opt => opt.MapFrom(src => src.turnoNavigation.turno))
                .ForMember(dest => dest.nombreAprobador, opt => opt.MapFrom(src => src.aprobadoPorNavigation.nombres + " " + src.aprobadoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.estadoNavigation.nombreEstado))
                .ReverseMap();
            CreateMap<turnoAuditoria, AddTurnoAuditoriaDto>().ReverseMap();
            CreateMap<UpdateTurnoAuditoriaDto, turnoAuditoria>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBatchTurnoAuditoriaDto, turnoAuditoria>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Secuencia de color
            CreateMap<secuenciaColor, SecuenciaColorDto>().ReverseMap();
            CreateMap<secuenciaColor, AddSecuenciaColorDto>().ReverseMap();
            CreateMap<UpdateSecuenciaColorDto, secuenciaColor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<secuenciaColor, AddBatchSecuenciaColor>().ReverseMap();
            CreateMap<UpdateBatchSecuenciaColor, secuenciaColor>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Registro de lámparas
            CreateMap<registroLamparas, RegistroLamparasDto>()
                .ForMember(dest => dest.nombreVariable, opt => opt.MapFrom(src => src.idVariableNavigation.nombre))
                .ReverseMap();
            CreateMap<registroLamparas, AddRegistroLamparasDto>().ReverseMap();
            CreateMap<UpdateRegistroLamparasDto, registroLamparas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<registroLamparas, AddBatchRegistroLamparas>().ReverseMap();
            CreateMap<UpdateBatchRegistroLamparas, registroLamparas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // TINTAS ================================================================================================
            // formulacion de tintas
            CreateMap<formulacionTinta, FormulacionTintaDto>()
                .ForMember(dest => dest.especificacionTintas, opt => opt.MapFrom(src => src.especificacionTintas))
                .ReverseMap();
            CreateMap<formulacionTinta, AddFormulacionTintaDto>()
                .ForMember(dest => dest.especificacionTintas, opt => opt.MapFrom(src => src.especificacionTintas))
                .ReverseMap();
            CreateMap<UpdateFormulacionTintaDto, formulacionTinta>()
                .ForMember(dest => dest.especificacionTintas, opt => opt.Ignore())
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // especificacion de tintas
            CreateMap<especificacionTintas, EspecificacionTintasDto>().ReverseMap();
            CreateMap<especificacionTintas, AddEspecificacionTintasDto>().ReverseMap();
            CreateMap<UpdateEspecificacionTintasDto, especificacionTintas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ========================================================================================================

            // TABLERO / AREA / POSTURA ================================================================================
            // Tablero
            CreateMap<tablerosOf, TablerosOfDto>()
                .ForMember(dest => dest.posturasOfDto, opt => opt.MapFrom(src => src.posturasOf))
                .ForMember(dest => dest.idFamiliaMaquina, opt => opt.MapFrom(src => src.idMaquinaNavigation.idFamilia))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idMaquinaNavigation.nombreMaquina))
                .ForMember(dest => dest.nombreAlternoMaquina, opt => opt.MapFrom(src => src.idMaquinaNavigation.nombreAlterno))
                .ForMember(dest => dest.nombreArea, opt => opt.MapFrom(src => src.idAreaNavigation.nombreArea))
                .ReverseMap();
            CreateMap<tablerosOf, ProcesoTablerosOfDto>()
                .ForMember(dest => dest.AreasDto, opt => opt.MapFrom(src => src.idAreaNavigation))
                .ForMember(dest => dest.Maquina, opt => opt.MapFrom(src => src.idMaquinaNavigation))
                .ReverseMap();
            CreateMap<tablerosOf, AddTablerosOfDto>().ReverseMap();
            CreateMap<UpdateTablerosOfDto, tablerosOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //Postura
            CreateMap<posturasOf, PosturasOfDto>()
                .ForMember(dest => dest.tablerosOfDto, opt => opt.MapFrom(src => src.idTableroNavigation.nombreTablero))
                //.ForMember(dest => dest.procesosOf, opt => opt.MapFrom(src => src.procesoOf))
                .ReverseMap();
            CreateMap<posturasOf, ProcesoPosturasOfDto>().ReverseMap();
            CreateMap<posturasOf, AddPosturasOfDto>().ReverseMap();
            CreateMap<UpdatePosturasOfDto, posturasOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<UpdateBatchPosturasOf, posturasOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Area
            CreateMap<areas, AreasDto>().ReverseMap(); // <------ General
            CreateMap<areas, AreasTablerosDto>()
                .ForMember(dest => dest.tablerosOfDto, opt => opt.MapFrom(src => src.tablerosOf)) // <---- Para Tableros
                .ReverseMap();
            CreateMap<areas, AreasUsuariosDto>()
                .ForMember(dest => dest.usuarios, opt => opt.MapFrom(src => src.usuario)) // <---- Para Usuarios
                .ReverseMap();
            CreateMap<areas, AddAreasDto>().ReverseMap();
            CreateMap<UpdateAreasDto, areas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // SESION DE OPERADOR ======================================================================================
            CreateMap<sesionOperador, SesionOperadorDto>().ReverseMap();
            CreateMap<sesionOperador, AddSesionOperadorDto>().ReverseMap();
            CreateMap<UpdateSesionOperadorDto, sesionOperador>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<turnos, TurnosDto>().ReverseMap();
            CreateMap<turnos, AddTurnosDto>().ReverseMap();
            CreateMap<UpdateTurnosDto, turnos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            {
                CreateMap<pausasMaquina, PausasMaquinaDto>().ReverseMap();
                CreateMap<pausasMaquina, AddPausasMaquinaDto>().ReverseMap();
                CreateMap<UpdatePausasMaquinaDto, pausasMaquina>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            // PERMISOS ============================================================================================

            // PERMISOS TIPO
            CreateMap<permisoTipo, PermisoTipoDto>().ReverseMap();
            CreateMap<permisoTipo, AddPermisoTipoDto>().ReverseMap();
            CreateMap<UpdatePermisoTipoDto, permisoTipo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // PERMISOS ESPECIFICOS
            CreateMap<permisoEspecifico, PermisoEspecificoDto>()
                .ForMember(dest => dest.idSubmodulo, opt => opt.MapFrom(src => src.idPermisoTipoNavigation.idSubModulo))
                .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.idPermisoTipoNavigation.descripcion))
                .ForMember(dest => dest.clave, opt => opt.MapFrom(src => src.idPermisoTipoNavigation.clave))
                .ReverseMap();
            CreateMap<permisoEspecifico, AddPermisoEspecificoDto>().ReverseMap();
            CreateMap<UpdatePermisoEspecificoDto, permisoEspecifico>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<permisoEspecifico, AddBatchPermisoEspecifico>().ReverseMap();
            CreateMap<UpdateBatchPermisoEspecifico, permisoEspecifico>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // CORRIDAS COMBINADAS =====================================================================================
            CreateMap<corridaCombinada, CorridaCombinadaDto>()
                .ForMember(dest => dest.oF, opt => opt.MapFrom(src => src.subordinadoNavigation.oF))
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.subordinadoNavigation.oFNavigation.clienteOf))
                .ForMember(dest => dest.productoOf, opt => opt.MapFrom(src => src.subordinadoNavigation.productoOf))
                .ForMember(dest => dest.cantOf, opt => opt.MapFrom(src => src.subordinadoNavigation.oFNavigation.cantidadOf))
                .ForMember(dest => dest.fechaVencmiento, opt => opt.MapFrom(src => src.subordinadoNavigation.fechaVencimiento))
                .ForMember(dest => dest.fechaCreacionOf, opt => opt.MapFrom(src => src.subordinadoNavigation.oFNavigation.fechaCreacion))
                .ForMember(dest => dest.serie, opt => opt.MapFrom(src => src.subordinadoNavigation.oFNavigation.seriesOf))
                .ReverseMap();
            CreateMap<corridaCombinada, AddCorridaCombinadaDto>().ReverseMap();
            CreateMap<UpdateCorridaCombinadaDto, corridaCombinada>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<corridaCombinada, AddBatchCorridaCombinada>().ReverseMap();

            // ANEXOS NEXO =============================================================================================
            CreateMap<anexos_NEXO, anexos_NEXO_Dto>().ReverseMap();
            CreateMap<anexos_NEXO, AddAnexoNEXODto>().ReverseMap();
            CreateMap<UpdateAnexoNEXODto, anexos_NEXO>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // LISTA DE OPERACIONES ===================================================================================
            CreateMap<listaDeOperaciones, listaDeOperacionesDto>()
                .ForMember(dest => dest.items, opt => opt.MapFrom(src => src.listaItem))
                .ReverseMap();
            CreateMap<listaDeOperaciones, AddListaDeOperacionesDto>().ReverseMap();
            CreateMap<UpdateListaDeOperacionesDto, listaDeOperaciones>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            {
                CreateMap<listaItem, listaItemDto>()
                    .ForMember(dest => dest.prefijo, opt => opt.MapFrom(src => src.idOperacionNavigation.prefijo))
                    .ForMember(dest => dest.tipoOperacion, opt => opt.MapFrom(src => src.idOperacionNavigation.tipoOperacion))
                    .ForMember(dest => dest.nombreOperacion, opt => opt.MapFrom(src => src.idOperacionNavigation.nombreOperacion))
                    .ReverseMap();
                CreateMap<listaItem, AddListaItemDto>().ReverseMap();
                CreateMap<UpdateListaItemDto, listaItem>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<listaItem, BatchAddListaItemDto>().ReverseMap();
                CreateMap<BatchUpdateListaItemDto, listaItem>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<listaMaquina, listaMaquinaDto>()
                    .ForMember(dest => dest.listaDeOperacionesDto, opt => opt.MapFrom(src => src.idListaNavigation))
                    .ReverseMap();
                CreateMap<listaMaquina, listaMaquinaCatalogoDto>()
                    .ForMember(dest => dest.nombreLista, opt => opt.MapFrom(src => src.idListaNavigation.nombreLista))
                    .ReverseMap();

                CreateMap<listaMaquina, AddListaMaquinaDto>().ReverseMap();

                CreateMap<UpdateListaMaquinaDto, listaMaquina>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<listaMaquina, BatchAddListaMaquinaDto>().ReverseMap();

                CreateMap<BatchUpdateListaMaquinaDto, listaMaquina>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            // SOLICITUD DE MATERIALES ================================================================================
            CreateMap<solicitudMateriales, solicitudMaterialesDto>()
                .ForMember(dest => dest.solicitudMaterialOf, opt => opt.MapFrom(src => src.solicitudMaterialesOf))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idMaquinaNavigation.nombreMaquina))
                .ForMember(dest => dest.etiquetaSolicitudDto, opt => opt.MapFrom(src => src.etiquetaSolicitud))
                .ReverseMap();
            CreateMap<solicitudMateriales, AddSolicitudMaterialesDto>().ReverseMap();
            CreateMap<UpdateSolicitudMaterialesDto, solicitudMateriales>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<solicitudMateriales, UpdateBatchPosicionSMDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<solicitudMateriales, solicitudMaterialesProcesoOfDto>()
                .ForMember(dest => dest.solicitudMaterialOf, opt => opt.MapFrom(src => src.solicitudMaterialesOf))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idMaquinaNavigation.nombreMaquina))
                .ReverseMap();

            CreateMap<solicitudMaterialesOf, solicitudMaterialesOfDto>()
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.descripcionOf, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.cantidadOf, opt => opt.MapFrom(src => src.oFNavigation.cantidadOf))
                .ForMember(dest => dest.fechaEntrega, opt => opt.MapFrom(src => src.oFNavigation.fechaVencimiento))
                .ReverseMap();
            CreateMap<solicitudMaterialesOf, AddSolicitudMaterialesOfDto>().ReverseMap();
            CreateMap<UpdateSolicitudMaterialesOfDto, solicitudMaterialesOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<etiquetaSolicitud, EtiquetaSolicitudDto>()
                .ForMember(dest => dest.color, opt => opt.MapFrom(src => src.idEtiquetaNavigation.color))
                .ForMember(dest => dest.texto, opt => opt.MapFrom(src => src.idEtiquetaNavigation.texto))
                .ReverseMap();
            CreateMap<etiquetaSolicitud, AddEtiquetaSolicitudDto>().ReverseMap();
            CreateMap<UpdateEtiquetaSolicitudDto, etiquetaSolicitud>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<etiquetaSolicitud, AddBatchEtiquetaSolicitudDto>().ReverseMap();
            CreateMap<UpdateBatchEtiquetaSolicitudDto, etiquetaSolicitud>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<lotePliego, lotePliegoDto>()
                .ForMember(dest => dest.nombreUnidad, opt => opt.MapFrom(src => src.unidadMedidaNavigation.nombre))
                .ForMember(dest => dest.simboloUnidad, opt => opt.MapFrom(src => src.unidadMedidaNavigation.simbolo))
                .ReverseMap();

            CreateMap<lotePliego, AddLotePliegoDto>().ReverseMap();
            CreateMap<UpdateLotePliegoDto, lotePliego>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<transferenciaProceso, transferenciaProcesoDto>()
                .ForMember(dest => dest.nombreTablero, opt => opt.MapFrom(src => src.idOrigenNavigation.idTableroNavigation.nombreTablero))
                .ForMember(dest => dest.nombreEnviadoPor, opt => opt.MapFrom(src => src.enviadoPorNavigation.nombres + " " + src.enviadoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreRecibidoPor, opt => opt.MapFrom(src => src.recibidoPorNavigation.nombres + " " + src.recibidoPorNavigation.apellidos))
                .ReverseMap();
            CreateMap<transferenciaProceso, AddTransferenciaProcesoDto>().ReverseMap();
            CreateMap<UpdateTransferenciaProcesoDto, transferenciaProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<valeBobina, ValeBobinaDto>()
                .ForMember(dest => dest.descripcionMaterial, opt => opt.MapFrom(src => src.idMaterialNavigation.nombreMaterial))
                .ForMember(dest => dest.proveedorMaterial, opt => opt.MapFrom(src => src.idMaterialNavigation.marca))
                .ForMember(dest => dest.anchoMaterial, opt => opt.MapFrom(src => src.idMaterialNavigation.ancho))
                .ForMember(dest => dest.calibreMaterial, opt => opt.MapFrom(src => src.idMaterialNavigation.calibre))
                .ForMember(dest => dest.gramajeMaterial, opt => opt.MapFrom(src => src.idMaterialNavigation.gramaje))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.estadoNavigation.nombreEstado))
                .ReverseMap();

            CreateMap<valeBobina, AddValeBobinaDto>().ReverseMap();
            CreateMap<UpdateValeBobinaDto, valeBobina>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<valeBobinaCorteEstado, ValeBobinaCorteEstadoDto>().ReverseMap();
            CreateMap<valeBobinaCorteEstado, AddValeBobinaCorteEstadoDto>().ReverseMap();
            CreateMap<UpdateValeBobinaCorteEstadoDto, valeBobinaCorteEstado>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<bobinasAsignadas, BobinasAsignadasDto>()
                .ReverseMap();
            CreateMap<bobinasAsignadas, AddBobinasAsignadasDto>().ReverseMap();
            CreateMap<UpdateBobinasAsignadasDto, bobinasAsignadas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<bobinasAsignadas, AddBatchBobinasAsignadasDto>().ReverseMap();

            CreateMap<tipoComponente, TipoComponenteDto>().ReverseMap();
            CreateMap<tipoComponente, AddTipoComponenteDto>().ReverseMap();
            CreateMap<UpdateTipoComponenteDto, tipoComponente>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<componenteProduccion, ComponenteProduccionDto>()
                .ForMember(dest => dest.descripcion, opt => opt.MapFrom(src => src.tipoComponenteNavigation.descripcion))
                .ForMember(dest => dest.codigo, opt => opt.MapFrom(src => src.tipoComponenteNavigation.codigo))
                .ReverseMap();
            CreateMap<componenteProduccion, AddComponenteProduccionDto>().ReverseMap();
            CreateMap<UpdateComponenteProduccionDto, componenteProduccion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<componenteProduccion, AddBatchComponenteProd>().ReverseMap();
            CreateMap<UpdateBatchComponenteProd, componenteProduccion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // HORARIOS OPERATIVOS ====================================================================================

            CreateMap<horariosOperativos, HorariosOperativosDto>()
                .ForMember(dest => dest.nombreArea, opt => opt.MapFrom(src => src.idAreaNavigation.nombreArea))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idMaquinaNavigation.nombreMaquina))
                .ForMember(dest => dest.nombreOperador, opt => opt.MapFrom(src => src.operadorNavigation.nombres + " " + src.operadorNavigation.apellidos))
                .ReverseMap();
            CreateMap<horariosOperativos, AddHorariosOperativosDto>().ReverseMap();
            CreateMap<UpdateHorariosOperativosDto, horariosOperativos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<horariosOperativos, AddBatchHorariosOperativosDto>().ReverseMap();
            CreateMap<UpdateBatchHorariosOperativosDto, horariosOperativos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Turnos Operativos Area
            CreateMap<turnosOperativos, TurnosOperativosDto>().ReverseMap();
            CreateMap<turnosOperativos, AddTurnosOperativosDto>().ReverseMap();
            CreateMap<UpdateTurnosOperativosDto, turnosOperativos>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<turnosOperativosArea, TurnosOperativosAreaDto>()
                .ForMember(dest => dest.nombreTurno, opt => opt.MapFrom(src => src.idTurnoNavigation.turno))
                .ForMember(dest => dest.horaInicio, opt => opt.MapFrom(src => src.idTurnoNavigation.horaInicio))
                .ForMember(dest => dest.horaFin, opt => opt.MapFrom(src => src.idTurnoNavigation.horaFin))
                .ReverseMap();

            CreateMap<turnosOperativosArea, AddTurnosOperativosAreaDto>().ReverseMap();
            CreateMap<UpdateTurnosOperativosAreaDto, turnosOperativosArea>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // INDISPONIBILIDAD MAQUINAS ============================================================================

            CreateMap<indisponibilidadMaquinas, IndisponibilidadMaquinasDto>()
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idMaquinaNavigation.nombreMaquina))
                .ReverseMap();

            CreateMap<indisponibilidadMaquinas, AddIndisponibilidadMaquinasDto>().ReverseMap();

            CreateMap<UpdateIndisponibilidadMaquinasDto, indisponibilidadMaquinas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // TIEMPOS ESTIMADOS =========================================================================================
            // OF
            CreateMap<ffeTiemposOfGlobal, ffeTiemposOfDto>()
                .ReverseMap();

            // PROCESOS
            CreateMap<ffeTiemposProcesosGlobal, ffeTiemposProcesosDto>()
                .ReverseMap();

            // SOPORTE NEXO =========================================================================================
            CreateMap<logSoporteNexo, LogSoporteNexoDto>().ReverseMap();

            // BUSCADOR GLOBAL =========================================================================================
            CreateMap<tarjetaOf, SB_TarjetaOfDto>().ReverseMap();

            CreateMap<procesoOf, SB_ProcesoOfDto>()
                .ForMember(dest => dest.nombreTablero, opt => opt.MapFrom(src => src.idTableroNavigation.nombreTablero))
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.productoOf, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.oV, opt => opt.MapFrom(src => src.oFNavigation.oV))
                .ReverseMap();

            CreateMap<entregasProductoTerminado, SB_ProductoTerminadoDto>()
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.ofNavigation.clienteOf))
                .ForMember(dest => dest.productoOf, opt => opt.MapFrom(src => src.ofNavigation.productoOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.ofNavigation.codArticulo))
                .ForMember(dest => dest.oV, opt => opt.MapFrom(src => src.ofNavigation.oV))
                .ReverseMap();

            CreateMap<solicitudMaterialesOf, SB_SolicitudMaterialesOfDto>()
               .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
               .ForMember(dest => dest.productoOf, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
               .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
               .ForMember(dest => dest.materialDescripcion, opt => opt.MapFrom(src => src.idSolicitudNavigation.materialDescripcion))
               .ForMember(dest => dest.oV, opt => opt.MapFrom(src => src.oFNavigation.oV))
               .ReverseMap();

            CreateMap<fichaTecnicaCliente, SB_FichaTecnicaClienteDto>()
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                .ForMember(dest => dest.producto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ReverseMap();

            CreateMap<valeBobina, SB_ValeBobinaDto>()
                .ForMember(dest => dest.descripcionMaterial, opt => opt.MapFrom(src => src.idMaterialNavigation.nombreMaterial))
                .ForMember(dest => dest.proveedorMaterial, opt => opt.MapFrom(src => src.idMaterialNavigation.marca))
                .ReverseMap();

            CreateMap<fichaTecnicaProcesos, SB_FichaTecnicaProcesosDto>()
                .ForMember(dest => dest.nombreCliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ForMember(dest => dest.nombreProducto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.maquinaNavigation.nombreMaquina))
                .ReverseMap();

            CreateMap<certificadoCalidad, SB_CertificadoCalidadDto>()
                .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.oFNavigation.vendedorOf))
                .ForMember(dest => dest.producto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.codArticulo, opt => opt.MapFrom(src => src.oFNavigation.codArticulo))
                .ReverseMap();

            // VALIDACION DE ARRANQUE =========================================================================================
            CreateMap<validacionArranque, ValidacionArranqueDto>()
                .ForMember(dest => dest.detalleValidacionArranque, opt => opt.MapFrom(src => src.detalleValidacionArranque))
                .ReverseMap();
            CreateMap<validacionArranque, AddValidacionArranqueDto>().ReverseMap();
            CreateMap<UpdateValidacionArranqueDto, validacionArranque>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleValidacionArranque, DetalleValidacionArranqueDto>().ReverseMap();
            CreateMap<detalleValidacionArranque, AddDetalleValidacionArranqueDto>().ReverseMap();
            CreateMap<UpdateDetalleValidacionArranqueDto, detalleValidacionArranque>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            CreateMap<detalleValidacionArranque, AddBatchDetalleVArranqueDto>().ReverseMap();
            CreateMap<UpdateBatchDetalleVArranqueDto, detalleValidacionArranque>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // condicion Inicial =============================================================================================
            CreateMap<condicionInicial, CondicionInicialDto>()
                .ForMember(dest => dest.detalleCondicionInicial, opt => opt.MapFrom(src => src.detalleCondicionInicial))
                .ReverseMap();
            CreateMap<condicionInicial, AddCondicionInicialDto>().ReverseMap();
            CreateMap<UpdateCondicionInicialDto, condicionInicial>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            {
                // detalle condicion inicial
                CreateMap<detalleCondicionInicial, DetalleCondicionInicialDto>()
                    .ForMember(dest => dest.etiqueta, etiqueta => etiqueta.MapFrom(src => src.idVariableNavigation.etiqueta))
                    .ReverseMap();
                CreateMap<detalleCondicionInicial, AddDetalleCondicionInicialDto>().ReverseMap();
                CreateMap<UpdateDetalleCondicionInicialDto, detalleCondicionInicial>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<detalleCondicionInicial, AddBatchDetalleCondicionInicialDto>().ReverseMap();
                CreateMap<UpdateBatchDetalleCondicionInicialDto, detalleCondicionInicial>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            // Log programacion
            CreateMap<logProgramacion, LogProgramacionDto>()
                .ForMember(dest => dest.logProgramacionDetalle, opt => opt.MapFrom(src => src.logProgramacionDetalle))
                .ForMember(dest => dest.nombreTablero, opt => opt.MapFrom(src => src.tableroNavigation.nombreTablero))
                .ForMember(dest => dest.nombreProgramadoPor, opt => opt.MapFrom(src => src.programadoPorNavigation.nombres + " " + src.programadoPorNavigation.apellidos))
                .ReverseMap();
            CreateMap<logProgramacion, AddLogProgramacionDto>().ReverseMap();
            CreateMap<UpdateLogProgramacionDto, logProgramacion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            {
                // detalle log programacion
                CreateMap<logProgramacionDetalle, LogProgramacionDetalleDto>()
                    .ForMember(dest => dest.cliente, opt => opt.MapFrom(src => src.idProcesoNavigation != null && src.idProcesoNavigation.oFNavigation != null ? src.idProcesoNavigation.oFNavigation.clienteOf : null))
                    .ForMember(dest => dest.articulo, opt => opt.MapFrom(src => src.idProcesoNavigation != null && src.idProcesoNavigation.oFNavigation != null ? src.idProcesoNavigation.oFNavigation.productoOf : null))
                    .ForMember(dest => dest.oF, opt => opt.MapFrom(src => src.idProcesoNavigation != null ? src.idProcesoNavigation.oF : null))
                    .ForMember(dest => dest.vendedor, opt => opt.MapFrom(src => src.idProcesoNavigation != null && src.idProcesoNavigation.oFNavigation != null ? src.idProcesoNavigation.oFNavigation.vendedorOf : null))
                    .ForMember(dest => dest.fechaVenceOf, opt => opt.MapFrom(src => src.idProcesoNavigation != null && src.idProcesoNavigation.oFNavigation != null ? src.idProcesoNavigation.oFNavigation.fechaVencimiento : null))
                    .ForMember(dest => dest.serieOf, opt => opt.MapFrom(src => src.idProcesoNavigation != null && src.idProcesoNavigation.oFNavigation != null ? src.idProcesoNavigation.oFNavigation.seriesOf : null))
                    .ForMember(dest => dest.nombreEstadoAnterior, opt => opt.MapFrom(src => src.estadoAnteriorNavigation != null ? src.estadoAnteriorNavigation.nombrePostura : null))
                    .ForMember(dest => dest.nombreEstadoNuevo, opt => opt.MapFrom(src => src.estadoNuevoNavigation != null ? src.estadoNuevoNavigation.nombrePostura : null))
                    .ForMember(dest => dest.indicador, opt => opt.MapFrom(src => src.idProcesoNavigation != null ? src.idProcesoNavigation.indicador : null))
                    .ForMember(dest => dest.corridaCombinada, opt => opt.MapFrom(src => src.idProcesoNavigation != null ? src.idProcesoNavigation.corridaCombinada : null))
                    .ForMember(dest => dest.tiroRetiro, opt => opt.MapFrom(src => src.idProcesoNavigation != null ? src.idProcesoNavigation.tiroRetiro : null))
                    .ForMember(dest => dest.indicadorProceso, opt => opt.MapFrom(src => src.idProcesoNavigation != null ? src.idProcesoNavigation.indicadorProceso : null))
                    .ForMember(dest => dest.reproceso, opt => opt.MapFrom(src => src.idProcesoNavigation != null ? src.idProcesoNavigation.reproceso : null))
                    .ForMember(dest => dest.correlativoCC, opt => opt.MapFrom(src => src.idProcesoNavigation != null ? src.idProcesoNavigation.correlativoCC : null));

                CreateMap<logProgramacionDetalle, AddLogProgramacionDetalleDto>().ReverseMap();
                CreateMap<UpdateLogProgramacionDetalleDto, logProgramacionDetalle>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
                CreateMap<logProgramacionDetalle, AddBatchLogProgramacionDetalleDto>().ReverseMap();
                CreateMap<logProgramacionDetalle, UpdateBatchLogProgramacionDetalleDto>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
            }

            // bitacoraCaso ==============================================================================================
            CreateMap<bitacoraCaso, BitacoraCasoDto>()
                .ForMember(dest => dest.nombreTipoEvento, opt => opt.MapFrom(src => src.idTipoEventoNavigation != null ? src.idTipoEventoNavigation.nombre : null))
                .ForMember(dest => dest.nombreEstadoAnterior, opt => opt.MapFrom(src => src.estadoAnteriorNavigation != null ? src.estadoAnteriorNavigation.nombreEstado : null))
                .ForMember(dest => dest.nombreEstadoNuevo, opt => opt.MapFrom(src => src.estadoNuevoNavigation != null ? src.estadoNuevoNavigation.nombreEstado : null))
                .ForMember(dest => dest.nombreDictamen, opt => opt.MapFrom(src => src.idDictamenNavigation != null ? src.idDictamenNavigation.nombre : null))
                .ForMember(dest => dest.nombreAnexo, opt => opt.MapFrom(src => src.idAnexoNavigation != null ? src.idAnexoNavigation.NombreArchivo : null))
                .ForMember(dest => dest.rutaAnexo, opt => opt.MapFrom(src => src.idAnexoNavigation != null ? src.idAnexoNavigation.RutaArchivo : null))
                .ForMember(dest => dest.tipoAnexo, opt => opt.MapFrom(src => src.idAnexoNavigation != null ? src.idAnexoNavigation.TipoEntidad : null))
                //.ForMember(dest => dest.nombreUsuario, opt => opt.MapFrom(src => src.usuarioNavigation != null ? src.usuarioNavigation.nombres + " " + src.usuarioNavigation.apellidos : null))
                .ReverseMap();

            CreateMap<bitacoraCaso, AddBitacoraCasoDto>().ReverseMap();
            CreateMap<UpdateBitacoraCasoDto, bitacoraCaso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // casoAccionSolicitada ========================================================================================
            CreateMap<casoAccionSolicitada, CasoAccionSolicitadaDto>()
                .ForMember(dest => dest.nombreAccion, opt => opt.MapFrom(src => src.idAccionNavigation != null ? src.idAccionNavigation.nombre : null))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.idEstadoNavigation != null ? src.idEstadoNavigation.nombreEstado : null))
                //.ForMember(dest => dest.colorEstado, opt => opt.MapFrom(src => src.idEstadoNavigation != null ? src.idEstadoNavigation.color : null))
                //.ForMember(dest => dest.nombreActualizadoPor, opt => opt.MapFrom(src => src.actualizadoPorNavigation != null ? src.actualizadoPorNavigation.nombres + " " + src.actualizadoPorNavigation.apellidos : null))
                .ReverseMap();

            CreateMap<casoAccionSolicitada, AddCasoAccionSolicitadaDto>().ReverseMap();
            CreateMap<UpdateCasoAccionSolicitadaDto, casoAccionSolicitada>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // casoCalidad ============================================================================================
            CreateMap<casoCalidad, CasoCalidadDto>()
                .ForMember(dest => dest.nombreTipoCaso, opt => opt.MapFrom(src => src.idTipoCasoNavigation.nombre))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.idEstadoNavigation.nombreEstado))
                .ForMember(dest => dest.nombreSeveridad, opt => opt.MapFrom(src => src.idSeveridadNavigation.nombre))
                .ForMember(dest => dest.nombreCategoria, opt => opt.MapFrom(src => src.idCategoriaDefectoNavigation.nombre))
                .ForMember(dest => dest.nombreSubtipo, opt => opt.MapFrom(src => src.idSubtipoDefectoNavigation.nombre))
                .ForMember(dest => dest.nombreCausaRaiz, opt => opt.MapFrom(src => src.idCausaRaizNavigation.nombre))
                .ForMember(dest => dest.nombreResolucion, opt => opt.MapFrom(src => src.idResolucionNavigation.nombre))
                .ForMember(dest => dest.nombreAreaResponsable, opt => opt.MapFrom(src => src.areaResponsableNavigation.nombreArea))

                // OF
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.productoOf, opt => opt.MapFrom(src => src.oFNavigation.productoOf))

                // ProcesoOf
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idProcesoNavigation.idTableroNavigation.idMaquinaNavigation.nombreMaquina))
                //.ForMember(dest => dest.proceso, opt => opt.MapFrom(src => src.idProcesoNavigation.proceso))

                //.ForMember(dest => dest.nombreOperador, opt => opt.MapFrom(src => src.)
                .ForMember(dest => dest.nombreRegistradoPor, opt => opt.MapFrom(src => src.registradoPorNavigation.nombres + " " + src.registradoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreResponsable, opt => opt.MapFrom(src => src.responsableNavigation.nombres + " " + src.responsableNavigation.apellidos))
                .ForMember(dest => dest.nombreActualizadoPor, opt => opt.MapFrom(src => src.actualizadoPorNavigation.nombres + " " + src.actualizadoPorNavigation.apellidos))
                .ForMember(dest => dest.bitacoraCaso, opt => opt.MapFrom(src => src.bitacoraCaso))
                .ForMember(dest => dest.accionesSolicitadas, opt => opt.MapFrom(src => src.casoAccionSolicitada))
                .ReverseMap();

            CreateMap<casoCalidad, CasoCalidadListaDTO>()
                .ForMember(dest => dest.nombreTipoCaso, opt => opt.MapFrom(src => src.idTipoCasoNavigation.nombre))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.idEstadoNavigation.nombreEstado))
                .ForMember(dest => dest.nombreSeveridad, opt => opt.MapFrom(src => src.idSeveridadNavigation.nombre))
                .ForMember(dest => dest.nombreCategoria, opt => opt.MapFrom(src => src.idCategoriaDefectoNavigation.nombre))
                .ForMember(dest => dest.nombreSubtipo, opt => opt.MapFrom(src => src.idSubtipoDefectoNavigation.nombre))
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.productoOf, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idProcesoNavigation.idTableroNavigation.idMaquinaNavigation.nombreMaquina))
                .ForMember(dest => dest.nombreRegistradoPor, opt => opt.MapFrom(src => src.registradoPorNavigation.nombres + " " + src.registradoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreResponsable, opt => opt.MapFrom(src => src.responsableNavigation.nombres + " " + src.responsableNavigation.apellidos))
                .ForMember(dest => dest.nombreActualizadoPor, opt => opt.MapFrom(src => src.actualizadoPorNavigation.nombres + " " + src.actualizadoPorNavigation.apellidos))
                .ForMember(dest => dest.totalEventos, opt => opt.Ignore())
                .ForMember(dest => dest.totalAcciones, opt => opt.Ignore());

            CreateMap<casoCalidad, AddCasoCalidadDto>().ReverseMap();
            CreateMap<UpdateCasoCalidadDto, casoCalidad>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // tipoEventoBitacora ============================================================================================
            CreateMap<tipoEventoBitacora, TipoEventoBitacoraDto>().ReverseMap();
            CreateMap<tipoEventoBitacora, AddTipoEventoBitacoraDto>().ReverseMap();
            CreateMap<UpdateTipoEventoBitacoraDto, tipoEventoBitacora>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // SubtipoDefecto ============================================================================================
            CreateMap<subtipoDefecto, SubtipoDefectoDto>()
                .ReverseMap();

            CreateMap<subtipoDefecto, AddSubtipoDefectoDto>().ReverseMap();
            CreateMap<UpdateSubtipoDefectoDto, subtipoDefecto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // categoriaDefecto ============================================================================================
            CreateMap<categoriaDefecto, CategoriaDefectoDto>()
                .ForMember(dest => dest.subtipoDefecto, opt => opt.MapFrom(src => src.subtipoDefecto))
                .ReverseMap();

            CreateMap<categoriaDefecto, AddCategoriaDefectoDto>().ReverseMap();
            CreateMap<UpdateCategoriaDefectoDto, categoriaDefecto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // severidadCaso ============================================================================================
            CreateMap<severidadCaso, SeveridadCasoDto>()
                .ReverseMap();
            CreateMap<severidadCaso, AddSeveridadCasoDto>().ReverseMap();
            CreateMap<UpdateSeveridadCasoDto, severidadCaso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // accionSolicitada ============================================================================================
            CreateMap<accionSolicitada, AccionSolicitadaDto>()
                .ReverseMap();
            CreateMap<accionSolicitada, AddAccionSolicitadaDto>().ReverseMap();
            CreateMap<UpdateAccionSolicitadaDto, accionSolicitada>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // tipoCaso ===================================================================================================
            CreateMap<tipoCaso, TipoCasoDto>()
                .ReverseMap();
            CreateMap<tipoCaso, AddTipoCasoDto>().ReverseMap();
            CreateMap<UpdateTipoCasoDto, tipoCaso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Confirmacion Preliminar ============================================================================================
            CreateMap<confirmacionPreliminar, ConfirmacionPreliminarDto>()
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.nombreProducto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.serieOf, opt => opt.MapFrom(src => src.oFNavigation.seriesOf))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.idEstadoNavigation.nombreEstado))
                .ForMember(dest => dest.nombreTurno, opt => opt.MapFrom(src => src.idTurnoNavigation.turno))
                .ForMember(dest => dest.nombreUnidad, opt => opt.MapFrom(src => src.idUnidadNavigation.nombre))
                .ForMember(dest => dest.simboloUnidad, opt => opt.MapFrom(src => src.idUnidadNavigation.simbolo))  
                .ForMember(dest => dest.idMaquina, opt => opt.MapFrom(src => src.idProcesoNavigation.idTableroNavigation.idMaquinaNavigation.idMaquina))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idProcesoNavigation.idTableroNavigation.idMaquinaNavigation.nombreMaquina))
                .ForMember(dest => dest.nombreOperador, opt => opt.MapFrom(src => src.operadorNavigation.nombres + " " + src.operadorNavigation.apellidos))
                .ForMember(dest => dest.nombreEntregadoPor, opt => opt.MapFrom(src => src.entregadoPorNavigation.nombres + " " + src.entregadoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreRegistradoPor, opt => opt.MapFrom(src => src.registradoPorNavigation.nombres + " " + src.registradoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreActualizadoPor, opt => opt.MapFrom(src => src.actualizadoPorNavigation.nombres + " " + src.actualizadoPorNavigation.apellidos))
                .ForMember(dest => dest.transferenciaProcesos, opt => opt.MapFrom(src => src.transferenciaProceso))
                .ReverseMap();

            CreateMap<confirmacionPreliminar, ConfirmacionPreliminarListaDTO>()
                .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
                .ForMember(dest => dest.nombreProducto, opt => opt.MapFrom(src => src.oFNavigation.productoOf))
                .ForMember(dest => dest.serieOf, opt => opt.MapFrom(src => src.oFNavigation.seriesOf))
                .ForMember(dest => dest.idMaquina, opt => opt.MapFrom(src => src.idProcesoNavigation.idTableroNavigation.idMaquinaNavigation.idMaquina))
                .ForMember(dest => dest.nombreMaquina, opt => opt.MapFrom(src => src.idProcesoNavigation.idTableroNavigation.idMaquinaNavigation.nombreCorto))
                .ForMember(dest => dest.nombreUnidad, opt => opt.MapFrom(src => src.idUnidadNavigation.nombre))
                .ForMember(dest => dest.simboloUnidad, opt => opt.MapFrom(src => src.idUnidadNavigation.simbolo))
                .ForMember(dest => dest.nombreTurno, opt => opt.MapFrom(src => src.idTurnoNavigation.turno))
                .ForMember(dest => dest.nombreEstado, opt => opt.MapFrom(src => src.idEstadoNavigation.nombreEstado))
                .ForMember(dest => dest.nombreEntregadoPor, opt => opt.MapFrom(src => src.entregadoPorNavigation.nombres + " " + src.entregadoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreOperador, opt => opt.MapFrom(src => src.operadorNavigation.nombres + " " + src.operadorNavigation.apellidos))
                .ForMember(dest => dest.nombreRegistradoPor, opt => opt.MapFrom(src => src.registradoPorNavigation.nombres + " " + src.registradoPorNavigation.apellidos))
                .ForMember(dest => dest.nombreActualizadoPor, opt => opt.MapFrom(src => src.actualizadoPorNavigation.nombres + " " + src.actualizadoPorNavigation.apellidos))
                .ForMember(dest => dest.totalTransferencias, opt => opt.Ignore())
                .ForMember(dest => dest.tieneConciliacion, opt => opt.MapFrom(src => src.conciliacion != null))
                .ForMember(dest => dest.cantidadEnviada, opt => opt.Ignore())
                .ForMember(dest => dest.cantidadConfirmada, opt => opt.Ignore())
                .ForMember(dest => dest.saldo, opt => opt.Ignore());

            CreateMap<confirmacionPreliminar, AddConfirmacionPreliminarDto>().ReverseMap();
            CreateMap<UpdateConfirmacionPreliminarDto, confirmacionPreliminar>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // ConciliacionDto ============================================================================================
            CreateMap<conciliacion, ConciliacionDto>()
                .ForMember(dest => dest.cantidadRecibida, opt => opt.MapFrom(src => src.idPreliminarNavigation.cantidadRecibida))
                .ForMember(dest => dest.idProceso, opt => opt.MapFrom(src => src.idPreliminarNavigation.idProceso))
                .ForMember(dest => dest.maquina, opt => opt.MapFrom(src => src.idPreliminarNavigation.idProcesoNavigation.idTableroNavigation.idMaquinaNavigation.nombreMaquina))
                .ForMember(dest => dest.oF, opt => opt.MapFrom(src => src.idPreliminarNavigation.idProcesoNavigation.oF))
                .ForMember(dest => dest.operador, opt => opt.MapFrom(src => src.idPreliminarNavigation.operador))
                .ForMember(dest => dest.nombreOperador, opt => opt.MapFrom(src => src.idPreliminarNavigation.operadorNavigation.nombres + " " + src.idPreliminarNavigation.operadorNavigation.apellidos))
                .ForMember(dest => dest.motivoConciliacion, opt => opt.MapFrom(src => src.idMotivoNavigation))
                .ForMember(dest => dest.decisionConciliacion, opt => opt.MapFrom(src => src.idDecisionNavigation))
                .ForMember(dest => dest.nombreResponsable, opt => opt.MapFrom(src => src.responsableNavigation.nombres + " " + src.responsableNavigation.apellidos))
                .ForMember(dest => dest.confirmacionPreliminar, opt => opt.MapFrom(src => src.idPreliminarNavigation))
                .ReverseMap();

            CreateMap<conciliacion, ConciliacionResumenDTO>()
                .ForMember(dest => dest.nombreResponsable, opt => opt.MapFrom(src =>
                    src.responsableNavigation.nombres + " " + src.responsableNavigation.apellidos))
                .ForMember(dest => dest.nombreMotivo, opt => opt.MapFrom(src => src.idMotivoNavigation.nombre))
                .ForMember(dest => dest.nombreDecision, opt => opt.MapFrom(src => src.idDecisionNavigation.nombre));

            CreateMap<conciliacion, AddConciliacionDto>().ReverseMap();
            CreateMap<UpdateConciliacionDto, conciliacion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            //motivo conciliacion
            CreateMap<motivoConciliacion, MotivoConciliacionDto>().ReverseMap();

            // decision conciliacion
            CreateMap<decisionConciliacion, DecisionConciliacionDto>().ReverseMap();

            // HistorialVencimientoOf ============================================================================================
            CreateMap<historialVencimientoOf, HistorialVencimientoOfDto>()
                .ForMember(dest => dest.nombreRegistradoPor, opt => opt.MapFrom(src => src.registradoPorNavigation.nombres + " " + src.registradoPorNavigation.apellidos))
                .ReverseMap();
            CreateMap<historialVencimientoOf, AddHistorialVencimientoOfDto>().ReverseMap();
            CreateMap<UpdateHistorialVencimientoOfDto, historialVencimientoOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // Dictamen Calidad
            CreateMap<dictamenCalidad, DictamenCalidadDto>().ReverseMap();

            // Resolucion Calidad
            CreateMap<resolucionCalidad, ResolucionCalidadDto>().ReverseMap();

            // Causa Raiz Calidad
            CreateMap<causaRaizCalidad, CausaRaizCalidadDto>().ReverseMap();
        }
    }
}
