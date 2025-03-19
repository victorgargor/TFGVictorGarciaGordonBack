namespace EjerciciosVictorAPI
{
    /// <summary>
    /// Clase que contiene las constantes utilizadas en la aplicación.
    /// Estas constantes se utilizan para la validación de entradas y para definir valores por defecto.
    /// </summary>
    public static class Constantes
    {
        /// <summary>
        /// Número mínimo de decimales permitidos en las operaciones.
        /// </summary>
        public const int MIN_DECIMALES = 0;

        /// <summary>
        /// Número máximo de decimales permitidos en las operaciones.
        /// </summary>
        public const int MAX_DECIMALES = 8;

        /// <summary>
        /// Mensaje de error que se muestra cuando se utiliza una coma (,) en lugar de un punto (.) como separador decimal.
        /// </summary>
        public const string ERROR_SEPARADOR_DECIMAL = "El formato de los números no es válido, se debe usar punto (.) como separador decimal.";

        /// <summary>
        /// Mensaje de error que se muestra cuando los números de entrada no son válidos o no se pueden convertir a double.
        /// </summary>
        public const string ERROR_NUMEROS_INVALIDOS = "Los números no son válidos o no se pueden convertir a double.";

        /// <summary>
        /// Mensaje de error que se muestra cuando se intenta dividir o realizar la operación módulo por 0.
        /// </summary>
        public const string ERROR_DIVISION_CERO = "No se puede dividir por 0.";

        /// <summary>
        /// Número de decimales que se utiliza por defecto en las operaciones si no se especifica otro valor.
        /// </summary>
        public const int DECIMALES_POR_DEFECTO = 2;
    }
}
