using System;

namespace XMemes.Models.ViewModels
{
    public class TemplateViewModel : BaseViewModel
    {
        public string? Name { get; set; }
        public string? MemerId { get; set; }
        public bool Nsfw { get; set; }
        public DateTimeOffset SubmittedAt { get; set; }
        public string? ImageId { get; set; }
    }
}