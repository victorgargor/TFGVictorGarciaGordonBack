using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using EjerciciosVictorAPI.DTOs;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;

namespace EjerciciosVictorAPI.Controllers
{
    [ApiController]
    [Route("api/cuentas")]
    public class CuentasController : ControllerBase
    {
        private readonly UserManager<IdentityUser> userManager;
        private readonly SignInManager<IdentityUser> signInManager;
        private readonly IConfiguration configuration;

        public CuentasController(UserManager<IdentityUser> userManager,
            SignInManager<IdentityUser> signInManager,
            IConfiguration configuration)
        {
            this.userManager = userManager;
            this.signInManager = signInManager;
            this.configuration = configuration;
        }

        [HttpPost("crear")]
        public async Task<ActionResult<UserTokenDTO>> CreateUser([FromBody] UserInfoDTO model)
        {
            var usuario = new IdentityUser { UserName = model.Email, Email = model.Email };
            var resultado = await userManager.CreateAsync(usuario, model.Password);

            if (resultado.Succeeded)
            {
                var rolResult = await userManager.AddToRoleAsync(usuario, "usuario");
                return await BuildToken(model);
            }
            else
            {
                return BadRequest(resultado.Errors.First());
            }
        }

        [HttpPost("login")]
        public async Task<ActionResult<UserTokenDTO>> Login([FromBody] UserInfoDTO model)
        {
            // 1. Buscamos el usuario por email
            var usuario = await userManager.FindByEmailAsync(model.Email);

            if (usuario == null)
            {
                return Unauthorized("El correo electrónico introducido es incorrecto o no existe");
  
            }

            var resultado = await signInManager
                .CheckPasswordSignInAsync(usuario, model.Password, lockoutOnFailure: false);

            if (!resultado.Succeeded)
            {
                return Unauthorized("La contraseña introducida es incorrecta");
    
            }

            // 4. Si todo va bien, generamos el token
            return await BuildToken(model);
        }


        private async Task<UserTokenDTO> BuildToken(UserInfoDTO userInfo)
        {
            var claims = new List<Claim>()
            {
                new Claim(ClaimTypes.Name, userInfo.Email),
                new Claim(ClaimTypes.Email, userInfo.Email)
            };

            var usuario = await userManager.FindByEmailAsync(userInfo.Email);
            var roles = await userManager.GetRolesAsync(usuario!);

            foreach (var rol in roles)
            {
                claims.Add(new Claim(ClaimTypes.Role, rol));
            }

            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(configuration["jwtkey"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var expiration = DateTime.UtcNow.AddYears(1);

            var token = new JwtSecurityToken(
                issuer: null,
                audience: null,
                claims: claims,
                expires: expiration,
                signingCredentials: creds
                );

            return new UserTokenDTO
            {
                Token = new JwtSecurityTokenHandler().WriteToken(token),
                Expiration = expiration
            };
        }

    }
}
