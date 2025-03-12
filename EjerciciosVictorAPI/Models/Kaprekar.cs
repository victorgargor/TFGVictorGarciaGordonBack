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
            // Asigna el valor del número a la propiedad Numero.
            Numero = numero;

            // Verifica si el número es un número de Kaprekar.
            EsKaprekar = VerificarKaprekar(numero, out int operaciones);
            Operaciones = operaciones; // Asigna el número de operaciones.
        }

        /// <summary>
        /// Verifica si el número especificado es un número Kaprekar.
        /// </summary>
        /// <param name="numero">El número a verificar.</param>
        /// <param name="operaciones">El número de operaciones realizadas para verificar.</param>
        /// <returns><c>true</c> si el número es un número de Kaprekar; en caso contrario, <c>false</c>.</returns>
        private bool VerificarKaprekar(int numero, out int operaciones)
        {
            // Inicializo el contador de operaciones a 0.
            operaciones = 0;

            // Calculo el cuadrado del número.
            int cuadrado = numero * numero;

            // Convierto el cuadrado a una cadena de texto.
            string cuadradoStr = cuadrado.ToString();

            for (int i = 1; i < cuadradoStr.Length; i++)
            {
                operaciones++; // Se incrementa el contador de operaciones por vuelta.

                // Divido la cadena en parte izquierda y derecha.
                int parteIzquierda = int.Parse(cuadradoStr.Substring(0, i));
                int parteDerecha = int.Parse(cuadradoStr.Substring(i));

                // Verifico que la suma de las partes sea igual al número original y que la parte derecha no sea 0.
                if (parteIzquierda + parteDerecha == numero && parteDerecha != 0)
                {
                    return true; // Si es un número Kaprekar válido.
                }
            }

            // Si no es un número de Kaprekar.
            return false;
        }

        //(Versión Constante de Kaprekar)
        //public int Numero { get; set; }
        //public int Pasos { get; set; }

        //// Constructor
        //public Kaprekar(int num)
        //{
        //    Numero = num;
        //    Pasos = 0;  // Inicializamos el contador de pasos
        //}

        //// Método para verificar si el número llega a 6174
        //public bool EsKaprekar()
        //{
        //    // Si el número es 6174 desde el principio
        //    if (Numero == 6174)
        //    {
        //        return true;
        //    }

        //    int numeroActual = Numero;
        //    Pasos = 0;

        //    // Si el número tiene todos los dígitos iguales (ejemplo: 1111, 2222, etc.), no se puede hacer el proceso
        //    if (EsNumeroConDigitosIguales(numeroActual))
        //    {
        //        return false;
        //    }

        //    // Realizamos el proceso hasta llegar a 6174 o alcanzar el máximo de 7 pasos
        //    while (numeroActual != 6174 && Pasos < 8)
        //    {
        //        // Convertir el número a una lista de dígitos
        //        string numStr = numeroActual.ToString().PadLeft(4, '0'); // Aseguramos que tenga 4 dígitos
        //        char[] digitos = numStr.ToCharArray();

        //        // Ordenamos los dígitos en orden ascendente y descendente
        //        Array.Sort(digitos);
        //        string ascendente = new(digitos);
        //        Array.Reverse(digitos);
        //        string descendente = new(digitos);

        //        // Convertimos los números ascendentes y descendentes
        //        int numeroAscendente = int.Parse(ascendente);
        //        int numeroDescendente = int.Parse(descendente);

        //        // Realizamos la resta
        //        numeroActual = numeroDescendente - numeroAscendente;
        //        Pasos++;  // Contamos el paso

        //        // Si llegamos a 6174, terminamos el proceso
        //        if (numeroActual == 6174)
        //        {
        //            return true;
        //        }
        //    }

        //    return numeroActual == 6174;
        //}

        //// Método auxiliar para verificar si el número tiene todos los dígitos iguales
        //private static bool EsNumeroConDigitosIguales(int number)
        //{
        //    string numStr = number.ToString();
        //    return numStr.All(c => c == numStr[0]);
        //}
    }
}
