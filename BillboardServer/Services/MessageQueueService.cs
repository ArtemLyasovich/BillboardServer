using System.Collections.Concurrent;

namespace BillboardServer.Services;

public class MessageQueueService
{
    private readonly ConcurrentQueue<string> _messageQueue;
    private readonly CancellationTokenSource _cts;

    private string _currentMessage;
    public MessageQueueService()
    {
        _messageQueue = new();
        _cts = new();

        _currentMessage = "";

        _ = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (_messageQueue.TryDequeue(out var nextMessage))
                {
                    _currentMessage = nextMessage;
                }

                await Task.Delay(TimeSpan.FromMinutes(1), _cts.Token);
            }
        }, _cts.Token);
    }

    public string GetCurrentMessage()
    {
        return _currentMessage;
    }

    public void EnqueueMessage(string message)
    {
        _messageQueue.Enqueue(message);
    }

    public void Stop()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
