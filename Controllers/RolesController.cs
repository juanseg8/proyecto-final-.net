using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPi.Context;
using WebAPi.DTOs;
using WebAPi.Models;

namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RolesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public RolesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Roles
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Role>>> GetRole()
        {
            return await _context.Roles.ToListAsync();
        }

        // GET: api/Roles/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Role>> GetRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);

            if (role == null)
            {
                return NotFound(new { Message = "El rol no fue encontrado." });
            }

            return role;
        }

        // PUT: api/Roles/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutRole(int id, RoleDTO roleDTO)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound(new { Message = "El rol no fue encontrada." }); // Retorna 404 con mensaje
            }

            role.Name = roleDTO.Name;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!RoleExists(id))
                {
                    return NotFound(new { Message = "El rol no fue encontrado al momento de actualizar." });

                }
                else
                {
                    throw;
                }
            }

            return Ok(new
            {
                Message = "El rol ha sido actualizado correctamente.",
                role

            });
        }

        // POST: api/Roles
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Role>> PostRole(RoleDTO roleDTO)
        {

            var role = new Role
            {
                Name = roleDTO.Name
            };

            _context.Roles.Add(role);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "El rol ha sido creado correctamente.",
                role

            });
        }

        // DELETE: api/Roles/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteRole(int id)
        {
            var role = await _context.Roles.FindAsync(id);
            if (role == null)
            {
                return NotFound();
            }

            _context.Roles.Remove(role);
            await _context.SaveChangesAsync();


            return Ok(new
            {
                Message = "El rol ha sido eliminado correctamente.",
                role

            });
        }

        private bool RoleExists(int id)
        {
            return _context.Roles.Any(e => e.Id == id);
        }
    }
}
