using System.ComponentModel.DataAnnotations;

namespace Webhooks.Sender.ResourceModels
{
    public class WebhookPayload
    {
        [Required]
        public string Message { get; set; }
    }
}
