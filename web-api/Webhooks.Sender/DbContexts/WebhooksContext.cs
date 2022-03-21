using Microsoft.EntityFrameworkCore;
using Webhooks.Sender.Models;

namespace Webhooks.Sender.DbContexts
{
    public class WebhooksContext : DbContext
    {
        public WebhooksContext(DbContextOptions<WebhooksContext> options) : base(options)
        {
        }

        public DbSet<Webhook> Webhooks { get; set; }
    }
}