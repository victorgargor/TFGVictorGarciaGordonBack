using System.Globalization;
using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador de la API que maneja las solicitudes para procesar una cadena con el formato "NombreItem$$##PrecioItem$$##CantidadItem".
    /// Este controlador recibe la solicitud del usuario, separa la cadena de entrada y devuelve los datos del ítem.
    /// Si no se proporciona una cadena, se utiliza el valor por defecto.
    /// </summary>
    [ApiController]
    [Route("api/itemseparator")]
    public class ItemSeparatorController : ControllerBase
    {
        /// <summary>
        /// Propiedad que almacena una instancia de <see cref="ItemSeparator"/> con los valores procesados.
        /// Se utiliza para almacenar el valor por defecto al inicializar el controlador.
        /// </summary>
        public ItemSeparator ItemSeparator { get; set; }

        /// <summary>
        /// Constructor del controlador. Inicializa una nueva instancia de <see cref="ItemSeparator"/> con un valor predeterminado.
        /// El valor por defecto utilizado es "Bread$$##12.5$$##10".
        /// </summary>
        public ItemSeparatorController()
        {
            string valorPorDefecto = "Bread$$##12.5$$##10";  
            ItemSeparator = new ItemSeparator(valorPorDefecto);
        }

        /// <summary>
        /// Procesa la cadena de entrada recibida en la solicitud y devuelve los datos del ítem.
        /// Si no se proporciona una cadena de entrada en la solicitud, se utiliza un valor por defecto.
        /// </summary>
        /// <param name="request">Objeto que contiene la cadena de entrada proporcionada por el cliente. 
        /// Si está vacía o nula, se utiliza el valor por defecto.</param>
        /// <returns>Una respuesta con los datos procesados del ítem o un mensaje de error si ocurre una excepción.</returns>
        [HttpPost]
        public ActionResult SepararCadena([FromBody] ItemSeparatorRequest request)
        {
            try
            {
                // Verifico si la cadena de entrada está vacía o nula y si es así uso el valor por defecto.
                string cadenaEntrada = string.IsNullOrEmpty(request.CadenaEntrada) ?
                    $"{ItemSeparator.Name}$$##{ItemSeparator.Price.ToString(CultureInfo.InvariantCulture)}$$##{ItemSeparator.Quantity}" :
                    request.CadenaEntrada;

                // Divido la cadena en partes
                string[] partes = cadenaEntrada.Split("$$##");

                // Se valida si el precio contiene (,).
                if (partes[1].Contains(','))
                {
                    return BadRequest(new { mensaje = "El separador decimal debe ser un punto (.) y no una coma (,)." });
                }

                // Crear una nueva instancia de ItemSeparator con la cadena de entrada o la que está por defecto.
                var itemSeparator = new ItemSeparator(cadenaEntrada);

                // Devolver la respuesta con la información.
                return Ok(new
                {
                    ItemName = itemSeparator.Name,
                    ItemPrice = itemSeparator.Price,
                    ItemQuantity = itemSeparator.Quantity
                });
            }
            catch (FormatException ex)
            {
                // En caso de error de formato.
                return BadRequest(new
                {
                    mensaje = "Error de formato en la cadena de entrada.",
                    error = ex.Message
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones.
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al procesar la cadena, el formato debe ser 'NombreItem$$##PrecioItem$$##CantidadItem'",
                    error = ex.Message
                });
            }
        }
    }
}
