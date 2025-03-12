using System.ComponentModel.DataAnnotations; 

namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Representa la solicitud que contiene la cadena de entrada que procesará la API.
    /// Esta clase se utiliza para recibir los datos del cliente en formato JSON.
    /// </summary>
    public class ItemSeparatorRequest
    {
        /// <summary>
        /// La cadena de entrada que contiene los datos del ítem en el formato esperado:
        /// "NombreItem$$##PrecioItem$$##CantidadItem".
        /// Esta propiedad puede estar vacía si el controlador proporciona un valor por defecto.
        /// </summary>
        public string CadenaEntrada { get; set; }
    }
}