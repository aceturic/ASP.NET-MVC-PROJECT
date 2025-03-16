using System;
using System.ComponentModel.DataAnnotations;

namespace UsersApp.Models
{
    public class Ticket
    {
        public int Id { get; set; }

        [Required]
        public string UserId { get; set; } // User who opened the ticket

        public string? AdminId { get; set; } // Admin assigned to the ticket

        [Required]
        [StringLength(100)]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        public TicketStatus Status { get; set; } = TicketStatus.Open;

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public List<Message>? Messages { get; set; } = new List<Message>();
    }

    public enum TicketStatus
    {
        Open,
        InProgress,
        Resolved,
        Closed
    }
}
