using Microsoft.AspNetCore.Http;

namespace XMemes.Models.InputModels
{
    public class TemplateInput : BaseInput
    {
        public string? Name { get; set; }
        public bool Nsfw { get; set; }

        //[Required]
        public IFormFile? Image { get; set; }
    }
}