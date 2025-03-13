using System.Diagnostics;
using System.Text;

namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que con métodos para trabajar con textos.
    /// </summary>
    public class Texto
    {
        /// <summary>
        /// Cuenta el número de carácteres del texto proporcionado.
        /// </summary>
        /// <param name="texto">Texto del cual contar los carácteres.</param>
        /// <returns>Número de carácteres en el texto.</returns>
        public int ContarCaracteres(string texto)
        {
            return texto.Length;
        }

        /// <summary>
        /// Convierte todo el texto a mayúsculas.
        /// </summary>
        /// <param name="texto">Texto a convertir a mayúsculas.</param>
        /// <returns>Texto en mayúsculas.</returns>
        public string ConvertirAMayusculas(string texto)
        {
            return texto.ToUpper();
        }

        /// <summary>
        /// Convierte todo el texto a minúsculas.
        /// </summary>
        /// <param name="texto">Texto a convertir a minúsculas.</param>
        /// <returns>Texto en minúsculas.</returns>
        public string ConvertirAMinusculas(string texto)
        {
            return texto.ToLower();
        }

        /// <summary>
        /// Cuenta las palabras repetidas en el texto y devuelve un diccionario con la frecuencia de cada palabra.
        /// </summary>
        /// <param name="texto">Texto para contar las palabras repetidas.</param>
        /// <returns>Diccionario con las palabras repetidas y su frecuencia.</returns>
        public Dictionary<string, int> ContarRepetidas(string texto)
        {
            // Divido el texto en palabras, separadas por espacios
            var palabras = texto.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            // Creo un diccionario con las palabras repetidas y su frecuencia
            var contadorPalabras = palabras
                // Agrupo las palabras iguales
                .GroupBy(palabra => palabra)
                // Selecciono los que tienen repetidas solo
                .Where(grupo => grupo.Count() > 1)
                // Creo un diccionario con las palabras y su frecuencia
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Count()); 

            return contadorPalabras; 
        }

        /// <summary>
        /// Reemplaza la palabra "Proconsi" por "Isnocorp" en el texto.
        /// </summary>
        /// <param name="texto">Texto en el cual reemplazar la palabra.</param>
        /// <returns>Texto con la palabra reemplazada.</returns>
        public string ReemplazarPalabra(string texto)
        {
            // Verifico si el texto contiene la palabra "Proconsi"
            if (texto.Contains("Proconsi")) 
            {
                // Reemplazo "Proconsi" con "Isnocorp"
                texto = texto.Replace("Proconsi", "Isnocorp"); 
            }

            return texto; 
        }

        /// <summary>
        /// Concatena el texto un número determinado de veces y mide el tiempo que tarda en hacerlo.
        /// </summary>
        /// <param name="texto">Texto a concatenar.</param>
        /// <param name="veces">Número de veces que el texto debe ser concatenado.</param>
        /// <returns>Una tupla con la longitud del texto concatenado y el tiempo que tardó en ms.</returns>
        public static (int, long) ConcatenarTexto(string texto, int veces)
        {
            // Creo un StringBuilder con una capacidad inicial para el texto concatenado
            var textoConcatenado = new StringBuilder(texto.Length * veces);

            // Inicio el cronómetro para medir el tiempo
            var cronometro = Stopwatch.StartNew();

            // Concateno el texto el número de veces especificado
            for (var i = 0; i < veces; i++)
            {
                textoConcatenado.Append(texto);
            }

            // Se detiene el cronómetro y guarda el tiempo en ms
            cronometro.Stop();
            long tiempoTardado = cronometro.ElapsedMilliseconds;

            // Devuelvo una tupla con la longitud del texto concatenado y el tiempo que tardó en hacerlo
            return (textoConcatenado.Length, tiempoTardado);
        }
    }
}
