using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Routing;
using Microsoft.EntityFrameworkCore;
using FluentAssertions;
using Xunit;
using Webhooks.Sender.Models;
using Webhooks.Sender.ResourceModels;

namespace Webhooks.Sender.Tests.System.Controllers
{
    public partial class WebhooksControllerSpecs
    {
        [Fact]
        public async Task PostWebhook_IfWebhookWithSamePayloadUrlAlreadyExists_ShouldProduceUnprocessableEntityResponse()
        {
            // Arrange
            var webhook = new Webhook { Id = 1, PayloadUrl = "https://url1.com", IsActive = true, CreatedAt = DateTimeOffset.UtcNow };
            _dbContext.Webhooks.Add(webhook);
            _dbContext.SaveChanges();
            var request = new CreateWebhookRequest { PayloadUrl = webhook.PayloadUrl, IsActive = false };

            // Act
            var result = await _controller.PostWebhook(request);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<UnprocessableEntityObjectResult>();
            var unprocessableEntityObjectResult = result.Result.As<UnprocessableEntityObjectResult>();
            unprocessableEntityObjectResult.Value.Should().BeOfType<string>();
            var actualError = unprocessableEntityObjectResult.Value.As<string>();
            actualError.Should().Be($"A {nameof(Webhook)} with {nameof(Webhook.PayloadUrl)} {request.PayloadUrl} already exists.");
        }

        [Fact]
        public async Task PostWebhook_ShouldAddNewWebhookToDatabase()
        {
            // Arrange
            var request = new CreateWebhookRequest { PayloadUrl = "https://url1.com", IsActive = true };
            var utcNow = DateTimeOffset.UtcNow;

            // Act
            await _controller.PostWebhook(request);

            // Assert
            _dbContext.Webhooks.Count().Should().Be(1);
            var actualWebhook = _dbContext.Webhooks.Single();
            actualWebhook.Id.Should().BePositive();
            actualWebhook.PayloadUrl.Should().Be(request.PayloadUrl);
            actualWebhook.IsActive.Should().Be(request.IsActive);
            actualWebhook.CreatedAt.Should().BeOnOrAfter(utcNow);
            _dbContext.Entry(actualWebhook).State.Should().Be(EntityState.Unchanged);
        }

        [Fact]
        public async Task PostWebhook_ShouldProduceCreatedResponse()
        {
            // Arrange
            var request = new CreateWebhookRequest { PayloadUrl = "https://url1.com", IsActive = true };

            // Act
            var result = await _controller.PostWebhook(request);

            // Assert
            result.Should().NotBeNull();
            result.Result.Should().BeOfType<CreatedAtActionResult>();
            var createdAtActionResult = result.Result.As<CreatedAtActionResult>();
            createdAtActionResult.ActionName.Should().Be(nameof(_controller.GetWebhook));
            createdAtActionResult.RouteValues.Should().BeOfType<RouteValueDictionary>();
            var routeValueDictionary = createdAtActionResult.RouteValues.As<RouteValueDictionary>();
            routeValueDictionary.Should().HaveCount(1);
            var routeValue = routeValueDictionary.Single();
            routeValue.Key.Should().Be("id");
            var webhook = _dbContext.Webhooks.Single();
            routeValue.Value.Should().Be(webhook.Id);
            createdAtActionResult.Value.Should().Be(webhook);
        }
    }
}
