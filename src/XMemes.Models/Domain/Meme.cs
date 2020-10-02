using System;
using System.Collections.Generic;

namespace XMemes.Models.Domain
{
    public class Meme : TrackedEntity
    {
        public string? Name { get; set; }
        public Guid MemerId { get; set; }
        public Guid TemplateId { get; set; }
        public IList<Guid> TagIds { get; set; } = new List<Guid>();
        public string? Description { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public IList<Guid> LikerIds { get; set; } = new List<Guid>();
        public bool Nsfw { get; set; }
        public bool Original { get; set; }
        public string? ImageId { get; set; }
    }
}
