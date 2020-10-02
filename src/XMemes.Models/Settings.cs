namespace XMemes.Models
{
    public class Settings
    {
        public int PageSize { get; set; }
        public int PopularityThreshHold { get; set; }
        public int MaxImageWidth { get; set; }
        public int MaxImageHeight { get; set; }
        public string? AllowedOrigins { get; set; }
    }
}