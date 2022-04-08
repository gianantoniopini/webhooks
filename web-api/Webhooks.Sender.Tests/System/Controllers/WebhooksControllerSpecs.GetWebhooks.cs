using Microsoft.AspNetCore.Mvc;
using FluentAssertions;
using Xunit;
using Webhooks.Sender.Models;

namespace Webhooks.Sender.Tests.System.Controllers
{
    public partial class WebhooksControllerSpecs
    {
        [Fact]
        public async Task GetWebhooks_ShouldReturnAllWebhooks()
        {
            // Arrange
            var expectedWebhooks = new List<Webhook>
            {
                new Webhook { Id = 1, PayloadUrl = "https://url1.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 2, PayloadUrl = "https://url2.com", IsActive = false, CreatedAt = DateTimeOffset.UtcNow },
                new Webhook { Id = 3, PayloadUrl = "https://url3.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow },
            };
            _dbContext.Webhooks.AddRange(expectedWebhooks);
            _dbContext.SaveChanges();

            // Act
            var result = await _controller.GetWebhooks();

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<OkObjectResult>();
            var okObjectResult = result.Result.As<OkObjectResult>();
            okObjectResult.Value.Should().BeOfType<List<Webhook>>();
            var actualWebhooks = okObjectResult.Value.As<List<Webhook>>();
            actualWebhooks.Should().BeEquivalentTo(expectedWebhooks);
        }
    }
}
