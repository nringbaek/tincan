using System;
using System.ComponentModel.DataAnnotations;

namespace Tincan.Web.Controllers.Models
{
    public class MessageDto
    {
        public DateTime ExpiresAt { get; set; }
    }

    public class CreateMessageDto
    {
        [Required]
        public string Key { get; set; }

        [Required]
        public string Content { get; set; }

        [Required]
        public DateTime ExpiresAt { get; set; }
    }

    public class DecryptMessageDto
    {
        [Required]
        public string Key { get; set; }
    }
}
