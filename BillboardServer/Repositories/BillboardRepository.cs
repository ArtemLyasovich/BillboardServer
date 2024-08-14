using BillboardServer.Data;
using BillboardServer.Models;

using Microsoft.EntityFrameworkCore;

namespace BillboardServer.Repositories
{
    public class BillboardRepository
    {
        private readonly BillboardDbContext _context;

        public BillboardRepository(BillboardDbContext context)
        {
            _context = context;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            var allMessages = await _context.Messages.OrderBy(m => m.CreatedAt).ToListAsync();

            _context.Messages.RemoveRange(allMessages);
            await _context.SaveChangesAsync();

            return allMessages;
        }


        public async Task AddMessagesAsync(IEnumerable<Message> messages)
        {
            await _context.Messages.AddRangeAsync(messages);
            await _context.SaveChangesAsync();
        }
    }
}
