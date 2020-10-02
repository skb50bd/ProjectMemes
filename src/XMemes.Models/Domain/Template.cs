using System;

namespace XMemes.Models.Domain
{
    public class Template : TrackedEntity
    {
        public string? Name { get; set; }

        public Guid MemerId { get; set; }

        public DateTimeOffset SubmittedAt { get; set; }
        public bool Nsfw { get; set; }
        public string? ImageId { get; set; }
    }
}