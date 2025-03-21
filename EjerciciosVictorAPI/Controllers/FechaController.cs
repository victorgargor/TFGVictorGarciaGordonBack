using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador que maneja las peticiones relacionadas con operaciones con fechas.
    /// </summary>
    [ApiController]
    [Route("api/fecha")]
    public class FechaController : ControllerBase
    {
        /// <summary>
        /// Instancia de la clase Fecha que contiene los métodos para trabajar con fechas.
        /// </summary>
        public Fecha Fecha { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia de la clase Fecha.
        /// </summary>
        public FechaController()
        {
            Fecha = new Fecha();
        }

        /// <summary>
        /// Método que calcula la diferencia en días entre dos fechas.
        /// </summary>
        /// <param name="fechaRequest">Objeto que contiene las dos fechas a comparar.</param>
        /// <returns>La diferencia en días entre las dos fechas proporcionadas.</returns>
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
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al calcular la diferencia",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Método que obtiene el primer y último día del año para dos fechas proporcionadas.
        /// </summary>
        /// <param name="fechaRequest">Objeto que contiene las dos fechas para las cuales se obtendrán el inicio y fin del año.</param>
        /// <returns>Las fechas del inicio y fin del año de las dos fechas proporcionadas.</returns>
        [HttpPost("inicio-fin")]
        public ActionResult MostrarInicioYFinAnyo([FromBody] FechasRequest fechaRequest)
        {
            try
            {
                // Convertir las fechas desde el request
                var (fecha1, fecha2) = fechaRequest.ConvertirFechas();

                return Ok(new
                {
                    primeraFecha = $"{Environment.NewLine}El inicio del año es {Fecha.SacarPrimerDiaAnyo(fecha1)} y el final es {Fecha.SacarUltimoDiaAnyo(fecha1)}",
                    segundaFecha = $"{Environment.NewLine}El inicio del año es {Fecha.SacarPrimerDiaAnyo(fecha2)} y el final es {Fecha.SacarUltimoDiaAnyo(fecha2)}"
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al obtener alguno de los datos",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Método que calcula el número de días del año para las dos fechas proporcionadas.
        /// </summary>
        /// <param name="fechaRequest">Objeto que contiene las dos fechas para calcular los días del año.</param>
        /// <returns>El número de días en el año de cada una de las fechas proporcionadas.</returns>
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
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al calcular el número de días del año",
                    error = ex.Message
                });
            }
        }

        /// <summary>
        /// Método que calcula el número de la semana del año para las dos fechas proporcionadas.
        /// </summary>
        /// <param name="fechaRequest">Objeto que contiene las dos fechas para calcular el número de la semana.</param>
        /// <returns>El número de semana en que se encuentra cada una de las fechas proporcionadas.</returns>
        [HttpPost("semana")]
        public ActionResult MostrarNumeroSemana([FromBody] FechasRequest fechaRequest)
        {
            try
            {
                // Convertir las fechas desde el request
                var (fecha1, fecha2) = fechaRequest.ConvertirFechas();

                return Ok(new
                {
                    primeraFecha = $"Es la {Fecha.CalcularSemana(fecha1)}ª semana del año",
                    segundaFecha = $"Es la {Fecha.CalcularSemana(fecha2)}ª semana del año"
                });
            }
            catch (Exception ex)
            {
                // En caso de error
                return StatusCode(500, new
                {
                    mensaje = "Ocurrió un error al calcular la semana",
                    error = ex.Message
                });
            }
        }
    }
}
