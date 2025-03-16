using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersApp.Models;
using UsersApp.Data;
using UsersApp.Models;

namespace UsersApp.Services
{
    public class TicketService
    {
        private readonly AppDbContext _context;

        public TicketService(AppDbContext context)
        {
            _context = context;
        }

    public async Task<Ticket?> GetUserActiveTicketAsync(string userId)
        {
            return await _context.Tickets
                .FirstOrDefaultAsync(t => t.UserId == userId && t.Status != TicketStatus.Closed);
        }

        public async Task<List<Ticket>> GetAllTicketsAsync()
        {
            return await _context.Tickets.Include(t => t.Messages).ToListAsync();
        }

        public async Task<Ticket?> GetTicketByIdAsync(int id)
        {
            return await _context.Tickets.Include(t => t.Messages).FirstOrDefaultAsync(t => t.Id == id);
        }

        public async Task CreateTicketAsync(Ticket ticket)
        {
            var existingTicket = await GetUserActiveTicketAsync(ticket.UserId);
            if (existingTicket != null) return;

            _context.Tickets.Add(ticket);
            await _context.SaveChangesAsync();
        }

        public async Task AssignTicketAsync(int ticketId, string adminId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null && string.IsNullOrEmpty(ticket.AdminId))
            {
                ticket.AdminId = adminId;
                ticket.Status = TicketStatus.InProgress;
                await _context.SaveChangesAsync();
            }
        }

        public async Task CloseTicketAsync(int ticketId)
        {
            var ticket = await _context.Tickets.FindAsync(ticketId);
            if (ticket != null)
            {
                ticket.Status = TicketStatus.Closed;
                await _context.SaveChangesAsync();
            }
        }
       

   
    }
}
