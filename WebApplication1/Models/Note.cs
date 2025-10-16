﻿using System.ComponentModel.DataAnnotations;

namespace SafeScribe.Models
{
    public class Note
    {
        public int Id { get; set; }
        
        public string Title { get; set; } = string.Empty;
        
        public string Content { get; set; } = string.Empty;
        
        public DateTime CreatedAt { get; set; } = DateTime.Now;
       
        public int UserId { get; set; }
        public User? User { get; set; }
    }
}
