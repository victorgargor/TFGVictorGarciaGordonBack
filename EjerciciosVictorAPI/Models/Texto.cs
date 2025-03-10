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
            // Divide el texto en palabras, separadas por espacios
            var palabras = texto.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);

            // Crea un diccionario con las palabras repetidas y su frecuencia
            var contadorPalabras = palabras
                .GroupBy(palabra => palabra)
                .Where(grupo => grupo.Count() > 1)
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
            if (texto.Contains("Proconsi"))
            {
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
            var textoConcatenado = new StringBuilder(texto.Length * veces);
            var cronometro = Stopwatch.StartNew();

            for (var i = 0; i < veces; i++)
            {
                textoConcatenado.Append(texto);
            }

            cronometro.Stop();
            long tiempoTardado = cronometro.ElapsedMilliseconds;

            return (textoConcatenado.Length, tiempoTardado);
        }
    }
}
