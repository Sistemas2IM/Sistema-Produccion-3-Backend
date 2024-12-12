using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Modulo.SubModulo;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class subModulosController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public subModulosController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/subModulos
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<SubModuloDto>>> GetsubModulo()
        {
            var subModulo = await _context.subModulo
                .Include(u => u.idModuloNavigation)
                .ThenInclude(m => m.idMenuNavigation)
                .ToListAsync();
            var subModuloDto = _mapper.Map<List<SubModuloDto>>(subModulo);

            return Ok(subModuloDto);
        }

        // GET: api/subModulos/5
        [HttpGet("get/{id}")]
        public async Task<ActionResult<SubModuloDto>> GetsubModulo(int id)
        {
            var subModulo = await _context.subModulo
                .Include(u => u.idModuloNavigation)
                .ThenInclude(m => m.idMenuNavigation)
                .FirstOrDefaultAsync(u => u.idSubModulo == id);
            var subModuloDto = _mapper.Map<SubModuloDto>(subModulo);

            if (subModuloDto == null)
            {
                return NotFound("No se encontro el SubModulo con el ID: " + id);
            }

            return Ok(subModuloDto);
        }     

        private bool subModuloExists(int id)
        {
            return _context.subModulo.Any(e => e.idSubModulo == id);
        }
    }
}
