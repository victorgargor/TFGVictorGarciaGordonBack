using System.Text;
using System.Text.Json.Serialization;
using EjerciciosVictorAPI.Datos;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

var builder = WebApplication.CreateBuilder(args);

// Configuraci�n de CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowBlazor", policy =>
    {
        // Permitir solicitudes desde el puerto de Blazor (7231)
        policy.WithOrigins("http://localhost:7231", "https://localhost:7231")
              .AllowAnyHeader()
              .AllowAnyMethod();
    });
});

// �rea de servicios

// Habilitamos los controladores
// Le ponemos las opciones de JSON ya que hay errores con el serializador de JSON
// Lo configuramos para que ignore los ciclos 
builder.Services.AddControllers().AddJsonOptions(opciones =>
opciones.JsonSerializerOptions.ReferenceHandler = ReferenceHandler.IgnoreCycles);

// Configuramos el ApplicationDbContext como un servicio para usarlo en cualquier parte de la aplicaci�n
// Configuro que voy a utilizar PostgreSQL y el nombre del ConnectionStrings es DefaultConnection
builder.Services.AddDbContext<ApplicationDbContext>(opciones =>
    opciones.UseNpgsql("name=DefaultConnection"));

// Configura Identity para la gesti�n de usuarios y roles con Entity Framework
builder.Services.AddIdentity<IdentityUser, IdentityRole>()
    // Guarda usuarios y roles en la base de datos
    .AddEntityFrameworkStores<ApplicationDbContext>()
    // Habilita tokens para login, recuperaci�n, etc.
    .AddDefaultTokenProviders();

// Configura autenticaci�n con JWT Bearer
builder.Services.AddAuthentication(JwtBearerDefaults.AuthenticationScheme)
    .AddJwtBearer(options =>
    {
        options.TokenValidationParameters = new TokenValidationParameters
        {
            // No validoel issuer (emisor del token)
            ValidateIssuer = false,

            // No valido el audience (a qui�n va dirigido el token)
            ValidateAudience = false,

            // Valido que el token no haya expirado
            ValidateLifetime = true,

            // Valido que el token est� firmado con la clave correcta
            ValidateIssuerSigningKey = true,

            // Clave usada para firmar el token, sacada de appsettings.json (clave: jwtkey)
            IssuerSigningKey = new SymmetricSecurityKey(
                Encoding.UTF8.GetBytes(builder.Configuration["jwtkey"])),

            // No permitimos margen de diferencia entre reloj del servidor y cliente
            ClockSkew = TimeSpan.Zero
        };
    });

var app = builder.Build();

// �rea de Middlewares

app.UseExceptionHandler("/Home/Error");  // Agrega esto para manejar excepciones.
// Usar CORS
app.UseCors("AllowBlazor");

app.UseAuthentication();
app.UseAuthorization();

// Para que los controladores den respuesta a las peticiones
app.MapControllers();


app.Run();
