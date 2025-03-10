using System;
using System.Numerics;

namespace EjerciciosVictorAPI.Models
{
    public class ItemSeparator
    { 
        private string name;
        private double price;
        private int quantity;

        public ItemSeparator(string rawInput)
        {
            // Intentamos dividir la cadena
            string[] partes = rawInput.Split("$$##");

            // Verificamos si la cantidad de partes es la esperada
            if (partes.Length == 3)
            {
                this.name = partes[0].Trim();
                this.price = Double.Parse(partes[1].Trim());
                this.quantity = int.Parse(partes[2].Trim());
            }
            else
            {
                // Si no tiene el formato correcto, lanzamos una excepción o usamos valores predeterminados
                throw new ArgumentException("La cadena no tiene el formato esperado. Se esperaba un formato 'NombreItem$$##PrecioItem$$##CantidadItem'.");
            }
        }

        public string GetName()
        {
            return name;
        }

        public double GetPrice()
        {
            return price;
        }

        public int GetQuantity()
        {
            return quantity;
        }
    }
}
