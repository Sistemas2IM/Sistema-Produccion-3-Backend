using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.ProcesoOf;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.TablerosOf
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValidarOfGestionController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public ValidarOfGestionController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/procesoOf/5
        [HttpGet("get/diseño/oF/")]
        public async Task<ActionResult<string>> GetprocesoOfTarjetaDiseño(int of)
        {
            var procesoOf = await _context.procesoOf
                .Where(p => p.idTablero == 42)               
                .FirstOrDefaultAsync(u => u.oF == of);

            if (procesoOf == null)
            {
                return "false"; // No se encontraron registros
            }

            return "true"; // Se encontraron registros
        }

        // GET: api/procesoOf/5
        [HttpGet("get/digital/oF/")]
        public async Task<ActionResult<string>> GetprocesoOfTarjetaDigital(int of)
        {
            var procesoOf = await _context.procesoOf
                .Where(p => p.idTablero == 44)              
                .FirstOrDefaultAsync(u => u.oF == of);

            if (procesoOf == null)
            {
                return "false"; // No se encontraron registros
            }

            return "true"; // Se encontraron registros
        }
    }
}
