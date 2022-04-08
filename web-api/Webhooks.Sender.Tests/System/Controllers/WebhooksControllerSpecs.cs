using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Webhooks.Sender.Controllers;
using Webhooks.Sender.DbContexts;

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
    }
}
