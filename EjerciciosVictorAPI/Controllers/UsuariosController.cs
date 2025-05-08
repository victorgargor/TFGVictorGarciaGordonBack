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
    /// <summary>
    /// Controlador para operaciones relacionadas con usuarios y sus roles.
    /// </summary>
    [ApiController]
    [Route("api/usuarios")]
    [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
    public class UsuariosController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly UserManager<IdentityUser> userManager;

        /// <summary>
        /// Constructor del controlador de usuarios.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        /// <param name="userManager">Manejador de usuarios de Identity.</param>
        public UsuariosController(ApplicationDbContext context, UserManager<IdentityUser> userManager)
        {
            this.context = context;
            this.userManager = userManager;
        }

        /// <summary>
        /// Obtiene la lista paginada de usuarios registrados.
        /// </summary>
        /// <param name="paginacion">Parámetros de paginación.</param>
        /// <returns>Lista de usuarios.</returns>
        [HttpGet]
        public async Task<ActionResult<List<UsuarioDTO>>> Get([FromQuery] PaginacionDTO paginacion)
        {
            var queryable = context.Users.AsQueryable();

            // Inserto en la cabecera de la respuesta los parámetros de paginación
            await HttpContext.InsertarParametrosPaginacionEnRespuesta(queryable, paginacion.CantidadRegistros);

            // Pagino los usuarios y los listo
            var usuarios = await queryable.Paginar(paginacion)
                .Select(x => new UsuarioDTO { Id = x.Id, Email = x.Email! })
                .ToListAsync();

            return Ok(usuarios);
        }

        /// <summary>
        /// Obtiene todos los roles disponibles.
        /// </summary>
        /// <returns>Lista de roles con sus nombres e IDs.</returns>
        [HttpGet("roles")]
        public async Task<ActionResult<List<RolDTO>>> Get()
        {
            // Devuelvo todos los roles disponibles
            return await context.Roles
                .Select(x => new RolDTO
                {
                    Id = x.Id,
                    Nombre = x.Name!
                })
                .ToListAsync();
        }

        /// <summary>
        /// Obtiene los roles asignados a un usuario específico.
        /// </summary>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <returns>Lista de nombres de roles.</returns>
        [HttpGet("obtenerRoles/{usuarioId}")]
        public async Task<ActionResult<List<string>>> ObtenerRolesPorUsuario(string usuarioId)
        {
            // Compruebo si el usuario existe
            var usuario = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            if (usuario is null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Hago el JOIN para obtener los roles asignados
            var rolesAsignados = await (
                from ur in context.UserRoles
                join r in context.Roles on ur.RoleId equals r.Id
                where ur.UserId == usuarioId
                select r.Name!
            ).ToListAsync();

            // Devuelvo los roles
            return Ok(rolesAsignados);
        }

        /// <summary>
        /// Asigna un rol a un usuario.
        /// </summary>
        /// <param name="editarRolDTO">DTO con ID del usuario y rol a asignar.</param>
        /// <returns>NoContent si se asigna correctamente, BadRequest si el usuario no existe.</returns>
        [HttpPost("asignarRol")]
        public async Task<ActionResult> AsignarRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UsuarioId);

            // Si el usuario no existe
            if (usuario is null)
            {
                return BadRequest("Usuario no existe");
            }

            // Le asigno el rol
            await userManager.AddToRoleAsync(usuario, editarRolDTO.Rol);
            return NoContent();
        }

        /// <summary>
        /// Remueve un rol previamente asignado a un usuario.
        /// </summary>
        /// <param name="editarRolDTO">DTO con ID del usuario y rol a remover.</param>
        /// <returns>NoContent si se remueve correctamente, BadRequest si el usuario no existe.</returns>
        [HttpPost("removerRol")]
        public async Task<ActionResult> RemoverRolUsuario(EditarRolDTO editarRolDTO)
        {
            var usuario = await userManager.FindByIdAsync(editarRolDTO.UsuarioId);

            // Si no existe
            if (usuario is null)
            {
                return BadRequest("Usuario no existe");
            }

            // Le quito el rol
            await userManager.RemoveFromRoleAsync(usuario, editarRolDTO.Rol);
            return NoContent();
        }

        /// <summary>
        /// Obtiene el nombre de usuario (UserName) a partir del ID.
        /// </summary>
        /// <param name="usuarioId">ID del usuario.</param>
        /// <returns>Nombre de usuario.</returns>
        [HttpGet("obtenerNombre/{usuarioId}")]
        public async Task<ActionResult<string>> ObtenerNombreUsuario(string usuarioId)
        {
            // Busco el usuario del que quiero obtener el nombre
            var usuario = await context.Users
                .AsNoTracking()
                .FirstOrDefaultAsync(u => u.Id == usuarioId);

            // Sino se encuentra
            if (usuario is null)
            {
                return NotFound("Usuario no encontrado");
            }

            // Devuelvo el nombre del usuario
            return Ok(usuario.UserName);
        }
    }
}
