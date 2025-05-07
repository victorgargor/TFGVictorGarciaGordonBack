using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.DTOs;
using EjerciciosVictorAPI.Extensions;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/usuarios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        public UsuariosController(ApplicationDbContext context, UserManager<IdentityUser> userManager) 
        {
            this.context = context;
            this.userManager = userManager;
        }

        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Users.AsQueryable();

            // Inserta los parámetros de paginación en la respuesta
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacion.CantidadRegistros);

            var usuarios = await queryable.Paginar(paginacion)
                .Select(x => new UsuarioDTO { Id = x.Id, Email = x.Email! })
                .ToListAsync();

            return Ok(usuarios);
        }

        [HttpGet("roles")]
        public async Task<ActionResult<List<RolDTO>>> Get()
        {
            // Modificado para incluir el Id junto con el Nombre
            return await context.Roles
                .Select(x => new RolDTO
                {
                    Id = x.Id,  // Incluye el Id del rol
                    Nombre = x.Name!
                })
                .ToListAsync();
        }

        [HttpGet("obtenerRoles/{usuarioId}")]
        public async Task<ActionResult<List<string>>> ObtenerRolesPorUsuario(string usuarioId)
        {
            // 1) Verificamos que el usuario exista
            var usuario = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario is null)
            {
                return NotFound("Usuario no encontrado");
            }

            // 2) Hacemos JOIN manual entre UserRoles y Roles
            var rolesAsignados = await (
                from ur in context.UserRoles
                join r in context.Roles on ur.RoleId equals r.Id
                where ur.UserId == usuarioId
                select r.Name!
            ).ToListAsync();

            return Ok(rolesAsignados);
        }



        [HttpPost("asignarRol")]
        public async Task<ActionResult> AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UsuarioId);

            if(usuario is null)
            {
                return BadRequest("Usuario no existe");
            }

            await userManager.AddToRoleAsync(usuario, editarRolDTO.Rol);
            return NoContent();
        }

        [HttpPost("removerRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UsuarioId);

            if (usuario is null)
            {
                return BadRequest("Usuario no existe");
            }

            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.Rol);
            return NoContent();
        }

        [HttpGet("obtenerNombre/{usuarioId}")]
        public async Task<ActionResult<string>> ObtenerNombreUsuario(string usuarioId)
        {
            var usuario = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario is null)
            {
                return NotFound("Usuario no encontrado");
            }

            return Ok(usuario.UserName); 
        }
    }
}
