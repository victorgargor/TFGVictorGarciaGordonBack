namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Representa un triángulo, forma geométrica con una base y altura.
    /// Hereda de la clase base Forma y proporciona la implementación para calcular el área.
    /// </summary>
    public class Triangulo : Forma
    {
        /// <summary>
        /// Base del triángulo.
        /// La base es uno de los lados del triángulo y se usa para calcular el área.
        /// </summary>
        public double Base { get; set; }

        /// <summary>
        /// Altura del triángulo.
        /// La altura es la distancia perpendicular desde la base hasta el vértice opuesto.
        /// </summary>
        public double Altura { get; set; }

        /// <summary>
        /// Constructor para crear un triángulo con base y altura específicos.
        /// Genera un color y unas coordenadas aleatorias para el triángulo.
        /// </summary>
        /// <param name="baseTriangulo">La base del triángulo.</param>
        /// <param name="altura">La altura del triángulo.</param>
        public Triangulo(double baseTriangulo, double altura)
        {
            // En caso de que se introduzcan valores negativos
            if (baseTriangulo <= 0 || altura <= 0)
            {
                throw new ArgumentException("La base y la altura del triángulo deben ser mayores que 0.");
            }
               
            Base = baseTriangulo;
            Altura = altura;

            // Generación de color aleatorio para el triángulo
            Color = this.GenerarColorAleatorio();

            // Generación de coordenadas aleatorias para el triángulo
            Centro = this.GenerarCoordenadasAleatorias();  
        }

        /// <summary>
        /// Calcula el área del triángulo.
        /// El área se calcula como 0.5 * base * altura.
        /// </summary>
        /// <returns>El área calculada del triángulo.</returns>
        public override double CalcularArea()
        {
            return 0.5 * Base * Altura;  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ObtenerPropiedades()
        {
            return $"Base: {Base}, Altura: {Altura}, Color: {Color}, Coordenadas: ({Centro.x}, {Centro.y})";
        }
    }
}
