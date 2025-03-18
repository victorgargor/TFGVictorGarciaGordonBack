namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// 
    /// </summary>
    public interface ICalculadora
    {
        public double CalcularSuma(double num1, double num2, int numDec);

        public double CalcularResta(double num1, double num2, int numDec);

        public double CalcularDivision(double num1, double num2, int numDec);

        public double CalcularMultiplicacion(double num1, double num2, int numDec);

        public double CalcularModulo(double num1, double num2, int numDec);

        public int CompararNumeros(double num1, double num2);
    }
}
