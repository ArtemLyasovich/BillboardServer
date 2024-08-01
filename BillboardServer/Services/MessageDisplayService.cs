using static BillboardServer.Consts.Consts;

namespace BillboardServer.Services;

public class MessageDisplayService
{
    private readonly MessageQueueService _messageQueueService;
    private readonly CancellationTokenSource _cts;
    private string _currentMessage;

    public MessageDisplayService(MessageQueueService messageQueueService)
    {
        _messageQueueService = messageQueueService;
        _cts = new CancellationTokenSource();

        _currentMessage = DEFAULT_MESSAGE;

        _ = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                if (_messageQueueService.TryDequeueMessage(out var nextMessage))
                {
                    _currentMessage = nextMessage;
                    await Task.Delay(TimeSpan.FromMinutes(1), _cts.Token);
                }
                else
                {
                    _currentMessage = DEFAULT_MESSAGE;
                    await Task.Delay(TimeSpan.FromSeconds(1), _cts.Token);
                }
            }
        }, _cts.Token);
    }

    public string GetCurrentMessage()
    {
        return _currentMessage;
    }

    public void Stop()
    {
        _cts.Cancel();
        _cts.Dispose();
    }
}
