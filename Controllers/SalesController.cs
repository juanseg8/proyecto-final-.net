using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
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
    public class SalesController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SalesController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/Sales
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Sale>>> GetSales()
        {
            try
            {
                // Obtener todas las ventas incluyendo sus SaleItems
                var sales = await _context.Sales
                    .Include(s => s.SaleItems)  // Incluir la colección de SaleItems
                    .ToListAsync();

                return Ok(sales);  // Retornar todas las ventas con sus SaleItems
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al obtener las ventas.", Error = ex.Message });
            }
        }

        // GET: api/Sales/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Sale>> GetSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);

            if (sale == null)
            {
                return NotFound();
            }

            return sale;
        }

        // PUT: api/Sales/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSale(int id, Sale sale)
        {
            if (id != sale.Id)
            {
                return BadRequest();
            }

            _context.Entry(sale).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return NoContent();
        }

        // POST: api/Sales
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Sale>> PostSale(SaleDTO saleDTO)
        {
            if (saleDTO == null || saleDTO.SaleItems == null || saleDTO.SaleItems.Count == 0)
            {
                return BadRequest(new { Message = "La venta debe incluir al menos un ítem." });
            }

            // Crear la entidad Sale
            var sale = new Sale
            {
                Buyer = saleDTO.Buyer,
                Withdraw = saleDTO.Withdraw,
                Address = saleDTO.Address,
                PaymentMethod = saleDTO.PaymentMethod,
                Date = saleDTO.Date,
                Total = saleDTO.Total,
                SaleItems = new List<SaleItem>()
            };

            // Agregar los SaleItem a la venta
            foreach (var item in saleDTO.SaleItems)
            {
                var saleItem = new SaleItem
                {
                    ProductId = item.ProductId,
                    Quantity = item.Quantity,
                    Price = item.Price,
                    Subtotal = item.Subtotal
                };

                sale.SaleItems.Add(saleItem);
            }

            // Guardar la venta y sus ítems en la base de datos
            try
            {
                _context.Sales.Add(sale); // Agregar la venta y automáticamente incluir sus SaleItems
                await _context.SaveChangesAsync();

                return Ok(new
                {
                    Message = "Venta registrada exitosamente.",
                    SaleId = sale.Id
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { Message = "Ocurrió un error al registrar la venta.", Error = ex.Message });
            }
        }

        // DELETE: api/Sales/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSale(int id)
        {
            var sale = await _context.Sales.FindAsync(id);
            if (sale == null)
            {
                return NotFound();
            }

            _context.Sales.Remove(sale);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleExists(int id)
        {
            return _context.Sales.Any(e => e.Id == id);
        }
    }
}
