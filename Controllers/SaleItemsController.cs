using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebAPi.Context;
using WebAPi.Models;

namespace WebAPi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class SaleItemsController : ControllerBase
    {
        private readonly AppDbContext _context;

        public SaleItemsController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/SaleItems
        [HttpGet]
        public async Task<ActionResult<IEnumerable<SaleItem>>> GetSaleItems()
        {
            return await _context.SaleItems.ToListAsync();
        }

        // GET: api/SaleItems/5
        [HttpGet("{id}")]
        public async Task<ActionResult<SaleItem>> GetSaleItem(int id)
        {
            var saleItem = await _context.SaleItems.FindAsync(id);

            if (saleItem == null)
            {
                return NotFound();
            }

            return saleItem;
        }

        // PUT: api/SaleItems/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutSaleItem(int id, SaleItem saleItem)
        {
            if (id != saleItem.Id)
            {
                return BadRequest();
            }

            _context.Entry(saleItem).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!SaleItemExists(id))
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

        // POST: api/SaleItems
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<SaleItem>> PostSaleItem(SaleItem saleItem)
        {
            _context.SaleItems.Add(saleItem);
            await _context.SaveChangesAsync();

            return CreatedAtAction("GetSaleItem", new { id = saleItem.Id }, saleItem);
        }

        // DELETE: api/SaleItems/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteSaleItem(int id)
        {
            var saleItem = await _context.SaleItems.FindAsync(id);
            if (saleItem == null)
            {
                return NotFound();
            }

            _context.SaleItems.Remove(saleItem);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        private bool SaleItemExists(int id)
        {
            return _context.SaleItems.Any(e => e.Id == id);
        }
    }
}
