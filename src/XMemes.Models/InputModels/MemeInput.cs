namespace XMemes.Models.InputModels
{
    public class MemeInput : BaseInput
    {
        public string? Name { get; set; }
        public string[] Tags { get; set; } = new string[0];
        public bool Nsfw { get; set; }
        public bool Original { get; set; }
        public string ImageId { get; set; } = string.Empty;
    }
}