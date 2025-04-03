using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        // GET: api/clientes
        [HttpGet]
        public async Task<IEnumerable<Cliente>> ListarClientes([FromQuery] string orden = "dni", [FromQuery] bool? activos = null)
        {
            var query = context.Clientes.AsQueryable();

            if (activos.HasValue)
            {
                query = activos.Value
                    ? query.Where(c => c.FechaBaja == null)
                    : query.Where(c => c.FechaBaja != null);
            }

            return orden.ToLower() == "fecha"
                ? await query.OrderBy(c => c.FechaAlta).ToListAsync()
                : await query.OrderBy(c => c.DNI).ToListAsync();
        }

        // GET: api/clientes/{dni}
        [HttpGet("{dni}")]
        public async Task<ActionResult<Cliente>> ObtenerClientePorDNI(string dni)
        {
            var cliente = await context.Clientes.Include(c => c.Recibos)
                                                .FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente == null)
            {
                return NotFound();
            }
            return cliente;
        }

        // POST: api/clientes
        [HttpPost]
        public async Task<ActionResult> CrearCliente(Cliente cliente)
        {
            var existe = await context.Clientes.AnyAsync(c => c.DNI == cliente.DNI);
            if (existe)
            {
                return BadRequest("Ya existe un cliente con ese DNI.");
            }

            if (cliente.Tipo == TipoCliente.REGISTRADO)
            {
                if (cliente.CuotaMaxima is null || cliente.CuotaMaxima <= 0)
                {
                    return BadRequest("Los clientes REGISTRADOS deben tener una cuota máxima válida.");
                }
            }
            else if (cliente.Tipo == TipoCliente.SOCIO)
            {
                if (cliente.CuotaMaxima.HasValue)
                {
                    return BadRequest("Los SOCIOS no deben tener una cuota máxima.");
                }
            }

            context.Add(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }

        // PUT: api/clientes/{dni}
        [HttpPut("{dni}")]
        public async Task<ActionResult> EditarCliente(string dni, Cliente cliente)
        {
            if (dni != cliente.DNI)
            {
                return BadRequest("El DNI no puede ser modificado");
            }

            if (cliente.Tipo == TipoCliente.REGISTRADO)
            {
                if (cliente.CuotaMaxima is null || cliente.CuotaMaxima <= 0)
                {
                    return BadRequest("Los clientes REGISTRADOS deben tener una cuota máxima válida.");
                }
            }
            else if (cliente.Tipo == TipoCliente.SOCIO)
            {
                if (cliente.CuotaMaxima.HasValue)
                {
                    return BadRequest("Los SOCIOS no deben tener una cuota máxima.");
                }
            }

            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }

        // DELETE: api/clientes/{dni}
        [HttpDelete("{dni}")]
        public async Task<ActionResult> EliminarCliente(string dni)
        {
            var registrosBorrados = await context.Clientes.Where(c => c.DNI == dni).ExecuteDeleteAsync();
            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            return Ok();
        }

        // PUT: api/clientes/baja/{dni}
        [HttpPut("baja/{dni}")]
        public async Task<ActionResult> DarBajaCliente(string dni)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            if (cliente.FechaBaja != null)
            {
                return BadRequest("El cliente ya se encuentra dado de baja.");
            }

            // Convertir la fecha actual a UTC
            cliente.FechaBaja = DateTime.Now.ToUniversalTime();
            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok("Cliente dado de baja.");
        }

        // PUT: api/clientes/reactivar/{dni}
        [HttpPut("reactivar/{dni}")]
        public async Task<ActionResult> ReactivarCliente(string dni)
        {
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            if (cliente.FechaBaja == null)
            {
                return BadRequest("El cliente ya está activo.");
            }

            cliente.FechaBaja = null;
            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok("Cliente reactivado.");
        }

        // NUEVO ENDPOINT: GET: api/clientes/buscar
        [HttpGet("buscar")]
        public async Task<ActionResult<IEnumerable<Cliente>>> BuscarClientes(
            [FromQuery] string? termino,
            [FromQuery] DateTime? fechaInicio,
            [FromQuery] DateTime? fechaFin,
            [FromQuery] string orden = "dni",
            [FromQuery] string estado = "todos")
        {
            var query = context.Clientes.AsQueryable();

            // Filtrar por término en DNI, Nombre, Apellido1 y Apellido2
            if (!string.IsNullOrWhiteSpace(termino))
            {
                query = query.Where(c =>
                    c.DNI.Contains(termino) ||
                    c.Nombre.Contains(termino) ||
                    (c.Apellido1 != null && c.Apellido1.Contains(termino)) ||
                    (c.Apellido2 != null && c.Apellido2.Contains(termino)));
            }

            // Convertir fechas a UTC si el Kind es Unspecified
            if (fechaInicio.HasValue)
            {
                var fi = fechaInicio.Value;
                if (fi.Kind == DateTimeKind.Unspecified)
                    fi = DateTime.SpecifyKind(fi, DateTimeKind.Utc);
                query = query.Where(c => c.FechaAlta >= fi);
            }
            if (fechaFin.HasValue)
            {
                var ff = fechaFin.Value;
                if (ff.Kind == DateTimeKind.Unspecified)
                    ff = DateTime.SpecifyKind(ff, DateTimeKind.Utc);
                query = query.Where(c => c.FechaAlta <= ff);
            }

            // Filtrar por estado
            switch (estado.ToLower())
            {
                case "activos":
                    query = query.Where(c => c.FechaBaja == null);
                    break;
                case "baja":
                    query = query.Where(c => c.FechaBaja != null);
                    break;
                    // "todos": sin filtro adicional
            }

            // Ordenar
            query = orden.ToLower() == "fecha"
                ? query.OrderBy(c => c.FechaAlta)
                : query.OrderBy(c => c.DNI);

            var clientes = await query.ToListAsync();
            if (clientes.Count == 0)
            {
                return NotFound("No se encontraron clientes con esos criterios.");
            }
            return Ok(clientes);
        }
    }
}