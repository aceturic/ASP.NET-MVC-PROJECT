using UsersApp.Data;
using UsersApp.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;

namespace UsersApp.Services
{
    public class CouponService
    {
        private readonly AppDbContext _context;

        public CouponService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Coupon>> GetAllCouponsAsync()
        {
            return await _context.Coupons.ToListAsync();
        }

        public async Task CreateCouponAsync(Coupon coupon)
        {
            _context.Coupons.Add(coupon);
            await _context.SaveChangesAsync();
        }

        public async Task<Coupon?> GetValidCouponAsync(string code)
        {
            var now = DateTime.UtcNow;

            return await _context.Coupons
                .Where(c => c.Code == code &&
                            (c.ValidUntil == null || c.ValidUntil > now) &&
                            (c.MaxUsages == null || c.TimesUsed < c.MaxUsages))
                .FirstOrDefaultAsync();
        }

    }
}

