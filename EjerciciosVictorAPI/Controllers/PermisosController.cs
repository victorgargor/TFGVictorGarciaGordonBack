using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.DTOs;
using EjerciciosVictorAPI.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de permisos y su asignación a roles.
    /// </summary>
    [ApiController]
    [Route("api/permisos")]
    public class PermisosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor que recibe el contexto de base de datos.
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public PermisosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Obtiene todos los permisos registrados en la base de datos.
        /// </summary>
        /// <returns>Lista de permisos en formato DTO.</returns>
        [HttpGet]
        public async Task<ActionResult<List<PermisoDTO>>> GetTodos()
        {
            var permisos = await context.Permisos
                .Select(p => new PermisoDTO { Id = p.Id.ToString(), Nombre = p.Nombre })
                .ToListAsync();

            return Ok(permisos);
        }

        /// <summary>
        /// Crea un nuevo permiso en la base de datos.
        /// </summary>
        /// <param name="permisoDTO">Permiso a crear.</param>
        /// <returns>Respuesta sin contenido si se guarda correctamente.</returns>
        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] PermisoDTO permisoDTO)
        {
            var permiso = new Permiso
            {
                Id = Guid.Parse(permisoDTO.Id),
                Nombre = permisoDTO.Nombre
            };

            // Lo añado y guardo en la base de datos
            context.Permisos.Add(permiso);
            await context.SaveChangesAsync();
            return NoContent(); 
        }

        /// <summary>
        /// Asigna un permiso a un rol específico.
        /// </summary>
        /// <param name="dto">Datos de la relación rol-permiso.</param>
        /// <returns>BadRequest si ya existe, NoContent si se asigna exitosamente.</returns>
        [HttpPost("asignar")]
        public async Task<ActionResult> AsignarPermiso([FromBody] RolPermisoDTO dto)
        {
            var permisoId = Guid.Parse(dto.PermisoId);

            // Compruebo si la relación ya existe
            var yaExiste = await context.RolPermisos
                .AnyAsync(rp => rp.RolId == dto.RolId && rp.PermisoId == permisoId);

            if (yaExiste)
                return BadRequest("Este permiso ya está asignado al rol.");

            // Creo la relación
            var rolPermiso = new RolPermiso
            {
                RolId = dto.RolId,
                PermisoId = permisoId
            };

            // Añado y guardo en la base de datos
            context.RolPermisos.Add(rolPermiso);
            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Remueve un permiso previamente asignado a un rol.
        /// </summary>
        /// <param name="dto">Datos de la relación a eliminar.</param>
        /// <returns>NotFound si no existe la relación, NoContent si se elimina correctamente.</returns>
        [HttpPost("remover")]
        public async Task<ActionResult> RemoverPermiso([FromBody] RolPermisoDTO dto)
        {
            var permisoId = Guid.Parse(dto.PermisoId);

            // Busco la relación existente
            var rolPermiso = await context.RolPermisos
                .FirstOrDefaultAsync(rp => rp.RolId == dto.RolId && rp.PermisoId == permisoId);

            // Sino se encuentra
            if (rolPermiso == null)
            {
                return NotFound("Relación no encontrada.");
            }
                

            // Elimino la relación y guardo los cambios
            context.RolPermisos.Remove(rolPermiso);
            await context.SaveChangesAsync();
            return NoContent();
        }

        /// <summary>
        /// Obtiene todos los permisos asignados a un rol específico.
        /// </summary>
        /// <param name="rolId">ID del rol.</param>
        /// <returns>Lista de permisos en formato DTO asociados al rol.</returns>
        [HttpGet("{rolId}")]
        public async Task<ActionResult<List<PermisoDTO>>> ObtenerPermisosPorRol(string rolId)
        {
            // Obtengo los permisos relacionados con el rol
            var permisos = await (
                from rp in context.RolPermisos
                join p in context.Permisos on rp.PermisoId equals p.Id
                where rp.RolId == rolId
                select new PermisoDTO
                {
                    Id = p.Id.ToString(),
                    Nombre = p.Nombre
                }
            ).ToListAsync();

            return Ok(permisos);
        }
    }
}