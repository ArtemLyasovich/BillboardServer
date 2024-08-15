using System.Collections.Concurrent;

using BillboardServer.Models;
using BillboardServer.Repositories;

namespace BillboardServer.Services;

public class MessageQueueService
{
    private readonly ConcurrentQueue<Message> _messageQueue;

    public MessageQueueService(IServiceProvider serviceProvider)
    {
        _messageQueue = new ConcurrentQueue<Message>();

        using (var scope = serviceProvider.CreateScope())
        {
            var billboardRepository = scope.ServiceProvider.GetRequiredService<BillboardRepository>();
            InitializeQueueAsync(billboardRepository).Wait();
        }
    }

    private async Task InitializeQueueAsync(BillboardRepository billboardRepository)
    {
        var allMessages = await billboardRepository.GetAllMessagesAsync();
        
        foreach (var message in allMessages)
        {
            _messageQueue.Enqueue(message);
        }
    }

    public void EnqueueMessage(string messageContent)
    {
        var message = new Message 
        { 
            Content = messageContent,
            CreatedAt = DateTime.UtcNow
        };

        _messageQueue.Enqueue(message);
    }

    public bool TryDequeueMessage(out Message message)
    {
        return _messageQueue.TryDequeue(out message!);
    }

    public async Task SaveRemainingMessagesToDatabase(BillboardRepository billboardRepository)
    {
        await billboardRepository.AddMessagesAsync(_messageQueue);
    }
}