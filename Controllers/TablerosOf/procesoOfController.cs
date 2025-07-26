using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.CodeAnalysis.Elfie.Serialization;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.BusquedaProcesos;
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
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.UpdateMaquina;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf.UpdateSAP;
using Sistema_Produccion_3_Backend.Models;
using Sistema_Produccion_3_Backend.Services.RequestLock;
using System.Diagnostics;
using System.Linq;
using System.Text.Json;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class procesoOfController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly IRequestLockService _lockService;

        public procesoOfController(base_nuevaContext context, IMapper mapper, IRequestLockService lockService)
        {
            _context = context;
            _mapper = mapper;
            _lockService = lockService;
        }

        // GET: api/procesoOf
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOf()
        {
            var procesoOf = await _context.procesoOf
                .Where(x => x.archivada == false)
                .Include(u => u.detalleReporte)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(u => u.detalleReporte)
                .ThenInclude(m => m.maquinaNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .ThenInclude(e => e.idEtiquetaNavigation)
                .Include(f => f.oFNavigation)
                .Include(l => l.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(a => a.asignacion)
                .ThenInclude(u => u.userNavigation)
                .Include(p => p.corridaCombinadamaestroNavigation)
                .Include(p => p.corridaCombinadasubordinadoNavigation)
                .ToListAsync();

            foreach (var proceso in procesoOf)
            {
                // Cargar subordinados de corridaCombinadamaestroNavigation (lista 1:N)
                if (proceso.corridaCombinadamaestroNavigation != null)
                {
                    foreach (var corrida in proceso.corridaCombinadamaestroNavigation)
                    {
                        if (corrida.subordinado != null)
                        {
                            var subordinado = await _context.procesoOf
                                .Include(p => p.oFNavigation)
                                .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                            corrida.subordinadoNavigation = subordinado;
                        }
                    }
                }

                // Cargar subordinado de corridaCombinadasubordinadoNavigation (1:1)
                if (proceso.corridaCombinadasubordinadoNavigation?.subordinado != null)
                {
                    var subordinado = await _context.procesoOf
                        .Include(p => p.oFNavigation)
                        .FirstOrDefaultAsync(p => p.idProceso == proceso.corridaCombinadasubordinadoNavigation.subordinado);

                    proceso.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
                }
            }


            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesoOf);

            return Ok(procesoOfDto);
        }

        [HttpGet("get/filtros")]
        public async Task<ActionResult<IEnumerable<ProcesoOfDto>>> GetprocesoOffiltros(
        [FromQuery] DateTime? fechaInicio = null,
        [FromQuery] DateTime? fechaFin = null,
        [FromQuery] string? cliente = null,
        [FromQuery] string? ejecutivo = null,
        [FromQuery] string? articulo = null,
        [FromQuery] int? oF = null,
        [FromQuery] int? oV = null,
        [FromQuery] string? lineaNegocio = null,
        [FromQuery] string? idsEtiquetas = null,
        [FromQuery] int? idProceso = null,
        [FromQuery] bool mostrarArchivados = false,
        [FromQuery] int? tablero = null,
        [FromQuery] string? disenador = null)
        {
            // ============================
            // 1. Procesos NORMALES (con OF)
            // ============================
            var queryNormales = _context.procesoOf
                .OrderBy(p => p.posicion)
                .Include(u => u.detalleReporte).ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta).ThenInclude(e => e.idEtiquetaNavigation)
                .Include(f => f.oFNavigation)
                .Include(l => l.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(a => a.asignacion).ThenInclude(u => u.userNavigation)
                .Include(p => p.corridaCombinadamaestroNavigation)
                .Include(p => p.corridaCombinadasubordinadoNavigation)
                .Where(p => p.oF != null && (mostrarArchivados || p.archivada == false))
                .AsQueryable();

            // Filtros para procesos normales
            if (fechaInicio.HasValue && fechaFin.HasValue)
                queryNormales = queryNormales.Where(p => p.fechaVencimiento >= fechaInicio && p.fechaVencimiento <= fechaFin);
            else if (fechaInicio.HasValue)
                queryNormales = queryNormales.Where(p => p.fechaVencimiento >= fechaInicio);
            else if (fechaFin.HasValue)
                queryNormales = queryNormales.Where(p => p.fechaVencimiento <= fechaFin);

            if (!string.IsNullOrEmpty(cliente))
                queryNormales = queryNormales.Where(p => p.oFNavigation.clienteOf.Contains(cliente));

            if (!string.IsNullOrEmpty(ejecutivo))
                queryNormales = queryNormales.Where(p => p.oFNavigation.vendedorOf.Contains(ejecutivo));

            if (!string.IsNullOrEmpty(articulo))
                queryNormales = queryNormales.Where(p => p.productoOf.Contains(articulo));

            if (oF.HasValue)
                queryNormales = queryNormales.Where(p => p.oF == oF);

            if (oV.HasValue)
                queryNormales = queryNormales.Where(p => p.oV == oV);

            if (!string.IsNullOrEmpty(lineaNegocio))
                queryNormales = queryNormales.Where(p => p.oFNavigation.lineaDeNegocio.Contains(lineaNegocio));

            if (!string.IsNullOrEmpty(disenador))
                queryNormales = queryNormales.Where(p =>
                    p.asignacion.Any(a =>
                        (a.userNavigation.nombres + " " + a.userNavigation.apellidos).Contains(disenador)));

            if (!string.IsNullOrEmpty(idsEtiquetas))
            {
                var ids = idsEtiquetas.Split(',').Select(int.Parse).ToList();
                queryNormales = queryNormales.Where(p =>
                    p.tarjetaEtiqueta.Any(te => te.idEtiqueta.HasValue && ids.Contains(te.idEtiqueta.Value)));
            }

            if (idProceso.HasValue)
                queryNormales = queryNormales.Where(p => p.idProceso == idProceso);

            if (tablero.HasValue)
                queryNormales = queryNormales.Where(p => p.idTablero == tablero.Value);

            var procesosNormales = await queryNormales.ToListAsync();

            // ============================================
            // 2. Procesos MAESTROS de corrida combinada
            // ============================================
            var queryMaestros = _context.procesoOf
                .Where(p => p.corridaCombinada == true && p.oF == null && (mostrarArchivados || p.archivada == false))
                .Include(p => p.corridaCombinadamaestroNavigation)
                    .ThenInclude(c => c.subordinadoNavigation)
                        .ThenInclude(s => s.oFNavigation)
                .Include(p => p.detalleReporte).ThenInclude(o => o.idOperacionNavigation)
                .Include(p => p.tarjetaCampo)
                .Include(p => p.tarjetaEtiqueta).ThenInclude(e => e.idEtiquetaNavigation)
                .Include(p => p.idPosturaNavigation)
                .Include(p => p.idMaterialNavigation)
                .Include(p => p.asignacion).ThenInclude(u => u.userNavigation);

            var procesosMaestrosRaw = await queryMaestros.ToListAsync();

            // Filtrar procesos maestros con base en filtros aplicados a subordinados
            var procesosMaestros = procesosMaestrosRaw
                .Where(m => m.corridaCombinadamaestroNavigation.Any(c =>
                {
                    var sub = c.subordinadoNavigation;
                    var ofNav = sub?.oFNavigation;

                    if (sub == null || ofNav == null) return false;

                    return
                        (!fechaInicio.HasValue || sub.fechaVencimiento >= fechaInicio) &&
                        (!fechaFin.HasValue || sub.fechaVencimiento <= fechaFin) &&
                        (string.IsNullOrEmpty(cliente) || ofNav.clienteOf.Contains(cliente)) &&
                        (string.IsNullOrEmpty(ejecutivo) || ofNav.vendedorOf.Contains(ejecutivo)) &&
                        (string.IsNullOrEmpty(articulo) || ofNav.productoOf.Contains(articulo)) &&
                        (!oF.HasValue || sub.oF == oF) &&
                        (!oV.HasValue || sub.oV == oV) &&
                        (!idProceso.HasValue || m.idProceso == idProceso) &&
                        (string.IsNullOrEmpty(lineaNegocio) || ofNav.lineaDeNegocio.Contains(lineaNegocio));
                }))
                .ToList();

            // ========================
            // 3. Combinar resultados
            // ========================
            var procesos = procesosNormales.Concat(procesosMaestros).ToList();

            // ========================
            // 4. Cargar subordinados
            // ========================
            foreach (var proceso in procesos)
            {
                if (proceso.corridaCombinadamaestroNavigation != null)
                {
                    foreach (var corrida in proceso.corridaCombinadamaestroNavigation)
                    {
                        if (corrida.subordinado != null)
                        {
                            var subordinado = await _context.procesoOf
                                .Include(p => p.oFNavigation)
                                .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                            corrida.subordinadoNavigation = subordinado;
                        }
                    }
                }

                if (proceso.corridaCombinadasubordinadoNavigation?.subordinado != null)
                {
                    var subordinado = await _context.procesoOf
                        .Include(p => p.oFNavigation)
                        .FirstOrDefaultAsync(p => p.idProceso == proceso.corridaCombinadasubordinadoNavigation.subordinado);

                    proceso.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
                }
            }

            // ========================
            // 5. Mapear y devolver DTOs
            // ========================
            var procesoOfDto = _mapper.Map<List<ProcesoOfDto>>(procesos);
            return Ok(procesoOfDto);
        }


        // GET GENERAL
        [HttpGet("get/{id}")]
        public async Task<ActionResult<ProcesoOfDto>> GetprocesoOf(int id)
        {
            var procesoOf = await _context.procesoOf
                .Include(u => u.detalleReporte).ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .Include(p => p.corridaCombinadamaestroNavigation)
                .Include(p => p.corridaCombinadasubordinadoNavigation)
                .FirstOrDefaultAsync(u => u.idProceso == id);

            // Cargar subordinadoNavigation manualmente
            if (procesoOf?.corridaCombinadamaestroNavigation != null)
            {
                foreach (var corrida in procesoOf.corridaCombinadamaestroNavigation)
                {
                    if (corrida.subordinado != null)
                    {
                        var subordinado = await _context.procesoOf
                            .Include(p => p.oFNavigation)
                            .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                        corrida.subordinadoNavigation = subordinado;
                    }
                }
            }

            if (procesoOf?.corridaCombinadasubordinadoNavigation?.subordinado != null)
            {
                var subordinado = await _context.procesoOf
                    .Include(p => p.oFNavigation)
                    .FirstOrDefaultAsync(p => p.idProceso == procesoOf.corridaCombinadasubordinadoNavigation.subordinado);

                procesoOf.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
            }

            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesoOf);

            if (procesoOfDto == null)
            {
                return NotFound("No se encontro el proceso de la Of con el id: " + id);
            }

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/get/procesosByOf/{of}
        [HttpGet("get/oF/{of}")]
        public async Task<ActionResult<IEnumerable<ProcesoOfMaquinas>>> GetProcesosByOf(int of)
        {
            // Obtener todos los procesos asociados a la OF
            var procesos = await _context.procesoOf
                .Where(x => x.archivada == false)
                .Include(p => p.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .Include(p => p.corridaCombinadamaestroNavigation)
                .Include(p => p.corridaCombinadasubordinadoNavigation)
                .FirstOrDefaultAsync(u => u.oF == of);

            // Cargar subordinadoNavigation manualmente
            if (procesos?.corridaCombinadamaestroNavigation != null)
            {
                foreach (var corrida in procesos.corridaCombinadamaestroNavigation)
                {
                    if (corrida.subordinado != null)
                    {
                        var subordinado = await _context.procesoOf
                            .Include(p => p.oFNavigation)
                            .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                        corrida.subordinadoNavigation = subordinado;
                    }
                }
            }

            if (procesos?.corridaCombinadasubordinadoNavigation?.subordinado != null)
            {
                var subordinado = await _context.procesoOf
                    .Include(p => p.oFNavigation)
                    .FirstOrDefaultAsync(p => p.idProceso == procesos.corridaCombinadasubordinadoNavigation.subordinado);

                procesos.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
            }

            var procesoOfDto = _mapper.Map<ProcesoOfDto>(procesos);

                if (procesoOfDto == null)
                {
                    return NotFound("No se encontro el proceso de la Of con el of: " + of);
                }

            return Ok(procesoOfDto);
        }

        // GET: api/procesoOf/5 LISTA
        [HttpGet("get/lista/oF/{of}")]
        public async Task<ActionResult<ListaProcesoOfDto>> GetprocesoOfTarjetaLista(int of)
        {
            // Procesos normales ligados a una OF
            var procesosNormales = await _context.procesoOf
                .Where(o => o.oF == of && o.archivada == false)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                    .ThenInclude(v => v.idAreaNavigation)
                .Include(c => c.idTableroNavigation)
                    .ThenInclude(m => m.idMaquinaNavigation)
                .Include(f => f.oFNavigation)
                .Include(n => n.tarjetaCampo)
                .Include(e => e.tarjetaEtiqueta)
                .Include(a => a.asignacion)
                    .ThenInclude(u => u.userNavigation)
                .Include(p => p.corridaCombinadamaestroNavigation)
                .Include(p => p.corridaCombinadasubordinadoNavigation)
                .ToListAsync();

            // Procesos maestros de corrida combinada (no ligados directamente a una OF)
            var procesosMaestros = await _context.procesoOf
                .Where(p => p.corridaCombinada == true && p.oF == null && p.archivada == false)
                .Include(p => p.corridaCombinadamaestroNavigation)
                    .ThenInclude(c => c.subordinadoNavigation)
                        .ThenInclude(s => s.oFNavigation)
                .Include(d => d.idPosturaNavigation)
                .Include(c => c.idTableroNavigation)
                    .ThenInclude(v => v.idAreaNavigation)
                .Include(c => c.idTableroNavigation)
                    .ThenInclude(m => m.idMaquinaNavigation)
                .Include(n => n.tarjetaCampo)
                .Include(e => e.tarjetaEtiqueta)
                .Include(a => a.asignacion)
                    .ThenInclude(u => u.userNavigation)
                .ToListAsync();

            // Filtramos solo los procesos maestros cuyos subordinados tengan la OF solicitada
            var procesosMaestrosConOf = procesosMaestros
                .Where(m => m.corridaCombinadamaestroNavigation
                    .Any(c => c.subordinadoNavigation?.oF == of))
                .ToList();

            // Combinamos ambos conjuntos
            var procesos = procesosNormales.Concat(procesosMaestrosConOf).ToList();

            foreach (var proceso in procesos)
            {
                // Cargar subordinados de corridaCombinadamaestroNavigation (lista 1:N)
                if (proceso.corridaCombinadamaestroNavigation != null)
                {
                    foreach (var corrida in proceso.corridaCombinadamaestroNavigation)
                    {
                        if (corrida.subordinado != null)
                        {
                            var subordinado = await _context.procesoOf
                                .Include(p => p.oFNavigation)
                                .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                            corrida.subordinadoNavigation = subordinado;
                        }
                    }
                }

                // Cargar subordinado de corridaCombinadasubordinadoNavigation (1:1)
                if (proceso.corridaCombinadasubordinadoNavigation?.subordinado != null)
                {
                    var subordinado = await _context.procesoOf
                        .Include(p => p.oFNavigation)
                        .FirstOrDefaultAsync(p => p.idProceso == proceso.corridaCombinadasubordinadoNavigation.subordinado);

                    proceso.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
                }
            }


            var dtos = new List<ListaProcesoOfDto>();

            foreach (var proceso in procesos)
            {
                var dto = _mapper.Map<ListaProcesoOfDto>(proceso);

                switch (proceso.tipoMaquinaSAP)
                {
                    case "preprensa":
                        var detallePreprensa = await _context.procesoPreprensa
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoPreprensaDto>(detallePreprensa);
                        break;

                    case "impresion":
                        var detalleImpresora = await _context.procesoImpresora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoImpresoraDto>(detalleImpresora);
                        break;

                    case "troquel":
                        var detalleTroqueladora = await _context.procesoTroqueladora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoTroqueladoraDto>(detalleTroqueladora);
                        break;

                    case "barniz":
                        var detalleBarnizadora = await _context.procesoBarniz
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoBarnizDto>(detalleBarnizadora);
                        break;

                    case "pegadora":
                        var detallePegadora = await _context.procesoPegadora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoPegadoraDto>(detallePegadora);
                        break;

                    case "acabado":
                        var detalleAcabado = await _context.procesoAcabado
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoAcabadoDto>(detalleAcabado);
                        break;                   

                    case "serigrafia":
                        var detalleSerigrafia = await _context.procesoSerigrafia
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoSerigrafiaDto>(detalleSerigrafia);
                        break;

                    case "impresionFlexo":
                        var detalleImpresionFlexo = await _context.procesoImpresoraFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoImpresoraFlexoDto>(detalleImpresionFlexo);
                        break;

                    case "acabadoFlexo":
                        var detalleAcabadoFlexo = await _context.procesoAcabadoFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoAcabadoFlexoDto>(detalleAcabadoFlexo);
                        break;

                    case "mangaFlexo":
                        var detalleMangaFlexo = await _context.procesoMangaFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoMangaFlexoDto>(detalleMangaFlexo);
                        break;

                        case "procesosFlexo":
                        var detalleProcesosFlexo = await _context.procesosFlexo
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesosFlexoDto>(detalleProcesosFlexo);
                        break;

                    default:
                        dto.DetalleProceso = null;
                        break;
                }

                dtos.Add(dto);
            }
            // Ordenamos por secuenciaArea
            dtos = dtos.OrderBy(d => d.secuenciaArea ?? 999).ToList();

            return Ok(dtos);
        }

        // GET: api/procesoOf
        [HttpGet("get/tablero/{id}")]
        public async Task<ActionResult<IEnumerable<ProcesoOfVistaTableroDto>>> GetprocesoOfTablero(int id)
        {
            var procesos = await _context.procesoOf
    .OrderBy(p => p.posicion)
    .Where(t => t.idTablero == id && t.archivada == false)
    .Include(u => u.detalleReporte)
        .ThenInclude(o => o.idOperacionNavigation)
    .Include(u => u.detalleReporte)
        .ThenInclude(m => m.maquinaNavigation)
    .Include(m => m.tarjetaCampo)
    .Include(s => s.tarjetaEtiqueta)
        .ThenInclude(e => e.idEtiquetaNavigation)
    .Include(f => f.oFNavigation)
    .Include(l => l.idPosturaNavigation)
    .Include(v => v.idMaterialNavigation)
    .Include(a => a.asignacion)
    .ThenInclude(u => u.userNavigation)
    .Include(p => p.corridaCombinadamaestroNavigation)
    .Include(p => p.corridaCombinadasubordinadoNavigation)
    .ToListAsync();

            foreach (var proceso in procesos)
            {
                // Cargar subordinados de corridaCombinadamaestroNavigation (lista 1:N)
                if (proceso.corridaCombinadamaestroNavigation != null)
                {
                    foreach (var corrida in proceso.corridaCombinadamaestroNavigation)
                    {
                        if (corrida.subordinado != null)
                        {
                            var subordinado = await _context.procesoOf
                                .Include(p => p.oFNavigation)
                                .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                            corrida.subordinadoNavigation = subordinado;
                        }
                    }
                }

                // Cargar subordinado de corridaCombinadasubordinadoNavigation (1:1)
                if (proceso.corridaCombinadasubordinadoNavigation?.subordinado != null)
                {
                    var subordinado = await _context.procesoOf
                        .Include(p => p.oFNavigation)
                        .FirstOrDefaultAsync(p => p.idProceso == proceso.corridaCombinadasubordinadoNavigation.subordinado);

                    proceso.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
                }
            }


            var dtos = new List<ProcesoOfVistaTableroDto>();

            foreach (var proceso in procesos)
            {
                var dto = _mapper.Map<ProcesoOfVistaTableroDto>(proceso);

                switch (proceso.tipoMaquinaSAP)
                {
                    case "impresion":
                        var detalleImpresora = await _context.procesoImpresora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoImpresoraDto>(detalleImpresora);
                        break;

                    case "troquel":
                        var detalleTroqueladora = await _context.procesoTroqueladora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoTroqueladoraDto>(detalleTroqueladora);
                        break;

                    case "barniz":
                        var detalleBarnizadora = await _context.procesoBarniz
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoBarnizDto>(detalleBarnizadora);
                        break;

                    case "pegadora":
                        var detallePegadora = await _context.procesoPegadora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoPegadoraDto>(detallePegadora);
                        break;

                    case "acabado":
                        var detalleAcabado = await _context.procesoAcabado
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoAcabadoDto>(detalleAcabado);
                        break;

                    case "preprensa":
                        var detallePreprensa = await _context.procesoPreprensa
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoPreprensaDto>(detallePreprensa);
                        break;

                        case "serigrafia":
                        var detalleSerigrafia = await _context.procesoSerigrafia
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoSerigrafiaDto>(detalleSerigrafia);
                        break;

                        case "impresionFlexo":
                        var detalleImpresionFlexo = await _context.procesoImpresoraFlexo
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoImpresoraFlexoDto>(detalleImpresionFlexo);
                        break;

                        case "acabadoFlexo":
                        var detalleAcabadoFlexo = await _context.procesoAcabadoFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoAcabadoFlexoDto>(detalleAcabadoFlexo);
                        break;

                        case "mangaFlexo":
                        var detalleMangaFlexo = await _context.procesoMangaFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoMangaFlexoDto>(detalleMangaFlexo);
                        break;

                        case "procesosFlexo":
                        var detalleProcesosFlexo = await _context.procesosFlexo
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesosFlexoDto>(detalleProcesosFlexo);
                        break;

                    default:
                        dto.DetalleProceso = null;
                        break;
                }

                dtos.Add(dto);
            }

            return Ok(dtos);
        }

        // GET: api/procesoOf
        [HttpGet("get/tablero/user/{user}")]
        public async Task<ActionResult<IEnumerable<ProcesoOfVistaTableroDto>>> GetprocesoOfTableroUser(string user)
        {
            var procesos = await _context.procesoOf
                .OrderBy(p => p.posicion)
                .Where(t => t.asignacion.Any(u => u.user == user) && t.archivada == false)
                .Include(u => u.detalleReporte)
                .ThenInclude(o => o.idOperacionNavigation)
                .Include(m => m.tarjetaCampo)
                .Include(s => s.tarjetaEtiqueta)
                .ThenInclude(e => e.idEtiquetaNavigation)
                .Include(f => f.oFNavigation)
                .Include(l => l.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(a => a.asignacion)
                .ThenInclude(u => u.userNavigation)
                .Include(p => p.corridaCombinadamaestroNavigation)
                .Include(p => p.corridaCombinadasubordinadoNavigation)
                .ToListAsync();

            foreach (var proceso in procesos)
            {
                // Cargar subordinados de corridaCombinadamaestroNavigation (lista 1:N)
                if (proceso.corridaCombinadamaestroNavigation != null)
                {
                    foreach (var corrida in proceso.corridaCombinadamaestroNavigation)
                    {
                        if (corrida.subordinado != null)
                        {
                            var subordinado = await _context.procesoOf
                                .Include(p => p.oFNavigation)
                                .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                            corrida.subordinadoNavigation = subordinado;
                        }
                    }
                }

                // Cargar subordinado de corridaCombinadasubordinadoNavigation (1:1)
                if (proceso.corridaCombinadasubordinadoNavigation?.subordinado != null)
                {
                    var subordinado = await _context.procesoOf
                        .Include(p => p.oFNavigation)
                        .FirstOrDefaultAsync(p => p.idProceso == proceso.corridaCombinadasubordinadoNavigation.subordinado);

                    proceso.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
                }
            }


            var dtos = new List<ProcesoOfVistaTableroDto>();

            foreach (var proceso in procesos)
            {
                var dto = _mapper.Map<ProcesoOfVistaTableroDto>(proceso);

                switch (proceso.tipoMaquinaSAP)
                {
                    case "impresion":
                        var detalleImpresora = await _context.procesoImpresora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoImpresoraDto>(detalleImpresora);
                        break;

                    case "troquel":
                        var detalleTroqueladora = await _context.procesoTroqueladora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoTroqueladoraDto>(detalleTroqueladora);
                        break;

                    case "barniz":
                        var detalleBarnizadora = await _context.procesoBarniz
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoBarnizDto>(detalleBarnizadora);
                        break;

                    case "pegadora":
                        var detallePegadora = await _context.procesoPegadora
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoPegadoraDto>(detallePegadora);
                        break;

                    case "acabado":
                        var detalleAcabado = await _context.procesoAcabado
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoAcabadoDto>(detalleAcabado);
                        break;

                    case "preprensa":
                        var detallePreprensa = await _context.procesoPreprensa
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoPreprensaDto>(detallePreprensa);
                        break;

                    case "serigrafia":
                        var detalleSerigrafia = await _context.procesoSerigrafia
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoSerigrafiaDto>(detalleSerigrafia);
                        break;

                    case "impresionFlexo":
                        var detalleImpresionFlexo = await _context.procesoImpresoraFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoImpresoraFlexoDto>(detalleImpresionFlexo);
                        break;

                    case "acabadoFlexo":
                        var detalleAcabadoFlexo = await _context.procesoAcabadoFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoAcabadoFlexoDto>(detalleAcabadoFlexo);
                        break;

                    case "mangaFlexo":
                        var detalleMangaFlexo = await _context.procesoMangaFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesoMangaFlexoDto>(detalleMangaFlexo);
                        break;

                    case "procesosFlexo":
                        var detalleProcesosFlexo = await _context.procesosFlexo
                            .Where(p => p.idProceso == proceso.idProceso)
                            .FirstOrDefaultAsync();
                        dto.DetalleProceso = _mapper.Map<ProcesosFlexoDto>(detalleProcesosFlexo);
                        break;

                    default:
                        dto.DetalleProceso = null;
                        break;
                }

                dtos.Add(dto);
            }

            return Ok(dtos);
        }

        // CONTROLADOR DINAMICO PARA OBTENER LOS DETALLES POR MAQUINA

        [HttpGet("get/procesoOfMaquina/{id}")]
        public async Task<ActionResult<ProcesoOfMaquinas>> GetProcesoOf(int id)
        {
            // Cargar datos generales del proceso
            var proceso = await _context.procesoOf
                .Include(p => p.idPosturaNavigation)
                .Include(v => v.idMaterialNavigation)
                .Include(f => f.oFNavigation)
                .Include(p => p.corridaCombinadamaestroNavigation)
                .Include(p => p.corridaCombinadasubordinadoNavigation)
                .FirstOrDefaultAsync(u => u.idProceso == id);

            // Cargar subordinadoNavigation manualmente
            if (proceso?.corridaCombinadamaestroNavigation != null)
            {
                foreach (var corrida in proceso.corridaCombinadamaestroNavigation)
                {
                    if (corrida.subordinado != null)
                    {
                        var subordinado = await _context.procesoOf
                            .Include(p => p.oFNavigation)
                            .FirstOrDefaultAsync(p => p.idProceso == corrida.subordinado);

                        corrida.subordinadoNavigation = subordinado;
                    }
                }
            }

            if (proceso?.corridaCombinadasubordinadoNavigation?.subordinado != null)
            {
                var subordinado = await _context.procesoOf
                    .Include(p => p.oFNavigation)
                    .FirstOrDefaultAsync(p => p.idProceso == proceso.corridaCombinadasubordinadoNavigation.subordinado);

                proceso.corridaCombinadasubordinadoNavigation.subordinadoNavigation = subordinado;
            }

            if (proceso == null) return NotFound();

            // Mapear el modelo general al DTO principal
            var dto = _mapper.Map<ProcesoOfMaquinas>(proceso);

            // Cargar detalles específicos según el tipo de máquina
            switch (proceso.tipoMaquinaSAP)
            {
                case "impresion":
                    var detalleImpresora = await _context.procesoImpresora
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoImpresoraDto>(detalleImpresora);
                    break;

                case "troquel":
                    var detalleTroqueladora = await _context.procesoTroqueladora
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoTroqueladoraDto>(detalleTroqueladora);
                    break;

                case "barniz":
                    var detalleBarnizadora = await _context.procesoBarniz
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoBarnizDto>(detalleBarnizadora);
                    break;

                case "pegadora":
                    var detallePegadora = await _context.procesoPegadora
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoPegadoraDto>(detallePegadora);
                    break;

                case "acabado":
                    var detalleAcabado = await _context.procesoAcabado
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoAcabadoDto>(detalleAcabado);
                    break;

                case "preprensa":
                    var detallePreprensa = await _context.procesoPreprensa
                        .Where(p => p.idProceso == id)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoPreprensaDto>(detallePreprensa);
                    break;
                case "serigrafia":
                    var detalleSerigrafia = await _context.procesoSerigrafia
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoSerigrafiaDto>(detalleSerigrafia);
                    break;

                case "impresionFlexo":
                    var detalleImpresionFlexo = await _context.procesoImpresoraFlexo
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoImpresoraFlexoDto>(detalleImpresionFlexo);
                    break;

                case "acabadoFlexo":
                    var detalleAcabadoFlexo = await _context.procesoAcabadoFlexo
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoAcabadoFlexoDto>(detalleAcabadoFlexo);
                    break;

                case "mangaFlexo":
                    var detalleMangaFlexo = await _context.procesoMangaFlexo
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesoMangaFlexoDto>(detalleMangaFlexo);
                    break;

                case "procesosFlexo":
                    var detalleProcesosFlexo = await _context.procesosFlexo
                        .Where(p => p.idProceso == proceso.idProceso)
                        .FirstOrDefaultAsync();
                    dto.DetalleProceso = _mapper.Map<ProcesosFlexoDto>(detalleProcesosFlexo);
                    break;

                default:
                    dto.DetalleProceso = null;
                    break;
            }

            return Ok(dto);
        }

        // PUT: api/procesoOf/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutprocesoOf(int id, UpdateProcesoOfDto updateProcesoOf)
        {
            var procesoOf = await _context.procesoOf.FindAsync(id);

            if (procesoOf == null)
            {
                return NotFound("No se encontro el proceso de la of con el id: " + id);
            }

            _mapper.Map(updateProcesoOf, procesoOf);
            _context.Entry(procesoOf).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!procesoOfExists(id))
                {
                    return NotFound("No se encontro el proceso de la of con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateProcesoOf);
        }

        [HttpPut("put/BatchUpdate")]
        public async Task<IActionResult> BatchUpdateProcesos([FromBody] BatchUpdateProcesoOfDto batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.ProcesosOf == null || !batchUpdateDto.ProcesosOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.ProcesosOf.Select(t => t.idProceso).ToList();

            // Obtener todas los procesos of relacionadas
            var procesos = await _context.procesoOf.Where(t => ids.Contains(t.idProceso)).ToListAsync();

            if (!procesos.Any())
            {
                return NotFound("No se encontraron procesos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.ProcesosOf)
            {
                var proceso = procesos.FirstOrDefault(t => t.idProceso == dto.idProceso);
                if (proceso != null)
                {
                    if (dto.idProceso > 0)
                    {
                        proceso.posicion = dto.posicion;
                    }

                    _context.Entry(proceso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los procesos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        [HttpPut("put/BatchUpdateArchivada")]
        public async Task<IActionResult> BatchUpdateProcesosArchivo([FromBody] BatchUpdateArchivadaOf batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.archivadaProcesosOf == null || !batchUpdateDto.archivadaProcesosOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.archivadaProcesosOf.Select(t => t.idProceso).ToList();

            // Obtener todas los procesos of relacionadas
            var procesos = await _context.procesoOf.Where(t => ids.Contains(t.idProceso)).ToListAsync();

            if (!procesos.Any())
            {
                return NotFound("No se encontraron procesos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.archivadaProcesosOf)
            {
                var proceso = procesos.FirstOrDefault(t => t.idProceso == dto.idProceso);
                if (proceso != null)
                {
                    if (dto.idProceso > 0)
                    {
                        proceso.archivada = dto.archivada;
                        proceso.cancelada = dto.cancelada;
                    }

                    _context.Entry(proceso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los procesos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        [HttpPut("put/BatchUpdateSAP")]
        public async Task<IActionResult> BatchUpdateProcesosSAP([FromBody] ListSAPUpdateProcesoOf batchUpdateDtoSAP)
        {
            if (batchUpdateDtoSAP == null || batchUpdateDtoSAP.SAPProcesoOf == null || !batchUpdateDtoSAP.SAPProcesoOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDtoSAP.SAPProcesoOf.Select(t => t.idProceso).Distinct().ToList();

            // Obtener procesos relacionados
            var procesos = await _context.procesoOf.Where(t => ids.Contains(t.idProceso)).ToListAsync();

            if (!procesos.Any())
            {
                return NotFound("No se encontraron procesos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDtoSAP.SAPProcesoOf)
            {

                var proceso = procesos.FirstOrDefault(t => t.idProceso == dto.idProceso);
                if (proceso != null)
                {
                    if (dto.posicion.HasValue)
                    {
                        proceso.posicion = dto.posicion.Value;
                    }
                    _context.Entry(proceso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los Procesos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        [HttpPut("put/BatchUpdateMaquina")]
        public async Task<IActionResult> BatchUpdateProcesosMaquina([FromBody] BatchUpdateMaquinaProcesoOF batchUpdateDto)
        {
            if (batchUpdateDto == null || batchUpdateDto.updateMaquinaProcesoOf == null || !batchUpdateDto.updateMaquinaProcesoOf.Any())
            {
                return BadRequest("No se enviaron datos para actualizar.");
            }

            var ids = batchUpdateDto.updateMaquinaProcesoOf.Select(t => t.idProceso).ToList();

            // Obtener todas los procesos of relacionadas
            var procesos = await _context.procesoOf.Where(t => ids.Contains(t.idProceso)).ToListAsync();

            if (!procesos.Any())
            {
                return NotFound("No se encontraron procesos para los IDs proporcionados.");
            }

            foreach (var dto in batchUpdateDto.updateMaquinaProcesoOf)
            {
                var proceso = procesos.FirstOrDefault(t => t.idProceso == dto.idProceso);
                if (proceso != null)
                {
                    if (dto.idProceso > 0)
                    {
                        proceso.idTablero = dto.idTablero;
                        proceso.idPostura = dto.idPostura;
                        proceso.posicion = dto.posicion;
                        proceso.fechaActualización = dto.fechaActualización;
                        proceso.comentario = dto.comentario;
                        proceso.actualizadoPor = dto.actualizadoPor;
                    }

                    _context.Entry(proceso).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los procesos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        [HttpPut("put/procesoOfMaquina/{idProceso}")]
        public async Task<IActionResult> UpdateProcesoOf(int idProceso, [FromBody] UpProcesoOfMaquinas dto)
        {
            // Buscar el proceso general existente
            var procesoExistente = await _context.procesoOf.FindAsync(idProceso);
            if (procesoExistente == null)
            {
                return NotFound("El proceso con el ID proporcionado no existe.");
            }

            // Actualizar los campos del proceso general
            _mapper.Map(dto, procesoExistente);
            _context.procesoOf.Update(procesoExistente);

            // Verificar si se necesita actualizar el detalle según el tipo de máquina
            if (!string.IsNullOrEmpty(dto.tipoMaquinaSAP) && dto.DetalleProceso != null)
            {
                switch (dto.tipoMaquinaSAP)
                {
                    case "preprensa":
                        await ActualizarDetalleMaquina<procesoPreprensa, UpProcesoPreprensaDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "impresion":
                        await ActualizarDetalleMaquina<procesoImpresora, UpProcesoImpresoraDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "troquel":
                        await ActualizarDetalleMaquina<procesoTroqueladora, UpProcesoTroqueladoDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "pegadora":
                        await ActualizarDetalleMaquina<procesoPegadora, UpProcesoPegadoraDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "acabado":
                        await ActualizarDetalleMaquina<procesoAcabado, UpProcesoAcabadoDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "barniz":
                        await ActualizarDetalleMaquina<procesoBarniz, UpProcesoBarnizDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "serigrafia":
                        await ActualizarDetalleMaquina<procesoSerigrafia, UpProcesoSerigrafia>(idProceso, dto.DetalleProceso);
                        break;

                    case "impresionFlexo":
                        await ActualizarDetalleMaquina<procesoImpresoraFlexo, UpProcesoImpresoraFlexoDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "acabadoFlexo":
                        await ActualizarDetalleMaquina<procesoAcabadoFlexo, UpProcesoAcabadoFlexoDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "mangaFlexo":
                        await ActualizarDetalleMaquina<procesoMangaFlexo, UpProcesoMangaFlexoDto>(idProceso, dto.DetalleProceso);
                        break;

                    case "procesosFlexo":
                        await ActualizarDetalleMaquina<procesosFlexo, UpProcesosFlexoDto>(idProceso, dto.DetalleProceso);
                        break;

                    default:
                        return BadRequest("Tipo de máquina no soportado.");
                }
            }

            // Guardar cambios en la base de datos
            await _context.SaveChangesAsync();

            return Ok("Proceso Of actualizado exitosamente.");
        }

        // POST: api/procesoOf
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("post")]
        public async Task<ActionResult<procesoOf>> PostprocesoOf(AddProcesoOfDto addProcesoOf)
        {
            // Supongamos que oF debe ser único
            var lockKey = $"post-proceso-of-{addProcesoOf.oF}";

            using var _ = await _lockService.AcquireLockAsync(lockKey);

            var procesoOf = _mapper.Map<procesoOf>(addProcesoOf);
            _context.procesoOf.Add(procesoOf);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetprocesoOf", new { id = procesoOf.idProceso }, procesoOf);
        }


        // POST: BATCH
        [HttpPost("post/BatchAdd")]
        public async Task<IActionResult> BatchAddProcesoOf([FromBody] BatchAddProcesoOf batchAddDto)
        {
            if (batchAddDto == null || batchAddDto.addBatchProcesoDto == null || !batchAddDto.addBatchProcesoDto.Any())
            {
                return BadRequest("No se enviaron datos para agregar.");
            }

            // Mapear los DTOs a las entidades
            var procesos = batchAddDto.addBatchProcesoDto.Select(dto => _mapper.Map<procesoOf>(dto)).ToList();

            // Agregar los procesos a la base de datos
            await _context.procesoOf.AddRangeAsync(procesos);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar los procesos: {ex.Message}");
            }

            // Retornar los registros creados
            return Ok(new
            {
                Message = "Procesos agregados exitosamente.",
                ProcesosAgregados = procesos
            });
        }

        // POST PARA DETALLES POR MAQUINA
        [HttpPost("post/procesoOfMaquina")]
        public async Task<ActionResult> CreateProcesoOf(AddProcesoOfMaquinas dto)
        {
            // Validar que el DTO contenga los datos necesarios
            if (dto.DetalleProceso == null || string.IsNullOrEmpty(dto.tipoMaquinaSAP))
            {
                return BadRequest("El detalle del proceso o el tipo de máquina son inválidos.");
            }

            // Mapear el DTO al modelo general
            var proceso = _mapper.Map<procesoOf>(dto);

            // Guardar los datos generales en la tabla `procesoOf`
            _context.procesoOf.Add(proceso);
            await _context.SaveChangesAsync();

            // Insertar los detalles específicos según el tipo de máquina
            switch (dto.tipoMaquinaSAP) // Convertir a minúsculas por seguridad
            {
                case "preprensa":
                    if (dto.DetalleProceso is JsonElement jsonPreprensa)
                    {
                        var detallePreprensa = JsonSerializer.Deserialize<procesoPreprensa>(jsonPreprensa.GetRawText());
                        if (detallePreprensa != null)
                        {
                            detallePreprensa.idProceso = proceso.idProceso;
                            _context.procesoPreprensa.Add(detallePreprensa);
                        }
                    }
                    break;

                case "impresion":
                    if (dto.DetalleProceso is JsonElement jsonImpresion)
                    {
                        var detalleImpresora = JsonSerializer.Deserialize<procesoImpresora>(jsonImpresion.GetRawText());
                        if (detalleImpresora != null)
                        {
                            detalleImpresora.idProceso = proceso.idProceso;
                            _context.procesoImpresora.Add(detalleImpresora);
                        }
                    }
                    break;

                case "troquel":
                    if (dto.DetalleProceso is JsonElement jsonTroquel)
                    {
                        var detalleTroqueladora = JsonSerializer.Deserialize<procesoTroqueladora>(jsonTroquel.GetRawText());
                        if (detalleTroqueladora != null)
                        {
                            detalleTroqueladora.idProceso = proceso.idProceso;
                            _context.procesoTroqueladora.Add(detalleTroqueladora);
                        }
                    }
                    break;

                case "pegadora":
                    if (dto.DetalleProceso is JsonElement jsonPegadora)
                    {
                        var detallePegadora = JsonSerializer.Deserialize<procesoPegadora>(jsonPegadora.GetRawText());
                        if (detallePegadora != null)
                        {
                            detallePegadora.idProceso = proceso.idProceso;
                            _context.procesoPegadora.Add(detallePegadora);
                        }
                    }
                    break;

                case "acabado":
                    if (dto.DetalleProceso is JsonElement jsonAcabado)
                    {
                        var detalleAcabado = JsonSerializer.Deserialize<procesoAcabado>(jsonAcabado.GetRawText());
                        if (detalleAcabado != null)
                        {
                            detalleAcabado.idProceso = proceso.idProceso;
                            _context.procesoAcabado.Add(detalleAcabado);
                        }
                    }
                    break;

                case "barniz":
                    if (dto.DetalleProceso is JsonElement jsonBarniz)
                    {
                        var detalleBarniz = JsonSerializer.Deserialize<procesoBarniz>(jsonBarniz.GetRawText());
                        if (detalleBarniz != null)
                        {
                            detalleBarniz.idProceso = proceso.idProceso;
                            _context.procesoBarniz.Add(detalleBarniz);
                        }
                    }
                    break;

                case "serigrafia":
                    if (dto.DetalleProceso is JsonElement jsonSeriagrafia)
                    {
                        var detalleSerigrafia = JsonSerializer.Deserialize<procesoSerigrafia>(jsonSeriagrafia.GetRawText());
                        if (detalleSerigrafia != null)
                        {
                            detalleSerigrafia.idProceso = proceso.idProceso;
                            _context.procesoSerigrafia.Add(detalleSerigrafia);
                        }
                    }
                    break;

                    case "impresionFlexo":
                    if (dto.DetalleProceso is JsonElement jsonImpresionFlexo)
                    {
                        var detalleImpresionFlexo = JsonSerializer.Deserialize<procesoImpresoraFlexo>(jsonImpresionFlexo.GetRawText());
                        if (detalleImpresionFlexo != null)
                        {
                            detalleImpresionFlexo.idProceso = proceso.idProceso;
                            _context.procesoImpresoraFlexo.Add(detalleImpresionFlexo);
                        }
                    }
                    break;

                    case "acabadoFlexo":
                    if (dto.DetalleProceso is JsonElement jsonAcabadoFlexo)
                    {
                        var detalleAcabadoFlexo = JsonSerializer.Deserialize<procesoAcabadoFlexo>(jsonAcabadoFlexo.GetRawText());
                        if (detalleAcabadoFlexo != null)
                        {
                            detalleAcabadoFlexo.idProceso = proceso.idProceso;
                            _context.procesoAcabadoFlexo.Add(detalleAcabadoFlexo);
                        }
                    }
                    break;

                    case "mangaFlexo":
                    if (dto.DetalleProceso is JsonElement jsonMangaFlexo)
                    {
                        var detalleMangaFlexo = JsonSerializer.Deserialize<procesoMangaFlexo>(jsonMangaFlexo.GetRawText());
                        if (detalleMangaFlexo != null)
                        {
                            detalleMangaFlexo.idProceso = proceso.idProceso;
                            _context.procesoMangaFlexo.Add(detalleMangaFlexo);
                        }
                    }
                    break;

                    case "procesosFlexo":
                    if (dto.DetalleProceso is JsonElement jsonProcesosFlexo)
                    {
                        var detalleProcesosFlexo = JsonSerializer.Deserialize<procesosFlexo>(jsonProcesosFlexo.GetRawText());
                        if (detalleProcesosFlexo != null)
                        {
                            detalleProcesosFlexo.idProceso = proceso.idProceso;
                            _context.procesosFlexo.Add(detalleProcesosFlexo);
                        }
                    }
                    break;

                default:
                    return BadRequest("Tipo de máquina no soportado.");
            }

            // Correlativo para Corrida Combinada
            if (proceso.corridaCombinada == true)
            {
                var ultimos = await _context.procesoOf
                    .Where(p => p.corridaCombinada == true && p.correlativoCC != null)
                    .OrderByDescending(p => p.correlativoCC)
                    .Select(p => p.correlativoCC)
                    .ToListAsync();

                int ultimoNumero = 0;
                if (ultimos.Any())
                {
                    var ultimo = ultimos.First();
                    int.TryParse(ultimo, out ultimoNumero);
                }

                proceso.correlativoCC = (ultimoNumero + 1).ToString("D5");
            }


            await _context.SaveChangesAsync();
            return Ok(new { idProceso = proceso.idProceso });

        }

        [HttpPost("post/procesoOfMaquina/batch")]
        public async Task<ActionResult> CreateProcesoOfBatch(List<AddProcesoOfMaquinas> dtos)
        {
            if (dtos == null || !dtos.Any())
            {
                return BadRequest("La lista de procesos no puede estar vacía.");
            }

            var resultados = new List<BatchResult>();
            using var transaction = await _context.Database.BeginTransactionAsync();

            try
            {
                foreach (var dto in dtos)
                {
                    var resultadoItem = new BatchResult { OF = dto.oF, TipoMaquina = dto.tipoMaquinaSAP };

                    try
                    {
                        // Validación básica
                        if (dto.DetalleProceso == null || string.IsNullOrEmpty(dto.tipoMaquinaSAP))
                        {
                            resultadoItem.Estatus = "Error";
                            resultadoItem.Mensaje = "Detalle del proceso o tipo de máquina inválidos";
                            resultados.Add(resultadoItem);
                            continue;
                        }

                        // Crear proceso principal
                        var proceso = _mapper.Map<procesoOf>(dto);
                        _context.procesoOf.Add(proceso);
                        await _context.SaveChangesAsync(); // Guardar para obtener ID

                        // Procesar detalle
                        var (procesadoExitoso, mensajeError) = await ProcesarDetalleMaquina(dto, proceso.idProceso);

                        if (!procesadoExitoso)
                        {
                            resultadoItem.Estatus = "Error";
                            resultadoItem.Mensaje = mensajeError ?? "Tipo de máquina no soportado o datos inválidos";
                        }
                        else
                        {
                            resultadoItem.Estatus = "Éxito";
                            resultadoItem.Mensaje = "Proceso creado correctamente";
                            resultadoItem.IdProceso = proceso.idProceso;
                        }
                    }
                    catch (Exception ex)
                    {
                        resultadoItem.Estatus = "Error";
                        resultadoItem.Mensaje = $"Error interno: {ex.Message}";
                    }

                    resultados.Add(resultadoItem);
                }

                await transaction.CommitAsync();
                return Ok(new
                {
                    TotalProcesados = dtos.Count,
                    Exitosos = resultados.Count(r => r.Estatus == "Éxito"),
                    Fallidos = resultados.Count(r => r.Estatus == "Error"),
                    Detalles = resultados
                });
            }
            catch (Exception ex)
            {
                await transaction.RollbackAsync();
                return StatusCode(500, $"Error general en el procesamiento batch: {ex.Message}");
            }
        }

        private async Task<(bool Success, string? ErrorMessage)> ProcesarDetalleMaquina(AddProcesoOfMaquinas dto, int idProceso)
        {
            if (dto.DetalleProceso is not JsonElement jsonElement)
                return (false, "Formato de detalle inválido");

            try
            {
                object? detalle = dto.tipoMaquinaSAP switch
                {
                    "preprensa" => JsonSerializer.Deserialize<procesoPreprensa>(jsonElement.GetRawText()),
                    "impresion" => JsonSerializer.Deserialize<procesoImpresora>(jsonElement.GetRawText()),
                    "troquel" => JsonSerializer.Deserialize<procesoTroqueladora>(jsonElement.GetRawText()),
                    "pegadora" => JsonSerializer.Deserialize<procesoPegadora>(jsonElement.GetRawText()),
                    "acabado" => JsonSerializer.Deserialize<procesoAcabado>(jsonElement.GetRawText()),
                    "barniz" => JsonSerializer.Deserialize<procesoBarniz>(jsonElement.GetRawText()),
                    "serigrafia" => JsonSerializer.Deserialize<procesoSerigrafia>(jsonElement.GetRawText()),
                    "impresionflexo" => JsonSerializer.Deserialize<procesoImpresoraFlexo>(jsonElement.GetRawText()),
                    "acabadoflexo" => JsonSerializer.Deserialize<procesoAcabadoFlexo>(jsonElement.GetRawText()),
                    "mangaflexo" => JsonSerializer.Deserialize<procesoMangaFlexo>(jsonElement.GetRawText()),
                    "procesosflexo" => JsonSerializer.Deserialize<procesosFlexo>(jsonElement.GetRawText()),
                    _ => null
                };

                if (detalle == null)
                    return (false, "Tipo de máquina no soportado");

                // Asignar el idProceso usando reflexión
                var prop = detalle.GetType().GetProperty("idProceso");
                prop?.SetValue(detalle, idProceso);

                // Agregar al contexto
                await _context.AddAsync(detalle);
                await _context.SaveChangesAsync(); // Guardar cambios para el detalle

                return (true, null);
            }
            catch (JsonException ex)
            {
                return (false, $"Error al deserializar detalle: {ex.Message}");
            }
            catch (Exception ex)
            {
                return (false, $"Error inesperado: {ex.Message}");
            }
        }

        public class BatchResult
        {
            public int? IdProceso { get; set; }
            public int? OF { get; set; }
            public string? TipoMaquina { get; set; }
            public string? Estatus { get; set; }
            public string? Mensaje { get; set; }
        }

        private bool procesoOfExists(int id)
        {
            return _context.procesoOf.Any(e => e.idProceso == id);
        }

        private async Task ActualizarDetalleMaquina<TEntity, TDto>(int idProceso, object detalleProceso)
    where TEntity : class
    where TDto : class
        {
            // Buscar el detalle existente en la base de datos
            var detalleExistente = await _context.Set<TEntity>().FirstOrDefaultAsync(e => EF.Property<int>(e, "idProceso") == idProceso);

            if (detalleExistente == null)
            {
                // Si no existe, se crea un nuevo detalle
                var nuevoDetalle = JsonSerializer.Deserialize<TDto>(detalleProceso.ToString());
                if (nuevoDetalle != null)
                {
                    var entidad = _mapper.Map<TEntity>(nuevoDetalle);
                    _context.Set<TEntity>().Add(entidad);
                }
            }
            else
            {
                // Si existe, se actualiza el detalle existente
                var detalleDto = JsonSerializer.Deserialize<TDto>(detalleProceso.ToString());
                if (detalleDto != null)
                {
                    _mapper.Map(detalleDto, detalleExistente);
                    _context.Set<TEntity>().Update(detalleExistente);
                }
            }
        }

    }
}
