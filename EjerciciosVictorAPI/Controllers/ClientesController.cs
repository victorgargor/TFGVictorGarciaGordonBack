using System.Text.Json;
using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.Entidades; 
using Microsoft.AspNetCore.Mvc; 
using Microsoft.EntityFrameworkCore; 

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes.
    /// </summary>
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor del controlador de clientes.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Obtiene la lista de todos los clientes, ordenados según la preferencia del usuario.
        /// </summary>
        /// <param name="orden">Criterio de ordenación ("dni" o "fecha").</param>
        [HttpGet]
        public async Task<IEnumerable<Cliente>> Get([FromQuery] string orden = "dni")
        {
            // Devuelve la lista de clientes ordenada según el criterio del usuario
            return orden.ToLower() == "fecha"
                ? await context.Clientes.OrderBy(c => c.FechaAlta).ToListAsync()
                : await context.Clientes.OrderBy(c => c.DNI).ToListAsync();
        }

        /// <summary>
        /// Obtiene un cliente por su DNI.
        /// </summary>
        /// <param name="dni">DNI del cliente a obtener.</param>
        [HttpGet("{dni}")]
        public async Task<ActionResult<Cliente>> ObtenerClientePorDNI(string dni)
        {
            // Busca el cliente por DNI e incluye los recibos relacionados
            var cliente = await context.Clientes.Include(c => c.Recibos).FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente is null)
            {
                return NotFound();
            }
            return cliente;
        }

        /// <summary>
        /// Agrega un nuevo cliente asegurando que los datos sean correctos.
        /// </summary>
        /// <param name="cliente">Datos del cliente a agregar.</param>
        [HttpPost]
        public async Task<ActionResult> CrearCliente(Cliente cliente)
        {
            // Verifica si el DNI ya existe en la base de datos
            var existe = await context.Clientes.AnyAsync(c => c.DNI == cliente.DNI);
            if (existe)
            {
                return BadRequest("Ya existe un cliente con ese DNI.");
            }

            // Valido según el tipo de cliente
            if (cliente.Tipo == "REGISTRADO")
            {
                if (cliente.CuotaMaxima is null || cliente.CuotaMaxima <= 0)
                {
                    return BadRequest("Los clientes REGISTRADOS deben tener una cuota máxima válida.");
                }
            }
            else if (cliente.Tipo == "SOCIO")
            {
                if (cliente.CuotaMaxima.HasValue)
                {
                    return BadRequest("Los SOCIOS no deben tener una cuota máxima.");
                }
            }

            // Agrega el cliente y guarda los cambios en la base de datos
            context.Add(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// </summary>
        /// <param name="dni">DNI del cliente a actualizar.</param>
        /// <param name="cliente">Nuevos datos del cliente.</param>
        [HttpPut("{dni}")]
        public async Task<ActionResult> EditarCliente(string dni, Cliente cliente)
        {
            // Verifico que el DNI no ha sido modificado
            if (dni != cliente.DNI)
            {
                return BadRequest("El DNI no puede ser modificado");
            }

            // Actualiza el cliente y guarda los cambios en la base de datos
            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Elimina un cliente por su DNI.
        /// </summary>
        /// <param name="dni">DNI del cliente a eliminar.</param>
        [HttpDelete("{dni}")]
        public async Task<ActionResult> EliminarCliente(string dni)
        {
            // Busca y elimina el cliente por su DNI
            var registrosBorrados = await context.Clientes.Where(c => c.DNI == dni).ExecuteDeleteAsync();
            if (registrosBorrados == 0)
            {
                return NotFound();
            }
            return Ok();
        }
    }
}
