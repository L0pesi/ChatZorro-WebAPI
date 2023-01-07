using System.ComponentModel.DataAnnotations;

namespace ChatZorro.Models
{
    public class NewMessageModel
    {
        public int ChatCode { get; set; }
        [Required]
        public string Text { get; set; }
    }
}
