using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.Models;
using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;

namespace Sistema_Produccion_3_Backend.DTO.OV
{
    public class ArticuloDto
    {
        public int idArticulo { get; set; }

        public int? idOv { get; set; }

        public string articulo { get; set; }

        public string unidad { get; set; }

        public string detalleArticulo { get; set; }

        public decimal? precioUnidad { get; set; }

        public string lineaDeNegocio { get; set; }

        public string departamento { get; set; }

    }
}
