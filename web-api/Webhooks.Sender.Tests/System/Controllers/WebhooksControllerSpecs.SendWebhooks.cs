using System.Net;
using Moq.Contrib.HttpClient;
using Xunit;
using Webhooks.Sender.Models;

namespace Webhooks.Sender.Tests.System.Controllers
{
    public partial class WebhooksControllerSpecs
    {
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

            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[0].PayloadUrl).ReturnsResponse(HttpStatusCode.OK);
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[1].PayloadUrl).ReturnsResponse(HttpStatusCode.InternalServerError);
            _httpMessageHandler.SetupRequest(HttpMethod.Post, webhooks[2].PayloadUrl).ReturnsResponse(HttpStatusCode.OK);

            // Act
            await _controller.SendWebhooks();

            // Assert
            _httpMessageHandler.VerifyAll();
        }
    }
}
