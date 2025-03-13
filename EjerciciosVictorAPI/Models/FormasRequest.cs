namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Representa la solicitud para generar formas, indicando cuántos círculos, cuadrados y triángulos se desean crear.
    /// </summary>
    public class FormasRequest
    {
        /// <summary>
        /// Cantidad de círculos para generar.
        /// </summary>
        public int Circulos { get; set; }

        /// <summary>
        /// Cantidad de cuadrados para generar.
        /// </summary>
        public int Cuadrados { get; set; }

        /// <summary>
        /// Cantidad de triángulos para generar.
        /// </summary>
        public int Triangulos { get; set; }
    }
}
