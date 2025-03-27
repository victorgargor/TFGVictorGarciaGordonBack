namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que representa un número de Kaprekar.
    /// </summary>
    public class Kaprekar
    {
        /// <summary>
        /// Obtiene o establece el número.
        /// </summary>
        public int Numero { get; set; }

        /// <summary>
        /// Obtiene o establece un valor que indica si el número es un número de Kaprekar.
        /// </summary>
        public bool EsKaprekar { get; set; }

        /// <summary>
        /// Obtiene o establece el número de operaciones realizadas para verificar si es un número Kaprekar.
        /// </summary>
        public int Operaciones { get; set; }

        /// <summary>
        /// Inicializa una nueva instancia de la clase <see cref="Kaprekar"/>.
        /// </summary>
        /// <param name="numero">El número a verificar.</param>
        public Kaprekar(int numero)
        {
            // Verifica si el número es menor o igual a 0.
            if (numero <= 0)
            {
                throw new ArgumentException("El número debe ser mayor que 0.");
            }

            // Asigno el número.
            Numero = numero;

            // Verifica si el número es un número de Kaprekar.
            EsKaprekar = VerificarKaprekar(numero, out int operaciones);

            // Asigno las operaciones.
            Operaciones = operaciones;
        }

        /// <summary>
        /// Verifica si el número especificado es un número Kaprekar.
        /// </summary>
        /// <param name="numero">El número a verificar.</param>
        /// <param name="operaciones">El número de operaciones realizadas para verificar.</param>
        /// <returns><c>true</c> si el número es un número de Kaprekar; en caso contrario, <c>false</c>.</returns>
        private bool VerificarKaprekar(int numero, out int operaciones)
        {
            operaciones = 0;
            long cuadrado = (long)numero * numero;
            string cuadradoStr = cuadrado.ToString();

            for (int i = 1; i <= cuadradoStr.Length; i++)
            {
                // Contador de operaciones
                operaciones++;

                // Divido la cadena en partes (derecha e izquierda)
                string parteIzquierdaStr = cuadradoStr.Substring(0, i);
                string parteDerechaStr = cuadradoStr.Substring(i);

                // Las convierto a long para que se pueda trabajar con números más grandes y no haya problemas
                // En el caso 
                long parteIzquierda = long.Parse(parteIzquierdaStr);
                long parteDerecha = parteDerechaStr.Length > 0 ? long.Parse(parteDerechaStr) : 0;

                // Si la suma de la parte izquierda y la parte derecha es igual al número, este es Kaprekar
                if (parteIzquierda + parteDerecha == numero)
                {
                    return true;
                }
            }

            // En el caso de que no sea Kaprekar
            return false;
        }
    }
}
