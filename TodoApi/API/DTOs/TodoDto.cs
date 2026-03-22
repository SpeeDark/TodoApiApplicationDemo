using System;
using System.ComponentModel.DataAnnotations;

namespace TodoApi.API.DTOs
{
    #region RequestDtos

    public class AddTodoDto
    {
        [Required]
        [StringLength(100)]
        public string Title { get; set; } = string.Empty;

        [StringLength(500)]
        public string? Description { get; set; }

        public bool IsComplete { get; set; } = false;

        public int? CategoryId { get; set; }
    }

    public class PutTodoDto
    {
        public string? Title { get; set; }
        public string? Description { get; set; }
        public bool IsComplete { get; set; } = false;
    }

    #endregion

    #region ResponseDtos

    public class TodoResponseDto
    {
        public int Id { get; set; }
        public string Title { get; set; } = String.Empty;
        public string? Description { get; set; }
        public bool IsComplete { get; set; }
        public DateTime CreatedAt { get; set; }
        public CategoryResponseDto? Category { get; set; }
    }

    #endregion
}
