using System.ComponentModel.DataAnnotations;
using PracticasVictorAPI.Validaciones;

namespace PracticasVictorAPI.Entidades
{
    public class Autor
    {
        public int Id { get; set; }
        [Required(ErrorMessage = "El campo {0} es requerido")] // Para que sino hay nombre ASP.NET Core rechaze la petición y se puede personalizar el mensaje de error
        [StringLength(10, ErrorMessage = "El campo {0} debe tener {1} carácteres o menos")]
        [PrimeraLetraMayuscula]
        public required string Nombre { get; set; } // Required de C# para que todo autor tenga que tener un nombre
        public List<Libro> Libros { get; set; } = new List<Libro>(); // Propiedad de Navegación (Listado de libros)
 
        // SE PUEDE UTILIZAR TAMBIÉN VALIDACIÓN POR MODELO (Interfaz IValidatableObject)

        //[Range(18, 120)]
        //public int Edad { get; set; }

        //[CreditCard]
        //public string? TarjetaDeCredito { get; set; }

        //[Url]
        //public string? URL { get; set; }
        
    }
}
