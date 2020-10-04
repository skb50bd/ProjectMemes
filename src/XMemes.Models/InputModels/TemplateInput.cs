namespace XMemes.Models.InputModels
{
    public class TemplateInput : BaseInput
    {
        public string? Name { get; set; }
        public bool Nsfw { get; set; }

        public string? ImageId { get; set; }
    }
}