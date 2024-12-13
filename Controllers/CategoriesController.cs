using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
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
    public class CategoriesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CategoriesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Categories
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Category>>> GetCategories()
        {
            return await _context.Categories.ToListAsync();
        }

        // GET: api/Categories/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Category>> GetCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);

            if (category == null)
            {
                return NotFound(new { Message = "La categoría no fue encontrada." }); // Retorna 404 con mensaje
            }

            return category;
        }

        // PUT: api/Categories/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> PutCategory(int id, CategoryDTO categoryDTO  )
        {
            // Busca la categoría existente
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "La categoría no fue encontrada." }); // Retorna 404 con mensaje
            }

            // Actualiza las propiedades de la categoría con los datos del DTO
            category.Name = categoryDTO.Name;

            try
            {
                // Guarda los cambios
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!CategoryExists(id))
                {
                    return NotFound(new { Message = "La categoría no fue encontrada al intentar actualizar." });
                }
                else
                {
                    throw;
                }
            }

            // Retorna un mensaje de éxito
            return Ok(new
            {
                Message = "La categoría ha sido actualizada correctamente.",
                UpdatedCategory = new
                {
                    Id = category.Id,
                    Name = category.Name
                }
            });
        }

        // POST: api/Categories
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult<Category>> PostCategory(CategoryDTO categoryDTO)
        {

            // Crea una nueva instancia de Category usando los datos del DTO
            var category = new Category
            {
                Name = categoryDTO.Name
            };

            // Agrega la nueva categoría al contexto
            _context.Categories.Add(category);
            await _context.SaveChangesAsync();

            // Retorna la categoría creada con un código 201 Created
            return CreatedAtAction(nameof(GetCategory), new { id = category.Id }, category);
        }

        // DELETE: api/Categories/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteCategory(int id)
        {
            var category = await _context.Categories.FindAsync(id);
            if (category == null)
            {
                return NotFound(new { Message = "La categoría no fue encontrada." }); // Retorna 404 con mensaje
            }

            _context.Categories.Remove(category);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "La categoría ha sido eliminada correctamente."
         
            });
        }

        private bool CategoryExists(int id)
        {
            return _context.Categories.Any(e => e.Id == id);
        }
    }
}
