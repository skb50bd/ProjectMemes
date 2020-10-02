using Microsoft.AspNetCore.Http;

namespace XMemes.Models.InputModels
{
    public class MemerInput : BaseInput
    {
        public string? Name { get; set; }
        public string? Username { get; set; }
        public string? Bio { get; set; }
        public IFormFile? Image { get; set; }
    }
}