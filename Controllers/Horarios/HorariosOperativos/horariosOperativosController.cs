using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Horarios.HorariosOperativos;
using Sistema_Produccion_3_Backend.DTO.Horarios.HorariosOperativos.Batch;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Horarios.HorariosOperativos
{
    [Route("api/[controller]")]
    [ApiController]
    public class horariosOperativosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;
        private readonly ILogger<horariosOperativosController> _logger;
        private readonly IHttpClientFactory _httpClientFactory;

        public horariosOperativosController(base_nuevaContext context, IMapper mapper, ILogger<horariosOperativosController> logger, IHttpClientFactory httpClientFactory)
        {
            _context = context;
            _mapper = mapper;
            _logger=logger;
            _httpClientFactory=httpClientFactory;
        }

        // GET: api/<horariosOperativosController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<HorariosOperativosDto>>> GetHorariosOperativos()
        {
            var horariosOperadores = await _context.horariosOperativos
                .Include(h => h.idAreaNavigation)
                .Include(h => h.idMaquinaNavigation)
                .Include(h => h.operadorNavigation)
                .ToListAsync();

            var horariosOperativosDto = _mapper.Map<List<HorariosOperativosDto>>(horariosOperadores);

            return Ok(horariosOperativosDto);
        }

        // GET api/<horariosOperativosController>/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<HorariosOperativosDto>> GetHorariosOperativos(int id)
        {
            var horariosOperativos = await _context.horariosOperativos
                .Include(h => h.idAreaNavigation)
                .Include(h => h.idMaquinaNavigation)
                .Include(h => h.operadorNavigation)
                .FirstOrDefaultAsync(u => u.idHorario == id);

            if (horariosOperativos == null)
            {
                return NotFound("No se cnontro el registro con el id: " + id);
            }

            var horariosOperativosDto = _mapper.Map<HorariosOperativosDto>(horariosOperativos);

            return Ok(horariosOperativosDto);
        }

        // POST api/<horariosOperativosController>
        [HttpPost("post")]
        public async Task<ActionResult<horariosOperativos>> PostHorariosOperativos(AddHorariosOperativosDto addHorariosOperativosDto)
        {
            var horariosOperativos = _mapper.Map<horariosOperativos>(addHorariosOperativosDto);

            _context.horariosOperativos.Add(horariosOperativos);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetHorariosOperativos", new { id = horariosOperativos.idHorario }, horariosOperativos);
        }

        // POST BATCH
        [HttpPost("post/batch")]
        public async Task<IActionResult> BatchAddHorario([FromBody] BatchAddHorariosOperativosDto batchAddHorarios)
        {
            // 1. Validación de entrada
            if (batchAddHorarios == null || batchAddHorarios.listaHorariosOperativos == null || !batchAddHorarios.listaHorariosOperativos.Any())
            {
                return BadRequest("La lista de horarios operativos no puede estar vacía.");
            }

            // 2. Mapeo de DTO a Entidad
            var horariosOperativos = batchAddHorarios.listaHorariosOperativos.Select(dto => _mapper.Map<horariosOperativos>(dto)).ToList();

            // 3. Preparación para guardar en el Context
            await _context.horariosOperativos.AddRangeAsync(horariosOperativos);

            // 4. Guardado en la Base de Datos
            try
            {
                await _context.SaveChangesAsync();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ocurrió un error al guardar el batch de horarios operativos.");
                return StatusCode(StatusCodes.Status500InternalServerError, $"Ocurrió un error al guardar: {ex.Message}");
            }

            // === INICIO DE LA INTEGRACIÓN CON GOOGLE CHAT (MODIFICADO) ===
            try
            {
                var count = batchAddHorarios.listaHorariosOperativos.Count;
                var listaDeHorarios = batchAddHorarios.listaHorariosOperativos;
                const int maxToShow = 15;

                // 1. === OBTENER NOMBRES DE MÁQUINAS (NUEVO) ===

                // 1a. Obtener los IDs únicos de las máquinas que se van a mostrar
                var idsMaquinasAMostrar = listaDeHorarios.Take(maxToShow)
                                                       .Select(h => h.idMaquina)
                                                       .Distinct()
                                                       .ToList();

                // 1b. Consultar la BD una sola vez para traer esos nombres
                var maquinasDict = await _context.maquinas
                    .Where(m => idsMaquinasAMostrar.Contains(m.idMaquina))
                    .ToDictionaryAsync(
                        m => m.idMaquina,  // Clave del diccionario (ID)
                        m => m.nombreCorto);    // Valor del diccionario (Nombre)

                // 2. Usamos StringBuilder para construir el mensaje
                var messageBuilder = new System.Text.StringBuilder();
                messageBuilder.AppendLine($"Se crearon *{count}* nuevo(s) horario(s):"); // Encabezado

                // 3. Iteramos sobre cada horario en la lista (hasta el límite)
                foreach (var horario in listaDeHorarios.Take(maxToShow))
                {
                    // 3a. Buscar el nombre de la máquina en el diccionario
                    string nombreMaquina = maquinasDict.TryGetValue((int)horario.idMaquina, out var nombre)
                        ? nombre // Si se encontró, usa el nombre
                        : $"ID: {horario.idMaquina}"; // Si no (por si acaso), usa el ID

                    // 3b. Añadimos la línea con el nombre de la máquina
                    messageBuilder.AppendLine($"- *Op:* {horario.operador}, *Maq:* {nombreMaquina}, *Fecha:* {horario.fecha}, *Hora:* {horario.horaInicio} - {horario.horaFin}");
                }

                // 4. Si hay más horarios de los que mostramos, añadimos un resumen
                if (count > maxToShow)
                {
                    messageBuilder.AppendLine($"... y {count - maxToShow} más.");
                }

                // 5. Llamar al webhook con el mensaje completo
                await SendToGoogleChat(messageBuilder.ToString());
            }
            catch (Exception chatEx)
            {
                _logger.LogError(chatEx, "Error al preparar el mensaje de Google Chat.");
            }
            // === FIN DE LA INTEGRACIÓN ===

            return Ok(new
            {
                batchAddHorarios
            });
        }


        // PUT api/<horariosOperativosController>/5
        [HttpPut("put/{id}")]
        public async Task<IActionResult> PutHorariosOperativos(int id, UpdateHorariosOperativosDto updateHorariosOperativosDto)
        {
            var horariosOperativos = await _context.horariosOperativos.FindAsync(id);

            if (horariosOperativos == null)
            {
                return NotFound("No se cnontro el registro con el id: " + id);
            }

            _mapper.Map(updateHorariosOperativosDto, horariosOperativos);
            _context.Entry(horariosOperativos).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!horariosOperativosExists(id))
                {
                    return NotFound("No se cnontro el registro con el id: " + id);
                }
                else
                {
                    throw;
                }
            }

            return Ok(updateHorariosOperativosDto);
        }

        // PUT BATCH
        [HttpPut("put/batch")]
        public async Task<IActionResult> BatchUpdateHorarios([FromBody] BatchUpdateHorariosOperativosDto batchUpdateHorarios)
        {
            if (batchUpdateHorarios == null || batchUpdateHorarios.listaHorariosOperativos == null || !batchUpdateHorarios.listaHorariosOperativos.Any())
            {
                return BadRequest("La lista de horarios operativos a actualizar no puede estar vacía.");
            }

            var ids = batchUpdateHorarios.listaHorariosOperativos.Select(t => t.idHorario).ToList();

            var horariosOperativosExistentes = await _context.horariosOperativos
                .Where(h => ids.Contains(h.idHorario))
                .ToListAsync();

            if (horariosOperativosExistentes.Count != ids.Count)
            {
                return NotFound("Uno o más horarios operativos no se encontraron para los IDs proporcionados.");
            }

            foreach (var updateDto in batchUpdateHorarios.listaHorariosOperativos)
            {
                var horarioOperativo = horariosOperativosExistentes.FirstOrDefault(h => h.idHorario == updateDto.idHorario);
                if (horarioOperativo != null)
                {
                    _mapper.Map(updateDto, horarioOperativo);
                    _context.Entry(horarioOperativo).State = EntityState.Modified;
                }
            }

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, "Error al actualizar los horarios operativos.");
            }

            return Ok("Actualización realizada correctamente.");
        }

        private bool horariosOperativosExists(int id)
        {
            return _context.horariosOperativos.Any(e => e.idHorario == id);
        }

        /// <summary>
        /// Envía una notificación a Google Chat de forma asíncrona.
        /// </summary>
        private async Task SendToGoogleChat(string message)
        {
            // ❗️ IMPORTANTE: Mueve esta URL a tu appsettings.json
            var url = "https://chat.googleapis.com/v1/spaces/AAQAWq4gutM/messages?key=AIzaSyDdI0hCZtE6vySjMm-WEfRq3CPzqKqqsHI&token=OWw9jgq6-DXEFzMCkl3HI4tZxjN1clp9_p-6xPfdRjM";

            try
            {
                var client = _httpClientFactory.CreateClient();

                // Crear el payload
                var payload = new { text = message };
                var jsonPayload = System.Text.Json.JsonSerializer.Serialize(payload);
                var content = new StringContent(jsonPayload, System.Text.Encoding.UTF8, "application/json");

                // Enviar la petición
                await client.PostAsync(url, content);

                _logger.LogInformation("Notificación enviada a Google Chat.");
            }
            catch (Exception ex)
            {
                // Solo registrar el error, no fallar la solicitud principal
                _logger.LogError(ex, "Error al enviar notificación a Google Chat.");
            }
        }
    }
}
