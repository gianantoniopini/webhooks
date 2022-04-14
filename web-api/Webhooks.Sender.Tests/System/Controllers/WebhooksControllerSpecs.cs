using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Moq;
using Moq.Contrib.HttpClient;
using Webhooks.Sender.Controllers;
using Webhooks.Sender.DbContexts;

namespace Webhooks.Sender.Tests.System.Controllers
{
    public partial class WebhooksControllerSpecs
    {
        private readonly Mock<ILogger<WebhooksController>> _logger;

        private readonly WebhooksContext _dbContext;

        private readonly Mock<HttpMessageHandler> _httpMessageHandler;

        private readonly WebhooksController _controller;

        public WebhooksControllerSpecs()
        {
            _logger = new Mock<ILogger<WebhooksController>>();

            var options = new DbContextOptionsBuilder<WebhooksContext>().UseInMemoryDatabase(databaseName: Guid.NewGuid().ToString()).Options;
            _dbContext = new WebhooksContext(options);

            _httpMessageHandler = new Mock<HttpMessageHandler>();
            var clientFactory = _httpMessageHandler.CreateClientFactory();

            _controller = new WebhooksController(_logger.Object, _dbContext, clientFactory);
        }
    }
}
