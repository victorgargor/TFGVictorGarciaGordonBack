using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using PracticasVictorAPI.Datos;
using PracticasVictorAPI.Entidades;


namespace PracticasVictorAPI.Controllers
{
    [ApiController]
    [Route("api/libros")]
    public class LibrosController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public LibrosController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [HttpGet] 
        public async Task<IEnumerable<Libro>> Get() 
        {
            return await context.Libros
                .Include(x => x.Autor)
                .ToListAsync();
        }

        [HttpGet("{id:int}")] 
        public async Task<ActionResult<Libro>> Get(int id)
        { 
            var libro = await context.Libros
                .Include(x => x.Autor) // Para incluir el Autor del libro
                .FirstOrDefaultAsync(x => x.Id == id);
 
            if (libro is null)
            {
                return NotFound();
            }

            return libro; 
        }

        [HttpPost]
        public async Task<ActionResult> Post(Libro libro)
        {
            // Para crear un libro comprobamos si existe el id del autor que queremos poner a nuestro libro
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            // Si no existe el autor se manda BadRequest ya que es problema del usuario
            if (!existeAutor)
            {
                ModelState.AddModelError(nameof(libro.AutorId), $"El autor de id {libro.AutorId} no existe");
                // return BadRequest($"El autor de id {libro.AutorId} no existe");
                return ValidationProblem();
            }

            context.Add(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpPut("{id:int}")] 
        public async Task<ActionResult> Put(int id, Libro libro)
        {
            if (id != libro.Id)
            {
                return BadRequest("Los ids deben de coincidir");
            }

            // Para crear un libro comprobamos si existe el id del autor que queremos poner a nuestro libro
            var existeAutor = await context.Autores.AnyAsync(x => x.Id == libro.AutorId);

            // Si no existe el autor se manda BadRequest ya que es problema del usuario
            if (!existeAutor)
            {
                return BadRequest($"El autor de id {libro.AutorId} no existe");
            }

            context.Update(libro);
            await context.SaveChangesAsync();
            return Ok();
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult> Delete(int id)
        {
            var registrosBorrados = await context.Libros.Where(x => x.Id == id).ExecuteDeleteAsync();

            if (registrosBorrados == 0)
            {
                return NotFound();
            }

            return Ok();
        }
    }
}
