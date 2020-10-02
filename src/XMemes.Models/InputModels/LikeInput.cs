using System;
using System.ComponentModel.DataAnnotations;

namespace XMemes.Models.InputModels
{
    public class LikeInput
    {
        [Required]
        public Guid MemeId { get; set; }
        
        [Required]
        public Guid LikerId { get; set; }
    }
}
