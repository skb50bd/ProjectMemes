using System.Collections.Generic;

namespace XMemes.Models.ViewModels
{
    public class MemeViewModel : BaseViewModel
    {
        public string? Name { get; set; }
        public string MemerName { get; set; }
        public string MemerUserName { get; set; }
        public string TemplateId { get; set; }
        public IList<TagViewModel> Tags { get; set; }
    }
}