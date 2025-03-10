using Microsoft.EntityFrameworkCore;
using PracticasVictorAPI.Entidades;

namespace PracticasVictorAPI.Datos
{
    public class ApplicationDbContext : DbContext // Clase a partir de la cual configuro las tablas de mi BD
    {
        // Para realizar configuraciones de EntityFrameworkCore fuera de esta clase
        public ApplicationDbContext(DbContextOptions options) : base(options)
        {
        }

        // Para indicar que quiero que se cree una tabla en mi BD a partir de las propiedades de la clase Autor
        public DbSet<Autor> Autores { get; set; }

        // Para indicar que quiero que se cree una tabla en mi BD a partir de las propiedades de la clase Libro
        public DbSet<Libro> Libros { get; set; }
    }
}
