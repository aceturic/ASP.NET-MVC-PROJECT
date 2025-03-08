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
                .Select(r => new Review
                {
                    Author = r.Author,
                    Content = r.Content,
                    Rating = r.Rating, // Ensure rating is included
                    CreatedAt = r.CreatedAt
                })
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

            // If validation fails, reload the product details page with existing data
            var product = _context.Products.FirstOrDefault(p => p.Id == review.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            var reviews = _context.Reviews
                .Where(r => r.ProductId == review.ProductId)
                .OrderByDescending(r => r.CreatedAt)
                .Select(r => new Review
                {
                    Author = r.Author,
                    Content = r.Content,
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                })
                .ToList();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = reviews
            };

            return View("Index", viewModel);
        }
    }
}
