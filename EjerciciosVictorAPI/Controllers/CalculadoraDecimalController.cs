using System.Globalization;
using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para manejar las operaciones de la Calculadora.
    /// Utiliza distintos endpoints para sumar, restar, dividir, multiplicar, calcular el módulo y comparar números.
    /// </summary>
    [ApiController]
    [Route("api/calculadoraDecimal")]
    public class CalculadoraDecimalController : ControllerBase
    {
        /// <summary>
        /// Propiedad que instancia la implementación de ICalculadoraDecimal para realizar los cálculos.
        /// </summary>
        public ICalculadoraDecimal Calculadora { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia de la Calculadora.
        /// Se utiliza la implementación concreta de ICalculadoraDecimal (CalculadoraDecimal).
        /// </summary>
        public CalculadoraDecimalController()
        {
            // Se crea una nueva instancia de la clase CalculadoraDecimal.
            Calculadora = new CalculadoraDecimal();
        }

        /// <summary>
        /// Método privado que valida y convierte los parámetros numéricos (en formato string) a tipo decimal.
        /// Si hay error en el formato, se devuelve un BadRequest con un mensaje de error.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numParseado1">Número 1 convertido a decimal.</param>
        /// <param name="numParseado2">Número 2 convertido a decimal.</param>
        /// <returns>Retorna null si la validación es correcta o un ActionResult con error en caso contrario.</returns>
        private ActionResult? ValidarYParsearNumeros(string num1, string num2, out decimal numParseado1, out decimal numParseado2)
        {
            numParseado1 = 0;
            numParseado2 = 0;

            // Verifico que no se use la coma (,) como separador decimal.
            if (num1.Contains(',') || num2.Contains(','))
            {
                // En caso de que si haya
                return BadRequest(new { Error = Constantes.ERROR_SEPARADOR_DECIMAL });
            }

            // Convierto a decimal
            if (!decimal.TryParse(num1, NumberStyles.Any, CultureInfo.InvariantCulture, out numParseado1) ||
                !decimal.TryParse(num2, NumberStyles.Any, CultureInfo.InvariantCulture, out numParseado2))
            {
                // En caso de error
                return BadRequest(new { Error = Constantes.ERROR_NUMEROS_INVALIDOSDECIMAL });
            }

            return null;
        }

        /// <summary>
        /// Método privado para validar que el número de decimales esté dentro del rango permitido.
        /// </summary>
        /// <param name="numDec">Número de decimales a usar en la operación.</param>
        /// <returns>Retorna null si es válido o un ActionResult con error en caso contrario.</returns>
        private ActionResult? ValidarDecimales(int numDec)
        {
            // Verifico el rango entre 0 y 8
            if (numDec < Constantes.MIN_DECIMALES || numDec > Constantes.MAX_DECIMALES)
            {
                // En caso de error
                return BadRequest(new { Error = $"El número de decimales debe estar entre {Constantes.MIN_DECIMALES} y {Constantes.MAX_DECIMALES}." });
            }
            return null;
        }

        /// <summary>
        /// Endpoint para realizar la suma de dos números.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numDec">Número de decimales a mostrar en el resultado.</param>
        /// <returns>Retorna el resultado de la suma o un error si ocurre algún fallo.</returns>
        [HttpGet("suma/{num1}/{num2}/{numDec}")]
        public ActionResult Sumar(string num1, string num2, int numDec)
        {
            // Valido y convierto a decimal
            var error = ValidarYParsearNumeros(num1, num2, out decimal numParseado1, out decimal numParseado2);
            if (error != null)
            {
                return error;
            }

            // Validación del número de decimales
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            try
            {
                decimal resultado = Calculadora.CalcularSuma(numParseado1, numParseado2, numDec);

                // Formateo el resultado según el número de decimales solicitado
                string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);

                // Devuelvo el resultado formateado
                return Ok(new { Resultado = resultadoFormateado });
            }
            catch (OverflowException)
            {
                // Si ocurre un OverflowException, retorno un error 500 con mensaje personalizado
                return StatusCode(500, new { Error = "El resultado es demasiado grande o pequeño para procesarlo." });
            }
            catch (Exception ex)
            {
                // Capturo cualquier otra excepción 
                return StatusCode(500, new { Error = "Ocurrió un error: " + ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para realizar la resta de dos números.
        /// </summary>
        [HttpGet("resta/{num1}/{num2}/{numDec}")]
        public ActionResult Restar(string num1, string num2, int numDec)
        {
            // Valido y convierto a decimal
            var error = ValidarYParsearNumeros(num1, num2, out decimal numParseado1, out decimal numParseado2);
            if (error != null)
            {
                return error;
            }

            // Validación del número de decimales
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            try
            {
                decimal resultado = Calculadora.CalcularResta(numParseado1, numParseado2, numDec);

                // Se formatea el resultado y se devuelve
                string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);
                return Ok(new { Resultado = resultadoFormateado });
            }
            // Manejo de excepciones
            catch (OverflowException)
            {
                return StatusCode(500, new { Error = "El resultado es demasiado grande o pequeño para procesarlo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ocurrió un error: " + ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para realizar la división de dos números.
        /// </summary>
        [HttpGet("division/{num1}/{num2}/{numDec}")]
        public ActionResult Dividir(string num1, string num2, int numDec)
        {
            // Valido y convierto a decimal
            var error = ValidarYParsearNumeros(num1, num2, out decimal numParseado1, out decimal numParseado2);
            if (error != null)
            {
                return error;
            }

            // Validación del número de decimales
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            // Verifica que el divisor no sea cero
            if (numParseado2 == 0)
            {
                return BadRequest(new { Error = Constantes.ERROR_DIVISION_CERO });
            }

            try
            {
                // Se realiza la división
                decimal resultado = Calculadora.CalcularDivision(numParseado1, numParseado2, numDec);

                // Formateo y devuelvo el resultado
                string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);
                return Ok(new { Resultado = resultadoFormateado });
            }
            // Manejo de excepciones
            catch (OverflowException)
            {
                return StatusCode(500, new { Error = "El resultado es demasiado grande o pequeño para procesarlo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ocurrió un error: " + ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para realizar la multiplicación de dos números.
        /// </summary>
        [HttpGet("multiplicacion/{num1}/{num2}/{numDec}")]
        public ActionResult Multiplicar(string num1, string num2, int numDec)
        {
            // Valido y convierto a decimal
            var error = ValidarYParsearNumeros(num1, num2, out decimal numParseado1, out decimal numParseado2);
            if (error != null)
            {
                return error;
            }

            // Validación del número de decimales
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            try
            {
                decimal resultado = Calculadora.CalcularMultiplicacion(numParseado1, numParseado2, numDec);

                // Formateo y devuelvo el resultado
                string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);
                return Ok(new { Resultado = resultadoFormateado });
            }
            // Manejo de excepciones
            catch (OverflowException)
            {
                return StatusCode(500, new { Error = "El resultado es demasiado grande o pequeño para procesarlo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ocurrió un error: " + ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para calcular el módulo de dos números.
        /// </summary>
        [HttpGet("modulo/{num1}/{num2}/{numDec}")]
        public ActionResult Modular(string num1, string num2, int numDec)
        {
            // Valido y convierto los números
            var error = ValidarYParsearNumeros(num1, num2, out decimal numParseado1, out decimal numParseado2);
            if (error != null) return error;

            // Valido del número de decimales
            error = ValidarDecimales(numDec);
            if (error != null) return error;

            // Verifico que el divisor no sea cero
            if (numParseado2 == 0)
            {
                return BadRequest(new { Error = Constantes.ERROR_DIVISION_CERO });
            }

            try
            {
                decimal resultado = Calculadora.CalcularModulo(numParseado1, numParseado2, numDec);

                // Formateo y devuelvo el resultado
                string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);
                return Ok(new { Resultado = resultadoFormateado });
            }
            // Manejo de excepciones
            catch (OverflowException)
            {
                return StatusCode(500, new { Error = "El resultado es demasiado grande o pequeño para procesarlo." });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ocurrió un error: " + ex.Message });
            }
        }

        /// <summary>
        /// Endpoint para comparar dos números.
        /// </summary>
        [HttpGet("comparacion/{num1}/{num2}")]
        public ActionResult Comparar(string num1, string num2)
        {
            // Valido y convierto a decimal
            var error = ValidarYParsearNumeros(num1, num2, out decimal numParseado1, out decimal numParseado2);
            if (error != null) return error;

            try
            {
                // Comparo los dos números
                int resultadoComparacion = Calculadora.CompararNumeros(numParseado1, numParseado2);

                // Selecciono el mensaje correspondiente de acuerdo al resultado de la operación de comparación
                string mensaje = resultadoComparacion switch
                {
                    1 => "El primer número es mayor.",
                    0 => "Los dos números son iguales.",
                    -1 => "El segundo número es mayor.",
                    _ => "Error al realizar la comparación."
                };
                return Ok(new { Resultado = mensaje });
            }
            // Manejo de excepciones
            catch (Exception ex)
            {
                return StatusCode(500, new { Error = "Ocurrió un error: " + ex.Message });
            }
        }
    }
}
