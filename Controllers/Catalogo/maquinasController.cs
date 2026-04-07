using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.Catalogo
{
    [Route("api/[controller]")]
    [ApiController]
    public class maquinasController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public maquinasController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/maquinas
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<MaquinaDto>>> Getmaquinas()
        {
            var maquina = await _context.maquinas
                .Include(m => m.idFamiliaNavigation)
                .Include(li => li.listaMaquina)
                .ThenInclude(lo => lo.idListaNavigation)
                .Include(a => a.idFamiliaNavigation.idAreaNavigation)
                .Include(u => u.idUsoTipico)
                .Include(tp => tp.idTipoPapel)
                .Include(ta => ta.idTipoAcabado)
                .ToListAsync();
            var maquinaDto = _mapper.Map<List<MaquinaDto>>(maquina);

            return Ok(maquinaDto);
        }

        // GET: api/maquinas/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<MaquinaDto>> Getmaquinas(int id)
        {
            var maquinas = await _context.maquinas
                .Include(m => m.idFamiliaNavigation)
                .Include(li => li.listaMaquina)
                .ThenInclude(lo => lo.idListaNavigation)
                .Include(a => a.idFamiliaNavigation.idAreaNavigation)
                .Include(u => u.idUsoTipico)
                .Include(tp => tp.idTipoPapel)
                .Include(ta => ta.idTipoAcabado)
                .FirstOrDefaultAsync(u => u.idMaquina == id);

            var maquinaDto = _mapper.Map<MaquinaDto>(maquinas);

            if (maquinaDto == null)
            {
                return NotFound("No se encontro la maquina con ID: " + id);
            }

            return Ok(maquinaDto);
        }

        // GET: api/maquinas/get/of/5
        [HttpGet("get/of/{numeroOf}")]
        public async Task<ActionResult<List<MaquinaOfDto>>> GetMaquinasPorOf(int numeroOf)
        {
            try
            {
                // 1. Buscamos los IDs de las máquinas únicas asignadas a los procesos de esta OF.
                // Asumiendo que en procesoOf tienes un campo que guarda el número de OF (ej. numeroOf)
                // y una propiedad de navegación hacia tablero (ej. tableroNavigation).
                var idsMaquinas = await _context.procesoOf
                    .Where(p => p.oF == numeroOf) // Ojo: Ajusta 'numeroOF' al nombre real de tu campo
                    .Select(p => p.idTableroNavigation.idMaquina) // Navegamos del proceso al tablero y sacamos el idMaquina
                    .Distinct() // ¡Magia! Esto asegura que no se repita ninguna máquina en la lista
                    .ToListAsync();

                // Si la orden no existe o no tiene máquinas, devolvemos 404
                if (idsMaquinas == null || !idsMaquinas.Any())
                {
                    return NotFound($"No se encontraron máquinas asignadas a los procesos de la OF: {numeroOf}");
                }

                // 2. Traemos la información completa de las máquinas usando los IDs únicos que encontramos
                var maquinas = await _context.maquinas
                    .Include(m => m.idFamiliaNavigation)
                        .ThenInclude(f => f.idAreaNavigation) // Sintaxis correcta para incluir "hijos de los hijos"
                    .Include(m => m.listaMaquina)
                        .ThenInclude(l => l.idListaNavigation)
                    .Include(m => m.idUsoTipico)
                    .Include(m => m.idTipoPapel)
                    .Include(m => m.idTipoAcabado)
                    .Where(m => idsMaquinas.Contains(m.idMaquina)) // Filtramos solo por las máquinas de la OF
                    .ToListAsync(); // Usamos ToListAsync para traer toda la colección

                // 3. Mapeamos la lista de Entidades a una Lista de DTOs
                var maquinasDto = _mapper.Map<List<MaquinaOfDto>>(maquinas);

                return Ok(maquinasDto);
            }
            catch (Exception ex)
            {
                // Siempre es bueno atrapar errores inesperados
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al consultar las máquinas: " + ex.Message);
            }
        }

        // PUT: api/maquinas/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("put/{id}")]
        //public async Task<IActionResult> Putmaquinas(int id, UpdateMaquinaDto updateMaquinas)
        //{
        //    var maquina = await _context.maquinas.FindAsync(id);

        //    if (maquina == null)
        //    {
        //        return NotFound("No se encontro la maquina con el ID: " + id);
        //    }

        //    _mapper.Map(updateMaquinas, maquina);
        //    _context.Entry(maquina).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!maquinasExists(id))
        //        {
        //            return BadRequest($"ID = {id} no coincide con el registro");
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }
        //    return Ok(updateMaquinas);
        //}

        [HttpPut("put/{id}")]
        public async Task<IActionResult> Putmaquinas(int id, UpdateMaquinaDto updateMaquinas)
        {
            // CAMBIO 1: Ya no usamos FindAsync. Necesitamos Include para cargar las relaciones actuales.
            var maquina = await _context.maquinas
                .Include(m => m.idUsoTipico)
                .Include(m => m.idTipoPapel)
                .Include(m => m.idTipoAcabado)
                .FirstOrDefaultAsync(m => m.idMaquina == id);

            if (maquina == null)
            {
                return NotFound("No se encontro la maquina con el ID: " + id);
            }

            // CAMBIO 2: Mapeamos los datos básicos. 
            // Al hacer esto sobre una entidad ya rastreada (tracked), EF detecta los cambios solo.
            _mapper.Map(updateMaquinas, maquina);

            // Ya no necesitas esta línea, de hecho con relaciones puede causar problemas:
            // _context.Entry(maquina).State = EntityState.Modified; 

            // CAMBIO 3: Procesar las colecciones (Limpiar y reconstruir)

            // --- USOS TÍPICOS ---
            maquina.idUsoTipico.Clear(); // Le dice a EF: "Borra las relaciones viejas"
            if (updateMaquinas.IdsUsoTipico != null && updateMaquinas.IdsUsoTipico.Any())
            {
                foreach (var usoId in updateMaquinas.IdsUsoTipico)
                {
                    // OJO AQUÍ: Como hicimos Include, algunos catálogos ya están en la memoria de EF.
                    // Si intentamos hacer Attach de un Stub nuevo con el mismo ID, EF dará error.
                    // Por eso, primero buscamos si ya lo tiene en memoria (.Local), si no, creamos el Stub.
                    var trackedEntity = _context.catalogoUsoTipico.Local.FirstOrDefault(e => e.idUsoTipico == usoId);
                    if (trackedEntity == null)
                    {
                        trackedEntity = new catalogoUsoTipico { idUsoTipico = usoId };
                        _context.Attach(trackedEntity);
                    }
                    maquina.idUsoTipico.Add(trackedEntity); // Agrega la nueva relación
                }
            }

            // --- TIPOS DE PAPEL ---
            maquina.idTipoPapel.Clear();
            if (updateMaquinas.IdsTipoPapel != null && updateMaquinas.IdsTipoPapel.Any())
            {
                foreach (var papelId in updateMaquinas.IdsTipoPapel)
                {
                    var trackedEntity = _context.catalogoTipoPapel.Local.FirstOrDefault(e => e.idTipoPapel == papelId);
                    if (trackedEntity == null)
                    {
                        trackedEntity = new catalogoTipoPapel { idTipoPapel = papelId };
                        _context.Attach(trackedEntity);
                    }
                    maquina.idTipoPapel.Add(trackedEntity);
                }
            }

            // --- TIPOS DE ACABADO ---
            maquina.idTipoAcabado.Clear();
            if (updateMaquinas.IdsTipoAcabado != null && updateMaquinas.IdsTipoAcabado.Any())
            {
                foreach (var acabadoId in updateMaquinas.IdsTipoAcabado)
                {
                    var trackedEntity = _context.catalogoTipoAcabado.Local.FirstOrDefault(e => e.idTipoAcabado == acabadoId);
                    if (trackedEntity == null)
                    {
                        trackedEntity = new catalogoTipoAcabado { idTipoAcabado = acabadoId };
                        _context.Attach(trackedEntity);
                    }
                    maquina.idTipoAcabado.Add(trackedEntity);
                }
            }

            // Guardar todo en la base de datos
            try
            {
                await _context.SaveChangesAsync(); // Aquí EF hace los UPDATE, DELETEs e INSERTs necesarios.
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!maquinasExists(id))
                {
                    return BadRequest($"ID = {id} no coincide con el registro");
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateMaquinas); // (O devuelve un ResponseDto si aplicaste lo del mensaje anterior)
        }

        // POST: api/maquinas
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPost("post")]
        //public async Task<ActionResult<maquinas>> Postmaquinas(AddMaquinaDto addMaquinas)
        //{
        //    var maquina = _mapper.Map<maquinas>(addMaquinas);
        //    _context.maquinas.Add(maquina);
        //    await _context.SaveChangesAsync();

        //    return CreatedAtAction("Getmaquinas", new { id = maquina.idMaquina }, maquina);
        //}

        [HttpPost("post")]
        public async Task<ActionResult<maquinas>> Postmaquinas(AddMaquinaDto addMaquinas)
        {
            // 1. Mapear los datos básicos de la máquina
            var maquina = _mapper.Map<maquinas>(addMaquinas);

            // Asegurarnos de que las colecciones de navegación estén inicializadas
            maquina.idUsoTipico ??= new List<catalogoUsoTipico>();
            maquina.idTipoPapel ??= new List<catalogoTipoPapel>();
            maquina.idTipoAcabado ??= new List<catalogoTipoAcabado>();

            // 2. Procesar el batch de Usos Típicos
            if (addMaquinas.IdsUsoTipico != null && addMaquinas.IdsUsoTipico.Any())
            {
                foreach (var id in addMaquinas.IdsUsoTipico)
                {
                    // Creamos un "Stub" solo con el ID (Cambia 'Id' por el nombre real de la PK en tu catálogo)
                    var usoTipico = new catalogoUsoTipico { idUsoTipico = id };

                    // Hacemos Attach para decirle a EF: "Este registro ya existe en la BD, no lo insertes de nuevo"
                    _context.Attach(usoTipico);

                    // Lo agregamos a la colección de la máquina para que EF cree el registro en la tabla puente
                    maquina.idUsoTipico.Add(usoTipico);
                }
            }

            // 3. Procesar el batch de Tipos de Papel
            if (addMaquinas.IdsTipoPapel != null && addMaquinas.IdsTipoPapel.Any())
            {
                foreach (var id in addMaquinas.IdsTipoPapel)
                {
                    var tipoPapel = new catalogoTipoPapel { idTipoPapel = id };
                    _context.Attach(tipoPapel);
                    maquina.idTipoPapel.Add(tipoPapel);
                }
            }

            // 4. Procesar el batch de Tipos de Acabado
            if (addMaquinas.IdsTipoAcabado != null && addMaquinas.IdsTipoAcabado.Any())
            {
                foreach (var id in addMaquinas.IdsTipoAcabado)
                {
                    var acabado = new catalogoTipoAcabado { idTipoAcabado = id };
                    _context.Attach(acabado);
                    maquina.idTipoAcabado.Add(acabado);
                }
            }

            // 5. Guardar todo en una sola transacción
            _context.maquinas.Add(maquina);
            await _context.SaveChangesAsync();
            // Al pasar por SaveChangesAsync, EF Core actualiza el objeto 'maquina' y le asigna su nuevo 'idMaquina' real.

            // OPCIÓN A: Devolver el DTO de entrada tal cual.
            // Nota: El frontend recibirá el nuevo ID en los headers de la respuesta (Location), pero no en el JSON del body.
            return CreatedAtAction("Getmaquinas", new { id = maquina.idMaquina }, addMaquinas);
        }

        private bool maquinasExists(int id)
        {
            return _context.maquinas.Any(e => e.idMaquina == id);
        }
    }
}
