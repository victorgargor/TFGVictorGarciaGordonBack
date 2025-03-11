namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que muestra los datos necesarios para realizar la concatenación.
    /// </summary>
    public class ConcatenacionRequest
    {
        /// <summary>
        /// El número de veces que se quiere concatenar el texto. 
        /// Este valor debe ser un número entero mayor que 0 y menor o igual a 100000.
        /// </summary>
        public int Veces { get; set; }

        /// <summary>
        /// El texto que se va a concatenar. 
        /// Puede ser nulo si no se especifica y en ese caso se utilizará el texto predeterminado.
        /// </summary>
        public string? Texto { get; set; }
    }
}
