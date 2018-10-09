using System;

namespace Tincan.Entities
{
    public class Message
    {
        public string Id { get; set; }
        public string EncryptedContent { get; set; }

        public DateTime CreatedAt { get; set; }
        public DateTime ExpiresAt { get; set; }
    }
}
