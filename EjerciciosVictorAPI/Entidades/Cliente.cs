using System.ComponentModel.DataAnnotations;
using EjerciciosVictorAPI.Models;

namespace EjerciciosVictorAPI.Entidades
{
    /// <summary>
    /// Representa a un cliente dentro del sistema de gestión de recibos.
    /// </summary>
    public class Cliente
    {
        /// <summary>
        /// DNI del cliente. Debe ser único y actúa como PK (primary key).
        /// </summary>
        [Key]
        [Required(ErrorMessage = "El DNI es requerido")]
        [StringLength(9, MinimumLength = 9, ErrorMessage = "El DNI debe tener 9 caracteres")]
        public required string DNI { get; set; }

        /// <summary>
        /// Nombre completo del cliente.
        /// </summary>
        [Required(ErrorMessage = "Se requiere nombre completo")]
        public required string Nombre { get; set; }

        /// <summary>
        /// Tipo de cliente: REGISTRADO o SOCIO.
        /// </summary>
        [Required(ErrorMessage = "Se requiere especificar el tipo de cliente")]
        public required string Tipo { get; set; }

        /// <summary>
        /// Cuota máxima permitida para emisión de recibos solo para clientes de tipo REGISTRADO.
        /// </summary>
        public decimal? CuotaMaxima { get; set; }

        /// <summary>
        /// Fecha de alta del cliente.
        /// </summary>
        [Required(ErrorMessage = "Se requiere la fecha de alta")]
        public DateTime FechaAlta { get; set; }

        /// <summary>
        /// Propiedad de Navegación (listado de recibos)
        /// </summary>
        public List<Recibo> Recibos { get; set; } = new List<Recibo>();
    }
}