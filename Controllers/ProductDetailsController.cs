using System;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using UsersApp.Data;
using UsersApp.Models;
using UsersApp.ViewModels;

namespace UsersApp.Controllers
{
    public class ProductDetailsController : Controller
    {
        private readonly AppDbContext _context;

        public ProductDetailsController(AppDbContext context)
        {
            _context = context;
        }

        public IActionResult Index(int id)
        {
            var product = _context.Products.FirstOrDefault(p => p.Id == id);
            if (product == null)
            {
                return NotFound();
            }

            var reviews = _context.Reviews
                .Where(r => r.ProductId == id)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = reviews
            };

            return View(viewModel);
        }

        [HttpPost]
        public IActionResult SubmitReview(Review review)
        {
            if (ModelState.IsValid)
            {
                review.CreatedAt = DateTime.Now;
                _context.Reviews.Add(review);
                _context.SaveChanges();
                return RedirectToAction("Index", new { id = review.ProductId });
            }
            // If review submission fails, you can reload the product details page.
            return RedirectToAction("Index", new { id = review.ProductId });
        }
    }
}
