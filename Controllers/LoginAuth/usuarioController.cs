using AutoMapper;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Diseño;
using Sistema_Produccion_3_Backend.DTO.PermisosUsuario.Operadores;
using Sistema_Produccion_3_Backend.DTO.ReporteOperador.DetalleReporte.Operaciones;
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
                .OrderByDescending(f => f.fechaDeCreacion)
                .Include(u => u.idRolNavigation)        // Include the role
                .ThenInclude(r => r.permiso)            // Include the permissions for each role
                .ThenInclude(p => p.idSubModuloNavigation) // Include the sub-modules for each permission
                .ThenInclude(sm => sm.idModuloNavigation)  // Include the modules for each sub-module
                //.ThenInclude(m => m.idMenuNavigation)   // Include the menu for each module
                .Include(c => c.idCargoNavigation)
                .Include(pm => pm.permisoMaquina)
                .ToListAsync();

            var usuariosDto = _mapper.Map<List<UsuarioDto>>(usuarios);

            return Ok(usuariosDto);
        }

        // GET: api/usuario
        [HttpGet("get/diseñadores")]
        public async Task<ActionResult<IEnumerable<UsuarioDisenoDto>>> GetusuarioDisenador()
        {
            var usuarios = await _context.usuario
                .Where(u => u.idCargo == 2)
                .Select(u => new
                {
                    UsuarioBase = u,
                    CantidadProcesos = u.asignacion.Count(),

                    TotalHoras = u.asignacion.Sum(a => a.idProcesoNavigation.tiempoEstimado ?? 0)
                })
                .OrderByDescending(f => f.UsuarioBase.fechaDeCreacion)               
                .ToListAsync();

            var usuariosDto = usuarios.Select(uc =>
            {
                var dto = _mapper.Map<UsuarioDisenoDto>(uc.UsuarioBase);
                dto.cantidadProcesos = uc.CantidadProcesos;
                dto.horasTotales = uc.TotalHoras;

                return dto;
            }).ToList();

            return Ok(usuariosDto);
        }

        // GET: api/usuario
        [HttpGet("get/operadores/area/{idArea}")]
        public async Task<ActionResult<IEnumerable<OperadoresDto>>> GetusuarioOperador(int idArea)
        {
            var usuarios = await _context.usuario
                // 🔹 Filtra SOLO usuarios del área solicitada
                .Where(u => u.idCargo == 1 && (idArea == 17 || u.idArea == idArea))
                .OrderByDescending(f => f.fechaDeCreacion)
                .Include(a => a.idAreaNavigation)
                // 🔹 Incluye solo las máquinas asignadas
                .Include(u => u.permisoMaquina
                    .Where(pm => pm.asignada == true))
                .ThenInclude(pm => pm.maquinaNavigation)
                .ToListAsync();

            var usuariosDto = _mapper.Map<List<OperadoresDto>>(usuarios);

            return Ok(usuariosDto);
        }

        // GET: api/usuario
        [HttpGet("get/area/{idArea}")]
        public async Task<ActionResult<IEnumerable<UsuarioCortoDto>>> GetusuarioArea(int idArea)
        {
            var usuarios = await _context.usuario
                // 🔹 Filtra SOLO usuarios del área solicitada
                .Where(u => idArea == 17 || u.idArea == idArea)
                .OrderByDescending(f => f.fechaDeCreacion)
                .Include(a => a.idAreaNavigation)
                .Include(r => r.idRolNavigation)
                .ToListAsync();

            var usuariosDto = _mapper.Map<List<UsuarioCortoDto>>(usuarios);

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
                .Include(u => u.idRolNavigation)
                .ThenInclude(r => r.permisoEspecifico)
                .ThenInclude(r => r.idPermisoTipoNavigation)
                //.ThenInclude(m => m.idMenuNavigation)
                .Include(c => c.idCargoNavigation)
                .Include(pm => pm.permisoMaquina)
                .FirstOrDefaultAsync(u => u.user == user); // Filtrar por el campo "user" (string)

            if (usuario == null)
            {
                return NotFound();
            }

            var usuarioDto = _mapper.Map<UsuarioDto>(usuario);

            return Ok(usuarioDto);
        }

        [HttpPut("put/{user}")]
        public async Task<IActionResult> UpdateUsuario(string user, UpdateUsuarioDto updateUsuarioDto)
        {
            var usuario = await _context.usuario.FirstOrDefaultAsync(u => u.user == user);
            if (usuario == null)
            {
                return NotFound();
            }

            _mapper.Map(updateUsuarioDto, usuario);

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException) when (!usuarioExists(user))
            {
                return NotFound();
            }

            return NoContent();
        }

        private bool usuarioExists(string id)
        {
            return _context.usuario.Any(e => e.user == id);
        }
    }
}
