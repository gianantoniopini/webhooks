using System;
using Webhooks.Sender.Models;
using Webhooks.Sender.ResourceModels;

namespace Webhooks.Sender.Mappers
{
    public class ModelMapper
    {
        public static Webhook ToModel(CreateWebhookRequest request)
        {
            var webhook = new Webhook { PayloadUrl = request.PayloadUrl, IsActive = request.IsActive, CreatedAt = DateTimeOffset.UtcNow };

            return webhook;
        }
    }
}
