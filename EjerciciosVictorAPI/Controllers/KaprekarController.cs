using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Maneja las solicitudes relacionadas con los números Kaprekar.
    /// </summary>
    [ApiController] 
    [Route("api/kaprekar")] 
    public class KaprekarController : ControllerBase
    {
        /// <summary>
        /// Obtiene la información sobre si un número es Kaprekar y el número de operaciones realizadas.
        /// </summary>
        /// <param name="numero">El número a verificar.</param>
        /// <returns>Un objeto Kaprekar que contiene la información del número.</returns>
        [HttpGet("{numero}")]
        public ActionResult<Kaprekar> GetKaprekar(string numero)
        {
            // Verifica si el parámetro ingresado es un número entero válido.
            if (!int.TryParse(numero, out int numeroEntero))
            {
                // Si hay error.
                return BadRequest(new { Mensaje = "El parámetro proporcionado no es un número entero válido." });
            }

            // Crea una instancia de la clase Kaprekar con el número proporcionado.
            var kaprekar = new Kaprekar(numeroEntero);

            // Devuelve la respuesta HTTP 200 OK con el objeto Kaprekar.
            return Ok(kaprekar);
        }
    }

    //(Versión Constante de Kaprekar) 
    //[ApiController]
    //[Route("api/kaprekar")]   
    //public class KaprekarController : ControllerBase
    //{
    //    [HttpPost]
    //    public IActionResult VerificarKaprekar([FromBody] int numero)
    //    {
    //        // Validamos que el número sea de 4 dígitos
    //        if (numero < 1000 || numero > 9999)
    //            return BadRequest("El número debe ser de 4 dígitos.");

    //        var kaprekar = new Kaprekar(numero);

    //        bool esKaprekar = kaprekar.EsKaprekar();

    //        // Si no es un número válido para el proceso
    //        if (!esKaprekar)
    //        {
    //            return Ok(new
    //            {
    //                EsKaprekar = false,
    //                Mensaje = "El número no es válido para el proceso de Kaprekar o tiene dígitos iguales.",
    //            });
    //        }

    //        return Ok(new
    //        {
    //            EsKaprekar = true,
    //            Pasos = $"{kaprekar.Pasos}"
    //        });
    //    }
    //}
}
