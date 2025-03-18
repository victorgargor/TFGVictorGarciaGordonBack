using System.Globalization;
using EjerciciosVictorAPI.Models;
using Microsoft.AspNetCore.Mvc;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para manejar las operaciones de la clase Calculadora.
    /// </summary>
    [ApiController]
    [Route("api/calculadora")]
    public class CalculadoraController : ControllerBase
    {
        /// <summary>
        /// Propiedad que instancia el objeto Calculadora para realizar las operaciones.
        /// </summary>
        public Calculadora Calculadora { get; set; }

        /// <summary>
        /// Constructor que inicializa la instancia de la clase Calculadora.
        /// </summary>
        public CalculadoraController()
        {
            Calculadora = new Calculadora();
        }

        /// <summary>
        /// Método que valida los números de entrada para asegurarse de que son correctos.
        /// </summary>
        /// <param name="num1">Primer número (en formato string).</param>
        /// <param name="num2">Segundo número (en formato string).</param>
        /// <returns>Un ActionResult con un error en caso de que la validación falle, o null si valida.</returns>
        private ActionResult? ValidarNumeros(string num1, string num2)
        {
            // Comprueba si los números contienen comas.
            if (num1.Contains(',') || num2.Contains(','))
            {
                return BadRequest(new
                {
                    Error = "El formato de los números no es válido, se debe usar punto (.) como separador decimal."
                });
            }

            // Comprueba si los números son válidos y se pueden convertir a double.
            if (!double.TryParse(num1, NumberStyles.Any, CultureInfo.InvariantCulture, out _) ||
                !double.TryParse(num2, NumberStyles.Any, CultureInfo.InvariantCulture, out _))
            {
                return BadRequest(new
                {
                    Error = "Los números no son válidos"
                });
            }

            return null;
        }

        /// <summary>
        /// Método para realizar la suma de dos números.
        /// </summary>
        /// <param name="num1">Primer número (en formato string).</param>
        /// <param name="num2">Segundo número (en formato string).</param>
        /// <param name="numDec">Número de decimales que debe mostrar el resultado.</param>
        /// <returns>Resultado de la suma en formato JSON.</returns>
        [HttpGet("suma/{num1}/{num2}/{numDec}")]
        public ActionResult Sumar(string num1, string num2, int numDec)
        {
            // Validar que los números sean correctos.
            var validacion = ValidarNumeros(num1, num2);
            if (validacion != null)
            {
                return validacion;
            }   

            // Convertir las cadenas a números tipo double.
            double.TryParse(num1, CultureInfo.InvariantCulture, out double n1);
            double.TryParse(num2, CultureInfo.InvariantCulture, out double n2);

            // Calcular la suma y devolver el resultado.
            return Ok(new
            {
                Resultado = Calculadora.CalcularSuma(n1, n2, numDec)
            });
        }

        /// <summary>
        /// Método para realizar la resta de dos números.
        /// </summary>
        /// <param name="num1">Primer número (en formato string).</param>
        /// <param name="num2">Segundo número (en formato string).</param>
        /// <param name="numDec">Número de decimales que debe mostrar el resultado.</param>
        /// <returns>Resultado de la resta en formato JSON.</returns>
        [HttpGet("resta/{num1}/{num2}/{numDec}")]
        public ActionResult Restar(string num1, string num2, int numDec)
        {
            // Validar que los números sean correctos.
            var validacion = ValidarNumeros(num1, num2);
            if (validacion != null) return validacion;

            // Convertir las cadenas a números tipo double.
            double.TryParse(num1, CultureInfo.InvariantCulture, out double n1);
            double.TryParse(num2, CultureInfo.InvariantCulture, out double n2);

            // Calcular la resta y devolver el resultado.
            return Ok(new
            {
                Resultado = Calculadora.CalcularResta(n1, n2, numDec)
            });
        }

        /// <summary>
        /// Método para realizar la división de dos números.
        /// </summary>
        /// <param name="num1">Primer número (en formato string).</param>
        /// <param name="num2">Segundo número (en formato string).</param>
        /// <param name="numDec">Número de decimales que debe mostrar el resultado.</param>
        /// <returns>Resultado de la división o error si el divisor es cero.</returns>
        [HttpGet("division/{num1}/{num2}/{numDec}")]
        public ActionResult Dividir(string num1, string num2, int numDec)
        {
            // Validar que los números sean correctos.
            var validacion = ValidarNumeros(num1, num2);
            if (validacion != null) return validacion;

            // Convertir las cadenas a números tipo double.
            double.TryParse(num1, CultureInfo.InvariantCulture, out double n1);
            double.TryParse(num2, CultureInfo.InvariantCulture, out double n2);

            // Comprobar si el divisor es cero.
            if (n2 == 0)
            {
                return BadRequest(new
                {
                    Error = "No se puede dividir por 0."
                });
            }

            // Calcular la división y devolver el resultado.
            return Ok(new
            {
                Resultado = Calculadora.CalcularDivision(n1, n2, numDec)
            });
        }

        /// <summary>
        /// Método para realizar la multiplicación de dos números.
        /// </summary>
        /// <param name="num1">Primer número (en formato string).</param>
        /// <param name="num2">Segundo número (en formato string).</param>
        /// <param name="numDec">Número de decimales que debe mostrar el resultado.</param>
        /// <returns>Resultado de la multiplicación en formato JSON.</returns>
        [HttpGet("multiplicacion/{num1}/{num2}/{numDec}")]
        public ActionResult Multiplicar(string num1, string num2, int numDec)
        {
            // Validar que los números sean correctos.
            var validacion = ValidarNumeros(num1, num2);
            if (validacion != null) return validacion;

            // Convertir las cadenas a números tipo double.
            double.TryParse(num1, CultureInfo.InvariantCulture, out double n1);
            double.TryParse(num2, CultureInfo.InvariantCulture, out double n2);

            // Calcular la multiplicación y devolver el resultado.
            return Ok(new
            {
                Resultado = Calculadora.CalcularMultiplicacion(n1, n2, numDec)
            });
        }

        /// <summary>
        /// Método para realizar la operación de módulo (residuo) entre dos números.
        /// </summary>
        /// <param name="num1">Primer número (en formato string).</param>
        /// <param name="num2">Segundo número (en formato string).</param>
        /// <param name="numDec">Número de decimales que debe mostrar el resultado.</param>
        /// <returns>Resultado del módulo o error si el divisor es cero.</returns>
        [HttpGet("modulo/{num1}/{num2}/{numDec}")]
        public ActionResult Modular(string num1, string num2, int numDec)
        {
            // Validar que los números sean correctos.
            var validacion = ValidarNumeros(num1, num2);
            if (validacion != null) return validacion;

            // Convertir las cadenas a números tipo double.
            double.TryParse(num1, CultureInfo.InvariantCulture, out double n1);
            double.TryParse(num2, CultureInfo.InvariantCulture, out double n2);

            // Comprobar si el divisor es cero.
            if (n2 == 0)
            {
                return BadRequest(new
                {
                    Error = "No se puede dividir por 0."
                });
            }

            // Calcular el módulo y devolver el resultado.
            return Ok(new
            {
                Resultado = Calculadora.CalcularModulo(n1, n2, numDec)
            });
        }

        /// <summary>
        /// Método para comparar dos números.
        /// </summary>
        /// <param name="num1">Primer número (en formato string).</param>
        /// <param name="num2">Segundo número (en formato string).</param>
        /// <returns>Resultado de la comparación entre los dos números.</returns>
        [HttpGet("comparacion/{num1}/{num2}")]
        public ActionResult Comparar(string num1, string num2)
        {
            // Validar que los números sean correctos.
            var validacion = ValidarNumeros(num1, num2);
            if (validacion != null) return validacion;

            // Convertir las cadenas a números tipo double.
            double.TryParse(num1, CultureInfo.InvariantCulture, out double n1);
            double.TryParse(num2, CultureInfo.InvariantCulture, out double n2);

            // Comparar los dos números.
            int comparacion = Calculadora.CompararNumeros(n1, n2);

            // Devolver el resultado de la comparación.
            if (comparacion == 1)
            {
                return Ok(new
                {
                    Resultado = "El primer número es mayor."
                });
            }
            else if (comparacion == 0)
            {
                return Ok(new
                {
                    Resultado = "Los dos números son iguales."
                });
            }

            return Ok(new
            {
                Resultado = "El segundo número es mayor."
            });
        }
    }
}

