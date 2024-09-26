using AutoMapper;
using Sistema_Produccion_3_Backend.DTO.OV;
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

            // OV - DTO
            CreateMap<oV, AddOVDto>().ReverseMap();
            // Articulo - DTO
            CreateMap<articuloOv, ArticuloDto>().ReverseMap();
            CreateMap<articuloOv, AddArticuloDto>().ReverseMap();
        }
    }
}
