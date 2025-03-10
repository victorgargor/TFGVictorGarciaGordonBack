using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/fecha")]
    public class FechaController : ControllerBase
    {
        public Fecha Fecha { get; set; }

        public FechaController()
        {
            Fecha = new Fecha();
        }

        [HttpPost("diferencia")]
        public ActionResult CalcularDiferencia([FromBody] FechasRequest fechaRequest)
        {
            try
            {
                // Convertir las fechas desde el request
                var (fecha1, fecha2) = fechaRequest.ConvertirFechas();

                // Responder con la diferencia en días
                return Ok(new
                {
                    diferencia = $"{Fecha.DiferenciarFechas(fecha1, fecha2)} días"
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al calcular la diferencia",
                    error = ex.Message
                });
            }
        }

        [HttpPost("inicio-fin")]
        public ActionResult MostrarInicioYFinAnyo([FromBody] FechasRequest fechaRequest)
        {
            try
            {
                // Convertir las fechas desde el request
                var (fecha1, fecha2) = fechaRequest.ConvertirFechas();

                return Ok(new
                {
                    primeraFecha = $"El inicio del año es {Fecha.SacarPrimerDiaAnyo(fecha1)} y el final es {Fecha.SacarUltimoDiaAnyo(fecha1)}",
                    segundaFecha = $"El inicio del año es {Fecha.SacarPrimerDiaAnyo(fecha2)} y el final es {Fecha.SacarUltimoDiaAnyo(fecha2)}"
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al obtener alguno de los datos",
                    error = ex.Message
                });
            }
        }

        [HttpPost("dias")]
        public ActionResult MostrarDiasAnyo([FromBody] FechasRequest fechaRequest)
        {
            try
            {
                // Convertir las fechas desde el request
                var (fecha1, fecha2) = fechaRequest.ConvertirFechas();

                return Ok(new
                {
                    primeraFecha = $"El año de esta fecha tiene {Fecha.CalcularDiasAnyo(fecha1)} días",
                    segundaFecha = $"El año de esta fecha tiene {Fecha.CalcularDiasAnyo(fecha2)} días"
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al calcular el número de días del año",
                    error = ex.Message
                });
            }
        }

        [HttpPost("semana")]
        public ActionResult MostrarNumeroSemana([FromBody] FechasRequest fechaRequest)
        {
            try
            {
                // Convertir las fechas desde el request
                var (fecha1, fecha2) = fechaRequest.ConvertirFechas();

                return Ok(new
                {
                    primeraFecha = $"Es la {Fecha.CalcularSemana(fecha1)}ª semana del mes",
                    segundaFecha = $"Es la {Fecha.CalcularSemana(fecha2)}ª semana del mes"
                });
            }
            catch (Exception ex)
            {
                // Manejo de excepciones
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al calcular la semana",
                    error = ex.Message
                });
            }
        }
    }
}
