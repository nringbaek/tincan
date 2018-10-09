using System;

namespace Tincan.Entities
{
    public class CreateMessage
    {
        public string Key { get; set; }
        public string Content { get; set; }
        public TimeSpan ExpiresIn { get; set; }
    }
}
