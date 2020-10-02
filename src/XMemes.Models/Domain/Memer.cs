namespace XMemes.Models.Domain
{
    public class Memer : TrackedEntity
    {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Bio { get; set; }
        public string? ImageId { get; set; }
    }
}