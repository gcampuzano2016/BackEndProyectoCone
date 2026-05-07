using Conexion.AccesoDatos.Repository.Usuario;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using WebAppConexion.Models;

namespace WebAppConexion.Controllers
{
    [AllowAnonymous]
    [Route("api/[controller]")]
    [ApiController]
    public class LoginController : Controller
    {
        private readonly LoginRepository _repository;
        private readonly IConfiguration _config;
        public LoginController(LoginRepository repository, IConfiguration config)
        {
            this._repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _config = config;
        }
        [HttpPost("[action]")]
        public async Task<IActionResult> Login([FromBody] LoginViewModel model)
        {
            if (model == null || string.IsNullOrWhiteSpace(model.Email) || string.IsNullOrWhiteSpace(model.Password))
                return Unauthorized();

            var response = await _repository.GetByMostrarLogin(model.Email);
            if (response == null || response.IdEmpleado == 0)
                return Unauthorized();

            if (response.ClaveTemporal == "")
            {
                if (!VerificarPasswordHash(model.Password, response.password_hash, response.password_salt))
                    return Unauthorized();
            }

            var claims = new List<Claim>
            {
                new Claim(ClaimTypes.NameIdentifier, response.IdEmpleado.ToString()),
                new Claim(ClaimTypes.Email, model.Email),
                new Claim("IdEmpleado", response.IdEmpleado.ToString()),
                new Claim("email", model.Email),
                new Claim("NombresApellidos", response.NombresApellidos),
                new Claim("Perfil", response.Rucedula),
                new Claim("Imagen", response.RutaImagen),
                new Claim("ClaveTemporal", response.ClaveTemporal),
                new Claim("Valor1", response.Valor1.ToString()),
                new Claim("Valor2", response.Valor2),
                new Claim("Cedula", response.Regimen.ToString())
            };

            return Ok(new { token = GenerarToken(claims) });
        }

        #region MetodosAdicionales

        #region CrearPasswordHash
        private void CrearPasswordHash(string password, out byte[] passwordHash, out byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512())
            {
                passwordSalt = hmac.Key;
                passwordHash = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
            }

        }
        #endregion

        #region VerificarPasswordHash
        private bool VerificarPasswordHash(string password, byte[] passwordHashAlmacenado, byte[] passwordSalt)
        {
            using (var hmac = new System.Security.Cryptography.HMACSHA512(passwordSalt))
            {
                var passwordHashNuevo = hmac.ComputeHash(System.Text.Encoding.UTF8.GetBytes(password));
                return new ReadOnlySpan<byte>(passwordHashAlmacenado).SequenceEqual(new ReadOnlySpan<byte>(passwordHashNuevo));
            }
        }
        #endregion

        #region GenerarToken
        private string GenerarToken(List<Claim> claims)
        {
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_config["Jwt:Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            var token = new JwtSecurityToken(
              _config["Jwt:Issuer"],
              _config["Jwt:Issuer"],
              expires: DateTime.Now.AddMinutes(30),
              signingCredentials: creds,
              claims: claims);

            return new JwtSecurityTokenHandler().WriteToken(token);
        }
        #endregion

        #endregion
    }
}
