using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models
{
    public class Coupon
    {
        public int Id { get; set; }

        [Required]
        public string Code { get; set; }

        [Range(1, 100)]
        public int DiscountPercent { get; set; }

        public int? MaxUsages { get; set; }

        public DateTime? ValidUntil { get; set; }

        public int TimesUsed { get; set; } = 0;
    }
}
