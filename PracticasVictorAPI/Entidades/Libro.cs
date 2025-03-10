using System.ComponentModel.DataAnnotations;

namespace PracticasVictorAPI.Entidades
{
    public class Libro
    {
        public int Id { get; set; }
        [Required]
        public required string Titulo { get; set; }
        public int AutorId { get; set; } // Propiedad que va a hacer de FK
        public Autor? Autor { get; set; } // Propiedad de navegación
    }
}
