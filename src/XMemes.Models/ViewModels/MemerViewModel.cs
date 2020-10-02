namespace XMemes.Models.ViewModels
{
    public class MemerViewModel : BaseViewModel
    {
        public string? Name { get; set; }
        public string? Bio { get; set; }
        public string? ImageId { get; set; }
        public int TotalLikes { get; set; }
    }
}