using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models
{
    public class Review
    {
        public int Id { get; set; }
        public int ProductId { get; set; }

        [Required]
        public string FullName { get; set; }

        [Required]
        [StringLength(1000)]
        public string Content { get; set; }

        public DateTime DatePosted { get; set; } = DateTime.Now;
    }
}
