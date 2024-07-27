using BillboardServer.Services;
using Microsoft.AspNetCore.Mvc;

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
            return Ok(_messageDisplayService.GetCurrentMessage());
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] string message)
        {
            if (string.IsNullOrEmpty(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            _messageQueueService.EnqueueMessage(message);
            return Ok();
        }
    }
}
