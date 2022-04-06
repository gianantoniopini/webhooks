using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentAssertions;
using Moq;
using Xunit;
using Webhooks.Sender.Controllers;
using Webhooks.Sender.DbContexts;
using Webhooks.Sender.Models;

namespace Webhooks.Sender.Tests.System.Controllers
{
    public partial class WebhooksControllerSpecs
    {
        private readonly Mock<ILogger<WebhooksController>> _logger;

        private readonly WebhooksContext _dbContext;

        private readonly Mock<IHttpClientFactory> _clientFactory;

        private readonly WebhooksController _controller;

        public WebhooksControllerSpecs()
        {
            _logger = new Mock<ILogger<WebhooksController>>();

            var options = new DbContextOptionsBuilder<WebhooksContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _dbContext = new WebhooksContext(options);

            _clientFactory = new Mock<IHttpClientFactory>();

            _controller = new WebhooksController(_logger.Object, _dbContext, _clientFactory.Object);
        }

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
