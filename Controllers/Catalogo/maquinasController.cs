using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaBarnizadora;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaCorteConversion;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaDigital;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaFlexografia;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPegadora;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPrensaOffset;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaPreprensa;
using Sistema_Produccion_3_Backend.DTO.Catalogo.Maquinas.infoMaquina.infoMaquinaTroqueladora;
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

            switch (maquinaDto.idFamilia)
            {
                // prensa offset
                case 1:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaPrensaOffsetDto>(
                        await _context.infoMaquinaPrensaOffset.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // troqueladora
                case 2:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaTroqueladoraDto>(
                        await _context.infoMaquinaTroqueladora.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                //pegado
                case 3:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaPegadoraDto>(
                        await _context.infoMaquinaPegadora.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                //barniz
                case 4:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaBarnizadoraDto>(
                        await _context.infoMaquinaBarnizadora.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                //corte y conver.
                case 5:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaCorteConversionDto>(
                        await _context.infoMaquinaCorteConversion.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // flexo prensa
                case 8:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaFlexografiaDto>(
                        await _context.infoMaquinaFlexografia.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // flexo proceso
                case 9:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaFlexografiaDto>(
                        await _context.infoMaquinaFlexografia.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // preprensa
                case 10:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaPreprensaDto>(
                        await _context.infoMaquinaPreprensa.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // digital
                case 11:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaDigitalDto>(
                        await _context.infoMaquinaDigital.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // flexo conversion
                case 13:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaFlexografiaDto>(
                        await _context.infoMaquinaFlexografia.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // flexo corte
                case 14:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaFlexografiaDto>(
                        await _context.infoMaquinaFlexografia.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                // flexo prensa digital
                case 15:
                    maquinaDto.infoMaquina = _mapper.Map<InfoMaquinaFlexografiaDto>(
                        await _context.infoMaquinaFlexografia.FirstOrDefaultAsync(i => i.idMaquina == id));
                    break;

                default:
                    maquinaDto.infoMaquina = null;
                    break;
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
            _mapper.Map(updateMaquinas, maquina);

            // CAMBIO 3: Procesar las colecciones (Sincronización inteligente)

            // --- USOS TÍPICOS ---
            if (updateMaquinas.IdsUsoTipico != null)
            {
                var actuales = maquina.idUsoTipico.Select(e => e.idUsoTipico).ToList();
                var aRemover = actuales.Except(updateMaquinas.IdsUsoTipico).ToList();
                var aAgregar = updateMaquinas.IdsUsoTipico.Except(actuales).ToList();

                // 1. Remover solo los que ya no vienen en el DTO
                foreach (var usoId in aRemover)
                {
                    var item = maquina.idUsoTipico.First(e => e.idUsoTipico == usoId);
                    maquina.idUsoTipico.Remove(item);
                }

                // 2. Agregar únicamente los que faltan
                foreach (var usoId in aAgregar)
                {
                    var stub = new catalogoUsoTipico { idUsoTipico = usoId };
                    _context.Attach(stub);
                    maquina.idUsoTipico.Add(stub);
                }
            }

            // --- TIPOS DE PAPEL ---
            if (updateMaquinas.IdsTipoPapel != null)
            {
                var actuales = maquina.idTipoPapel.Select(e => e.idTipoPapel).ToList();
                var aRemover = actuales.Except(updateMaquinas.IdsTipoPapel).ToList();
                var aAgregar = updateMaquinas.IdsTipoPapel.Except(actuales).ToList();

                foreach (var papelId in aRemover)
                {
                    var item = maquina.idTipoPapel.First(e => e.idTipoPapel == papelId);
                    maquina.idTipoPapel.Remove(item);
                }

                foreach (var papelId in aAgregar)
                {
                    var stub = new catalogoTipoPapel { idTipoPapel = papelId };
                    _context.Attach(stub);
                    maquina.idTipoPapel.Add(stub);
                }
            }

            // --- TIPOS DE ACABADO ---
            if (updateMaquinas.IdsTipoAcabado != null)
            {
                var actuales = maquina.idTipoAcabado.Select(e => e.idTipoAcabado).ToList();
                var aRemover = actuales.Except(updateMaquinas.IdsTipoAcabado).ToList();
                var aAgregar = updateMaquinas.IdsTipoAcabado.Except(actuales).ToList();

                foreach (var acabadoId in aRemover)
                {
                    var item = maquina.idTipoAcabado.First(e => e.idTipoAcabado == acabadoId);
                    maquina.idTipoAcabado.Remove(item);
                }

                foreach (var acabadoId in aAgregar)
                {
                    var stub = new catalogoTipoAcabado { idTipoAcabado = acabadoId };
                    _context.Attach(stub);
                    maquina.idTipoAcabado.Add(stub);
                }
            }

            // CAMBIO 4: Procesar el objeto dinámico infoMaquina
            if (updateMaquinas.infoMaquina != null)
            {
                var infoJsonString = updateMaquinas.infoMaquina.ToString();

                switch (updateMaquinas.idFamilia)
                {
                    // Prensa offset
                    case 1:
                        var newOffset = JsonSerializer.Deserialize<infoMaquinaPrensaOffset>(infoJsonString);
                        var existOffset = await _context.infoMaquinaPrensaOffset.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existOffset != null)
                        {
                            newOffset.idMaquina = id;
                            _context.Entry(existOffset).CurrentValues.SetValues(newOffset);
                        }
                        else
                        {
                            newOffset.idMaquina = id;
                            _context.infoMaquinaPrensaOffset.Add(newOffset);
                        }
                        break;

                    // Troqueladora
                    case 2:
                        var newTroq = JsonSerializer.Deserialize<infoMaquinaTroqueladora>(infoJsonString);
                        var existTroq = await _context.infoMaquinaTroqueladora.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existTroq != null)
                        {
                            newTroq.idMaquina = id;
                            _context.Entry(existTroq).CurrentValues.SetValues(newTroq);
                        }
                        else
                        {
                            newTroq.idMaquina = id;
                            _context.infoMaquinaTroqueladora.Add(newTroq);
                        }
                        break;

                    // Pegado
                    case 3:
                        var newPeg = JsonSerializer.Deserialize<infoMaquinaPegadora>(infoJsonString);
                        var existPeg = await _context.infoMaquinaPegadora.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existPeg != null)
                        {
                            newPeg.idMaquina = id;
                            _context.Entry(existPeg).CurrentValues.SetValues(newPeg);
                        }
                        else
                        {
                            newPeg.idMaquina = id;
                            _context.infoMaquinaPegadora.Add(newPeg);
                        }
                        break;

                    // Barniz
                    case 4:
                        var newBarniz = JsonSerializer.Deserialize<infoMaquinaBarnizadora>(infoJsonString);
                        var existBarniz = await _context.infoMaquinaBarnizadora.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existBarniz != null)
                        {
                            newBarniz.idMaquina = id;
                            _context.Entry(existBarniz).CurrentValues.SetValues(newBarniz);
                        }
                        else
                        {
                            newBarniz.idMaquina = id;
                            _context.infoMaquinaBarnizadora.Add(newBarniz);
                        }
                        break;

                    // Corte y conver.
                    case 5:
                        var newCorte = JsonSerializer.Deserialize<infoMaquinaCorteConversion>(infoJsonString);
                        var existCorte = await _context.infoMaquinaCorteConversion.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existCorte != null)
                        {
                            newCorte.idMaquina = id;
                            _context.Entry(existCorte).CurrentValues.SetValues(newCorte);
                        }
                        else
                        {
                            newCorte.idMaquina = id;
                            _context.infoMaquinaCorteConversion.Add(newCorte);
                        }
                        break;

                    // Flexografía (Agrupados)
                    case 8:
                    case 9:
                    case 13:
                    case 14:
                    case 15:
                        var newFlexo = JsonSerializer.Deserialize<infoMaquinaFlexografia>(infoJsonString);
                        var existFlexo = await _context.infoMaquinaFlexografia.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existFlexo != null)
                        {
                            newFlexo.idMaquina = id;
                            _context.Entry(existFlexo).CurrentValues.SetValues(newFlexo);
                        }
                        else
                        {
                            newFlexo.idMaquina = id;
                            _context.infoMaquinaFlexografia.Add(newFlexo);
                        }
                        break;

                    // Preprensa
                    case 10:
                        var newPreprensa = JsonSerializer.Deserialize<infoMaquinaPreprensa>(infoJsonString);
                        var existPreprensa = await _context.infoMaquinaPreprensa.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existPreprensa != null)
                        {
                            newPreprensa.idMaquina = id;
                            _context.Entry(existPreprensa).CurrentValues.SetValues(newPreprensa);
                        }
                        else
                        {
                            newPreprensa.idMaquina = id;
                            _context.infoMaquinaPreprensa.Add(newPreprensa);
                        }
                        break;

                    // Digital
                    case 11:
                        var newDigital = JsonSerializer.Deserialize<infoMaquinaDigital>(infoJsonString);
                        var existDigital = await _context.infoMaquinaDigital.FirstOrDefaultAsync(i => i.idMaquina == id);
                        if (existDigital != null)
                        {
                            newDigital.idMaquina = id;
                            _context.Entry(existDigital).CurrentValues.SetValues(newDigital);
                        }
                        else
                        {
                            newDigital.idMaquina = id;
                            _context.infoMaquinaDigital.Add(newDigital);
                        }
                        break;
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

            return Ok(updateMaquinas);
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

            // 6. Evaluar y guardar la información específica de la máquina según la Familia
            // Verificamos que el payload contenga la información específica
            if (addMaquinas.infoMaquina != null)
            {
                switch (addMaquinas.idFamilia)
                {
                    // Prensa offset
                    case 1:
                        var infoJsonString = addMaquinas.infoMaquina.ToString();
                        var infoOffset = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaPrensaOffset>(infoJsonString);
                        infoOffset.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaPrensaOffset.Add(infoOffset);
                        break;

                    // Troqueladora
                    case 2:
                        var infoTroqJsonString = addMaquinas.infoMaquina.ToString();
                        var infoTroq = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaTroqueladora>(infoTroqJsonString);
                        infoTroq.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaTroqueladora.Add(infoTroq);
                        break;

                    // Pegado
                    case 3:
                        var infoPegJsonString = addMaquinas.infoMaquina.ToString();
                        var infoPeg = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaPegadora>(infoPegJsonString);
                        infoPeg.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaPegadora.Add(infoPeg);
                        break;

                    // Barniz
                    case 4:
                        var infoBarnizJsonString = addMaquinas.infoMaquina.ToString();
                        var infoBarniz = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaBarnizadora>(infoBarnizJsonString);
                        infoBarniz.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaBarnizadora.Add(infoBarniz);
                        break;

                    // Corte y conver.
                    case 5:
                        var infoCorteConvJsonString = addMaquinas.infoMaquina.ToString();
                        var infoCorteConv = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaCorteConversion>(infoCorteConvJsonString);
                        infoCorteConv.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaCorteConversion.Add(infoCorteConv);
                        break;

                    // Flexografía (Agrupamos 8, 9, 13, 14 y 15 porque van a la misma tabla)
                    case 8:
                    case 9:
                    case 13:
                    case 14:
                    case 15:
                        var infoFlexJsonString = addMaquinas.infoMaquina.ToString();
                        var infoFlexo = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaFlexografia>(infoFlexJsonString);
                        infoFlexo.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaFlexografia.Add(infoFlexo);
                        break;

                    // Preprensa
                    case 10:
                        var infoPreprensaJsonString = addMaquinas.infoMaquina.ToString();
                        var infoPreprensa = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaPreprensa>(infoPreprensaJsonString);
                        infoPreprensa.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaPreprensa.Add(infoPreprensa);
                        break;

                    // Digital
                    case 11:
                        var infoDigitalJsonString = addMaquinas.infoMaquina.ToString();
                        var infoDigital = System.Text.Json.JsonSerializer.Deserialize<infoMaquinaDigital>(infoDigitalJsonString);
                        infoDigital.idMaquina = maquina.idMaquina;
                        _context.infoMaquinaDigital.Add(infoDigital);
                        break;
                }

                // 7. Guardar la tabla hija en la base de datos
                await _context.SaveChangesAsync();
            }

            return CreatedAtAction("Getmaquinas", new { id = maquina.idMaquina }, addMaquinas);
        }

        private bool maquinasExists(int id)
        {
            return _context.maquinas.Any(e => e.idMaquina == id);
        }
    }
}
