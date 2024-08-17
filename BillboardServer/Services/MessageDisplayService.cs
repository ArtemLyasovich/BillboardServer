using BillboardServer.Models;
using BillboardServer.Repositories;

using static BillboardServer.Consts.Consts;

namespace BillboardServer.Services;

public class MessageDisplayService
{
    private readonly MessageQueueService _messageQueueService;
    private readonly ILogger<MessageDisplayService> _logger;
    private readonly IServiceProvider _serviceProvider;
    private readonly CancellationTokenSource _cts;
    private Message _currentMessage;

    public MessageDisplayService(MessageQueueService messageQueueService, IServiceProvider serviceProvider, ILogger<MessageDisplayService> logger)
    {
        _messageQueueService = messageQueueService;
        _logger = logger;
        _serviceProvider = serviceProvider;
        _cts = new CancellationTokenSource();
        _currentMessage = CreateDefaultMessage();

        StartMessageProcessing();
    }

    private static Message CreateDefaultMessage()
    {
        return new Message
        {
            Content = DEFAULT_MESSAGE,
            CreatedAt = DateTime.UtcNow
        };
    }

    private void StartMessageProcessing()
    {
        _ = Task.Run(async () =>
        {
            while (!_cts.Token.IsCancellationRequested)
            {
                ProcessNextMessage();
                await Task.Delay(GetDelay(), _cts.Token);
            }
        }, _cts.Token);
    }

    private void ProcessNextMessage()
    {
        if (_messageQueueService.TryDequeueMessage(out var nextMessage))
        {
            _currentMessage = nextMessage;
            _logger.LogInformation("{0} - Message dequeued and displayed: \"{1}\"", MESSAGE_DISPLAY_SERVICE_LOGID, _currentMessage.Content);
        }
        else
        {
            _currentMessage = CreateDefaultMessage();
        }
    }

    private TimeSpan GetDelay()
    {
        return _currentMessage.Content == DEFAULT_MESSAGE
            ? TimeSpan.FromSeconds(1)
            : TimeSpan.FromMinutes(1);
    }

    public Message GetCurrentMessage()
    {
        return _currentMessage;
    }

    public async Task Stop()
    {
        await SaveRemainingMessages();

        _cts.Cancel();
        _cts.Dispose();
    }

    private async Task SaveRemainingMessages()
    {
        using (var scope = _serviceProvider.CreateScope())
        {
            var billboardRepository = scope.ServiceProvider.GetRequiredService<BillboardRepository>();
            await _messageQueueService.SaveRemainingMessagesToDatabase(billboardRepository);
        }
    }
}
