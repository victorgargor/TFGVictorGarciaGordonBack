namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Representa un triángulo definido por una base y una altura,
    /// con un vértice superior ubicado en una posición horizontal aleatoria.
    /// Con esto se pueden calcular de forma única los tres ángulos, que sumarán 180°.
    /// </summary>
    public class Triangulo : Forma
    {
        /// <summary>
        /// Base del triángulo.
        /// </summary>
        public double Base { get; set; }

        /// <summary>
        /// Altura del triángulo.
        /// </summary>
        public double Altura { get; set; }

        /// <summary>
        /// Desplazamiento horizontal del vértice superior respecto al origen.
        /// Se genera aleatoriamente para definir la posición del vértice.
        /// </summary>
        public double Desplazamiento { get; private set; }

        /// <summary>
        /// Ángulo en el vértice A (en grados), correspondiente al punto (0,0).
        /// </summary>
        public double AnguloA { get; private set; }

        /// <summary>
        /// Ángulo en el vértice B (en grados), correspondiente al punto (Base,0).
        /// </summary>
        public double AnguloB { get; private set; }

        /// <summary>
        /// Ángulo en el vértice C (en grados), correspondiente al vértice superior.
        /// </summary>
        public double AnguloC { get; private set; }

        /// <summary>
        /// Constructor para crear un triángulo a partir de una base y una altura.
        /// Se genera un desplazamiento horizontal aleatorio para el vértice superior,
        /// y se calculan los ángulos usando la ley de cosenos.
        /// </summary>
        /// <param name="baseTriangulo">La longitud de la base del triángulo.</param>
        /// <param name="altura">La altura del triángulo.</param>
        public Triangulo(double baseTriangulo, double altura)
        {
            if (baseTriangulo <= 0 || altura <= 0)
            {
                throw new ArgumentException("La base y la altura del triángulo deben ser mayores que 0.");
            }

            Base = baseTriangulo;
            Altura = altura;

            // Para evitar triángulos degenerados, el desplazamiento (x) se genera
            // entre un pequeño valor mínimo y (Base - ese valor mínimo).
            // 10% del valor de la base como margen
            double margen = Base * 0.1; 
            var random = new Random();
            // Generamos un valor entre 'margen' y 'Base - margen'
            Desplazamiento = random.NextDouble() * (Base - 2 * margen) + margen;

            // Definición de los puntos:
            // A = (0, 0)
            // B = (Base, 0)
            // C = (Desplazamiento, Altura)
            // Calcular las longitudes de los lados:
            double ladoAC = Math.Sqrt(Math.Pow(Desplazamiento, 2) + Math.Pow(Altura, 2));     
            double ladoBC = Math.Sqrt(Math.Pow(Base - Desplazamiento, 2) + Math.Pow(Altura, 2));
            double ladoAB = Base; 

            // Calcular el ángulo en A (entre lados AC y AB) usando la ley de cosenos:
            AnguloA = Math.Acos((Math.Pow(ladoAC, 2) + Math.Pow(ladoAB, 2) - Math.Pow(ladoBC, 2)) / (2 * ladoAC * ladoAB)) * (180 / Math.PI);

            // Calcular el ángulo en B (entre lados BC y AB):
            AnguloB = Math.Acos((Math.Pow(ladoBC, 2) + Math.Pow(ladoAB, 2) - Math.Pow(ladoAC, 2)) / (2 * ladoBC * ladoAB)) * (180 / Math.PI);

            // El ángulo en C se obtiene porque la suma de los ángulos es 180°
            AnguloC = 180 - AnguloA - AnguloB;

            // Generar color y coordenadas aleatorias para el triángulo
            Color = this.GenerarColorAleatorio();
            Centro = this.GenerarCoordenadasAleatorias();
        }

        /// <summary>
        /// Calcula el área del triángulo.
        /// </summary>
        /// <returns>El área calculada del triángulo.</returns>
        public override double CalcularArea()
        {
            return 0.5 * Base * Altura;
        }

        /// <summary>
        /// Devuelve las propiedades del triángulo, incluyendo base, altura, desplazamiento y ángulos.
        /// </summary>
        /// <returns>Una cadena con las propiedades del triángulo.</returns>
        public override string ObtenerPropiedades()
        {
            return $"Base: {Base} unidades, Altura: {Altura} unidades, Desplazamiento: {Desplazamiento:F2} unidades, " +
                   $"Ángulos: A = {AnguloA:F2}°, B = {AnguloB:F2}°, C = {AnguloC:F2}°, " +
                   $"Color: {Color}, Coordenadas: ({Centro.x}, {Centro.y})";
        }
    }
}
