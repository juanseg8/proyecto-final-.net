using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;
using WebAPi.Models;

namespace WebAPi.Services
{
    public class AuthService
    {
        private readonly IConfiguration _configuration;

        public AuthService(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        public string GenerateJwtToken(Users user)
        {
            var jwtSettings = _configuration.GetSection("JwtSettings");

            // Crear los claims
            var claims = new[]
            {
                new Claim(JwtRegisteredClaimNames.Sub, user.Username),
                new Claim(JwtRegisteredClaimNames.Jti, Guid.NewGuid().ToString()),
                new Claim(ClaimTypes.Role, user.Role.Name),
                new Claim(ClaimTypes.Name, user.Username),
                new Claim("UserId", user.Id.ToString())
            };

            // Configurar la clave de firma
            var key = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings["Key"]));
            var creds = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Crear el token
            var token = new JwtSecurityToken(
                issuer: jwtSettings["Issuer"],
                audience: jwtSettings["Audience"],
                claims: claims,
                expires: DateTime.Now.AddMinutes(Convert.ToDouble(jwtSettings["ExpiresInMinutes"])),
                signingCredentials: creds
            );

            return new JwtSecurityTokenHandler().WriteToken(token); // Devuelve el token como string
        }
    }
}

