namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Interfaz que define operaciones matemáticas básicas (con decimal)
    /// </summary>
    public interface ICalculadoraDecimal
    {
        public decimal CalcularSuma(decimal num1, decimal num2, int numDec);

        public decimal CalcularResta(decimal num1, decimal num2, int numDec);

        public decimal CalcularDivision(decimal num1, decimal num2, int numDec);

        public decimal CalcularMultiplicacion(decimal num1, decimal num2, int numDec);

        public decimal CalcularModulo(decimal num1, decimal num2, int numDec);

        public int CompararNumeros(decimal num1, decimal num2);
    }
}
