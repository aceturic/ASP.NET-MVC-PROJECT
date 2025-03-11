using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersApp.Models;
using UsersApp.Data;

namespace UsersApp.Services
{
    public class OrderService
    {
        private readonly AppDbContext _context;

        public OrderService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Order>> GetUserOrdersAsync(string userEmail)
        {
            if (string.IsNullOrEmpty(userEmail)) return new List<Order>();

            return await _context.Orders
                .Where(o => o.UserEmail == userEmail)
                .OrderByDescending(o => o.OrderDate) // Sort by latest orders
                .ToListAsync();
        }
    }
}
