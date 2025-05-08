using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EjerciciosVictorAPI.Datos;
using EjerciciosVictorAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;

namespace EjerciciosVictorAPI.Controllers
{
    /// <summary>
    /// Controlador para la gestión de las cuentas de usuario.
    /// </summary>
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;
        private readonly ApplicationDbContext context;

        /// <summary>
        /// Constructor que inyecta las dependencias necesarias para la gestión de cuentas.
        /// </summary>
        public CuentasController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration,
            ApplicationDbContext context)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
            this.context = context;
        }

        /// <summary>
        /// Crea un nuevo usuario en la base de datos y le asigna el rol "usuario".
        /// </summary>
        /// <param name="model">Datos del usuario (correo y contraseña).</param>
        /// <returns>Token JWT si el usuario fue creado correctamente.</returns>
        [HttpPost("crear")]
        public async Task<ActionResult<UserTokenDTO>> CreateUser([FromBody] UserInfoDTO model)
        {
            var usuario = new IdentityUser { UserName = model.Email, Email = model.Email };
            var resultado = await userManager.CreateAsync(usuario, model.Password);

            if (resultado.Succeeded)
            {
                // Asigno el rol "usuario" al nuevo usuario
                var rolResult = await userManager.AddToRoleAsync(usuario, "usuario");
                return await BuildToken(model);
            }
            else
            {
                // Devuelvo el primer error encontrado
                return BadRequest(resultado.Errors.First());
            }
        }

        /// <summary>
        /// Inicia sesión de un usuario existente validando sus credenciales.
        /// </summary>
        /// <param name="model">Datos del usuario (correo y contraseña).</param>
        /// <returns>Token JWT si las credenciales son válidas.</returns>
        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserInfoDTO model)
        {
            // Se busca al usuario por el email
            var usuario = await userManager.FindByEmailAsync(model.Email);

            if (usuario == null)
            {
                // Si no se encuentra
                return Unauthorized("El correo electrónico introducido es incorrecto o no existe");
            }

            // Compruebo la contraseña
            var resultado = await signInManager
                .CheckPasswordSignInAsync(usuario, model.Password, lockoutOnFailure: false);

            if (!resultado.Succeeded)
            {
                // Contraseña incorrecta
                return Unauthorized("La contraseña introducida es incorrecta");
            }

            // Si todo va bien, se genera el token
            return await BuildToken(model);
        }

        /// <summary>
        /// Genera un token JWT con los claims del usuario, incluyendo roles y permisos.
        /// </summary>
        /// <param name="userInfo">Información del usuario.</param>
        /// <returns>Un objeto que contiene el token JWT y su expiración.</returns>
        private async Task<UserTokenDTO> BuildToken(UserInfoDTO userInfo)
        {
            // Creo los claims básicos
            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email)
            };

            // Obtengo el usuario desde la base de datos
            var usuario = await userManager.FindByEmailAsync(userInfo.Email);

            // Obtengo los roles del usuario
            var roles = await userManager.GetRolesAsync(usuario!);

            // Añado los roles como claims
            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            // Obtengo los IDs de los roles del usuario
            var userRoleIds = await context.UserRoles
                .Where(ur => ur.UserId == usuario.Id)
                .Select(ur => ur.RoleId)
                .ToListAsync();

            // Por cada rol, obtengo sus permisos asociados
            var permisos = await (
                from rp in context.RolPermisos
                join p in context.Permisos on rp.PermisoId equals p.Id
                where userRoleIds.Contains(rp.RolId)
                select p.Nombre
            ).ToListAsync();

            // Añado los permisos como claims
            foreach (var permisoNombre in permisos.Distinct())
            {
                claims.Add(new Claim("permiso", permisoNombre));
            }

            // Generola clave simétrica y las credenciales de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);
            var expiration = DateTime.UtcNow.AddYears(1);

            // Creo el token JWT
            var jwt = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
            );

            // Devuelvo el token y la fecha de expiración
            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(jwt),
                Expiration = expiration
            };
        }
    }
}
