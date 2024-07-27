using System.Collections.Concurrent;

namespace BillboardServer.Services;

public class MessageQueueService
{
    private readonly ConcurrentQueue<string> _messageQueue;

    public MessageQueueService()
    {
        _messageQueue = new ConcurrentQueue<string>();
    }

    public void EnqueueMessage(string message)
    {
        _messageQueue.Enqueue(message);
    }

    public bool TryDequeueMessage(out string message)
    {
        return _messageQueue.TryDequeue(out message!);
    }
}