namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase abstracta que representa una forma geométrica.
    /// Todas las formas geométricas (Círculo, Cuadrado, Triángulo) deben heredar de esta clase.
    /// </summary>
    public abstract class Forma
    {
        /// <summary>
        /// Color de la forma.
        /// </summary>
        public string? Color { get; set; }

        /// <summary>
        /// Coordenadas del centro de la forma en el plano (x, y).
        /// </summary>
        public (int x, int y) Centro { get; set; }

        /// <summary>
        /// Método abstracto que debe ser implementado en las clases derivadas para calcular el área de la forma.
        /// </summary>
        /// <returns>El área calculada de la forma.</returns>
        public abstract double CalcularArea();

        /// <summary>
        /// Método abstracto para obtener las propiedades específicas de cada forma.
        /// </summary>
        public abstract string ObtenerPropiedades();

        /// <summary>
        /// Genera un color aleatorio para la forma.
        /// </summary>
        /// <returns>Un string con el nombre del color aleatorio.</returns>
        public string GenerarColorAleatorio()
        {
            // Instancia de la clase Random para generar valores aleatorios
            var random = new Random();

            // Array con los colores posibles
            var colores = new string[] { "Rojo", "Verde", "Azul", "Amarillo", "Naranja" };

            // Se devuelve un color aleatorio de los del array
            return colores[random.Next(colores.Length)];
        }

        /// <summary>
        /// Genera coordenadas aleatorias (x, y) para el centro de la forma.
        /// </summary>
        /// <returns>Una tupla (x, y) representando las coordenadas generadas.</returns>
        public (int, int) GenerarCoordenadasAleatorias()
        {
            // Instancia de la clase Random para generar valores aleatorios
            var random = new Random();

            // Genero dos números aleatorios para las coordenadas X e Y de 0 a 99 incluyendo a ambos
            return (random.Next(0, 100), random.Next(0, 100)); 
        }
    }
}

