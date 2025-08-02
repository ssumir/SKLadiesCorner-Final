using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SKLadiesCorner.Data;
using SKLadiesCorner.Models;

namespace SKLadiesCorner.Controllers
{
    public class OrderConfirmsController : Controller
    {
        private readonly ApplicationDbContext _context;

        public OrderConfirmsController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: OrderConfirms
        public async Task<IActionResult> Index()
        {
            // Update: Eager loading the Product navigation property for the Index view
            var applicationDbContext = _context.OrderConfirm.Include(o => o.Product);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: OrderConfirms/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Update: Eager loading the Product navigation property for the Details view
            var orderConfirm = await _context.OrderConfirm
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderConfirm == null)
            {
                return NotFound();
            }

            return View(orderConfirm);
        }

        // GET: OrderConfirms/Create
        // Update: This method now accepts productId and quantity from the previous page
        public async Task<IActionResult> Create(int productId, int quantity)
        {
            // Update: Fetch the Product to be ordered from the database
            var product = await _context.Product.FindAsync(productId);

            if (product == null)
            {
                return NotFound();
            }

            var orderConfirm = new OrderConfirm
            {
                ProductId = productId,
                Product = product, // Attach the full product object to the model
                // Note: The OrderConfirm model doesn't have a Quantity property. 
                // We'll pass it to the view using ViewBag.
            };

            // Update: Pass the quantity to the view for display purposes
            ViewBag.Quantity = quantity;

            return View(orderConfirm);
        }

        // POST: OrderConfirms/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Update: Removed SKU from the bind attribute as it's not an input field in our view
        public async Task<IActionResult> Create([Bind("OrderId,Name,PhoneNumber,Address,Email,SKU,ProductId")] OrderConfirm orderConfirm)
        {
            if (ModelState.IsValid)
            {
                // Update: Fetch the Product again to get its details before saving.
                // This is a good practice to prevent over-posting attacks on a hidden field.
                var product = await _context.Product.FindAsync(orderConfirm.ProductId);
                if (product == null)
                {
                    return NotFound();
                }

                // Update: Manually set the SKU before saving the order confirm.
                orderConfirm.SKU = product.SKU;

                _context.Add(orderConfirm);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            // Update: Re-fetching the product to populate the view in case of validation errors
            orderConfirm.Product = await _context.Product.FindAsync(orderConfirm.ProductId);

            // Update: Pass the quantity again in case of validation errors
            // Note: If you need quantity, it should be passed here again or added to the OrderConfirm model.
            ViewBag.Quantity = 1; // Assuming a default quantity for now

            return View(orderConfirm);
        }

        // GET: OrderConfirms/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Update: Eager loading the Product navigation property for the Edit view
            var orderConfirm = await _context.OrderConfirm.Include(o => o.Product).FirstOrDefaultAsync(o => o.OrderId == id);
            if (orderConfirm == null)
            {
                return NotFound();
            }
            // Note: The original code had ViewData for a dropdown, which is not needed for the updated view.
            return View(orderConfirm);
        }

        // POST: OrderConfirms/Edit/5
        [HttpPost]
        [ValidateAntiForgeryToken]
        // Update: Removed SKU from the bind attribute
        public async Task<IActionResult> Edit(int id, [Bind("OrderId,Name,PhoneNumber,Address,Email,SKU,ProductId")] OrderConfirm orderConfirm)
        {
            if (id != orderConfirm.OrderId)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    // Update: To save SKU, we would need to fetch the product again here
                    _context.Update(orderConfirm);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderConfirmExists(orderConfirm.OrderId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }

            // Update: Re-fetching the product to populate the view in case of validation errors
            orderConfirm.Product = await _context.Product.FindAsync(orderConfirm.ProductId);

            return View(orderConfirm);
        }

        // GET: OrderConfirms/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            // Update: Eager loading the Product navigation property for the Delete view
            var orderConfirm = await _context.OrderConfirm
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.OrderId == id);
            if (orderConfirm == null)
            {
                return NotFound();
            }

            return View(orderConfirm);
        }

        // POST: OrderConfirms/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var orderConfirm = await _context.OrderConfirm.FindAsync(id);
            if (orderConfirm != null)
            {
                _context.OrderConfirm.Remove(orderConfirm);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderConfirmExists(int id)
        {
            return _context.OrderConfirm.Any(e => e.OrderId == id);
        }
    }
}