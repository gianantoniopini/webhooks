namespace Webhooks.Sender.Models
{
    public class Webhook
    {
        public long Id { get; set; }

        public string PayloadUrl { get; set; }

        public EventType Event { get; set; }

        public bool IsActive { get; set; }
    }
}