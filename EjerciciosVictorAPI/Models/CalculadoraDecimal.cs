namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que implementa operaciones matemáticas básicas utilizando tipo decimal.
    /// </summary>
    public class CalculadoraDecimal : ICalculadoraDecimal
    {
        /// <summary>
        /// Calcula la suma de dos números y redondea el resultado a un número específico de decimales.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <param name="numDec">El número de decimales al que se redondeará el resultado.</param>
        /// <returns>El resultado de la suma redondeada.</returns>
        public decimal CalcularSuma(decimal num1, decimal num2, int numDec)
        {
            return Math.Round(num1 + num2, numDec);
        }

        /// <summary>
        /// Calcula la resta de dos números y redondea el resultado a un número específico de decimales.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <param name="numDec">El número de decimales al que se redondeará el resultado.</param>
        /// <returns>El resultado de la resta redondeada.</returns>
        public decimal CalcularResta(decimal num1, decimal num2, int numDec)
        {
            return Math.Round(num1 - num2, numDec);
        }

        /// <summary>
        /// Calcula la división de dos números y redondea el resultado a un número específico de decimales.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <param name="numDec">El número de decimales al que se redondeará el resultado.</param>
        /// <returns>El resultado de la división redondeada.</returns>
        public decimal CalcularDivision(decimal num1, decimal num2, int numDec)
        {
            return Math.Round(num1 / num2, numDec);
        }

        /// <summary>
        /// Calcula la multiplicación de dos números y redondea el resultado a un número específico de decimales.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <param name="numDec">El número de decimales al que se redondeará el resultado.</param>
        /// <returns>El resultado de la multiplicación redondeada.</returns>
        public decimal CalcularMultiplicacion(decimal num1, decimal num2, int numDec)
        {
            var resultado = Math.Round(num1 * num2, numDec);

            // Por si se multiplica un número negativo por 0 para que la salida no sea -0.
            if (resultado == 0)
            {
                return Math.Abs(resultado);
            }

            return resultado;
        }

        /// <summary>
        /// Calcula el módulo de dos números y redondea el resultado a un número específico de decimales.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <param name="numDec">El número de decimales al que se redondeará el resultado.</param>
        /// <returns>El resultado del módulo redondeado.</returns>
        public decimal CalcularModulo(decimal num1, decimal num2, int numDec)
        {
            var resultado = Math.Round(num1 % num2, numDec);

            // Devuelvo el valor absoluto ya que el módulo de un número negativo tiene que ser positivo o cero.
            return Math.Abs(resultado);
        }

        /// <summary>
        /// Compara dos números y devuelve un valor que indica su relación.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <returns>1 si num1 es mayor que num2, 0 si son iguales, -1 si num1 es menor que num2.</returns>
        public int CompararNumeros(decimal num1, decimal num2)
        {
            if (num1 > num2)
            {
                return 1;
            }
            else if (num1 == num2)
            {
                return 0;
            }
            return -1;
        }
    }
}
