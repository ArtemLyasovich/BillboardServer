using BillboardServer.Data;
using BillboardServer.Models;
using BillboardServer.Extensions;

using Microsoft.EntityFrameworkCore;

using static BillboardServer.Consts.Consts;

namespace BillboardServer.Repositories;

public class BillboardRepository
{
    private readonly BillboardDbContext _context;
    private readonly ILogger<BillboardRepository> _logger;

    public BillboardRepository(BillboardDbContext context, ILogger<BillboardRepository> logger)
    {
        _context = context;
        _logger = logger;
    }

    public async Task<IEnumerable<Message>> GetAllMessagesAsync()
    {
        var allMessages = await _context.Messages
            .OrderBy(m => m.CreatedAt)
            .ToListAsync();

        if (allMessages.Any())
        {
            _logger.LogInformation("{0} - Loaded {1} messages from the database.", REPOSITORY_LOGID, allMessages.Count);

            allMessages.NormalizeDates();
            _context.Messages.RemoveRange(allMessages);
            await _context.SaveChangesAsync();
        }
        else
        {
            _logger.LogInformation("{0} - No messages found in the database to load.", REPOSITORY_LOGID);
        }

        return allMessages;
    }

    public async Task AddMessagesAsync(IEnumerable<Message> messages)
    {
        if (messages.Any())
        {
            _logger.LogInformation("{0} - Adding {1} messages to the database.", REPOSITORY_LOGID, messages.Count());

            messages.NormalizeDates();
            _context.Messages.AddRange(messages);
            await _context.SaveChangesAsync();

            _logger.LogInformation("{0} - Successfully added {1} messages to the database.", REPOSITORY_LOGID, messages.Count());
        }
        else
        {
            _logger.LogInformation("{0} - No messages to add to the database.", REPOSITORY_LOGID);
        }
    }
}
