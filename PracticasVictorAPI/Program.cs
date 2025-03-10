using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;
using PracticasVictorAPI.Datos;

var builder = WebApplication.CreateBuilder(args);

// Área de servicios

// Habilitamos los controladores
// Le ponemos las opciones de JSON ya que hay errores con el serializador de JSON
// Lo configuramos para que ignore los ciclos 
builder.Services.AddControllers().AddJsonOptions(opciones =>
opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configuramos el ApplicationDbContext como un servicio para usarlo en cualquier parte de la aplicación
// Configuro que voy a utilizar PostgreSQL y el nombre del ConnectionStrings es DefaultConnection
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseNpgsql("name=DefaultConnection")); 

var app = builder.Build();

// Área de Middlewares

// Para que los controladores den respuesta a las peticiones
app.MapControllers(); 

app.Run();
