using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/kaprekar")]   
    public class KaprekarController : ControllerBase
    {
        [HttpPost]
        public IActionResult VerificarKaprekar([FromBody] int numero)
        {
            // Validamos que el número sea de 4 dígitos
            if (numero < 1000 || numero > 9999)
                return BadRequest("El número debe ser de 4 dígitos.");

            var kaprekar = new Kaprekar(numero);

            bool esKaprekar = kaprekar.EsKaprekar();

            // Si no es un número válido para el proceso
            if (!esKaprekar)
            {
                return Ok(new
                {
                    EsKaprekar = false,
                    Mensaje = "El número no es válido para el proceso de Kaprekar o tiene dígitos iguales.",
                });
            }

            return Ok(new
            {
                EsKaprekar = true,
                Pasos = $"{kaprekar.Pasos}"
            });
        }
    }
}
