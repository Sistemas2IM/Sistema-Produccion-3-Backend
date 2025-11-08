using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.Calidad.VariableUnidadMedida;
using Sistema_Produccion_3_Backend.Models;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Sistema_Produccion_3_Backend.Controllers.Calidad.VariableUnidadMedida
{
    [Route("api/[controller]")]
    [ApiController]
    public class variableUnidadMedidaController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public variableUnidadMedidaController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/<variableUnidadMedidaController>
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<VariableUnidadMedidaDto>>> GetVariableUnidadMedida()
        {
            var variableUnidadMedida = await _context.variableUnidadMedida
                .ToListAsync();

            var variableUnidadMedidaDto = _mapper.Map<List<VariableUnidadMedidaDto>>(variableUnidadMedida);

            return Ok(variableUnidadMedidaDto);
        }
    }
}
