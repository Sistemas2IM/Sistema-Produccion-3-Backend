using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.Calidad;
using Sistema_Produccion_3_Backend.DTO.Catalogo;
using Sistema_Produccion_3_Backend.DTO.Logistica;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
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

            CreateMap<tarjetaOf, AddTarjetaOfDto>().ReverseMap();
            CreateMap<UpdateTarjetaOfDto, tarjetaOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<estadosOf, EstadoOfDto>()
                .ForMember(dest => dest.tarjetaOfDtos, opt => opt.MapFrom(src => src.tarjetaOf)) // Mapea tarjetasOf a su DTO
                .ReverseMap();

            CreateMap<etiqueta, EtiquetaDto>().ReverseMap();

            // PROCESOS OF ==========================================================================================
            CreateMap<procesoOf, ProcesoOfDto>()
                .ForMember(dest => dest.detalleProcesoOf, opt => opt.MapFrom(src => src.detalleOperacionProceso))
                .ForMember(dest => dest.tarjetaCampoDto, opt => opt.MapFrom(src => src.tarjetaCampo))
                .ForMember(dest => dest.tarjetaEtiquetaDto, opt => opt.MapFrom(src => src.tarjetaEtiqueta))
                .ForMember(dest => dest.posturasOfDto, opt => opt.MapFrom(src => src.idPosturaNavigation))
                .ForMember(dest => dest.tablerosOfDto, opt => opt.MapFrom(src => src.idTableroNavigation))
                .ForMember(dest => dest.materialDto, opt => opt.MapFrom(src => src.idMaterialNavigation))
                .ReverseMap();

            CreateMap<procesoOf, AddProcesoOfDto>().ReverseMap();
            CreateMap<UpdateProcesoOfDto, procesoOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleOperacionProceso, DetalleProcesoOfDto>()
                .ForMember(dest => dest.operacionesDto, opt => opt.MapFrom(src => src.idOperacionNavigation))
                .ReverseMap();

            CreateMap<detalleOperacionProceso, AddDetalleProcesoOfDto>().ReverseMap();
            CreateMap<UpdateDetalleProcesoOfDto, detalleOperacionProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<tarjetaCampo, TarjetaCampoDto>()
               .ForMember(dest => dest.camposPersonalizadosDto, opt => opt.MapFrom(src => src.idCampoNavigation))
               .ReverseMap();

            CreateMap<camposPersonalizados, CamposPersonalizadosDto>().ReverseMap();

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
                .ForMember(dest => dest.contenidoEntregado, opt => opt.MapFrom(src => src.contenidoEntrega))
                .ForMember(dest => dest.detalleEntrega, opt => opt.MapFrom(src => src.detalleEntrega))
                .ForMember(dest => dest.estadoReporteDto, opt => opt.MapFrom(src => src.idEstadoReporteNavigation))
                .ForMember(dest => dest.maquinaDto, opt => opt.MapFrom(src => src.idMaquinaNavigation))
                .ForMember(dest => dest.ovDto, opt => opt.MapFrom(src => src.oVNavigation))
                .ForMember(dest => dest.tarjetaOfDto, opt => opt.MapFrom(src => src.oFNavigation))
                .ReverseMap();
            CreateMap<entregasProductoTerminado, AddProductoTerminadoDto>().ReverseMap();
            CreateMap<UpdateProductoTerminadoDto, entregasProductoTerminado>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<contenidoEntrega, ContenidoEntregaDto>().ReverseMap();
            CreateMap<contenidoEntrega, AddContenidoEntregaDto>().ReverseMap();
            CreateMap<UpdateContenidoEntregaDto, contenidoEntrega>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleEntrega, DetalleEntregaDto>().ReverseMap();
            CreateMap<detalleEntrega, AddDetalleEntregaDto>().ReverseMap();
            CreateMap<UpdateDetalleEntregaDto, detalleEntrega>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // REPORTES POR OPERADOR ==================================================================================
            CreateMap<reportesDeOperadores, ReporteOperadorDto>()
                .ForMember(dest => dest.estadoReporteDto, opt => opt.MapFrom(src => src.idEstadoReporteNavigation))
                .ForMember(dest => dest.tipoReporteDto, opt => opt.MapFrom(src => src.idTipoReporteNavigation))
                .ForMember(dest => dest.maquinaDto, opt => opt.MapFrom(src => src.idMaquinaNavigation))
                .ForMember(dest => dest.detalleReporte, opt => opt.MapFrom(src => src.detalleReporte))
                .ReverseMap();         

            CreateMap<reportesDeOperadores, AddReporteOperadorDto>().ReverseMap();

            CreateMap<UpdateReporteOperadorDto, reportesDeOperadores>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleReporte, DetalleReporteDto>()
               .ForMember(dest => dest.operacionesDto, opt => opt.MapFrom(src => src.idOperacionNavigation))
               .ForMember(dest => dest.materialDto, opt => opt.MapFrom(src => src.idMaterialNavigation))
               .ForMember(dest => dest.tipoCierreDto, opt => opt.MapFrom(src => src.idTipoCierreNavigation))
               .ForMember(dest => dest.numOf, opt => opt.MapFrom(src => src.oFNavigation.oF))
               .ForMember(dest => dest.descripcionOf, opt => opt.MapFrom(src => src.oFNavigation.nombreOf))
               .ForMember(dest => dest.clienteOf, opt => opt.MapFrom(src => src.oFNavigation.clienteOf))
               .ReverseMap();

            CreateMap<detalleReporte, UpdateDetalleReporteDto>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleReporte, AddDetalleReporteDto>().ReverseMap(); 
         
            CreateMap<estadosReporte, EstadoReporteDto>().ReverseMap();
            CreateMap<tipoReporte, TipoReporteDto>().ReverseMap();
            CreateMap<operaciones, OperacionesDto>().ReverseMap();
            CreateMap<material, MaterialDto>().ReverseMap();
            CreateMap<tipoCierre, TipoCierreDto>().ReverseMap();

            // CATALOGOS ==============================================================================================
            CreateMap<maquinas, MaquinaDto>().ReverseMap();

            // PERMISOS / USUARIO ====================================================================================
            CreateMap<usuario, UsuarioDto>()
                .ForMember(dest => dest.rol, opt => opt.MapFrom(src => src.idRolNavigation))
                .ForMember(dest => dest.cargo, opt => opt.MapFrom(src => src.idCargoNavigation.nombreCargo))
                .ReverseMap();

            CreateMap<rol, RolDto>()
                .ForMember(dest => dest.permisos, opt => opt.MapFrom(src => src.permiso))
                .ReverseMap();

            CreateMap<permiso, PermisoDto>()
                .ForMember(dest => dest.subModulo, opt => opt.MapFrom(src => src.idSubModuloNavigation))
                .ReverseMap();
            CreateMap<permiso, AddPermisoDto>().ReverseMap();
            CreateMap<UpdatePermisoDto, permiso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<subModulo, SubModuloDto>()
                .ForMember(dest => dest.modulo, opt => opt.MapFrom(src => src.idModuloNavigation))
                .ReverseMap();

            CreateMap<modulo, ModuloDto>()
                .ForMember(dest => dest.menu, opt => opt.MapFrom(src => src.idMenuNavigation))
                .ReverseMap();

            CreateMap<menu, MenuDto>().ReverseMap();

            CreateMap<cargo, CargoDto>()
                .ForMember(dest => dest.usuarios, opt => opt.MapFrom(src => src.usuario))
                .ReverseMap();

            // ASIGNACION: PROCESO OF - USUARIO =======================================================================
            CreateMap<asignacion, AsignacionDto>()
                .ForMember(dest => dest.nombreUsuario, opt => opt.MapFrom(src => src.userNavigation.nombres + " " + src.userNavigation.apellidos))
                .ForMember(dest => dest.procesoOf, opt => opt.MapFrom(src => src.idProcesoNavigation))
                .ReverseMap();
            CreateMap<asignacion, AddAsignacionDto>().ReverseMap();
            CreateMap<UpdateAsignacionDto, asignacion>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // CERTIFICADO DE CALIDAD =================================================================================
            CreateMap<certificadoDeCalidad, CertificadoCalidadDto>()
                .ForMember(dest => dest.detalleCertificado, opt => opt.MapFrom(src => src.detalleCertificado))
                .ReverseMap();
            CreateMap<certificadoDeCalidad, AddCertificadoCalidadDto>().ReverseMap();
            CreateMap<UpdateCertificadoCalidadDto, certificadoDeCalidad>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleCertificado, DetalleCertificadoDto>()
                .ForMember(dest => dest.especificaciones, opt => opt.MapFrom(src => src.idCaracteristaNavigation))
                .ReverseMap();
            CreateMap<detalleCertificado, AddDetalleCertificadoDto>().ReverseMap();
            CreateMap<UpdateDetalleCertificadoDto, detalleCertificado>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<especificaciones, EspecificacionesCerDto>().ReverseMap();
            CreateMap<especificaciones, AddEspecificacionesCerDto>().ReverseMap();
            CreateMap<UpdateEspecificacionesCerDto, especificaciones>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            // LOGISTICA - GIRA
            CreateMap<gira, GiraDto>()
                .ForMember(dest => dest.detalleGiraDto, opt => opt.MapFrom(src => src.detalleGira))
                .ForMember(dest => dest.vehiculoDto, opt => opt.MapFrom(src => src.idVehiculoNavigation))
                .ForMember(dest => dest.motoristaDto, opt => opt.MapFrom(src => src.idMotoristaNavigation))
                .ReverseMap();
            CreateMap<gira, AddGiraDto>().ReverseMap();
            CreateMap<UpdateGiraDto, gira>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleGira, DetalleGiraDto>()
                .ForMember(dest => dest.giraDto, opt => opt.MapFrom(src => src.idGiraNavigation))
                .ReverseMap();
            CreateMap<detalleGira, DetalleGiraDto>().ReverseMap();
            CreateMap<UpdateDetalleGiraDto, detalleGira>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<vehiculo, VehiculoDto>().ReverseMap();
            CreateMap<vehiculo, AddVehiculoDto>().ReverseMap();
            CreateMap<UpdateVehiculoDto, vehiculo>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<motorista, MotoristaDto>().ReverseMap();
            CreateMap<motorista, AddMotoristaDto>().ReverseMap();
            CreateMap<UpdateMotoristaDto, motorista>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));
        }
    }
}
