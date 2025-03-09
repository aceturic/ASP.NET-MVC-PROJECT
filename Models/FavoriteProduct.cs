using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace UsersApp.Models
{
    public class FavoriteProduct
    {
        public int Id { get; set; }
        public int ProductId { get; set; }
        public string UserId { get; set; } 
        public Product Product { get; set; }
    }
}
