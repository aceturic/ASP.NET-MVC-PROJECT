using System;

namespace UsersApp.Models
{
    public class Order
    {
        public int Id { get; set; }
        public string PurchaseId { get; set; } // Randomly generated unique ID
        public string ProductName { get; set; }
        public decimal Price { get; set; }
        public string DeliveryAddress { get; set; }
        public string City { get; set; }
        public string ZipCode { get; set; }
        public DateTime OrderDate { get; set; }
    }
}
