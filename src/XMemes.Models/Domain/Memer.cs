using System.Collections.Generic;

namespace XMemes.Models.Domain
{
    public class Memer: TrackedEntity
    {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public int TotalLikes { get; set; }
        public bool IsAdmin { get; set; }
        public List<Meme> Memes { get; set; } = new List<Meme>();
        public List<Template> Templates { get; set; } = new List<Template>();
    }
}