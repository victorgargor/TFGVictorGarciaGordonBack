using System;
using System.ComponentModel.DataAnnotations;

namespace EjerciciosVictorAPI.Models
{
    public class FechasRequest
    {
        [Required]
        [RegularExpression(@"^\d{4}/\d{2}/\d{2}$", ErrorMessage = "El formato de la fecha debe ser yyyy/MM/dd.")]
        public string Fecha1 { get; set; }

        [Required]
        [RegularExpression(@"^\d{4}/\d{2}/\d{2}$", ErrorMessage = "El formato de la fecha debe ser yyyy/MM/dd.")]
        public string Fecha2 { get; set; }

        // Método para convertir las fechas desde string a DateTime
        public (DateTime, DateTime) ConvertirFechas()
        {
            DateTime fecha1 = DateTime.ParseExact(Fecha1, "yyyy/MM/dd", null);
            DateTime fecha2 = DateTime.ParseExact(Fecha2, "yyyy/MM/dd", null);
            return (fecha1, fecha2);
        }
    }
}
