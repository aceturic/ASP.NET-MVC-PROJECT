using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using UsersApp.Models;
using System;
using System.Linq;
using System.Threading.Tasks;
using UsersApp.Data;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;

namespace UsersApp.Controllers
{
    [Authorize(Roles = "Admin")]
    public class PurchaseHistoryAdminController : Controller
    {
        private readonly AppDbContext _context;

        public PurchaseHistoryAdminController(AppDbContext context)
        {
            _context = context;
        }

        // GET: PurchaseHistoryAdmin
        public async Task<IActionResult> Index()
        {
            var orders = _context.Orders.Include(o => o.Product);
            return View(await orders.ToListAsync());
        }

        // GET: PurchaseHistoryAdmin/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name");

            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }
            return View(order);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("Id,ProductId,Price,FirstName,LastName,DeliveryAddress,City,Country,ZipCode,PhoneNumber,UserEmail,OrderDate")] Order order)
        {
            if (id != order.Id)
            {
                return NotFound();
            }

            if (ModelState.IsValid)
            {
                try
                {
                    _context.Update(order);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!OrderExists(order.Id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        ModelState.AddModelError("", "Concurrency error: The order was modified by another process.");
                        return View(order);
                    }
                }
                TempData["SuccessMessage"] = "Order updated successfully!";
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Products = new SelectList(await _context.Products.ToListAsync(), "Id", "Name", order.ProductId);
            return View(order);
        }

        // GET: PurchaseHistoryAdmin/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var order = await _context.Orders
                .Include(o => o.Product)
                .FirstOrDefaultAsync(m => m.Id == id);
            if (order == null)
            {
                return NotFound();
            }

            return View(order);
        }

        // POST: PurchaseHistoryAdmin/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            var order = await _context.Orders.FindAsync(id);
            _context.Orders.Remove(order);
            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool OrderExists(int id)
        {
            return _context.Orders.Any(e => e.Id == id);
        }
    }
}
