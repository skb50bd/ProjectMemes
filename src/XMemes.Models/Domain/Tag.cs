using System.Collections.Generic;

namespace XMemes.Models.Domain
{
    public class Tag: TrackedEntity
    {
        public string? Name { get; set; }
        public List<Meme> Memes { get; set; } = new List<Meme>();
    }
}