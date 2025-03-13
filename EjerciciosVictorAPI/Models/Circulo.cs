namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Representa un círculo que es una forma que tiene un radio.
    /// Hereda de la clase base Forma y proporciona la implementación para calcular su área.
    /// </summary>
    public class Circulo : Forma
    {
        /// <summary>
        /// Radio del círculo.
        /// El radio define el tamaño del círculo.
        /// </summary>
        public double Radio { get; set; }

        /// <summary>
        /// Constructor para crear un círculo con un radio específico.
        /// Genera un color y unas coordenadas aleatorias para el círculo.
        /// </summary>
        /// <param name="radio">El tamaño del radio del círculo.</param>
        public Circulo(double radio)
        {
            // Validación de valor positivo para el radio.
            if (radio <= 0)
            {
                throw new ArgumentException("El radio debe ser mayor que 0.");
            }   

            Radio = radio;

            // Generación de color aleatorio para el círculo
            Color = this.GenerarColorAleatorio();

            // Generación de coordenadas aleatorias para el círculo
            Centro = this.GenerarCoordenadasAleatorias();  
        }

        /// <summary>
        /// Calcula el área del círculo.
        /// El área se calcula como π * radio * radio.
        /// </summary>
        /// <returns>El área calculada del círculo.</returns>
        public override double CalcularArea()
        {
            return Math.PI * Radio * Radio;  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        public override string ObtenerPropiedades()
        {
            return $"Radio: {Radio}, Color: {Color}, Coordenadas: ({Centro.x}, {Centro.y})";
        }
    }
}
