using BillboardServer.Services;
using Microsoft.AspNetCore.Mvc;

using static BillboardServer.Consts.Consts;

namespace BillboardServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillboardController : ControllerBase
    {
        private readonly MessageQueueService _messageQueueService;
        private readonly MessageDisplayService _messageDisplayService;

        public BillboardController(MessageQueueService messageQueueService, MessageDisplayService messageDisplayService)
        {
            _messageQueueService = messageQueueService;
            _messageDisplayService = messageDisplayService;
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
                return BadRequest(ERROR_EMPTY_MESSAGE);
            }

            _messageQueueService.EnqueueMessage(message);
            return Ok(SUCCESS_MESSAGE_ENQUEUED);
        }
    }
}
