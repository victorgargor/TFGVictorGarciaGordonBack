using System.Globalization;
using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para manejar las operaciones de la Calculadora.
    /// Usa distintos endpoints para sumar, restar, dividir, multiplicar, calcular el módulo y comparar números.
    /// </summary>
    [ApiController]
    [Route("api/calculadora")]
    public class CalculadoraController : ControllerBase
    {
        /// <summary>
        /// Propiedad que instancia la implementación de ICalculadora para realizar los cálculos.
        /// </summary>
        public ICalculadora Calculadora { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia de la Calculadora.
        /// Se utiliza la implementación concreta de ICalculadora.
        /// </summary>
        public CalculadoraController()
        {
            // Se crea una nueva instancia de la clase Calculadora, que implementa ICalculadora.
            Calculadora = new Calculadora();
        }

        /// <summary>
        /// Método privado que valida y convierte los parámetros numéricos.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numParseado1">Primer número convertido a double.</param>
        /// <param name="numParseado2">Segundo número convertido a double.</param>
        /// <returns>Retorna un ActionResult con un error si la validación falla o null si pasa.</returns>
        private ActionResult? ValidarYParsearNumeros(string num1, string num2, out double numParseado1, out double numParseado2)
        {
            numParseado1 = 0;
            numParseado2 = 0;

            // Verifico que se use el punto (.) como separador decimal.
            if (num1.Contains(',') || num2.Contains(','))
            {
                // Si contiene coma, devuelve un error indicando que el separador decimal no es válido.
                return BadRequest(new { Error = Constantes.ERROR_SEPARADOR_DECIMAL });
            }

            // Convierto las cadenas a double.
            if (!double.TryParse(num1, NumberStyles.Any, CultureInfo.InvariantCulture, out numParseado1) ||
                !double.TryParse(num2, NumberStyles.Any, CultureInfo.InvariantCulture, out numParseado2))
            {
                // Si falla devuelvo el error correspondiente.
                return BadRequest(new { Error = Constantes.ERROR_NUMEROS_INVALIDOS });
            }

            // Si no hubo errores devuelve null.
            return null;
        }

        /// <summary>
        /// Método privado para validar el número de decimales permitido.
        /// </summary>
        /// <param name="numDec">Número de decimales solicitados.</param>
        /// <returns>Retorna un ActionResult con un error si el número de decimales no es válido, o null si es válido.</returns>
        private ActionResult? ValidarDecimales(int numDec)
        {
            // Verifico que el número de decimales esté dentro del rango permitido.
            if (numDec < Constantes.MIN_DECIMALES || numDec > Constantes.MAX_DECIMALES)
            {
                // Si no está en el rango devuelvo un error.
                return BadRequest(new { Error = $"El número de decimales debe estar entre {Constantes.MIN_DECIMALES} y {Constantes.MAX_DECIMALES}." });
            }
            return null;
        }

        /// <summary>
        /// Endpoint para realizar la operación de suma.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numDec">Número de decimales a mostrar en el resultado.</param>
        /// <returns>Retorna el resultado de la suma o un error en caso de fallo.</returns>
        [HttpGet("suma/{num1}/{num2}/{numDec}")]
        public ActionResult Sumar(string num1, string num2, int numDec)
        {
            // Valido y parseo los números proporcionados.
            var error = ValidarYParsearNumeros(num1, num2, out double numParseado1, out double numParseado2);
            if (error != null)
            {
                return error;
            }

            // Valido el número de decimales.
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            // Realizo la suma y devuelvo el resultado.
            double resultado = Calculadora.CalcularSuma(numParseado1, numParseado2, numDec);

            // Formateo el resultado para que me salga con el número de decimales que se ha proporcionado y con (.) como separador decimal y (,) de miles.
            string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);

            return Ok(new { Resultado = resultadoFormateado });
        }

        /// <summary>
        /// Endpoint para realizar la operación de resta.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numDec">Número de decimales a mostrar en el resultado.</param>
        /// <returns>Retorna el resultado de la resta o un error en caso de fallo.</returns>
        [HttpGet("resta/{num1}/{num2}/{numDec}")]
        public ActionResult Restar(string num1, string num2, int numDec)
        {
            // Valido y parseo los números proporcionados.
            var error = ValidarYParsearNumeros(num1, num2, out double numParseado1, out double numParseado2);
            if (error != null)
            {
                return error;
            }

            // Valido el número de decimales.
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            // Realizo la resta y devuelvo el resultado.
            double resultado = Calculadora.CalcularResta(numParseado1, numParseado2, numDec);

            // Formateo el resultado para que me salga con el número de decimales que se ha proporcionado y con (.) como separador decimal y (,) de miles.
            string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);

            return Ok(new { Resultado = resultadoFormateado });
        }

        /// <summary>
        /// Endpoint para realizar la operación de división.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numDec">Número de decimales a mostrar en el resultado.</param>
        /// <returns>Retorna el resultado de la división o un error si el divisor es cero.</returns>
        [HttpGet("division/{num1}/{num2}/{numDec}")]
        public ActionResult Dividir(string num1, string num2, int numDec)
        {
            // Valido y parseo los números proporcionados.
            var error = ValidarYParsearNumeros(num1, num2, out double numParseado1, out double numParseado2);
            if (error != null)
            {
                return error;
            }

            // Valido el número de decimales.
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            // Compruebo que el segundo número no sea 0.
            if (numParseado2 == 0)
            {
                // En el caso de que sea 0, devuelvo el error correspondiente.
                return BadRequest(new { Error = Constantes.ERROR_DIVISION_CERO });
            }

            // Realizo la división y devuelvo el resultado.
            double resultado = Calculadora.CalcularDivision(numParseado1, numParseado2, numDec);

            // Formateo el resultado para que me salga con el número de decimales que se ha proporcionado y con (.) como separador decimal y (,) de miles.
            string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);

            return Ok(new { Resultado = resultadoFormateado });
        }

        /// <summary>
        /// Endpoint para realizar la operación de multiplicación.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numDec">Número de decimales a mostrar en el resultado.</param>
        /// <returns>Retorna el resultado de la multiplicación o un error en caso de fallo.</returns>
        [HttpGet("multiplicacion/{num1}/{num2}/{numDec}")]
        public ActionResult Multiplicar(string num1, string num2, int numDec)
        {
            // Valido y parseo los números proporcionados.
            var error = ValidarYParsearNumeros(num1, num2, out double numParseado1, out double numParseado2);
            if (error != null)
            {
                return error;
            }

            // Valido el número de decimales.
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            // Realizo la multiplicación y devuelvo el resultado.
            double resultado = Calculadora.CalcularMultiplicacion(numParseado1, numParseado2, numDec);

            // Formateo el resultado para que me salga con el número de decimales que se ha proporcionado y con (.) como separador decimal y (,) de miles.
            string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);

            return Ok(new { Resultado = resultadoFormateado });
        }

        /// <summary>
        /// Endpoint para realizar la operación de módulo.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <param name="numDec">Número de decimales a mostrar en el resultado.</param>
        /// <returns>Retorna el resultado del módulo o un error si el divisor es cero.</returns>
        [HttpGet("modulo/{num1}/{num2}/{numDec}")]
        public ActionResult Modular(string num1, string num2, int numDec)
        {
            // Valido y parseo los números proporcionados.
            var error = ValidarYParsearNumeros(num1, num2, out double numParseado1, out double numParseado2);
            if (error != null)
            {
                return error;
            }

            // Valido el número de decimales.
            error = ValidarDecimales(numDec);
            if (error != null)
            {
                return error;
            }

            // Compruebo que el segundo número no sea 0.
            if (numParseado2 == 0)
            {
                // En el caso de que sea 0, devuelvo el error correspondiente.
                return BadRequest(new { Error = Constantes.ERROR_DIVISION_CERO });
            }

            // Calculo el módulo y devuelvo el resultado.
            double resultado = Calculadora.CalcularModulo(numParseado1, numParseado2, numDec);

            // Formateo el resultado para que me salga con el número de decimales que se ha proporcionado y con (.) como separador decimal y (,) de miles.
            string resultadoFormateado = resultado.ToString("N" + numDec, CultureInfo.InvariantCulture);

            return Ok(new { Resultado = resultadoFormateado });
        }

        /// <summary>
        /// Endpoint para comparar dos números.
        /// </summary>
        /// <param name="num1">Primer número en formato string.</param>
        /// <param name="num2">Segundo número en formato string.</param>
        /// <returns>Retorna el resultado de la comparación como un mensaje textual.</returns>
        [HttpGet("comparacion/{num1}/{num2}")]
        public ActionResult Comparar(string num1, string num2)
        {
            // Valido y parseo los números proporcionados.
            var error = ValidarYParsearNumeros(num1, num2, out double numParseado1, out double numParseado2);
            if (error != null)
            {
                return error;
            }

            // Comparo los dos números
            int resultadoComparacion = Calculadora.CompararNumeros(numParseado1, numParseado2);

            // Seleccionar el mensaje para el resultado dependiendo de la devolución del método de mi Calculadora.
            string mensaje = resultadoComparacion switch
            {
                1 => "El primer número es mayor.",
                0 => "Los dos números son iguales.",
                -1 => "El segundo número es mayor.",
                _ => "Error al realizar la comparación."
            };

            return Ok(new { Resultado = mensaje });
        }
    }
}
