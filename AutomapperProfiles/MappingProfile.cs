using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.Catalogo;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
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

            // TARJETAS OF - DTO
            CreateMap<tarjetaOf, TarjetaOfDto>()
                .ForMember(dest => dest.estadoOfDto, opt => opt.MapFrom(src => src.idEstadoOfNavigation))
                .ForMember(dest => dest.etiquetaDto, opt => opt.MapFrom(src => src.etiquetaOf))
                .ReverseMap();

            CreateMap<tarjetaOf, AddTarjetaOfDto>().ReverseMap();
            CreateMap<UpdateTarjetaOfDto, tarjetaOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<estadosOf, EstadoOfDto>().ReverseMap();
            CreateMap<etiqueta, EtiquetaDto>().ReverseMap();
            CreateMap<tarjetaCampo, TarjetaCampoDto>()
                .ForMember(dest => dest.camposPersonalizadosDto, opt => opt.MapFrom(src => src.idCampoNavigation))
                .ReverseMap();

            CreateMap<camposPersonalizados, CamposPersonalizadosDto>().ReverseMap();

            // PROCESOS OF - TARJETAS
            CreateMap<procesoOf, ProcesoOfDto>()
                .ForMember(dest => dest.detalleProcesoOfDto, opt => opt.MapFrom(src => src.detalleOperacionProceso))
                .ReverseMap();

            CreateMap<procesoOf, AddProcesoOfDto>().ReverseMap();
            CreateMap<UpdateProcesoOfDto, procesoOf>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));

            CreateMap<detalleOperacionProceso, DetalleProcesoDto>().ReverseMap();
            CreateMap<detalleOperacionProceso, AddDetalleProcesoDto>().ReverseMap();
            CreateMap<UpdateDetalleProcesoDto, detalleOperacionProceso>()
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) => srcMember != null));           

            // OV - DTO
            CreateMap<oV, OVDto>()
                .ForMember(dest => dest.articulo, opt => opt.MapFrom(src => src.articuloOv))
                .ReverseMap();

            CreateMap<oV, AddOVDto>().ReverseMap();

            // Articulo - DTO
            CreateMap<articuloOv, ArticuloDto>().ReverseMap();
            CreateMap<articuloOv, AddArticuloDto>().ReverseMap();

            // PRODUCTO TERMINADO
            CreateMap<entregasProductoTerminado, ProductoTerminadoDto>()
                .ForMember(dest => dest.contenidoEntregado, opt => opt.MapFrom(src => src.contenidoEntrega))
                .ForMember(dest => dest.detalleEntrega, opt => opt.MapFrom(src => src.detalleEntrega))
                .ForMember(dest => dest.estadoReporteDto, opt => opt.MapFrom(src => src.idEstadoReporteNavigation))
                .ForMember(dest => dest.maquinaDto, opt => opt.MapFrom(src => src.idMaquinaNavigation))
                .ForMember(dest => dest.ovDto, opt => opt.MapFrom(src => src.idOvNavigation))
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

            // REPORTES POR OPERADOR
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

            // CATALOGOS
            CreateMap<maquinas, MaquinaDto>().ReverseMap();

            // PERMISOS / USUARIO
            CreateMap<usuario, UsuarioDto>()
                .ForMember(dest => dest.rol, opt => opt.MapFrom(src => src.idRolNavigation))
                .ReverseMap();

            CreateMap<rol, RolDto>()
                .ForMember(dest => dest.permisos, opt => opt.MapFrom(src => src.permiso)) // Map permissions collection
                .ReverseMap();

            CreateMap<permiso, PermisoDto>()
                .ForMember(dest => dest.subModulo, opt => opt.MapFrom(src => src.idSubModuloNavigation))
                .ReverseMap();

            CreateMap<subModulo, SubModuloDto>()
                .ForMember(dest => dest.modulo, opt => opt.MapFrom(src => src.idModuloNavigation))
                .ReverseMap();

            CreateMap<modulo, ModuloDto>()
                .ForMember(dest => dest.menu, opt => opt.MapFrom(src => src.idMenuNavigation))
                .ReverseMap();

            CreateMap<menu, MenuDto>().ReverseMap();

        }
    }
}
