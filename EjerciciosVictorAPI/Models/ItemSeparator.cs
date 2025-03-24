using System;
using System.Globalization;

namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que representa un ítem a partir de una cadena de entrada con el formato "NombreItem$$##PrecioItem$$##CantidadItem".
    /// </summary>
    public class ItemSeparator
    {
        /// <summary>
        /// Nombre del ítem.
        /// </summary>
        public string Name { get; }

        /// <summary>
        /// Precio del ítem.
        /// </summary>
        public double Price { get; }

        /// <summary>
        /// Cantidad del ítem.
        /// </summary>
        public int Quantity { get; }

        /// <summary>
        /// Constructor que procesa la cadena de entrada.
        /// </summary>
        /// <param name="rawInput">Cadena en el formato "NombreItem$$##PrecioItem$$##CantidadItem".</param>
        /// <exception cref="FormatException">Lanza una excepción si el formato es incorrecto.</exception>
        public ItemSeparator(string rawInput)
        {
            // Divido la cadena usando el separador definido en Constantes.
            string[] partes = rawInput.Split(Constantes.SEPARADOR);

            // Verifico que existan exactamente 3 partes.
            if (partes.Length != 3)
            {
                throw new FormatException(Constantes.ERROR_SEPARADOR);
            }

            // Extraigo y valido el nombre.
            Name = partes[0].Trim();
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new FormatException(Constantes.ERROR_NOMBRE_OBLIGATORIO);
            }

            // Extraigo y valido el precio.
            string precioStr = partes[1].Trim();
            if (precioStr.Contains(','))
            {
                throw new FormatException(Constantes.ERROR_SEPARADOR_DECIMAL);
            }
            // Si el precio es negativo o contiene letras, símbolos etc.
            if (!double.TryParse(precioStr, NumberStyles.Any, CultureInfo.InvariantCulture, out double price) || price < 0)
            {
                throw new FormatException(Constantes.ERROR_PRECIO_NO_VALIDO);
            }

            // Extraigo y valido la cantidad.
            string cantidadStr = partes[2].Trim();
            if (cantidadStr.Length >= Constantes.MAX_DIGITOS_CANTIDAD)
            {
                throw new FormatException(Constantes.ERROR_CANTIDAD_DEMASIADO_LARGA);
            }
            if (!int.TryParse(cantidadStr, out int quantity))
            {
                throw new FormatException(Constantes.ERROR_CANTIDAD_NO_VALIDA);
            }
            // Si es negativa.
            if (quantity < 0)
            {
                throw new FormatException(Constantes.ERROR_CANTIDAD_NEGATIVA);
            }

            Price = price;
            Quantity = quantity;
        }
    }
}