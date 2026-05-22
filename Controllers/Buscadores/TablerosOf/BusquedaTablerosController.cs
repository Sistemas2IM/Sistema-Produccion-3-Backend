using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Buscadores.DTOGlobales;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ProductoTerminado;
using Sistema_Produccion_3_Backend.DTO.SolicitudDeMateriales.SolicitudMaterialOF;
using Sistema_Produccion_3_Backend.DTO.Tableros;
using Sistema_Produccion_3_Backend.DTO.TarjetasOF;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Buscadores.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusquedaTablerosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public BusquedaTablerosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/buscarGlobal/{termino}
        [HttpGet("buscarGlobal/{termino}")]
        public async Task<ActionResult<object>> BuscarGlobal(string termino)
        {
            termino = termino.Trim();

            // 1. INTELIGENCIA DE BÚSQUEDA: Separar en palabras
            // Ignoramos palabras de 1 sola letra para no saturar la base de datos (ej. "y", "a", "o")
            var terminosBusqueda = termino.Split(' ', StringSplitOptions.RemoveEmptyEntries)
                                          .Where(t => t.Length > 1)
                                          .ToList();

            // Si el usuario buscó literalmente solo una letra (ej. "A"), la usamos como único término
            if (!terminosBusqueda.Any())
            {
                terminosBusqueda.Add(termino);
            }

            // 2. PREPARAR LAS CONSULTAS BASE (Sin ejecutar aún)
            var queryProcesos = _context.procesoOf.AsNoTracking().AsSplitQuery()
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .AsQueryable();

            var queryTarjetas = _context.tarjetaOf.AsNoTracking().AsSplitQuery()
                .Include(u => u.idEstadoOfNavigation)
                .Include(r => r.etiquetaOf)
                .AsQueryable();

            var queryTableros = _context.tablerosOf.AsNoTracking().AsQueryable();

            var queryEntregas = _context.entregasProductoTerminado.AsNoTracking()
                .Include(f => f.ofNavigation)
                .AsQueryable();

            var querySolicitudes = _context.solicitudMaterialesOf.AsNoTracking()
                .Include(f => f.oFNavigation)
                .Include(sm => sm.idSolicitudNavigation)
                .AsQueryable();

            var queryValesBobina = _context.valeBobina.AsNoTracking()
                .Include(m => m.idMaterialNavigation)
                .Include(e => e.estadoNavigation)
                .AsQueryable();

            var queryFichaTecnicaCliente = _context.fichaTecnicaCliente.AsNoTracking()
                .Include(f => f.oFNavigation)
                .AsQueryable();

            var queryFichaTecnicaInterna = _context.fichaTecnicaProcesos.AsNoTracking()
                .Include(f => f.oFNavigation)
                .Include(m => m.maquinaNavigation)
                .AsQueryable();

            var queryCertificadosCalidad = _context.certificadoCalidad.AsNoTracking()
                .Include(of => of.oFNavigation)
                .AsQueryable();

            // 3. CONSTRUCCIÓN DINÁMICA: Aplicar cada palabra como un filtro obligatorio (AND)
            foreach (var t in terminosBusqueda)
            {
                var terminoActual = t.ToLower();
                bool esNumero = int.TryParse(terminoActual, out int numActual);

                queryProcesos = queryProcesos.Where(u =>
                    (esNumero && u.oF == numActual) ||
                    (esNumero && u.oFNavigation != null && u.oFNavigation.oV == numActual) ||
                    u.oF.ToString().Contains(terminoActual) ||
                    (u.oFNavigation != null && u.oFNavigation.oV.ToString().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.clienteOf != null && u.oFNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.vendedorOf != null && u.oFNavigation.vendedorOf.ToLower().Contains(terminoActual)) || // 🚀 ¡AQUÍ ESTÁ! Ahora sí busca al ejecutivo
                    (u.productoOf != null && u.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.codArticulo != null && u.oFNavigation.codArticulo.ToLower().Contains(terminoActual)) ||
                    (u.idMaquinaSAP != null && u.idMaquinaSAP.ToLower().Contains(terminoActual)));

                queryTarjetas = queryTarjetas.Where(u =>
                    (esNumero && u.oF == numActual) ||
                    (esNumero && u.oV == numActual) ||
                    u.oF.ToString().Contains(terminoActual) ||
                    (u.oV != null && u.oV.ToString().Contains(terminoActual)) ||
                    (u.clienteOf != null && u.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.vendedorOf != null && u.vendedorOf.ToLower().Contains(terminoActual)) || // (Este ya lo tenías, pero le agregamos protección null)
                    (u.productoOf != null && u.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.codArticulo != null && u.codArticulo.ToLower().Contains(terminoActual)));

                queryTableros = queryTableros.Where(u =>
                    (u.nombreTablero != null && u.nombreTablero.ToLower().Contains(terminoActual)) ||
                    (u.idSapMaquina != null && u.idSapMaquina.ToLower().Contains(terminoActual)));

                queryEntregas = queryEntregas.Where(u =>
                    (esNumero && u.idEntregaPt == numActual) ||
                    (esNumero && u.of == numActual) ||
                    u.of.ToString().Contains(terminoActual) ||
                    (u.ofNavigation != null && u.ofNavigation.clienteOf != null && u.ofNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.ofNavigation != null && u.ofNavigation.vendedorOf != null && u.ofNavigation.vendedorOf.ToLower().Contains(terminoActual)) || // 🚀 Vendedor agregado
                    (u.ofNavigation != null && u.ofNavigation.oV != null && u.ofNavigation.oV.ToString().Contains(terminoActual)) ||
                    (u.ofNavigation != null && u.ofNavigation.productoOf != null && u.ofNavigation.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.ofNavigation != null && u.ofNavigation.codArticulo != null && u.ofNavigation.codArticulo.ToLower().Contains(terminoActual)));

                querySolicitudes = querySolicitudes.Where(u =>
                    (esNumero && u.idSolicitud == numActual) ||
                    (esNumero && u.oF == numActual) ||
                    (u.idSolicitudNavigation != null && u.idSolicitudNavigation.materialDescripcion != null && u.idSolicitudNavigation.materialDescripcion.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.productoOf != null && u.oFNavigation.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.clienteOf != null && u.oFNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.vendedorOf != null && u.oFNavigation.vendedorOf.ToLower().Contains(terminoActual)) || // 🚀 Vendedor agregado
                    (u.oFNavigation != null && u.oFNavigation.oV != null && u.oFNavigation.oV.ToString().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.codArticulo != null && u.oFNavigation.codArticulo.ToLower().Contains(terminoActual)));

                queryValesBobina = queryValesBobina.Where(u =>
                    (esNumero && u.idVale == numActual) ||
                    u.idVale.ToString().Contains(terminoActual) ||
                    (u.idMaterial != null && u.idMaterial.ToLower().Contains(terminoActual)) ||
                    (u.idMaterialNavigation != null && u.idMaterialNavigation.nombreMaterial != null && u.idMaterialNavigation.nombreMaterial.ToLower().Contains(terminoActual)) ||
                    (u.idMaterialNavigation != null && u.idMaterialNavigation.marca != null && u.idMaterialNavigation.marca.ToLower().Contains(terminoActual)) ||
                    (u.descripcionBobina != null && u.descripcionBobina.ToLower().Contains(terminoActual)));

                queryFichaTecnicaCliente = queryFichaTecnicaCliente.Where(u =>
                    (esNumero && u.idFichaCliente == numActual) ||
                    (u.idFichaCliente.ToString().Contains(terminoActual)) ||
                    (u.oF != null && u.oF.ToString().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.codArticulo != null && u.oFNavigation.codArticulo.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.productoOf != null && u.oFNavigation.productoOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.clienteOf != null && u.oFNavigation.clienteOf.ToLower().Contains(terminoActual)));

                queryFichaTecnicaInterna = queryFichaTecnicaInterna.Where(u =>
                    (esNumero && u.idFichaProceso == numActual) ||
                    (u.idFichaProceso.ToString().Contains(terminoActual)) ||
                    (u.oF != null && u.oF.ToString().Contains(terminoActual)) ||
                    (u.idProceso != null && u.idProceso.ToString().Contains(terminoActual)) ||
                    (u.maquinaNavigation != null && u.maquinaNavigation.nombreMaquina != null && u.maquinaNavigation.nombreMaquina.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.clienteOf != null && u.oFNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.codArticulo != null && u.oFNavigation.codArticulo.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.productoOf != null && u.oFNavigation.productoOf.ToLower().Contains(terminoActual)));

                queryCertificadosCalidad = queryCertificadosCalidad.Where(u =>
                    (esNumero && u.idCertificadoCalidad == numActual) ||
                    (u.idCertificadoCalidad.ToString().Contains(terminoActual)) ||
                    (u.idFichaCliente != null && u.idFichaCliente.ToString().Contains(terminoActual)) ||
                    (u.oF != null && u.oF.ToString().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.clienteOf != null && u.oFNavigation.clienteOf.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.codArticulo != null && u.oFNavigation.codArticulo.ToLower().Contains(terminoActual)) ||
                    (u.oFNavigation != null && u.oFNavigation.productoOf != null && u.oFNavigation.productoOf.ToLower().Contains(terminoActual)));

            }

            // 4. EJECUTAR CONSULTAS CON LÍMITES
            // 4. EJECUTAR CONSULTAS ORDENANDO POR LOS MÁS RECIENTES Y CON LÍMITES
            var procesosOf = await queryProcesos
                .OrderByDescending(u => u.idProceso) // 🚀 Ordena del ID mayor al menor (más recientes primero)
                .Take(30)
                .ToListAsync();

            var tarjetasOf = await queryTarjetas
                .OrderByDescending(u => u.oF) // 🚀 Ajusta "oF" si tu tabla tiene una llave primaria como "idTarjeta"
                .Take(30)
                .ToListAsync();

            var tableros = await queryTableros
                // .OrderByDescending(u => u.idTablero) // Descomenta y ajusta si vas a usar tableros
                .Take(20)
                .ToListAsync();

            var entregasProductoTerminado = await queryEntregas
                .OrderByDescending(u => u.idEntregaPt) // 🚀 Los IDs de entrega más altos primero
                .Take(30)
                .ToListAsync();

            var solicitudesMateriales = await querySolicitudes
                .OrderByDescending(u => u.idSolicitud) // 🚀 Las solicitudes más recientes
                .Take(30)
                .ToListAsync();

            var valesBobina = await queryValesBobina
                .OrderByDescending(u => u.idVale) // 🚀 Vales de bobina más recientes
                .Take(30)
                .ToListAsync();

            var fichaTecnicaCliente = await queryFichaTecnicaCliente
                .OrderByDescending(u => u.idFichaCliente) // 🚀 Fichas técnicas de cliente más recientes
                .Take(30)
                .ToListAsync();

            var fichaTecnicaInterna = await queryFichaTecnicaInterna
                .OrderByDescending(u => u.idFichaProceso) // 🚀 Fichas técnicas internas más recientes
                .Take(30)
                .ToListAsync();

            var certificadosCalidad = await queryCertificadosCalidad
                .OrderByDescending(u => u.idCertificadoCalidad) // 🚀 Certificados de calidad más recientes
                .Take(30)
                .ToListAsync();

            // 5. MAPEO Y RESPUESTA
            var procesosOfDto = _mapper.Map<List<SB_ProcesoOfDto>>(procesosOf);
            var tarjetasOfDto = _mapper.Map<List<SB_TarjetaOfDto>>(tarjetasOf);
            var entregasProductoTerminadoDto = _mapper.Map<List<SB_ProductoTerminadoDto>>(entregasProductoTerminado);
            var solicitudesMaterialesDto = _mapper.Map<List<SB_SolicitudMaterialesOfDto>>(solicitudesMateriales);
            var valesBobinaDto = _mapper.Map<List<SB_ValeBobinaDto>>(valesBobina);
            var fichaTecnicaClienteDto = _mapper.Map<List<SB_FichaTecnicaClienteDto>>(fichaTecnicaCliente);
            var fichaTecnicaInternaDto = _mapper.Map<List<SB_FichaTecnicaProcesosDto>>(fichaTecnicaInterna);
            var certificadosCalidadDto = _mapper.Map<List<SB_CertificadoCalidadDto>>(certificadosCalidad);

            var resultado = new
            {
                ProcesosOf = procesosOfDto,
                TarjetasOf = tarjetasOfDto,
                //Tableros = tablerosDto,
                EntregaProductoTerminado = entregasProductoTerminadoDto,
                SolicitudesMateriales = solicitudesMaterialesDto,
                ValeBobina = valesBobinaDto,
                FichaTecnicaCliente = fichaTecnicaClienteDto,
                FichaTecnicaInterna = fichaTecnicaInternaDto,
                CertificadosCalidad = certificadosCalidadDto
            };

            if (!procesosOfDto.Any() && !tarjetasOfDto.Any() && !entregasProductoTerminadoDto.Any() && !solicitudesMaterialesDto.Any())
            {
                return NotFound($"No se encontraron resultados para el término de búsqueda: {termino}");
            }

            return Ok(resultado);
        }

    }
}
