using Microsoft.AspNetCore.Mvc;
using System.Globalization;
using Microsoft.EntityFrameworkCore;
using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.Entidades;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de recibos.
    /// </summary>
    [ApiController]
    [Route("api/recibos")]
    public class RecibosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor del controlador de recibos
        /// </summary>
        /// <param name="context">Contexto de la base de datos.</param>
        public RecibosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        /// <summary>
        /// Añade un nuevo recibo para un cliente.
        /// </summary>
        /// <param name="recibo">Datos del recibo a agregar.</param>
        [HttpPost]
        public async Task<ActionResult> AgregarRecibo(Recibo recibo)
        {
            // Valido que el número de recibo sea único
            if (await context.Recibos.AnyAsync(r => r.NumeroRecibo == recibo.NumeroRecibo))
            {
                return BadRequest("Ya existe un recibo con ese número.");
            }

            // Valido que el cliente exista
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == recibo.ClienteDNI);
            if (cliente == null)
            {
                return NotFound("No se encontró el cliente asociado al recibo.");
            }

            // Valido que el formato de la fecha de emisión sea (yyyy/MM/dd HHmmss)
            if (!EsFormatoFechaReciboValido(recibo.FechaEmision))
            {
                return BadRequest("La fecha de emisión debe tener el formato 'yyyy/MM/dd HHmmss'.");
            }

            // Validar que el importe no supere la cuota máxima si el cliente es REGISTRADO
            if (cliente.Tipo == "REGISTRADO" && cliente.CuotaMaxima.HasValue && recibo.Importe > cliente.CuotaMaxima.Value)
            {
                return BadRequest("El importe del recibo excede la cuota máxima permitida para este cliente.");
            }

            // Agrego el recibo a la base de datos
            context.Recibos.Add(recibo);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Obtiene todos los recibos asociados a un cliente, ordenados por fecha de emisión.
        /// </summary>
        /// <param name="dni">DNI del cliente.</param>
        [HttpGet("cliente/{dni}")]
        public async Task<ActionResult<IEnumerable<Recibo>>> ObtenerRecibosPorCliente(string dni)
        {
            var recibos = await context.Recibos
                .Where(r => r.ClienteDNI == dni)
                .OrderBy(r => r.FechaEmision)
                .ToListAsync();

            if (recibos == null || recibos.Count == 0)
            {
                return NotFound("No se encontraron recibos para el cliente especificado.");
            }

            return Ok(recibos);
        }

        /// <summary>
        /// Lista todos los recibos registrados en el sistema.
        /// </summary>
        /// <param name="orden">Criterio de ordenación: "cliente" o "fecha".</param>
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Recibo>>> ListarRecibos([FromQuery] string orden = "fecha")
        {
            List<Recibo> recibos;
            if (orden.ToLower() == "cliente")
            {
                recibos = await context.Recibos.OrderBy(r => r.ClienteDNI).ToListAsync();
            }
            // Ordeno por fecha de emisión
            else
            {
                recibos = await context.Recibos.OrderBy(r => r.FechaEmision).ToListAsync();
            }

            return Ok(recibos);
        }

        /// <summary>
        /// Edita los datos de un recibo existente.
        /// </summary>
        /// <param name="numeroRecibo">Número del recibo a editar.</param>
        /// <param name="reciboActualizado">Nuevos datos para el recibo.</param>
        [HttpPut("{numeroRecibo}")]
        public async Task<ActionResult> EditarRecibo(string numeroRecibo, Recibo reciboActualizado)
        {
            // Busco el recibo existente
            var reciboExistente = await context.Recibos.FirstOrDefaultAsync(r => r.NumeroRecibo == numeroRecibo);
            if (reciboExistente == null)
            {
                return NotFound("Recibo no encontrado.");
            }

            // Valido el formato de la fecha de emisión
            if (!EsFormatoFechaReciboValido(reciboActualizado.FechaEmision))
            {
                return BadRequest("La fecha de emisión debe tener el formato 'yyyy/MM/dd HHmmss'.");
            }

            // Obtengo el cliente asociado para validar la cuota máxima 
            var cliente = await context.Clientes.FirstOrDefaultAsync(c => c.DNI == reciboExistente.ClienteDNI);
            if (cliente == null)
            {
                return NotFound("Cliente asociado no encontrado.");
            }

            // Valido que el nuevo importe no supere la cuota máxima permitida para clientes REGISTRADOS
            if (cliente.Tipo == "REGISTRADO" && cliente.CuotaMaxima.HasValue && reciboActualizado.Importe > cliente.CuotaMaxima.Value)
            {
                return BadRequest("El importe del recibo excede la cuota máxima permitida para este cliente.");
            }

            // Actualizo solo el importe y la fecha de emisión
            reciboExistente.Importe = reciboActualizado.Importe;
            reciboExistente.FechaEmision = reciboActualizado.FechaEmision;

            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Elimina un recibo por su número.
        /// </summary>
        /// <param name="numeroRecibo">Número del recibo a eliminar.</param>
        [HttpDelete("{numeroRecibo}")]
        public async Task<ActionResult> EliminarRecibo(string numeroRecibo)
        {
            var recibo = await context.Recibos.FirstOrDefaultAsync(r => r.NumeroRecibo == numeroRecibo);
            if (recibo == null)
            {
                return NotFound("Recibo no encontrado.");
            }

            context.Recibos.Remove(recibo);
            await context.SaveChangesAsync();
            return Ok();
        }

        /// <summary>
        /// Valida que la fecha de emisión tenga el formato "yyyy/MM/dd HHmmss".
        /// </summary>
        /// <param name="fecha">Fecha a validar.</param>
        /// <returns>True si el formato es correcto, de lo contrario, false.</returns>
        private bool EsFormatoFechaReciboValido(DateTime fecha)
        {
            string formatoEsperado = "yyyy/MM/dd HHmmss";
            string fechaComoTexto = fecha.ToString(formatoEsperado);
            return DateTime.TryParseExact(fechaComoTexto, formatoEsperado, CultureInfo.InvariantCulture, DateTimeStyles.None, out _);
        }
    }
}
