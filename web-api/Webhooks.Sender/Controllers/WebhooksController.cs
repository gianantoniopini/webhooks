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

namespace Webhooks.Sender.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class WebhooksController : ControllerBase
    {
        private readonly ILogger<WebhooksController> _logger;

        private readonly WebhooksContext _context;

        public WebhooksController(ILogger<WebhooksController> logger, WebhooksContext context)
        {
            _logger = logger;
            _context = context;
        }

        // GET: api/Webhooks
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Webhook>>> GetWebhooks()
        {
            return await _context.Webhooks.ToListAsync();
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

            var webhook = ToModel(request);

            _context.Webhooks.Add(webhook);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetWebhook), new { id = webhook.Id }, webhook);
        }

        // DELETE: api/Webhooks/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteWebhook(long id)
        {
            var webhook = await _context.Webhooks.FindAsync(id);
            if (webhook == null)
            {
                return NotFound();
            }

            _context.Webhooks.Remove(webhook);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // POST: api/Webhooks/Send
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost("Send")]
        public async Task<ActionResult> SendWebhooks()
        {
            var activeWebhooks = _context.Webhooks.Where(e => e.IsActive);

            foreach (var webhook in activeWebhooks)
            {
                await SendWebhook(webhook);
            }

            return Ok();
        }

        private static Webhook ToModel(CreateWebhookRequest request)
        {
            var webhook = new Webhook { PayloadUrl = request.PayloadUrl, IsActive = request.IsActive, CreatedAt = DateTimeOffset.UtcNow };

            return webhook;
        }

        private bool WebhookExists(string payloadUrl)
        {
            return _context.Webhooks.Any(e => e.PayloadUrl == payloadUrl);
        }

        private async Task SendWebhook(Webhook webhook)
        {
            try
            {
                var webhookPayload = new WebhookPayload { Message = $"{DateTimeOffset.UtcNow.ToString(CultureInfo.InvariantCulture)} - Hallo from {nameof(Webhook)} {webhook.Id.ToString(CultureInfo.InvariantCulture)}" };

                using (HttpClient httpClient = new HttpClient())
                {
                    var response = await httpClient.PostAsJsonAsync<WebhookPayload>(webhook.PayloadUrl, webhookPayload);
                    if (!response.IsSuccessStatusCode)
                    {
                        _logger.LogError("Received response with status code {0} after sending POST request to {1}", response.StatusCode, webhook.PayloadUrl);
                    }
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Failed to send POST request to {0}", webhook.PayloadUrl);
            }

        }
    }

}
