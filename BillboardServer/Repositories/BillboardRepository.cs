using BillboardServer.Data;
using BillboardServer.Models;
using BillboardServer.Extensions;

using Microsoft.EntityFrameworkCore;

namespace BillboardServer.Repositories;

public class BillboardRepository
{
    private readonly BillboardDbContext _context;

    public BillboardRepository(BillboardDbContext context)
    {
        _context = context;
    }

    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        var allMessages = await _context.Messages
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        if (allMessages.Any())
        {
            allMessages.NormalizeDates();
            _context.Messages.RemoveRange(allMessages);
            await _context.SaveChangesAsync();
        }

        return allMessages;
    }

    public async Task AddMessagesAsync(IEnumerable<Message> messages)
    {
        if (messages.Any())
        {
            messages.NormalizeDates();
            _context.Messages.AddRange(messages);
            await _context.SaveChangesAsync();
        }
    }
}
