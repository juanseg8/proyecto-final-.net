using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using System.Threading.Tasks;
using WebAPi.Context;
using WebAPi.Models;
using WebAPi.Services;
using WebAPi.DTOs;

namespace WebAPi.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthController : ControllerBase
    {
        private readonly AppDbContext _context;
        private readonly IConfiguration _configuration;
        private readonly AuthService _authService; // Servicio de autenticación para generar el JWT

        public AuthController(AppDbContext context, IConfiguration configuration, AuthService authService)
        {
            _context = context;
            _configuration = configuration;
            _authService = authService; // Inyecta el servicio de autenticación
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login(LoginDTO loginDTO)
        {
            var user = await _context.Users
                .Include(u => u.Role) // Incluye el rol para el claim
                .FirstOrDefaultAsync(u => u.Username == loginDTO.Username);

            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDTO.Password, user.PasswordHash))
            {
                return Unauthorized(new { Message = "Usuario o contraseña incorrectos." });
            }

            // Generar el token JWT
            var token = _authService.GenerateJwtToken(user);

            return Ok(new
            {
                Message = "Inicio de sesión exitoso.",
                Token = token
            });
        }
    }
}
