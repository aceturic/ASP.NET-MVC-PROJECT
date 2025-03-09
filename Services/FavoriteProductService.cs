using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using UsersApp.Data;
using UsersApp.Models;

public class FavoriteProductService
{
    private readonly AppDbContext _context;

    public FavoriteProductService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<List<Product>> GetFavoriteProductsAsync(string userId)
    {
        return await _context.FavoriteProducts
            .Where(f => f.UserId == userId)
            .Include(f => f.Product)
            .Select(f => f.Product)
            .ToListAsync();
    }

    public async Task AddFavoriteAsync(int productId, string userId)
    {
        if (!_context.FavoriteProducts.Any(f => f.ProductId == productId && f.UserId == userId))
        {
            _context.FavoriteProducts.Add(new FavoriteProduct { ProductId = productId, UserId = userId });
            await _context.SaveChangesAsync();
        }
    }

    public async Task RemoveFavoriteAsync(int productId, string userId)
    {
        var favorite = await _context.FavoriteProducts
            .FirstOrDefaultAsync(f => f.ProductId == productId && f.UserId == userId);

        if (favorite != null)
        {
            _context.FavoriteProducts.Remove(favorite);
            await _context.SaveChangesAsync();
        }
    }

    public async Task<bool> IsProductFavoriteAsync(int productId, string userId)
    {
        return await _context.FavoriteProducts.AnyAsync(f => f.ProductId == productId && f.UserId == userId);
    }

}
