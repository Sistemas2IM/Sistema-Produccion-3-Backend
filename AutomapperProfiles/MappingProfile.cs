using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.AnexosNEXO;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad;
using Sistema_Produccion_3_Backend.DTO.Calidad.CertificadoCalidad.DetalleCertificadoCalidad.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes;
using Sistema_Produccion_3_Backend.DTO.Calidad.FichaTecnicaCliente.DetalleFichaClientes.Batch;
using Sistema_Produccion_3_Backend.DTO.Calidad.UnidadesMedida;
using Sistema_Produccion_3_Backend.DTO.Calidad.VariablesTecnicas;
using Sistema_Produccion_3_Backend.DTO.Calidad.VariableUnidadMedida;
using Sistema_Produccion_3_Backend.DTO.Catalogo.FamiliaMaquina;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Turnos;
using Sistema_Produccion_3_Backend.DTO.CorridaCombinada;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.Etiqueta.BathcEtiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.EtiquetaOf.BatchEtiquetaOf;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta;
using Sistema_Produccion_3_Backend.DTO.Etiquetas.TarjetaEtiqueta.BatchTarjetaEtiqueta;
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
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.LogCambiosProceso;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.MaterialOf;
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
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.LotePliego;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMateriales.ProcesosOf;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.TransferenciaProceso;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.ValeBobina;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.Tableros.Areas;
using Sistema_Produccion_3_Backend.DTO.Tableros.Posturas;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.BusquedaTarjetas;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.EstadoOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.logCambiosOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.NotasOf;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF.Reportes;
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
                .ForMember(dest => dest.fechaVencimiento, opt => opt.MapFrom(src =>
                        src.corridaCombinada == true
                            ? src.fechaVencimiento
                            : src.oFNavigation.fechaVencimiento))
                .ForMember(dest => dest.secuenciaArea, opt => opt.MapFrom(src => src.idTableroNavigation.idAreaNavigation.secuencia))
                .ForMember(dest => dest.subordinadas, opt => opt.MapFrom(src =>
                src.corridaCombinadamaestroNavigation
                    .Concat(src.corridaCombinadasubordinadoNavigation != null
                        ? new List<corridaCombinada> { src.corridaCombinadasubordinadoNavigation }
                        : new List<corridaCombinada>()))
                )
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

                CreateMap<procesoOf, AddProcesoOfMaquinas>()
                    .ForMember(dest => dest.posturasOfDto, opt => opt.MapFrom(src => src.idPosturaNavigation))
                    .ReverseMap();

                CreateMap<UpProcesoOfMaquinas, procesoOf>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

                CreateMap<UpdateBatchProcesoOfMaquina, procesoOf>()
                    .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));


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
                .ReverseMap();
            CreateMap<maquinas, ProcesoMaquinaDto>()
                .ForMember(dest => dest.familiaNombre, opt => opt.MapFrom(src => src.idFamiliaNavigation.nombreFamilia))
                .ReverseMap(); // <---- para lista procesos por OF
            CreateMap<maquinas, AddMaquinaDto>().ReverseMap();
            CreateMap<UpdateMaquinaDto, maquinas>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<familliaDeMaquina, FamilliaDeMaquinaDto>().ReverseMap();

            CreateMap<familliaDeMaquina, ListaFamilliaDeMaquinaDto>()
                .ForMember(dest => dest.maquinas, opt => opt.MapFrom(src => src.maquinas))
                .ReverseMap();


            // PERMISOS / USUARIO ====================================================================================
            CreateMap<usuario, UsuarioDto>()
                .ForMember(dest => dest.rol, opt => opt.MapFrom(src => src.idRolNavigation))
                .ForMember(dest => dest.cargo, opt => opt.MapFrom(src => src.idCargoNavigation.nombreCargo))
                .ForMember(dest => dest.permisosMaquina, opt => opt.MapFrom(src => src.permisoMaquina))
                .ReverseMap();

            CreateMap<usuario, UsuarioDisenoDto>().ReverseMap();

            CreateMap<usuario, OperadoresDto>()
                .ForMember(dest => dest.nombreArea, opt => opt.MapFrom(src => src.idAreaNavigation.nombreArea))
                .ForMember(dest => dest.maquinasAsignadas, opt => opt.MapFrom(src => src.permisoMaquina))
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
                .ReverseMap();
            CreateMap<solicitudMateriales, AddSolicitudMaterialesDto>().ReverseMap();
            CreateMap<UpdateSolicitudMaterialesDto, solicitudMateriales>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<solicitudMateriales, solicitudMaterialesProcesoOfDto>()
                .ForMember(dest => dest.solicitudMaterialOf, opt => opt.MapFrom(src => src.solicitudMaterialesOf))
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

            CreateMap<lotePliego, lotePliegoDto>().ReverseMap();
            CreateMap<lotePliego, AddLotePliegoDto>().ReverseMap();
            CreateMap<UpdateLotePliegoDto, lotePliego>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<transferenciaProceso, transferenciaProcesoDto>().ReverseMap();
            CreateMap<transferenciaProceso, AddTransferenciaProcesoDto>().ReverseMap();
            CreateMap<UpdateTransferenciaProcesoDto, transferenciaProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<valeBobina, ValeBobinaDto>().ReverseMap();
            CreateMap<valeBobina, AddValeBobinaDto>().ReverseMap();
            CreateMap<UpdateValeBobinaDto, valeBobina>()
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
        }
    }
}
