using System.ComponentModel.DataAnnotations;

namespace Webhooks.Sender.ResourceModels
{
    public class CreateWebhookRequest
    {
        [Required]
        public string PayloadUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
