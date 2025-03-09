using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Linq;
using System.Threading.Tasks;
using UsersApp.Data;
using UsersApp.Models;

namespace UsersApp.Controllers
{
    public class FavoritesController : Controller
    {
        private readonly AppDbContext _context;

        public FavoritesController(AppDbContext context)
        {
            _context = context;
        }

        // ✅ Add Product to Favorites (No Authentication Required)
        [HttpPost]
        public async Task<IActionResult> AddToFavorites([FromBody] Favorite favoriteModel)
        {
            // ✅ Ensure product exists before adding to favorites
            var product = await _context.Products.FirstOrDefaultAsync(p => p.Id == favoriteModel.ProductId);
            if (product == null)
            {
                return Json(new { success = false, message = "Product not found." });
            }

            // ✅ Check if already in favorites
            var existingFavorite = await _context.Favorites
                .FirstOrDefaultAsync(f => f.ProductId == favoriteModel.ProductId);

            if (existingFavorite == null)
            {
                var favorite = new Favorite
                {
                    ProductId = favoriteModel.ProductId
                };

                _context.Favorites.Add(favorite);
                await _context.SaveChangesAsync(); // ✅ Ensure it is saved

                return Json(new { success = true, message = "Added to favorites." });
            }

            return Json(new { success = false, message = "Already in favorites." });
        }

        // ✅ View Favorites Page (No Authentication Required)
        public async Task<IActionResult> Index()
        {
            var favorites = await _context.Favorites
                .Include(f => f.Product)
                .ToListAsync();

            return View(favorites);
        }
    }
}
