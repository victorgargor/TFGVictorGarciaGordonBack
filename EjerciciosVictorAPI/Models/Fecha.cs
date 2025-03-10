namespace EjerciciosVictorAPI.Models
{
    public class Fecha
    {
        public int DiferenciarFechas(DateTime fecha1, DateTime fecha2)
        {
            return Math.Abs((fecha2 - fecha1).Days);
        }

        public DateTime SacarPrimerDiaAnyo(DateTime fecha)
        {
            return new DateTime(fecha.Year, 1, 1);
        }

        public DateTime SacarUltimoDiaAnyo(DateTime fecha)
        {
            return new DateTime(fecha.Year, 12, 31);
        }

        public int CalcularDiasAnyo(DateTime fecha)
        {
            DateTime primerDia = SacarPrimerDiaAnyo(fecha);
            DateTime ultimoDia = SacarUltimoDiaAnyo(fecha);
            return (ultimoDia - primerDia).Days + 1;
        }

        public int CalcularSemana(DateTime fecha)
        {
            return (fecha.Day - 1) / 7 + 1;
        }
    }
}
