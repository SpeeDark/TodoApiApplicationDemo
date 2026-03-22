using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Todo
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = String.Empty;
        
        public string? Description { get; set; }
        
        public bool IsComplete { get; set; } = false;
        
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
        
        public DateTime? UpdatedAt { get; set; }

        // Foreign Key
        public int? CategoryId { get; set; }
        public int OwnerId { get; set; }
        
        // Navigation property
        public Category? Category { get; set; }
        public User Owner { get; set; } = null!;
    }
}
