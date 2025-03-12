using System;
using System.Globalization;

namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que representa un ítem separado a partir de una cadena de entrada con un formato específico.
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
        /// Constructor que recibe una cadena de entrada con el formato:
        /// "NombreItem$$##PrecioItem$$##CantidadItem".
        /// </summary>
        /// <param name="rawInput">Cadena de entrada con el formato esperado.</param>
        /// <exception cref="FormatException">Se lanza si la cadena de entrada no tiene el formato correcto.</exception>
        public ItemSeparator(string rawInput)
        {
            // Divido la cadena usando el delimitador "$$##".
            string[] partes = rawInput.Split("$$##");

            // Compruebo que la cantidad de partes sea exactamente 3 (Nombre, Precio, Cantidad).
            if (partes.Length != 3)
            {
                throw new FormatException("Formato inválido, debe ser 'NombreItem$$##PrecioItem$$##CantidadItem'.");
            }

            // Asigno el nombre del ítem, eliminando espacios en blanco.
            Name = partes[0].Trim();

            // Si el no hay nombre.
            if (string.IsNullOrWhiteSpace(Name))
            {
                throw new FormatException("El nombre del ítem es obligatorio.");
            }

            // Intenta convertir el precio a un número de tipo double.
            if (!double.TryParse(partes[1].Trim(), NumberStyles.Any, CultureInfo.InvariantCulture, out double price))
            {
                throw new FormatException("El precio debe ser un número válido.");
            }

            // Intenta convertir la cantidad a un número entero.
            if (!int.TryParse(partes[2].Trim(), out int quantity))
            {
                throw new FormatException("La cantidad debe ser un número entero válido.");
            }

            // Asigna los valores convertidos a las propiedades correspondientes.
            Price = price;
            Quantity = quantity;
        }
    }
}
