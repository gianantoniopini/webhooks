using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Microsoft.AspNetCore.SignalR;
using Webhooks.Receiver.Hubs;
using Webhooks.Receiver.Models;

namespace Webhooks.Receiver.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmailsController : ControllerBase
    {
        private readonly ILogger<EmailsController> _logger;

        private readonly IHubContext<NotificationHub> _notificationHubContext;

        public EmailsController(ILogger<EmailsController> logger, IHubContext<NotificationHub> notificationHubContext)
        {
            _logger = logger;
            _notificationHubContext = notificationHubContext;
        }

        [HttpPost("sent")]
        public async Task EmailSent(Email email)
        {
            await _notificationHubContext.Clients.All.SendAsync("emailSent", email);
        }
    }
}
