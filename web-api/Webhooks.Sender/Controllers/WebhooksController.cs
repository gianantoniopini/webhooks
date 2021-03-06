using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Webhooks.Sender.DbContexts;
using Webhooks.Sender.Models;
using Webhooks.Sender.ResourceModels;
using Webhooks.Sender.Mappers;

namespace Webhooks.Sender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhooksController : ControllerBase
    {
        private readonly ILogger<WebhooksController> _logger;

        private readonly WebhooksContext _context;

        private readonly IHttpClientFactory _clientFactory;

        public WebhooksController(ILogger<WebhooksController> logger, WebhooksContext context, IHttpClientFactory clientFactory)
        {
            _logger = logger;
            _context = context;
            _clientFactory = clientFactory;
        }

        // GET: api/Webhooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Webhook>>> GetWebhooks()
        {
            var webhooks = await _context.Webhooks.ToListAsync();

            return Ok(webhooks);
        }

        // GET: api/Webhooks/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Webhook>> GetWebhook(long id)
        {
            var webhook = await _context.Webhooks.FindAsync(id);

            if (webhook == null)
            {
                return NotFound();
            }

            return webhook;
        }

        // POST: api/Webhooks
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Webhook>> PostWebhook(CreateWebhookRequest request)
        {
            if (WebhookExists(request.PayloadUrl))
            {
                return UnprocessableEntity($"A {nameof(Webhook)} with {nameof(Webhook.PayloadUrl)} {request.PayloadUrl} already exists.");
            }

            var webhook = ModelMapper.ToModel(request);

            _context.Webhooks.Add(webhook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWebhook), new { id = webhook.Id }, webhook);
        }

        // POST: api/Webhooks/Send
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Send")]
        public async Task<ActionResult> SendWebhooks()
        {
            var activeWebhooks = _context.Webhooks.Where(e => e.IsActive);

            var tasks = activeWebhooks.Select(webhook => SendWebhook(_logger, _clientFactory, webhook));
            await Task.WhenAll(tasks);

            return Ok();
        }

        private static async Task SendWebhook(ILogger<WebhooksController> logger, IHttpClientFactory clientFactory, Webhook webhook)
        {
            var webhookPayload = new WebhookPayload { Message = $"{DateTimeOffset.UtcNow.ToString(CultureInfo.InvariantCulture)} - Hallo from {nameof(Webhook)} {webhook.Id.ToString(CultureInfo.InvariantCulture)}" };

            try
            {
                var httpClient = clientFactory.CreateClient("WebhookSender");

                var response = await httpClient.PostAsJsonAsync<WebhookPayload>(webhook.PayloadUrl, webhookPayload);

                response.EnsureSuccessStatusCode();
            }
            catch (Exception ex)
            {
                logger.LogError(ex, "Failed to send POST request to {0}", webhook.PayloadUrl);
            }
        }

        private bool WebhookExists(string payloadUrl)
        {
            return _context.Webhooks.Any(e => e.PayloadUrl == payloadUrl);
        }
    }
}
