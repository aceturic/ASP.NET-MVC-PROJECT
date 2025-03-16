using System;
using System.Linq;
using Microsoft.AspNetCore.Authorization;
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
                    Rating = r.Rating,
                    CreatedAt = r.CreatedAt
                })
                .ToList();

            var relatedProducts = _context.Products
                .Where(p => p.Category == product.Category && p.Id != product.Id)
                .OrderBy(p => Guid.NewGuid()) 
                .Take(4) 
                .ToList();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = reviews,
                RelatedProducts = relatedProducts
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

            var product = _context.Products.FirstOrDefault(p => p.Id == review.ProductId);
            if (product == null)
            {
                return NotFound();
            }

            var reviews = _context.Reviews
                .Where(r => r.ProductId == review.ProductId)
                .OrderByDescending(r => r.CreatedAt)
                .ToList();

            var relatedProducts = _context.Products
                .Where(p => p.Category == product.Category && p.Id != product.Id)
                .Take(4)
                .ToList();

            var viewModel = new ProductDetailsViewModel
            {
                Product = product,
                Reviews = reviews,
                RelatedProducts = relatedProducts
            };

            return View("Index", viewModel);
        }
        [Authorize(Roles = "Admin")]
        [HttpDelete]
        public IActionResult DeleteReview(int id)
        {
            var review = _context.Reviews.FirstOrDefault(r => r.Id == id);
            if (review == null)
            {
                return Json(new { success = false, message = "Review not found." });
            }

            _context.Reviews.Remove(review);
            _context.SaveChanges();

            return Json(new { success = true });
        }
    }
}
