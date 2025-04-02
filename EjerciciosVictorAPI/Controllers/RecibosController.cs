using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.Entidades;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de recibos.
    /// Permite agregar, listar, editar y eliminar recibos.
    /// Se validan reglas de negocio, como formato de fecha y restricciones según la cuota máxima del cliente.
    /// </summary>
    [ApiController]
    [Route("api/recibos")]
    public class RecibosController : ControllerBase
    {
        // Contexto de la base de datos inyectado a través del constructor
        private readonly ApplicationDbContext context;

        public RecibosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Agrega un nuevo recibo para un cliente.
        /// Se valida que el número de recibo sea único, que la fecha de emisión tenga el formato correcto,
        /// que el cliente exista y esté activo, y que el importe no exceda la cuota máxima (para clientes REGISTRADOS).
        /// </summary>
        /// <param name="recibo">Objeto Recibo con los datos a agregar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPost]
        public async Task<ActionResult> AgregarRecibo(Recibo recibo)
        {
            // Se verifica que no exista un recibo con el mismo número
            if (await context.Recibos.AnyAsync(r => r.NumeroRecibo == recibo.NumeroRecibo))
            {
                return BadRequest("Ya existe un recibo con ese número.");
            }

            // Se busca el cliente por su Id (clave foránea)
            var cliente = await context.Clientes.FindAsync(recibo.ClienteId);
            if (cliente == null)
            {
                return NotFound("No se encontró el cliente asociado al recibo.");
            }
            // Se verifica que el cliente esté activo (no tenga FechaBaja)
            if (cliente.FechaBaja != null)
            {
                return BadRequest("No se pueden agregar recibos para clientes inactivos.");
            }

            // Se valida que la fecha de emisión tenga el formato esperado "yyyy/MM/dd HHmmss"
            if (!EsFormatoFechaReciboValido(recibo.FechaEmision))
            {
                return BadRequest("La fecha de emisión debe tener el formato 'yyyy/MM/dd HHmmss'.");
            }

            // Para clientes REGISTRADOS, se valida que el importe no supere la cuota máxima
            if (cliente.Tipo == TipoCliente.REGISTRADO && cliente.CuotaMaxima.HasValue && recibo.Importe > cliente.CuotaMaxima.Value)
            {
                return BadRequest("El importe del recibo excede la cuota máxima permitida para este cliente.");
            }

            // Se agrega el recibo y se guardan los cambios en la base de datos
            context.Recibos.Add(recibo);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Obtiene todos los recibos asociados a un cliente, ordenados por fecha de emisión.
        /// </summary>
        /// <param name="clienteId">Id del cliente.</param>
        /// <returns>Lista de recibos del cliente.</returns>
        [HttpGet("cliente/{clienteId:int}")]
        public async Task<ActionResult<IEnumerable<Recibo>>> ObtenerRecibosPorCliente(int clienteId)
        {
            // Se filtran los recibos por ClienteId y se ordenan por FechaEmision
            var recibos = await context.Recibos
                .Where(r => r.ClienteId == clienteId)
                .OrderBy(r => r.FechaEmision)
                .ToListAsync();
            if (recibos == null || recibos.Count == 0)
            {
                return NotFound("No se encontraron recibos para el cliente especificado.");
            }
            return Ok(recibos);
        }

        /// <summary>
        /// Lista todos los recibos registrados en el sistema, permitiendo ordenar por cliente o por fecha de emisión.
        /// </summary>
        /// <param name="orden">Criterio de ordenación: "cliente" o "fecha".</param>
        /// <returns>Lista de recibos.</returns>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recibo>>> ListarRecibos([FromQuery] string orden = "fecha")
        {
            List<Recibo> recibos;
            // Si se ordena por "cliente", se ordena por ClienteId
            if (orden.ToLower() == "cliente")
            {
                recibos = await context.Recibos.OrderBy(r => r.ClienteId).ToListAsync();
            }
            else
            {
                // Por defecto, se ordena por FechaEmision
                recibos = await context.Recibos.OrderBy(r => r.FechaEmision).ToListAsync();
            }
            return Ok(recibos);
        }

        /// <summary>
        /// Edita los datos de un recibo existente.
        /// Se valida el formato de la fecha, que el cliente asociado esté activo y que el importe cumpla las reglas de cuota.
        /// </summary>
        /// <param name="id">Id del recibo a editar.</param>
        /// <param name="reciboActualizado">Objeto Recibo con los nuevos datos.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpPut("{id:int}")]
        public async Task<ActionResult> EditarRecibo(int id, Recibo reciboActualizado)
        {
            // Se busca el recibo existente por su Id
            var reciboExistente = await context.Recibos.FirstOrDefaultAsync(r => r.Id == id);
            if (reciboExistente == null)
            {
                return NotFound("Recibo no encontrado.");
            }
            // Se valida el formato de la fecha de emisión
            if (!EsFormatoFechaReciboValido(reciboActualizado.FechaEmision))
            {
                return BadRequest("La fecha de emisión debe tener el formato 'yyyy/MM/dd HHmmss'.");
            }

            // Se verifica que el cliente asociado al recibo esté activo
            var cliente = await context.Clientes.FindAsync(reciboExistente.ClienteId);
            if (cliente == null)
            {
                return NotFound("Cliente asociado no encontrado.");
            }
            if (cliente.FechaBaja != null)
            {
                return BadRequest("No se pueden editar recibos de clientes inactivos.");
            }
            // Se valida que para clientes REGISTRADOS el importe no supere la cuota máxima
            if (cliente.Tipo == TipoCliente.REGISTRADO && cliente.CuotaMaxima.HasValue && reciboActualizado.Importe > cliente.CuotaMaxima.Value)
            {
                return BadRequest("El importe del recibo excede la cuota máxima permitida para este cliente.");
            }

            // Se actualizan los campos modificables del recibo
            reciboExistente.Importe = reciboActualizado.Importe;
            reciboExistente.FechaEmision = reciboActualizado.FechaEmision;
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Elimina un recibo por su Id.
        /// </summary>
        /// <param name="id">Id del recibo a eliminar.</param>
        /// <returns>Resultado de la operación.</returns>
        [HttpDelete("{id:int}")]
        public async Task<ActionResult> EliminarRecibo(int id)
        {
            // Se busca el recibo por su Id
            var recibo = await context.Recibos.FirstOrDefaultAsync(r => r.Id == id);
            if (recibo == null)
            {
                return NotFound("Recibo no encontrado.");
            }
            // Se elimina el recibo de la base de datos
            context.Recibos.Remove(recibo);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Método privado que valida que la fecha de emisión tenga el formato "yyyy/MM/dd HHmmss".
        /// Se convierte la fecha a texto usando el formato y se intenta parsearla nuevamente.
        /// </summary>
        /// <param name="fecha">La fecha a validar.</param>
        /// <returns>True si el formato es correcto; de lo contrario, false.</returns>
        private bool EsFormatoFechaReciboValido(DateTime fecha)
        {
            string formatoEsperado = "yyyy/MM/dd HHmmss";
            string fechaComoTexto = fecha.ToString(formatoEsperado);
            return DateTime.TryParseExact(fechaComoTexto, formatoEsperado, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}

