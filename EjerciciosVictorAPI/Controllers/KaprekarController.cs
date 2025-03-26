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
            // Verifico si el parámetro ingresado es un número entero válido.
            if (!int.TryParse(numero, out int numeroEntero) || numeroEntero < 0)
            {
                // Si hay error.
                return BadRequest(new { Mensaje = "El parámetro proporcionado debe de ser un número entero positivo (máximo 9 dígitos)." });
            }

            // Creo una instancia de la clase Kaprekar con el número proporcionado.
            var kaprekar = new Kaprekar(numeroEntero);

            // Devuelvo la respuesta.
            return Ok(kaprekar);
        }
    }
}
