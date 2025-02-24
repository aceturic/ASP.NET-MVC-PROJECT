using UsersApp.Models;

namespace UsersApp.ViewModels
{
    public class PurchaseOrder
    {
        public int Id { get; set; }
        public string PurchaseId { get; set; }
        public string Email { get; set; }
        public string DeliveryAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime OrderDate { get; set; }
        public int ProductId { get; set; }
        public Product Product { get; set; }
    }
}
