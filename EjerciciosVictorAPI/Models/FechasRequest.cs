using System;
using System.ComponentModel.DataAnnotations;

namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que representa la solicitud de dos fechas para su procesarlas.
    /// </summary>
    public class FechasRequest
    {
        /// <summary>
        /// Primera fecha proporcionada por el usuario en formato de cadena.
        /// Se valida que no esté vacía y que siga el formato yyyy/MM/dd.
        /// </summary>
        [Required(ErrorMessage = "El campo fecha1 es requerido")]
        [RegularExpression(@"^\d{4}/\d{2}/\d{2}$", ErrorMessage = "El formato de la fecha debe ser yyyy/MM/dd.")]
        public string Fecha1 { get; set; }

        /// <summary>
        /// Segunda fecha proporcionada por el usuario en formato de cadena.
        /// Se valida que no esté vacía y que siga el formato yyyy/MM/dd.
        /// </summary>
        [Required(ErrorMessage = "El campo fecha2 es requerido")]
        [RegularExpression(@"^\d{4}/\d{2}/\d{2}$", ErrorMessage = "El formato de la fecha debe ser yyyy/MM/dd.")]
        public string Fecha2 { get; set; }

        /// <summary>
        /// Convierte las fechas proporcionadas en formato de cadena a objetos DateTime.
        /// </summary>
        /// <returns>Una tupla con las dos fechas convertidas a DateTime.</returns>
        public (DateTime, DateTime) ConvertirFechas()
        {
            // Convierte la cadena Fecha1 al tipo DateTime usando el formato especificado
            DateTime fecha1 = DateTime.ParseExact(Fecha1, "yyyy/MM/dd", null);
            // Convierte la cadena Fecha2 al tipo DateTime usando el formato especificado
            DateTime fecha2 = DateTime.ParseExact(Fecha2, "yyyy/MM/dd", null);
            // Devuelve una tupla con ambas fechas convertidas
            return (fecha1, fecha2);
        }
    }
}

