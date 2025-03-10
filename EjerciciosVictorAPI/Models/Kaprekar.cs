namespace EjerciciosVictorAPI.Models
{
    public class Kaprekar
    {
        public int Numero { get; set; }
        public int Pasos { get; set; }

        // Constructor
        public Kaprekar(int num)
        {
            Numero = num;
            Pasos = 0;  // Inicializamos el contador de pasos
        }

        // Método para verificar si el número llega a 6174
        public bool EsKaprekar()
        {
            // Si el número es 6174 desde el principio
            if (Numero == 6174)
            {
                return true;
            }

            int numeroActual = Numero;
            Pasos = 0;

            // Si el número tiene todos los dígitos iguales (ejemplo: 1111, 2222, etc.), no se puede hacer el proceso
            if (EsNumeroConDigitosIguales(numeroActual))
            {
                return false;
            }

            // Realizamos el proceso hasta llegar a 6174 o alcanzar el máximo de 7 pasos
            while (numeroActual != 6174 && Pasos < 8)
            {
                // Convertir el número a una lista de dígitos
                string numStr = numeroActual.ToString().PadLeft(4, '0'); // Aseguramos que tenga 4 dígitos
                char[] digitos = numStr.ToCharArray();

                // Ordenamos los dígitos en orden ascendente y descendente
                Array.Sort(digitos);
                string ascendente = new(digitos);
                Array.Reverse(digitos);
                string descendente = new(digitos);

                // Convertimos los números ascendentes y descendentes
                int numeroAscendente = int.Parse(ascendente);
                int numeroDescendente = int.Parse(descendente);

                // Realizamos la resta
                numeroActual = numeroDescendente - numeroAscendente;
                Pasos++;  // Contamos el paso

                // Si llegamos a 6174, terminamos el proceso
                if (numeroActual == 6174)
                {
                    return true;
                }
            }

            return numeroActual == 6174;
        }

        // Método auxiliar para verificar si el número tiene todos los dígitos iguales
        private static bool EsNumeroConDigitosIguales(int number)
        {
            string numStr = number.ToString();
            return numStr.All(c => c == numStr[0]);
        }
    }
}
