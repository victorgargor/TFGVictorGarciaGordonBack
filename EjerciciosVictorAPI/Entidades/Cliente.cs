using System.ComponentModel.DataAnnotations;
using EjerciciosVictorAPI.Models;

namespace EjerciciosVictorAPI.Entidades
{
    public class Cliente
    {
        [Required(ErrorMessage = "El campo {0} es requerido")]
        public string? Dni { get; private set; }
        public required string Nombre { get; set; }
        public TipoCliente Tipo { get; set; }
        public decimal? CuotaMaximaPermitida { get; set; }
        public DateTime FechaAlta { get; private set; }

        
    }
}
