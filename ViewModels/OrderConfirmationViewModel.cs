namespace UsersApp.ViewModels
{
    public class OrderConfirmationViewModel
    {
        public string PurchaseId { get; set; }
        public string Email { get; set; }
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string DeliveryAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
    }
}
