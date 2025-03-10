namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que proporciona métodos para realizar cálculos matemáticos.
    /// </summary>
    public class Calculadora
    {
        /// <summary>
        /// Calcula la suma de dos números y redondea el resultado a un número específico de decimales.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <param name="numDec">El número de decimales al que se redondeará el resultado.</param>
        /// <returns>El resultado de la suma redondeada.</returns>
        public double CalcularSuma(double num1, double num2, int numDec)
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
        public double CalcularResta(double num1, double num2, int numDec)
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
        public double CalcularDivision(double num1, double num2, int numDec)
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
        public double CalcularMultiplicacion(double num1, double num2, int numDec)
        {
            return Math.Round(num1 * num2, numDec);
        }

        /// <summary>
        /// Calcula el módulo de dos números y redondea el resultado a un número específico de decimales.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <param name="numDec">El número de decimales al que se redondeará el resultado.</param>
        /// <returns>El resultado del módulo redondeado.</returns>
        public double CalcularModulo(double num1, double num2, int numDec)
        {
            return Math.Round(num1 % num2, numDec);
        }

        /// <summary>
        /// Compara dos números y devuelve un valor que indica su relación.
        /// </summary>
        /// <param name="num1">El primer número.</param>
        /// <param name="num2">El segundo número.</param>
        /// <returns>1 si num1 es mayor que num2, 0 si son iguales, -1 si num1 es menor que num2.</returns>
        public int CompararNumeros(double num1, double num2)
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

