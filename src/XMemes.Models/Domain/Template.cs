using System;
using System.Collections.Generic;

namespace XMemes.Models.Domain
{
    public class Template: TrackedEntity
    {
        public string? Name { get; set; }

        //public int MemerId { get; set; }

        public Memer? Memer { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public bool Nsfw { get; set; }
        public List<Meme> Memes { get; set; } = new List<Meme>();
    }
}