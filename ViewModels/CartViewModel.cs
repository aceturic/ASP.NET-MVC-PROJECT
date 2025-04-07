namespace UsersApp.ViewModels
{
    public class CartViewModel
    {
        public List<CartItemViewModel> Items { get; set; } = new();

        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string DeliveryAddress { get; set; }
        public string City { get; set; }
        public string Country { get; set; }
        public string ZipCode { get; set; }
        public string PhoneNumber { get; set; }
        public string? UserEmail { get; set; }

        public string? CouponCode { get; set; }

        public decimal DiscountAmount { get; set; }

        public decimal GrandTotalAfterDiscount => GrandTotal - DiscountAmount;

        public decimal GrandTotal => Items.Sum(i => i.TotalPrice);
    }
}
