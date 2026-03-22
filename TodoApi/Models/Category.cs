using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class Category
    {
        [Key]
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string? Description { get; set; }

        // Foreign Key
        public int OwnerId { get; set; }

        // Navigation property
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
        public User Owner { get; set; } = null!;
    }
}
