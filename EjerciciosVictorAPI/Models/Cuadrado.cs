namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Representa un cuadrado.
    /// Hereda de la clase base Forma y proporciona la implementación para calcular su área.
    /// </summary>
    public class Cuadrado : Forma
    {
        /// <summary>
        /// Lado del cuadrado.
        /// El lado define el tamaño del cuadrado.
        /// </summary>
        public double Lado { get; set; }

        /// <summary>
        /// Constructor para crear un cuadrado con un lado específico.
        /// Genera un color y unas coordenadas aleatorias para el cuadrado.
        /// </summary>
        /// <param name="lado">El tamaño del lado del cuadrado.</param>
        public Cuadrado(double lado)
        {
            // Valido que el lado sea positivo.
            if (lado <= 0)
            {
                throw new ArgumentException("El lado del cuadrado debe ser mayor que 0.");
            }

            Lado = lado;

            // Generación de color aleatorio para el cuadrado
            Color = this.GenerarColorAleatorio();

            // Generación de coordenadas aleatorias para el cuadrado
            Centro = this.GenerarCoordenadasAleatorias();  
        }

        /// <summary>
        /// Calcula el área del cuadrado.
        /// El área se calcula como lado * lado.
        /// </summary>
        /// <returns>El área calculada del cuadrado.</returns>
        public override double CalcularArea()
        {
            return Lado * Lado;  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ObtenerPropiedades()
        {
            return $"Lado: {Lado}, Color: {Color}, Coordenadas: ({Centro.x}, {Centro.y})";
        }
    }
}

