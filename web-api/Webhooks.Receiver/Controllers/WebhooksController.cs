using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Webhooks.Receiver.Hubs;

namespace Webhooks.Receiver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhooksController : ControllerBase
    {
        private readonly ILogger<WebhooksController> _logger;

        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public WebhooksController(ILogger<WebhooksController> logger, IHubContext<NotificationHub> notificationHubContext)
        {
            _logger = logger;
            _notificationHubContext = notificationHubContext;
        }

        [HttpPost("Receive")]
        public async Task<ActionResult> ReceiveWebhook(string message)
        {
            await _notificationHubContext.Clients.All.SendAsync("webhookReceived", message);

            return NoContent();
        }
    }
}
