using BillboardServer.Services;
using Microsoft.AspNetCore.Mvc;

using static BillboardServer.Consts.Consts;

namespace BillboardServer.Controllers;

[ApiController]
[Route("[controller]")]
public class BillboardController : ControllerBase
{
    private readonly MessageQueueService _messageQueueService;
    private readonly MessageDisplayService _messageDisplayService;
    private readonly ILogger<BillboardController> _logger;

    public BillboardController(MessageQueueService messageQueueService,
                               MessageDisplayService messageDisplayService,
                               ILogger<BillboardController> logger)
    {
        _messageQueueService = messageQueueService;
        _messageDisplayService = messageDisplayService;
        _logger = logger;
    }

    [HttpGet]
    public IActionResult GetMessage()
    {
        var currentMessage = _messageDisplayService.GetCurrentMessage();
        return Ok(currentMessage.Content);
    }

    [HttpPost]
    public IActionResult PostMessage([FromBody] string message)
    {
        if (string.IsNullOrWhiteSpace(message))
        {
            _logger.LogWarning("{0} - Received an empty or whitespace message.", CONTROLLER_LOGID);
            return BadRequest(ERROR_EMPTY_MESSAGE);
        }

        _logger.LogInformation("{0} - Received message: {1}", CONTROLLER_LOGID, message);

        _messageQueueService.EnqueueMessage(message);
        return Ok(SUCCESS_MESSAGE_ENQUEUED);
    }
}
