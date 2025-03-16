using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using UsersApp.Models;
using UsersApp.Data;
using UsersApp.Models;

namespace UsersApp.Services
{
    public class ChatService
    {
        private readonly AppDbContext _context;

        public ChatService(AppDbContext context)
        {
            _context = context;
        }

        public async Task<List<Message>> GetMessagesAsync(int ticketId)
        {
            return await _context.Messages.Where(m => m.TicketId == ticketId).OrderBy(m => m.SentAt).ToListAsync();
        }

        public async Task SendMessageAsync(Message message)
        {
            _context.Messages.Add(message);
            await _context.SaveChangesAsync();
        }
    }
}
