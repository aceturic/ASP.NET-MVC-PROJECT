using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

[Authorize]
[Route("Favorites")]
public class FavoritesController : Controller
{
    private readonly FavoriteProductService _favoriteProductService;

    public FavoritesController(FavoriteProductService favoriteProductService)
    {
        _favoriteProductService = favoriteProductService;
    }

    [HttpGet("Index")]
    public async Task<IActionResult> Index()
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        var favorites = await _favoriteProductService.GetFavoriteProductsAsync(userId);
        return View(favorites);
    }

    [HttpPost("AddToFavorites")]
    public async Task<IActionResult> AddToFavorites([FromBody] FavoriteRequest request)
    {
        if (request == null || request.ProductId <= 0)
        {
            return Json(new { success = false, message = "Invalid product ID" });
        }

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (userId == null)
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        await _favoriteProductService.AddFavoriteAsync(request.ProductId, userId);
        return Json(new { success = true });
    }

    [HttpGet("IsFavorite/{productId}")]
    public async Task<IActionResult> IsFavorite(int productId)
    {
        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        bool isFavorite = await _favoriteProductService.IsProductFavoriteAsync(productId, userId);
        return Json(new { isFavorite });
    }

    [HttpPost("Remove")]
    public async Task<IActionResult> Remove([FromBody] FavoriteRequest request)
    {
        if (request == null || request.ProductId <= 0)
        {
            return Json(new { success = false, message = "Invalid product ID" });
        }

        string userId = User.FindFirstValue(ClaimTypes.NameIdentifier);
        if (string.IsNullOrEmpty(userId))
        {
            return Json(new { success = false, message = "User not authenticated" });
        }

        await _favoriteProductService.RemoveFavoriteAsync(request.ProductId, userId);
        return Json(new { success = true, message = "Product removed successfully!" });
    }
}

public class FavoriteRequest
{
    public int ProductId { get; set; }
}
