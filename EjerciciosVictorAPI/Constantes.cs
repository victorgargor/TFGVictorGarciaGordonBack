namespace EjerciciosVictorAPI
{
    /// <summary>
    /// Clase que contiene las constantes utilizadas en la aplicación.
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
        /// Mensaje de error cuando se utiliza una coma en lugar de un punto como separador decimal.
        /// </summary>
        public const string ERROR_SEPARADOR_DECIMAL = "El formato de los números no es válido, se debe usar punto (.) como separador decimal.";

        /// <summary>
        /// Mensaje de error para números inválidos en la conversión a double.
        /// </summary>
        public const string ERROR_NUMEROS_INVALIDOS = "Los números no son válidos o no se pueden convertir a double.";

        /// <summary>
        /// Mensaje de error para números inválidos en la conversión a decimal.
        /// </summary>
        public const string ERROR_NUMEROS_INVALIDOSDECIMAL = "Los números no son válidos o no se pueden convertir a decimal.";

        /// <summary>
        /// Mensaje de error cuando se intenta dividir o realizar la operación módulo por 0.
        /// </summary>
        public const string ERROR_DIVISION_CERO = "No se puede dividir por 0.";

        /// <summary>
        /// Número de decimales que se utiliza por defecto en las operaciones.
        /// </summary>
        public const int DECIMALES_POR_DEFECTO = 2;

        /// <summary>
        /// Separador utilizado para separar los datos del ítem.
        /// </summary>
        public const string SEPARADOR = "$$##";

        /// <summary>
        /// Mensaje de error para formato inválido de la cadena de entrada.
        /// </summary>
        public const string ERROR_SEPARADOR = $"Formato inválido, debe ser 'NombreItem{SEPARADOR}PrecioItem{SEPARADOR}CantidadItem'.";

        /// <summary>
        /// Número máximo de dígitos permitidos en la cantidad.
        /// </summary>
        public const int MAX_DIGITOS_CANTIDAD = 10;

        /// <summary>
        /// Mensaje de error cuando la cantidad excede la cantidad máxima de dígitos.
        /// </summary>
        public const string ERROR_CANTIDAD_DEMASIADO_LARGA = "El valor de la cantidad es demasiado largo (9 dígitos máximo).";

        /// <summary>
        /// Mensaje de error cuando el valor de la cantidad es negativo.
        /// </summary>
        public const string ERROR_CANTIDAD_NEGATIVA = "La cantidad no puede ser negativa.";

        /// <summary>
        /// Mensaje de error cuando el nombre del ítem está vacío.
        /// </summary>
        public const string ERROR_NOMBRE_OBLIGATORIO = "El nombre del ítem es obligatorio.";

        /// <summary>
        /// Mensaje de error cuando el precio no es un número válido.
        /// </summary>
        public const string ERROR_PRECIO_NO_VALIDO = "El precio debe ser un número positivo.";

        /// <summary>
        /// Mensaje de error cuando la cantidad no es válida.
        /// </summary>
        public const string ERROR_CANTIDAD_NO_VALIDA = "La cantidad debe ser un número entero mayor que 0 y de 9 dígitos máximo.";
    }
}
