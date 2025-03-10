using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticasVictorAPI.Datos;
using PracticasVictorAPI.Entidades;


namespace PracticasVictorAPI.Controllers
{
    [ApiController] // Atributo que indica que es un web API automaticamente revisa validaciones de los datos que manda el usuario
    [Route("api/autores")] // Indica a que URL tiene que ser enviada la petición HTTP para poder llamar al controlador
    public class AutoresController : ControllerBase // Clase para trabajar con web API
    {
        // Para tener acceso al contexto en todas la acciones de la clase
        private readonly ApplicationDbContext context;

        // Constructor para obtener una instancia de ApplicationDbContext
        // Así puedo interactuar con mi BD
        public AutoresController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet] // La acción responde al método GET (Para mandar datos al cliente)
        public async Task<IEnumerable<Autor>> Get() // IEnumerable colección de autores
        {
            // Coloca todos los autores de la BD en una lista para devolvérsela al cliente
            return await context.Autores
                .Include(x => x.Libros)
                .ToListAsync();
        }

        [HttpGet("primero")] // api/autores/primero
        public async Task<Autor> GetPrimerAutor()
        {
            return await context.Autores.FirstAsync(); // Solo cojo el primer autor pero en versión asíncrona
        }

        // Para probar varios parámetros
        [HttpGet("{parametro1}/{parametro2?}")]
        public ActionResult Get(string parametro1, string parametro2 = "valor por defecto")
        {
            // Creo un 200 OK y en cuerpo de la respuesta le mando los dos parámetros    
            return Ok(new { parametro1, parametro2 }); 
        }

        [HttpGet("{id:int}")] // Anexo a la URL el id api/autores/id para indicar que autor quiero ver
        // El ActionResult<Autor> vale para retornar un 404 o cualquier código de estado HTTP o un Autor
        public async Task<ActionResult<Autor>> Get(int id)
        {
            // Busco el primer autor que su iId en la BD (x registro de la BD) sea igual al id (parámetro)
            var autor = await context.Autores
                .Include(x => x.Libros)
                .FirstOrDefaultAsync(x => x.Id == id);

            // Sino encuentra autor devuelve no encontrado
            if (autor is null)
            {
                return NotFound();
            }

            return autor; // Si lo encuentra lo devolvemos
        }

        [HttpPost] // La acción responde al método POST (Para recibir datos del cliente)
        // Async para programación asíncrona (Tengo que devolver Task)
        public async Task<ActionResult> Post(Autor autor)
        {
            // Marca el objeto para que se añada en el futuro al guardar los cambios
            context.Add(autor);

            // Para guardar los cambios de manera asíncrona
            // El await sirve para cuando mando el query de insert para meter los datos en la tabla no se quede esperando la API
            await context.SaveChangesAsync();

            return Ok(); // Para hacer saber que todo se hizo correctamente
        }

        // La acción responde al método PUT (Para actualizar datos)
        [HttpPut("{id:int}")]  // Anexo a la URL el id api/autores/id para indicar de que autor quiero actualizar datos
        public async Task<ActionResult> Put(int id, Autor autor)
        {
            if (id != autor.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            // Marca el objeto para que se actualize en el futuro al guardar los cambios
            context.Update(autor);

            // Guardo los cambios       
            await context.SaveChangesAsync();

            return Ok();
        }

        // La acción responde al método DELETE (Para borrar datos) por el id del autor que quiero eliminar
        [HttpDelete("{id:int}")] 
        public async Task<ActionResult> Delete(int id)
        {
            // Variable que almacena todos los registros eliminados en los que el Id coincide con el que se le pasa
            var registrosBorrados = await context.Autores.Where(x => x.Id == id).ExecuteDeleteAsync();

            // Sino borra nada osea no encuentra ningún registro con el id proporcionado devuelve NotFound()
            if(registrosBorrados == 0)
            {
                return NotFound();
            }

            return Ok(); 
        }
    }
}
