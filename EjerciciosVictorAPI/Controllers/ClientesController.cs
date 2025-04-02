using System.Text.Json;
using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.Entidades;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de clientes.
    /// Permite listar, obtener, crear, editar y eliminar clientes (eliminación física).
    /// Se han añadido endpoints adicionales para dar de baja (baja lógica), reactivar y filtrar por clientes activos/inactivos.
    /// </summary>
    [ApiController]
    [Route("api/clientes")]
    public class ClientesController : ControllerBase
    {
        // Contexto de la base de datos inyectado a través del constructor
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor que recibe el ApplicationDbContext.
        /// </summary>
        /// <param name="context">Contexto de base de datos.</param>
        public ClientesController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Obtiene la lista de todos los clientes, ordenados según la preferencia del usuario.
        /// Se puede filtrar opcionalmente por clientes activos o inactivos.
        /// Si no se especifica el parámetro 'activos', se devuelven todos.
        /// </summary>
        /// <param name="orden">Criterio de ordenación ("dni" o "fecha").</param>
        /// <param name="activos">
        /// Parámetro opcional para filtrar:
        ///   - Si es true: solo se devuelven clientes activos (FechaBaja == null).
        ///   - Si es false: solo se devuelven clientes inactivos (FechaBaja != null).
        ///   - Si es nulo: se devuelven todos.
        /// </param>
        /// <returns>Lista de clientes.</returns>
        [HttpGet]
        public async Task<IEnumerable<Cliente>> ListarClientes([FromQuery] string orden = "dni", [FromQuery] bool? activos = null)
        {
            // Inicia la consulta a la base de datos
            var query = context.Clientes.AsQueryable();

            // Si se ha especificado el filtro de activos, se aplica la condición
            if (activos.HasValue)
            {
                if (activos.Value)
                {
                    query = query.Where(c => c.FechaBaja == null);
                }
                else
                {
                    query = query.Where(c => c.FechaBaja != null);
                }
            }

            // Se ordena la consulta según el parámetro 'orden'
            return orden.ToLower() == "fecha"
                ? await query.OrderBy(c => c.FechaAlta).ToListAsync()
                : await query.OrderBy(c => c.DNI).ToListAsync();
        }

        /// <summary>
        /// Obtiene un cliente por su DNI, incluyendo sus recibos.
        /// </summary>
        /// <param name="dni">DNI del cliente a obtener.</param>
        /// <returns>El cliente con sus recibos o NotFound si no se encuentra.</returns>
        [HttpGet("{dni}")]
        public async Task<ActionResult<Cliente>> ObtenerClientePorDNI(string dni)
        {
            // Busca el cliente usando el DNI y carga sus recibos relacionados
            var cliente = await context.Clientes.Include(c => c.Recibos)
                                                .FirstOrDefaultAsync(c => c.DNI == dni);
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
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public async Task<ActionResult> CrearCliente(Cliente cliente)
        {
            // Verifica si ya existe un cliente con ese DNI (suponiendo que es único)
            var existe = await context.Clientes.AnyAsync(c => c.DNI == cliente.DNI);
            if (existe)
            {
                return BadRequest("Ya existe un cliente con ese DNI.");
            }

            // Valida las reglas de negocio según el tipo de cliente
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

            // Agrega el cliente y guarda los cambios
            context.Add(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Actualiza los datos de un cliente existente.
        /// Se usa el DNI para identificar el cliente a actualizar.
        /// </summary>
        /// <param name="dni">DNI del cliente a actualizar.</param>
        /// <param name="cliente">Nuevos datos del cliente.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("{dni}")]
        public async Task<ActionResult> EditarCliente(string dni, Cliente cliente)
        {
            // Verifica que el DNI enviado en la URL coincida con el DNI del objeto
            if (dni != cliente.DNI)
            {
                return BadRequest("El DNI no puede ser modificado");
            }

            // Valida las reglas de negocio según el tipo de cliente
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

            // Actualiza el cliente y guarda los cambios en la base de datos
            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Elimina físicamente un cliente por su DNI.
        /// (Endpoint original que realiza eliminación física.)
        /// </summary>
        /// <param name="dni">DNI del cliente a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
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

        // ------------------- Endpoints adicionales para baja lógica y reactivación -------------------

        /// <summary>
        /// Da de baja (baja lógica) a un cliente, estableciendo la FechaBaja en la fecha actual.
        /// Este endpoint no elimina físicamente al cliente.
        /// </summary>
        /// <param name="dni">DNI del cliente a dar de baja.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("baja/{dni}")]
        public async Task<ActionResult> DarBajaCliente(string dni)
        {
            // Se busca el cliente por su DNI
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            // Si ya se encuentra dado de baja, se informa
            if (cliente.FechaBaja != null)
            {
                return BadRequest("El cliente ya se encuentra dado de baja.");
            }

            // Se asigna la fecha actual a FechaBaja para dar de baja lógicamente
            cliente.FechaBaja = DateTime.Now;
            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok("Cliente dado de baja.");
        }

        /// <summary>
        /// Reactiva un cliente que se había dado de baja, removiendo la FechaBaja.
        /// </summary>
        /// <param name="dni">DNI del cliente a reactivar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("reactivar/{dni}")]
        public async Task<ActionResult> ReactivarCliente(string dni)
        {
            // Se busca el cliente por su DNI
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == dni);
            if (cliente == null)
            {
                return NotFound("Cliente no encontrado.");
            }

            // Si el cliente ya está activo, se informa
            if (cliente.FechaBaja == null)
            {
                return BadRequest("El cliente ya está activo.");
            }

            // Se reactiva removiendo la fecha de baja
            cliente.FechaBaja = null;
            context.Update(cliente);
            await context.SaveChangesAsync();
            return Ok("Cliente reactivado.");
        }
    }
}