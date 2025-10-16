using System.ComponentModel.DataAnnotations;

namespace SafeScribe.Models
{
    public class Note
    {
        public int Id { get; set; }
        
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Title { get; set; } = string.Empty;
        
        [Required]
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }
        
        public int UserId { get; set; }
        public User? User { get; set; }
        
        public bool IsDeleted { get; set; } = false;
    }
}
