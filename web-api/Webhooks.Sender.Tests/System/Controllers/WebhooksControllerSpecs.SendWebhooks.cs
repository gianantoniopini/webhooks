using System.Net;
using Moq;
using Moq.Contrib.HttpClient;
using Xunit;
using Webhooks.Sender.Models;

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
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[0].PayloadUrl, Times.Exactly(1));
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[1].PayloadUrl, Times.Never());
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[2].PayloadUrl, Times.Exactly(1));
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
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[0].PayloadUrl, Times.Exactly(1));
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[1].PayloadUrl, Times.Exactly(1));
            _httpMessageHandler.VerifyRequest(HttpMethod.Post, webhooks[2].PayloadUrl, Times.Exactly(1));
        }
    }
}
