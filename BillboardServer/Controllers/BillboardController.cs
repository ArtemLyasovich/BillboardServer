using BillboardServer.Services;
using Microsoft.AspNetCore.Mvc;

namespace BillboardServer.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class BillboardController : ControllerBase
    {
        private readonly MessageQueueService _messageQueueService;

        public BillboardController(MessageQueueService messageQueueService)
        {
            _messageQueueService = messageQueueService;
        }

        [HttpGet]
        public IActionResult GetCurrentMessage()
        {
            var message = _messageQueueService.GetCurrentMessage();
            return Ok(message);
        }

        [HttpPost]
        public IActionResult PostMessage([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be empty.");
            }

            _messageQueueService.EnqueueMessage(message);
            return Ok();
        }
    }
}
