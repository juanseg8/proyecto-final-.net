using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.Json;
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
    public class ProductsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public ProductsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Products
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Product>>> GetProducts()
        {
            return await _context.Products.ToListAsync();
        }

        // GET: api/Products/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Product>> GetProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);

            if (product == null)
            {
                return NotFound(new { Message = "El producto no fue encontrada." }); // Retorna 404 con mensaje
            }

            return product;
        }

        // PUT: api/Products/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        [Authorize(Roles = "admin,user")]
        public async Task<IActionResult> PutProduct(int id, ProductDTO productDTO)
        {
            // Obtener el producto desde la base de datos
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "El producto no fue encontrado." }); // Retorna 404 con mensaje
            }

            // Actualizar solo los campos que no sean nulos
            if (productDTO.Name != null)
            {
                product.Name = productDTO.Name;
            }

            if (productDTO.Description != null)
            {
                product.Description = productDTO.Description;
            }

            if (productDTO.Price.HasValue) 
            {
                product.Price = productDTO.Price.Value; 
            }

            if (productDTO.CategoryId != null)
            {
                product.CategoryId = productDTO.CategoryId;
            }

            if (productDTO.Score.HasValue) 
            {
                product.Score = productDTO.Score.Value; 
            }

            if (productDTO.ImgUrl != null)
            {
                product.ImgUrl = productDTO.ImgUrl;
            }

            _context.Entry(product).State = EntityState.Modified;

            try
            {
                // Guardar los cambios en la base de datos
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                return NotFound(new { Message = "El producto no fue encontrado al intentar actualizar." }); // Retorna 404 con mensaje
            }
            // Retornar no content para indicar que la actualización fue exitosa
            return Ok(new
            {
                Message = "El producto ha sido actualizado correctamente.",
                product

            });
        }




        // POST: api/Products
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        [Authorize(Roles = "admin")]
        public async Task<ActionResult> PostProduct(ProductDTO productDTO)
        {
            // Verifica si la categoría existe
            var category = await _context.Categories.FindAsync(productDTO.CategoryId);
            if (category == null)
            {
                return BadRequest("La categoría especificada no existe.");
            }

            // Crea un nuevo producto usando el DTO
            var product = new Product
            {
                Name = productDTO.Name,
                Description = productDTO.Description,
                Price = productDTO.Price.Value,
                CategoryId = productDTO.CategoryId,
                Score = productDTO.Score.Value,
                ImgUrl = productDTO.ImgUrl,
                Category = category
            };

            _context.Products.Add(product);
            await _context.SaveChangesAsync();

            // Proyección para devolver solo los datos deseados
            var result = new
            {
                product.Id,
                product.Name,
                product.Description,
                product.Price,
                product.Score,
                product.ImgUrl,
                product.CategoryId,
                CategoryName = category.Name // Solo incluye el nombre de la categoría
            };

            return CreatedAtAction("GetProduct", new { id = product.Id }, result);
        }


        // DELETE: api/Products/5
        [HttpDelete("{id}")]
        [Authorize(Roles = "admin")]
        public async Task<IActionResult> DeleteProduct(int id)
        {
            var product = await _context.Products.FindAsync(id);
            if (product == null)
            {
                return NotFound(new { Message = "El producto no fue encontrada." }); // Retorna 404 con mensaje
            }

            _context.Products.Remove(product);
            await _context.SaveChangesAsync();

            return Ok(new
            {
                Message = "El producto ha sido eliminado correctamente.",
                product

            });
        }

        private bool ProductExists(int id)
        {
            return _context.Products.Any(e => e.Id == id);
        }
    }
}
