using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.DTOs;
using EjerciciosVictorAPI.Entidades;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/permisos")]
    public class PermisosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public PermisosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<PermisoDTO>>> GetTodos()
        {
            var permisos = await context.Permisos
                .Select(p => new PermisoDTO { Id = p.Id.ToString(), Nombre = p.Nombre })
                .ToListAsync();

            return Ok(permisos);
        }

        [HttpPost]
        public async Task<ActionResult> Crear([FromBody] PermisoDTO permisoDTO)
        {
            var permiso = new Permiso
            {
                Id = Guid.Parse(permisoDTO.Id),
                Nombre = permisoDTO.Nombre
            };

            context.Permisos.Add(permiso);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("asignar")]
        public async Task<ActionResult> AsignarPermiso([FromBody] RolPermisoDTO dto)
        {
            var permisoId = Guid.Parse(dto.PermisoId);

            var yaExiste = await context.RolPermisos
                .AnyAsync(rp => rp.RolId == dto.RolId && rp.PermisoId == permisoId);

            if (yaExiste)
                return BadRequest("Este permiso ya está asignado al rol.");

            var rolPermiso = new RolPermiso
            {
                RolId = dto.RolId,
                PermisoId = permisoId
            };

            context.RolPermisos.Add(rolPermiso);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpPost("remover")]
        public async Task<ActionResult> RemoverPermiso([FromBody] RolPermisoDTO dto)
        {
            var permisoId = Guid.Parse(dto.PermisoId);

            var rolPermiso = await context.RolPermisos
                .FirstOrDefaultAsync(rp => rp.RolId == dto.RolId && rp.PermisoId == permisoId);

            if (rolPermiso == null)
                return NotFound("Relación no encontrada.");

            context.RolPermisos.Remove(rolPermiso);
            await context.SaveChangesAsync();
            return NoContent();
        }

        [HttpGet("{rolId}")]
        public async Task<ActionResult<List<PermisoDTO>>> ObtenerPermisosPorRol(string rolId)
        {
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
