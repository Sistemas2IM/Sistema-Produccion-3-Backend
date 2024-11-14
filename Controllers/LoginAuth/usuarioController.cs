using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
using Sistema_Produccion_3_Backend.Models;

namespace Sistema_Produccion_3_Backend.Controllers.LoginAuth
{
    [Route("api/[controller]")]
    [ApiController]
    public class usuarioController : ControllerBase
    {
        private readonly base_nuevaContext _context;
        private readonly IMapper _mapper;

        public usuarioController(base_nuevaContext context, IMapper mapper)
        {
            _context = context;
            _mapper = mapper;
        }

        // GET: api/usuario
        [HttpGet("get")]
        public async Task<ActionResult<IEnumerable<UsuarioDto>>> Getusuario()
        {
            var usuarios = await _context.usuario
                .Include(u => u.idRolNavigation)        // Include the role
                .ThenInclude(r => r.permiso)            // Include the permissions for each role
                .ThenInclude(p => p.idSubModuloNavigation) // Include the sub-modules for each permission
                .ThenInclude(sm => sm.idModuloNavigation)  // Include the modules for each sub-module
                .ThenInclude(m => m.idMenuNavigation)   // Include the menu for each module
                .Include(c => c.idCargoNavigation)
                .ToListAsync();

            var usuariosDto = _mapper.Map<List<UsuarioDto>>(usuarios);

            return Ok(usuariosDto);
        }

        // GET: api/usuario/get/{user}
        [HttpGet("get/{user}")]
        public async Task<ActionResult<UsuarioDto>> Getusuario(string user)
        {
            var usuario = await _context.usuario
                .Include(u => u.idRolNavigation)
                .ThenInclude(r => r.permiso)
                .ThenInclude(p => p.idSubModuloNavigation)
                .ThenInclude(sm => sm.idModuloNavigation)
                .ThenInclude(m => m.idMenuNavigation)
                .Include(c => c.idCargoNavigation)
                .FirstOrDefaultAsync(u => u.user == user); // Filtrar por el campo "user" (string)

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            return Ok(usuarioDto);
        }


        private bool usuarioExists(string id)
        {
            return _context.usuario.Any(e => e.user == id);
        }
    }
}
