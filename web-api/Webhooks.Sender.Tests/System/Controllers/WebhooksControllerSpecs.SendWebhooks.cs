using System.Globalization;
using System.Net;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Newtonsoft.Json;
using Xunit;
using Webhooks.Sender.Models;
using Webhooks.Sender.ResourceModels;

namespace Webhooks.Sender.Tests.System.Controllers
{
    public partial class WebhooksControllerSpecs
    {
        [Fact]
        public async Task SendWebhooks_ShouldIgnoreDisabledWebhooks()
        {
            // Arrange
            var webhooks = new List<Webhook>
            {
                new Webhook { Id = 1, PayloadUrl = "https://url1.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 2, PayloadUrl = "https://url2.com", IsActive = false, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 3, PayloadUrl = "https://url3.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
            };
            _dbContext.Webhooks.AddRange(webhooks);
            _dbContext.SaveChanges();

            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[0].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(100));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[1].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(500));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[2].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(150));

            // Act
            await _controller.SendWebhooks();

            // Assert
            _httpMessageHandler.VerifyAnyRequest(Times.Exactly(2));
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[0].PayloadUrl, Times.Once());
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[1].PayloadUrl, Times.Never());
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[2].PayloadUrl, Times.Once());
        }

        [Fact]
        public async Task SendWebhooks_ShouldSendRequestsWithJsonPayload()
        {
            // Arrange
            var webhooks = new List<Webhook>
            {
                new Webhook { Id = 1, PayloadUrl = "https://url1.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 2, PayloadUrl = "https://url2.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 3, PayloadUrl = "https://url3.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
            };
            _dbContext.Webhooks.AddRange(webhooks);
            _dbContext.SaveChanges();

            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[0].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(100));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[1].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(500));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[2].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(150));

            // Act
            await _controller.SendWebhooks();

            // Assert
            _httpMessageHandler.VerifyAnyRequest(Times.Exactly(3));
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[0].PayloadUrl, async requestMessage => await MatchRequestBody(requestMessage, webhooks[0]), Times.Once());
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[1].PayloadUrl, async requestMessage => await MatchRequestBody(requestMessage, webhooks[1]), Times.Once());
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[2].PayloadUrl, async requestMessage => await MatchRequestBody(requestMessage, webhooks[2]), Times.Once());
        }

        [Fact]
        public async Task SendWebhooks_IfRequestToAPayloadUrlFails_ShouldNotStopRequestsToOtherPayloadUrls()
        {
            // Arrange
            var webhooks = new List<Webhook>
            {
                new Webhook { Id = 1, PayloadUrl = "https://url1.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 2, PayloadUrl = "https://url2.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 3, PayloadUrl = "https://url3.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
            };
            _dbContext.Webhooks.AddRange(webhooks);
            _dbContext.SaveChanges();

            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[0].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(100));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[1].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError), TimeSpan.FromMilliseconds(500));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[2].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(150));

            // Act
            await _controller.SendWebhooks();

            // Assert
            _httpMessageHandler.VerifyAnyRequest(Times.Exactly(3));
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[0].PayloadUrl, Times.Once());
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[1].PayloadUrl, Times.Once());
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[2].PayloadUrl, Times.Once());
        }

        [Fact]
        public async Task SendWebhooks_ShouldLogFailedRequests()
        {
            // Arrange
            var webhooks = new List<Webhook>
            {
                new Webhook { Id = 1, PayloadUrl = "https://url1.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 2, PayloadUrl = "https://url2.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 3, PayloadUrl = "https://url3.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
            };
            _dbContext.Webhooks.AddRange(webhooks);
            _dbContext.SaveChanges();

            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[0].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.RequestTimeout), TimeSpan.FromMilliseconds(100));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[1].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.InternalServerError), TimeSpan.FromMilliseconds(500));
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[2].PayloadUrl).ReturnsAsync(new HttpResponseMessage(HttpStatusCode.OK), TimeSpan.FromMilliseconds(150));

            // Act
            await _controller.SendWebhooks();

            // Assert
            VerifyLogError($"Failed to send POST request to {webhooks[0].PayloadUrl}", Times.Once());
            VerifyLogError($"Failed to send POST request to {webhooks[1].PayloadUrl}", Times.Once());
            VerifyLogError($"Failed to send POST request to {webhooks[2].PayloadUrl}", Times.Never());
        }

        private void VerifyLogError(string message, Times times)
        {
            _logger.Verify
            (
                l => l.Log
                (
                    LogLevel.Error,
                    It.IsAny<EventId>(),
                    It.Is<object>(o => string.Equals(message, o.ToString(), StringComparison.InvariantCulture)),
                    It.IsAny<Exception>(),
                    (Func<object, Exception?, string>)It.IsAny<object>()
                ),
                times
            );
        }

        private async Task<bool> MatchRequestBody(HttpRequestMessage requestMessage, Webhook webhook)
        {
            if (requestMessage.Content == null)
            {
                return false;
            }

            var json = await requestMessage.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<WebhookPayload>(json);
            if (payload?.Message == null)
            {
                return false;
            }

            return payload.Message.Contains($"Hallo from {nameof(Webhook)} {webhook.Id.ToString(CultureInfo.InvariantCulture)}", StringComparison.InvariantCulture);
        }
    }
}
