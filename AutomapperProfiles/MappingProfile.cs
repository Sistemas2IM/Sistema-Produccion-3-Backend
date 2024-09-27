using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.OV;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
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
            CreateMap<tarjetaOf, TarjetaOfDto>().ReverseMap();
            CreateMap<tarjetaOf, AddTarjetaOfDto>().ReverseMap();
            CreateMap<tarjetaOf, UpdateTarjetaOfDto>().ReverseMap();

            // ASIGNACION DE TARJETAS OF
            CreateMap<asignacion, AsignarOfDto>()
                .ForMember(dest => dest.NombreDisenador, opt => opt.MapFrom(src => src.idDisenadorNavigation.nombreDisenador))
                .ForMember(dest => dest.TarjetaOfDescripcion, opt => opt.MapFrom(src => src.idTarjetaOfNavigation.nombreOf))
                .ReverseMap();

            // OV - DTO
            CreateMap<oV, OVDto>().ReverseMap();
            CreateMap<oV, AddOVDto>().ReverseMap();

            // Articulo - DTO
            CreateMap<articuloOv, ArticuloDto>().ReverseMap();
            CreateMap<articuloOv, AddArticuloDto>().ReverseMap();

            // REPORTES POR OPERADOR
            CreateMap<reportesDeOperadores, AddReporteOperadorDto>().ReverseMap();
            CreateMap<reportesDeOperadores, ReporteOperadorDto>().ReverseMap();

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
