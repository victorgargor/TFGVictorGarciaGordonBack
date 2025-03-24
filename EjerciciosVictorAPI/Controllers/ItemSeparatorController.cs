using System.Globalization;
using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador de la API para procesar cadenas en el formato "NombreItem$$##PrecioItem$$##CantidadItem".
    /// </summary>
    [ApiController]
    [Route("api/itemseparator")]
    public class ItemSeparatorController : ControllerBase
    {
        /// <summary>
        /// Instancia por defecto de ItemSeparator.
        /// </summary>
        public ItemSeparator ItemSeparator { get; set; }

        /// <summary>
        /// Constructor que inicializa el controlador con un valor por defecto.
        /// </summary>
        public ItemSeparatorController()
        {
            // Se utiliza la constante para el separador al construir el valor por defecto.
            string valorPorDefecto = $"Bread{Constantes.SEPARADOR}12.5{Constantes.SEPARADOR}10";
            ItemSeparator = new ItemSeparator(valorPorDefecto);
        }

        /// <summary>
        /// Procesa la cadena de entrada y devuelve los datos del ítem.
        /// Si la cadena de entrada es nula o vacía, se utiliza un valor por defecto.
        /// </summary>
        /// <param name="request">Objeto con la cadena de entrada.</param>
        /// <returns>Respuesta con los datos procesados o mensaje de error.</returns>
        [HttpPost]
        public ActionResult SepararCadena([FromBody] ItemSeparatorRequest request)
        {
            try
            {
                // Si la cadena de entrada es nula o vacía, se usa el valor por defecto.
                string cadenaEntrada = string.IsNullOrWhiteSpace(request.CadenaEntrada)
                    ? $"{ItemSeparator.Name}{Constantes.SEPARADOR}{ItemSeparator.Price.ToString(CultureInfo.InvariantCulture)}{Constantes.SEPARADOR}{ItemSeparator.Quantity}"
                    : request.CadenaEntrada;

                // Valido y proceso con la clase ItemSeparator.
                var itemSeparator = new ItemSeparator(cadenaEntrada);

                // Salida
                return Ok(new
                {
                    ItemName = itemSeparator.Name,
                    ItemPrice = itemSeparator.Price,
                    ItemQuantity = itemSeparator.Quantity
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones.
                return StatusCode(500, new
                {
                    mensaje = $"Ocurrió un error al procesar la cadena, el formato debe ser 'NombreItem{Constantes.SEPARADOR}PrecioItem{Constantes.SEPARADOR}CantidadItem'",
                    error = ex.Message
                });
            }
        }
    }
}
