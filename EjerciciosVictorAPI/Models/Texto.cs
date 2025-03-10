using System.Diagnostics;
using System.Text;

namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public class Texto
    {
        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public int ContarCaracteres(string texto)
        {
            return texto.Length;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public string ConvertirAMayusculas(string texto)
        {
            return texto.ToUpper();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public string ConvertirAMinusculas(string texto)
        {
            return texto.ToLower();
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public Dictionary<string, int> ContarRepetidas(string texto)
        {
            // Divide el texto en palabras, separadas por espacios
            var palabras = texto.Split([' '], StringSplitOptions.RemoveEmptyEntries);

            // Muestro las palabras repetidas y su frecuencia
            var contadorPalabras = palabras
                .GroupBy(palabra => palabra)
                .Where(grupo => grupo.Count() > 1)
                .ToDictionary(grupo => grupo.Key, grupo => grupo.Count());

            return contadorPalabras;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <returns></returns>
        public string ReemplazarPalabra(string texto)
        {
            if (texto.Contains("Proconsi"))
            {
                texto = texto.Replace("Proconsi", "Isnocorp");
            }

            return texto;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="texto"></param>
        /// <param name="veces"></param>
        /// <returns></returns>
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
