using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPi.Context;
using WebAPi.Models;
using WebAPi.DTOs;
using BCrypt.Net;
using Microsoft.AspNetCore.Authorization;

namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        private readonly AppDbContext _context;

        public UsersController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Users
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Users>>> GetUser()
        {
            return await _context.Users.ToListAsync();
        }

        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUser(int id)
        {
            var user = await _context.Users.FindAsync(id);

            if (user == null)
            {
                return NotFound(new { Message = "El usuario no fue encontrado." });
            }

            return user;
        }

        // PUT: api/Users/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, UserDTO userDTO)
        {
            // Buscar al usuario en la base de datos
            var user = await _context.Users.Include(u => u.Role).FirstOrDefaultAsync(u => u.Id == id);
            if (user == null)
            {
                return NotFound(new { Message = "El usuario no fue encontrado." });
            }

            // Actualizar solo los campos proporcionados
            if (!string.IsNullOrEmpty(userDTO.Username))
            {
                user.Username = userDTO.Username;
            }

            if (!string.IsNullOrEmpty(userDTO.Password))
            {
                user.PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password);
            }

            try
            {
                // Guardar cambios en la base de datos
                _context.Entry(user).State = EntityState.Modified;
                await _context.SaveChangesAsync();

                return Ok(new { Message = "El usuario se actualizó correctamente.", User = user });
            }
            catch (DbUpdateConcurrencyException)
            {
                return Conflict(new { Message = "Ocurrió un error al intentar actualizar el usuario. Por favor, inténtalo de nuevo." });
            }
        }

        // POST: api/Users
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Users>> PostUser(UserDTO userDTO)
        {

            // Verifica los datos entrantes
            if (string.IsNullOrEmpty(userDTO.Password) || string.IsNullOrEmpty(userDTO.Username))
            {
                return BadRequest(new { Message = "El nombre de usuario y la contraseña son obligatorios." });
            }

            var existingUser = await _context.Users.FirstOrDefaultAsync(u => u.Username == userDTO.Username);
            if (existingUser != null)
            {
                return BadRequest(new { Message = "El nombre de usuario ya está en uso. Por favor, elige otro." });
            }

            // Crear el nuevo usuario
            var user = new Users
            {
                Username = userDTO.Username,
                RoleId = 2,
                PasswordHash = BCrypt.Net.BCrypt.HashPassword(userDTO.Password) // Genera el hash
            };

            _context.Users.Add(user);
            await _context.SaveChangesAsync();

            return Ok(new { Message = "Usuario creado exitosamente.", user});
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var user = await _context.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound(new { Message = "El usuario no fue encontrado." });
            }

            _context.Users.Remove(user);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "El usuario ha sido eliminado correctamente.",
                user

            });
        }

        [HttpGet("profile")]
        public async Task<IActionResult> GetProfile()
        {
            var username = User.Identity?.Name;

            if (string.IsNullOrEmpty(username))
            {
                return Unauthorized("El usuario no está autenticado.");
            }

            var user = await _context.Users
                .Include(u => u.Role)
                .FirstOrDefaultAsync(u => u.Username == username);

            if (user == null)
            {
                return NotFound("Usuario no encontrado.");
            }

            return Ok(new
            {
                user.Id,
                user.Username,
                Role = user.Role?.Name
            });
        }

        private bool UserExists(int id)
        {
            return _context.Users.Any(e => e.Id == id);
        }
    }
}
