using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.Models
{
    public class User
    {
        [Key]
        public int Id { get; set; }
        
        public string Name { get; set; } = string.Empty;

        public string Email { get; set; } = string.Empty;
        
        public string Password { get; set; } = string.Empty;

        // Navigation property
        public ICollection<Todo> Todos { get; set; } = new List<Todo>();
        public ICollection<Category> Categories { get; set; } = new List<Category>();
    }
}
