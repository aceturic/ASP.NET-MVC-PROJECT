using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace UsersApp.Models
{
    public class OrderItem
    {
        [Key] public int Id { get; set; }

        [Required] public int OrderId { get; set; }
        [ForeignKey("OrderId")]
        [ValidateNever] public virtual Order Order { get; set; }

        [Required] public int ProductId { get; set; }
        [ForeignKey("ProductId")]
        [ValidateNever] public virtual Product Product { get; set; }

        [Required] public int Quantity { get; set; }
        [Required] public decimal Price { get; set; }
    }
}
