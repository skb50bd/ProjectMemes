using System;
using System.Collections.Generic;

namespace XMemes.Models.Domain
{
    public class Meme: TrackedEntity
    {
        public string? Name { get; set; }

        //public int MemerId { get; set; }
        public Memer? Memer { get; set; }

        //public int TemplateId { get; set; }
        public Template? Template { get; set; }

        //public List<int>? TagIds { get; set; }
        public List<Tag> Tags { get; set; } = new List<Tag>();

        public string? Description { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }

        //public List<int>? LikersIds { get; set; }
        public List<Memer> Likers { get; set; } = new List<Memer>();

        public bool Nsfw { get; set; }
        public bool Original { get; set; }
        public string? Hash { get; set; }
        public string? ImageLocation { get; set; }
    }
}
