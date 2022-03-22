using System;
using System.ComponentModel.DataAnnotations;
using Microsoft.EntityFrameworkCore;

namespace Webhooks.Sender.Models
{
    [Index(nameof(PayloadUrl), IsUnique = true)]
    public class Webhook
    {
        public long Id { get; set; }

        [Required]
        public string PayloadUrl { get; set; }

        public bool IsActive { get; set; }
    }
}
