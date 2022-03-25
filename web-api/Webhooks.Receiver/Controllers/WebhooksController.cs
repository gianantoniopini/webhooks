using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Webhooks.Receiver.Hubs;
using Webhooks.Receiver.ResourceModels;

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
        public async Task<ActionResult> ReceiveWebhook(WebhookPayload webhookPayload)
        {
            await _notificationHubContext.Clients.All.SendAsync("webhookReceived", webhookPayload);

            return Ok();
        }
    }
}
