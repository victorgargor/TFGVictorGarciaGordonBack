using System.Globalization;

namespace EjerciciosVictorAPI.Models
{
    /// <summary>
    /// Clase que proporciona métodos para trabajar con fechas.
    /// </summary>
    public class Fecha
    {
        /// <summary>
        /// Calcula la diferencia en días entre dos fechas.
        /// </summary>
        /// <param name="fecha1">La primera fecha a comparar.</param>
        /// <param name="fecha2">La segunda fecha a comparar.</param>
        /// <returns>El número de días de diferencia entre las dos fechas.</returns>
        public int DiferenciarFechas(DateTime fecha1, DateTime fecha2)
        {
            // Se calcula la diferencia en días entre las dos fechas y se devuelve el valor absoluto
            return Math.Abs((fecha2 - fecha1).Days);
        }

        /// <summary>
        /// Obtiene el primer día del año de la fecha proporcionada.
        /// </summary>
        /// <param name="fecha">La fecha desde la cual se calcula el primer día del año.</param>
        /// <returns>El primer día del año de la fecha proporcionada.</returns>
        public DateTime SacarPrimerDiaAnyo(DateTime fecha)
        {
            // Se devuelve el primer día del año utilizando el año de la fecha dada
            return new DateTime(fecha.Year, 1, 1);
        }

        /// <summary>
        /// Obtiene el último día del año de la fecha proporcionada.
        /// </summary>
        /// <param name="fecha">La fecha desde la cual se calcula el último día del año.</param>
        /// <returns>El último día del año de la fecha proporcionada.</returns>
        public DateTime SacarUltimoDiaAnyo(DateTime fecha)
        {
            // Se devuelve el último día del año utilizando el año de la fecha dada
            return new DateTime(fecha.Year, 12, 31, 23, 59, 59);
        }

        /// <summary>
        /// Calcula el número de días que tiene el año de la fecha proporcionada.
        /// </summary>
        /// <param name="fecha">La fecha desde la cual se calcula el número de días del año.</param>
        /// <returns>El número de días en el año de la fecha proporcionada.</returns>
        public int CalcularDiasAnyo(DateTime fecha)
        {
            // Obtiene el primer y último día del año y calcula la diferencia en días
            DateTime primerDia = SacarPrimerDiaAnyo(fecha);
            DateTime ultimoDia = SacarUltimoDiaAnyo(fecha);
            // Se añade 1 porque ambos días están incluidos
            return (ultimoDia - primerDia).Days + 1; 
        }

        /// <summary>
        /// Calcula la semana del año en la que se encuentra la fecha proporcionada.
        /// </summary>
        /// <param name="fecha">La fecha de la cual se calculará la semana del año.</param>
        /// <returns>El número de la semana en el que se encuentra la fecha.</returns>
        public int CalcularSemana(DateTime fecha)
        {
            // Obtiene la configuración cultural actual
            var cultura = CultureInfo.CurrentCulture;

            // El CalendarWeekRule es para que la primera semana del año sea la que tiene al menos 4 días (ISO 8601)
            // El DayOfWeek.Monday para indicar que la semana empieza el lunes
            return cultura.Calendar.GetWeekOfYear(fecha, CalendarWeekRule.FirstFourDayWeek, DayOfWeek.Monday);
        }

    }
}
